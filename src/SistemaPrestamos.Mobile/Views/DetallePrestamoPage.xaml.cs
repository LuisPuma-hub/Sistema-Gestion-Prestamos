using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(IdTexto), "id")]
public partial class DetallePrestamoPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private readonly PagoService _pagoService;
    private PrestamoDto? _prestamo;

    public string IdTexto { get; set; } = string.Empty;

    public DetallePrestamoPage(
        PrestamoService prestamoService,
        PagoService pagoService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
        _pagoService = pagoService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

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

            PagarButton.IsVisible = estaActivo;
            MorosidadButton.IsVisible = estaActivo;

            var pagos = await _pagoService
                .ObtenerPorPrestamoAsync(_prestamo.Id);

            var listaPagos = pagos
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
