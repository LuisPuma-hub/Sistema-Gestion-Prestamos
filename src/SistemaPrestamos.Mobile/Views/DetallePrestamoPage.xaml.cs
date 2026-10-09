using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(IdTexto), "id")]
public partial class DetallePrestamoPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private readonly PagoService _pagoService;
    private readonly ClienteService _clienteService;
    private PrestamoDto? _prestamo;
    private string _telefono = string.Empty;

    public string IdTexto { get; set; } = string.Empty;

    public DetallePrestamoPage(
        PrestamoService prestamoService,
        PagoService pagoService,
        ClienteService clienteService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
        _pagoService = pagoService;
        _clienteService = clienteService;
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
            InteresLabel.Text = $"S/ {_prestamo.InteresSemanal:N2}";
            TasaLabel.Text = $"{_prestamo.TasaInteresSemanal:P0} semanal";
            FechaInicioLabel.Text = $"{_prestamo.FechaInicio:dd/MM/yyyy}";

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

        var perdonar = await DisplayAlertAsync(
            "Perdonar intereses",
            "¿Perdonar las semanas vencidas impagas?",
            "Sí, perdonar",
            "No");

        var motivo = await DisplayPromptAsync(
            "Motivo del ajuste",
            "Obligatorio (10 a 200 caracteres):",
            "Guardar",
            "Cancelar",
            maxLength: 200);

        if (string.IsNullOrWhiteSpace(motivo))
            return;

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
                motivo.Trim());

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
