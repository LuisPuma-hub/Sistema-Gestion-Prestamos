using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class ReportesPage : ContentPage
{
    private readonly ReporteService _reporteService;

    public ReportesPage(ReporteService reporteService)
    {
        InitializeComponent();
        _reporteService = reporteService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);
        await CargarAsync();
    }

    private async void OnReintentarClicked(object? sender, EventArgs e)
    {
        await CargarAsync();
    }

    private async Task CargarAsync()
    {
        try
        {
            MostrarCargando(true);
            EstadoLabel.IsVisible = false;
            ReintentarButton.IsVisible = false;

            var cobranzaTask = _reporteService.CobranzaHoyAsync();
            var carteraTask = _reporteService.CarteraAsync();
            var ingresosTask = _reporteService.IngresosUltimosDiasAsync(7);
            var capitalTask = _reporteService.CapitalAsync();

            await Task.WhenAll(
                cobranzaTask, carteraTask, ingresosTask, capitalTask);

            var cobranza = await cobranzaTask;
            var cartera = await carteraTask;
            var ingresos = await ingresosTask;
            var capital = await capitalTask;

            if (cobranza is not null)
            {
                CobradoLabel.Text = $"S/ {cobranza.Total:N2}";
                CobradoDetalleLabel.Text =
                    $"{cobranza.NumPagos} pago(s) · " +
                    $"Int. S/ {cobranza.AInteres:N2} · " +
                    $"Cap. S/ {cobranza.ACapital:N2}";
            }

            if (cartera is not null)
            {
                CarteraLabel.Text = $"S/ {cartera.DeudaTotal:N2}";
                CarteraDetalleLabel.Text =
                    $"{cartera.NumPrestamos} préstamo(s) · " +
                    $"Cap. S/ {cartera.CapitalTotal:N2} · " +
                    $"Int. S/ {cartera.InteresesTotal:N2}";
            }

            IngresosCollection.ItemsSource = ingresos;

            if (capital is not null)
            {
                DisponibleLabel.Text = $"S/ {capital.Disponible:N2}";
                CapitalDetalleLabel.Text =
                    $"Base S/ {capital.BaseEfectiva:N2} · " +
                    $"Colocado S/ {capital.Colocado:N2}\n" +
                    $"Ganado int. S/ {capital.GanadoIntereses:N2} · " +
                    $"Recuperado cap. S/ {capital.CapitalRecuperado:N2} · " +
                    $"ROI {capital.Roi:N2}%";
            }
        }
        catch (HttpRequestException)
        {
            MostrarError("Sin conexión. Verifique su red.");
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

    private async void OnExportarClicked(object? sender, EventArgs e)
    {
        try
        {
            MostrarCargando(true);

            var ruta = await _reporteService.DescargarCarteraCsvAsync();

            if (ruta is null)
            {
                MostrarError("No se pudo exportar.");
                return;
            }

            await Share.Default.RequestAsync(
                new ShareFileRequest
                {
                    Title = "Cartera por cobrar",
                    File = new ShareFile(ruta)
                });
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

    private async void OnAporteClicked(object? sender, EventArgs e)
    {
        await RegistrarMovimientoAsync("Aporte");
    }

    private async void OnRetiroClicked(object? sender, EventArgs e)
    {
        await RegistrarMovimientoAsync("Retiro");
    }

    private async Task RegistrarMovimientoAsync(string tipo)
    {
        var montoTexto = await DisplayPromptAsync(
            tipo,
            "Monto:",
            "Continuar",
            "Cancelar",
            maxLength: 20,
            keyboard: Keyboard.Numeric);

        if (!decimal.TryParse(
                montoTexto?.Trim(),
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture,
                out var monto) || monto <= 0)
        {
            if (!string.IsNullOrWhiteSpace(montoTexto))
                await DisplayAlertAsync(
                    "Monto inválido",
                    "Ingrese un monto mayor que cero.",
                    "OK");
            return;
        }

        var motivo = await DisplayPromptAsync(
            tipo,
            "Motivo (opcional):",
            "Guardar",
            "Cancelar",
            maxLength: 200);

        if (motivo is null)
            return;

        try
        {
            MostrarCargando(true);

            var (exito, error) = await _reporteService
                .RegistrarMovimientoAsync(
                    tipo,
                    monto,
                    string.IsNullOrWhiteSpace(motivo) ? null : motivo.Trim());

            if (!exito)
            {
                MostrarError(error ?? "No se pudo registrar.");
                return;
            }

            await CargarAsync();
        }
        catch (HttpRequestException)
        {
            MostrarError("Sin conexión. Verifique su red.");
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

    private void MostrarCargando(bool cargando)
    {
        CargandoIndicator.IsVisible = cargando;
        CargandoIndicator.IsRunning = cargando;
    }

    private void MostrarError(string mensaje)
    {
        EstadoLabel.Text = mensaje;
        EstadoLabel.IsVisible = true;
        ReintentarButton.IsVisible = true;
    }
}
