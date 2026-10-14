using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(IdTexto), "id")]
public partial class DetallePrestamoPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private readonly PagoService _pagoService;
    private readonly ClienteService _clienteService;
    private readonly CompromisoService _compromisoService;
    private PrestamoDto? _prestamo;
    private string _telefono = string.Empty;

    public string IdTexto { get; set; } = string.Empty;

    public DetallePrestamoPage(
        PrestamoService prestamoService,
        PagoService pagoService,
        ClienteService clienteService,
        CompromisoService compromisoService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
        _pagoService = pagoService;
        _clienteService = clienteService;
        _compromisoService = compromisoService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);

        if (Guid.TryParse(IdTexto, out var id))
        {
            await CargarAsync(id);
        }
        else
        {
            MostrarError("Identificador de préstamo no válido.");
        }
    }

    private async Task CargarAsync(Guid id)
    {
        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            _prestamo = await _prestamoService.ObtenerPorIdAsync(id);

            if (_prestamo is null)
            {
                MostrarError("Préstamo no encontrado.");
                return;
            }

            ClienteLabel.Text = _prestamo.ClienteNombre;
            AvatarLabel.Text = _prestamo.ClienteNombre.Length > 0
                ? _prestamo.ClienteNombre.Substring(0, 1).ToUpperInvariant()
                : "?";

            try
            {
                var cliente = await _clienteService
                    .ObtenerPorIdAsync(_prestamo.ClienteId);

                _telefono = cliente?.Telefono ?? string.Empty;
                TelefonoLabel.Text = _telefono;
                CopiarTelefonoButton.IsVisible =
                    !string.IsNullOrWhiteSpace(_telefono);
            }
            catch
            {
                CopiarTelefonoButton.IsVisible = false;
            }
            DocumentoLabel.Text = _prestamo.FechaAprobacion is null
                ? "Sin aprobar"
                : $"Aprobado {_prestamo.FechaAprobacion:dd/MM/yyyy}";
            EstadoPill.BindingContext = _prestamo;
            EstadoLabel2.Text = _prestamo.Estado;

            CapitalLabel.Text = $"S/ {_prestamo.CapitalInicial:N2}";
            PendienteLabel.Text = $"S/ {_prestamo.CapitalPendiente:N2}";
            InteresLabel.Text = _prestamo.EsPlazoFijo
                ? $"S/ {_prestamo.CuotaFija:N2}" +
                  (_prestamo.EsFrecuenciaSemanal ? "/sem." : "/día")
                : _prestamo.EsSaldo
                    ? $"S/ {_prestamo.CapitalPendiente * _prestamo.TasaInteresSemanal:N2}"
                    : $"S/ {_prestamo.InteresSemanal:N2}";
            TasaLabel.Text = $"{_prestamo.TasaInteresSemanal:P0} semanal " +
                (_prestamo.EsSaldo ? "s/saldo" : "s/inicial");
            FechaInicioLabel.Text = $"{_prestamo.FechaInicio:dd/MM/yyyy}";

            if (_prestamo.EsPlazoFijo && _prestamo.PlazoCuotas.HasValue)
            {
                DiariaCard.IsVisible = true;
                DiariaDetalleLabel.Text =
                    $"{_prestamo.PlazoCuotas.Value} cuotas " +
                    (_prestamo.EsFrecuenciaSemanal
                        ? "semanales"
                        : "diarias") +
                    $" de S/ {_prestamo.CuotaFija:N2} · Total " +
                    $"S/ {_prestamo.MontoTotal:N2} · Fin " +
                    $"{_prestamo.FechaFin:dd/MM/yyyy} · Sin domingos.";
            }
            else
            {
                DiariaCard.IsVisible = false;
            }

            AprobarButton.IsVisible =
                string.Equals(_prestamo.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase)
                && await EsAdminAsync();

            AnularButton.IsVisible =
                (string.Equals(_prestamo.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase)
                || string.Equals(_prestamo.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
                && await EsAdminAsync();

            var estaActivo = string.Equals(
                _prestamo.Estado,
                "Activo",
                StringComparison.OrdinalIgnoreCase);

            // En mora también se cobra: solo así regulariza.
            var cobra = estaActivo ||
                string.Equals(
                    _prestamo.Estado,
                    "Moroso",
                    StringComparison.OrdinalIgnoreCase);

            PagarButton.IsVisible = cobra;
            MorosidadButton.IsVisible = cobra;

            AjustarButton.IsVisible = cobra && await EsAdminAsync();

            var pagos = await _pagoService
                .ObtenerPorPrestamoAsync(_prestamo.Id);

            var listaPagos = pagos
                .Where(p => p.Cobrado)
                .OrderByDescending(p => p.FechaPago)
                .ToList();

            PagosCollection.ItemsSource = listaPagos;

            PagosResumenLabel.Text = listaPagos.Count == 0
                ? "Sin pagos registrados."
                : "";
            PagosResumenLabel.IsVisible = listaPagos.Count == 0;

            TotalPagadoLabel.Text = $"S/ {listaPagos.Sum(p => p.Monto):N2}";

            try
            {
                var periodos = await _prestamoService.ObtenerPeriodosAsync(_prestamo.Id);

                var proximo = periodos
                    .Where(p => p.InteresPendiente > 0)
                    .OrderBy(p => p.FechaVencimiento)
                    .FirstOrDefault();

                ProxVencLabel.Text = proximo is null
                    ? "-"
                    : $"{proximo.FechaVencimiento:dd/MM/yyyy}";
            }
            catch
            {
                ProxVencLabel.Text = "-";
            }

            CompromisosPrestamoLayout.IsVisible = await EsAdminAsync();

            if (CompromisosPrestamoLayout.IsVisible)
            {
                CompromisoPrestamoFechaPicker.Date = DateTime.Today;
                await CargarCompromisosPrestamoAsync();
            }
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            MostrarError($"Ocurrió un error: {ex.Message}");
        }
        finally
        {
            MostrarCargando(false);
        }
    }

    private async Task CargarCompromisosPrestamoAsync()
    {
        if (_prestamo is null)
            return;

        var todos = await _compromisoService.ListarPorClienteAsync(
            _prestamo.ClienteId);

        CompromisosPrestamoCollection.ItemsSource = todos
            .Where(c => c.PrestamoId == _prestamo.Id)
            .OrderByDescending(c => c.FechaCompromiso)
            .ToList();
    }

    private static readonly string[] MotivosCompromiso =
    [
        "Paga hoy",
        "Paga mañana",
        "Paga el lunes",
        "Abonará una parte",
        "Viene a regularizar"
    ];

    private async Task<string?> ElegirTextoCompromisoAsync(
        string escrito)
    {
        if (!string.IsNullOrWhiteSpace(escrito))
            return escrito.Trim();

        var elegido = await DisplayActionSheetAsync(
            "Compromiso",
            "Cancelar",
            null,
            [.. MotivosCompromiso, "✏️ Escribir otro..."]);

        if (string.IsNullOrWhiteSpace(elegido) ||
            elegido == "Cancelar")
            return null;

        if (elegido != "✏️ Escribir otro...")
            return elegido;

        var otro = await DisplayPromptAsync(
            "Compromiso",
            "Escribe el compromiso (máx. 200):",
            "Guardar",
            "Cancelar",
            maxLength: 200);

        return string.IsNullOrWhiteSpace(otro) ? null : otro.Trim();
    }

    private async void OnGuardarCompromisoPrestamoClicked(
        object? sender, EventArgs e)
    {
        if (_prestamo is null)
            return;

        var texto = await ElegirTextoCompromisoAsync(
            CompromisoPrestamoTextoEntry.Text?.Trim() ?? string.Empty);

        if (string.IsNullOrWhiteSpace(texto))
        {
            if (!string.IsNullOrWhiteSpace(
                    CompromisoPrestamoTextoEntry.Text))
                await DisplayAlertAsync(
                    "Aviso", "Escribe el compromiso.", "OK");
            return;
        }

        var (exito, error) = await _compromisoService.CrearAsync(
            _prestamo.ClienteId,
            texto,
            CompromisoPrestamoFechaPicker.Date ?? DateTime.Today,
            _prestamo.Id);

        if (!exito)
        {
            await DisplayAlertAsync(
                "Aviso", error ?? "No se pudo guardar.", "OK");
            return;
        }

        CompromisoPrestamoTextoEntry.Text = string.Empty;

        await CargarCompromisosPrestamoAsync();
    }

    private async void OnAnularCompromisoPrestamoClicked(
        object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Guid id)
            return;

        var confirma = await DisplayAlertAsync(
            "Anular", "¿Anular este compromiso?", "Sí", "No");

        if (!confirma)
            return;

        var (exito, error) = await _compromisoService
            .AnularAsync(id);

        if (!exito)
        {
            await DisplayAlertAsync(
                "Aviso", error ?? "No se pudo anular.", "OK");
            return;
        }

        await CargarCompromisosPrestamoAsync();
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnPagoTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Guid id)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DetallePagoPage)}?id={id}");
        }
    }

    private async void OnAprobarClicked(object? sender, EventArgs e)
    {
        if (_prestamo is null)
        {
            return;
        }

        var confirmar = await DisplayAlertAsync(
            "Aprobar préstamo",
            $"¿Aprobar el préstamo de {_prestamo.ClienteNombre} por S/ {_prestamo.CapitalInicial:N2}? Se generará el primer período semanal.",
            "Sí",
            "No");

        if (!confirmar)
        {
            return;
        }

        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var (exito, error) = await _prestamoService.AprobarAsync(_prestamo.Id);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo aprobar el préstamo.");
                return;
            }

            await DisplayAlertAsync(
                "Préstamo aprobado",
                "El préstamo ahora está activo.",
                "OK");

            await CargarAsync(_prestamo.Id);
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            MostrarError($"Ocurrió un error: {ex.Message}");
        }
        finally
        {
            MostrarCargando(false);
        }
    }

    private async void OnCopiarTelefonoClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_telefono))
            return;

        await Clipboard.Default.SetTextAsync(_telefono);

        await DisplayAlertAsync(
            "Copiado",
            $"Teléfono {_telefono} copiado al portapapeles.",
            "OK");
    }

    private async Task<(decimal Total, int Conteo)?> VencidoParaDescuentoAsync()
    {
        if (_prestamo is null)
            return null;

        try
        {
            var ahora = DateTime.UtcNow;
            var lista = new List<PeriodoInteresDto>();

            try
            {
                lista = (await _prestamoService
                        .ObtenerPeriodosAsync(_prestamo.Id))
                    .Where(p => p.FechaVencimiento < ahora &&
                                p.InteresPendiente > 0)
                    .ToList();
            }
            catch
            {
                return null;
            }

            if (lista.Count == 0)
                return null;

            return (lista.Sum(p => p.InteresPendiente), lista.Count);
        }
        catch
        {
            return null;
        }
    }

    private async void OnAjustarClicked(object? sender, EventArgs e)
    {
        if (_prestamo is null)
            return;

        var capitalTexto = await DisplayPromptAsync(
            "Ajustar saldo",
            $"Capital actual S/ {_prestamo.CapitalPendiente:N2}. " +
            "Nuevo capital (vacío = sin cambio):",
            "Continuar",
            "Cancelar",
            maxLength: 20,
            keyboard: Keyboard.Numeric);

        if (capitalTexto is null)
            return;

        decimal? nuevoCapital = null;

        if (!string.IsNullOrWhiteSpace(capitalTexto))
        {
            if (!decimal.TryParse(
                    capitalTexto.Trim(),
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.CurrentCulture,
                    out var cap) || cap < 0)
            {
                await DisplayAlertAsync(
                    "Monto inválido",
                    "Ingrese un capital válido mayor o igual a cero.",
                    "OK");
                return;
            }

            nuevoCapital = cap;
        }

        var perdonarOpcion = await DisplayActionSheetAsync(
            "Perdonar intereses",
            "Cancelar",
            null,
            "Perdonar todo lo vencido",
            "Monto parcial (descuento)",
            "No perdonar");

        if (string.IsNullOrWhiteSpace(perdonarOpcion) ||
            perdonarOpcion == "Cancelar")
        {
            return;
        }

        decimal? montoPerdonar = null;
        var perdonar = false;

        if (perdonarOpcion == "Perdonar todo lo vencido")
        {
            perdonar = true;
        }
        else if (perdonarOpcion == "Monto parcial (descuento)")
        {
            var vencidos = await VencidoParaDescuentoAsync();

            if (vencidos is null || vencidos.Value.Total <= 0)
            {
                await DisplayAlertAsync(
                    "Sin vencidos",
                    "No hay intereses vencidos para perdonar.",
                    "OK");
                return;
            }

            // Sugerencia: 10% por período vencido, tope 50%.
            var factor = Math.Min(0.10m * vencidos.Value.Conteo, 0.50m);
            var sugerido = Math.Round(
                vencidos.Value.Total * factor, 2);

            var montoTexto = await DisplayPromptAsync(
                "Descuento por mora",
                $"Vencido S/ {vencidos.Value.Total:N2}. " +
                $"Sugerido S/ {sugerido:N2} " +
                $"({factor:P0}). Monto a perdonar:",
                "Continuar",
                "Cancelar",
                maxLength: 20,
                keyboard: Keyboard.Numeric,
                initialValue: $"{sugerido:N2}");

            if (montoTexto is null)
                return;

            if (!decimal.TryParse(
                    montoTexto.Trim(),
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.CurrentCulture,
                    out var monto) || monto <= 0)
            {
                await DisplayAlertAsync(
                    "Monto inválido",
                    "Ingrese un monto mayor que cero.",
                    "OK");
                return;
            }

            montoPerdonar = monto;
            perdonar = true;
        }

        var motivoElegido = await DisplayActionSheetAsync(
            "Motivo del ajuste",
            "Cancelar",
            null,
            "Descuento pactado por pronto pago",
            "Descuento por mora prolongada",
            "Corrección de saldo",
            "Condonación autorizada",
            "Error de digitación",
            "✏️ Escribir otro...");

        if (string.IsNullOrWhiteSpace(motivoElegido) ||
            motivoElegido == "Cancelar")
        {
            return;
        }

        string motivo;

        if (motivoElegido == "✏️ Escribir otro...")
        {
            var escrito = await DisplayPromptAsync(
                "Motivo del ajuste",
                "Obligatorio (10 a 200 caracteres):",
                "Guardar",
                "Cancelar",
                maxLength: 200);

            if (string.IsNullOrWhiteSpace(escrito))
            {
                return;
            }

            motivo = escrito;
        }
        else
        {
            motivo = motivoElegido;
        }

        var confirma = await DisplayAlertAsync(
            "Confirmar ajuste",
            "Se registrará como AJUSTE/CONDONACIÓN en el historial " +
            "con su usuario y fecha. ¿Continuar?",
            "Sí, ajustar",
            "Cancelar");

        if (!confirma)
            return;

        try
        {
            MostrarCargando(true);

            var (exito, error) = await _pagoService.AjustarAsync(
                _prestamo.Id,
                nuevoCapital,
                perdonar,
                motivo.Trim(),
                montoPerdonar);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo ajustar.");
                return;
            }

            await DisplayAlertAsync(
                "Ajuste registrado",
                "El saldo fue actualizado.",
                "OK");

            await CargarAsync(_prestamo.Id);
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            MostrarError($"Ocurrió un error: {ex.Message}");
        }
        finally
        {
            MostrarCargando(false);
        }
    }

    private async void OnPagarClicked(object? sender, EventArgs e)
    {
        if (_prestamo is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"{nameof(RegistrarPagoPage)}?prestamoId={_prestamo.Id}");
    }

    private async void OnAnularClicked(object? sender, EventArgs e)
    {
        if (_prestamo is null)
        {
            return;
        }

        var motivo = await DisplayPromptAsync(
            "Anular préstamo",
            "Motivo (10 a 200 caracteres):",
            "Anular",
            "Cancelar",
            maxLength: 200);

        if (string.IsNullOrWhiteSpace(motivo))
        {
            return;
        }

        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var (exito, error) = await _prestamoService.AnularAsync(
                _prestamo.Id,
                motivo.Trim());

            if (!exito)
            {
                await DisplayAlertAsync(
                    "No se puede anular",
                    error ?? "No se pudo anular el préstamo.",
                    "OK");
                return;
            }

            await DisplayAlertAsync(
                "Préstamo anulado",
                "El préstamo quedó anulado.",
                "OK");

            await CargarAsync(_prestamo.Id);
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            MostrarError($"Ocurrió un error: {ex.Message}");
        }
        finally
        {
            MostrarCargando(false);
        }
    }

    private async void OnMorosidadClicked(object? sender, EventArgs e)    {
        if (_prestamo is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"{nameof(DetalleMorosidadPage)}?prestamoId={_prestamo.Id}");
    }

    private static async Task<bool> EsAdminAsync()
    {
        var rol = await SecureStorage.Default.GetAsync("usuario_rol");

        return string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase);
    }

    private async void OnWhatsappClicked(object? sender, EventArgs e)
    {
        if (_prestamo is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"{nameof(HistorialWhatsappPage)}?clienteId={_prestamo.ClienteId}&prestamoId={_prestamo.Id}");
    }

    private void MostrarCargando(bool cargando)
    {
        CargandoIndicator.IsVisible = cargando;
        CargandoIndicator.IsRunning = cargando;
    }

    private void MostrarError(string mensaje)
    {
        ErrorLabel.Text = TextoError.Limpiar(mensaje);
        ErrorLabel.IsVisible = true;
    }
}
