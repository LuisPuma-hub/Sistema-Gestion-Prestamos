using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(IdTexto), "id")]
public partial class DetallePagoPage : ContentPage
{
    private readonly PagoService _pagoService;
    private readonly PrestamoService _prestamoService;

    public string IdTexto { get; set; } = string.Empty;

    public DetallePagoPage(
        PagoService pagoService,
        PrestamoService prestamoService)
    {
        InitializeComponent();
        _pagoService = pagoService;
        _prestamoService = prestamoService;
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
            MostrarError("Identificador de pago no válido.");
        }
    }

    private async Task CargarAsync(Guid id)
    {
        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            PagoDto? pago = await _pagoService.ObtenerPorIdAsync(id);

            if (pago is null)
            {
                MostrarError("Pago no encontrado.");
                return;
            }

            MontoLabel.Text = $"S/ {pago.Monto:N2}";
            FechaLabel.Text = pago.FechaPago.ToLocalTime().ToString(
                "dd/MM/yyyy · h:mm tt",
                new System.Globalization.CultureInfo("es-PE"));

            InteresLabel.Text = $"S/ {pago.MontoInteres:N2}";
            CapitalLabel.Text = $"S/ {pago.MontoCapital:N2}";
            PendienteLabel.Text = $"S/ {pago.CapitalPendiente:N2}";

            ComprobanteLabel.Text = string.IsNullOrWhiteSpace(pago.Comprobante)
                ? "-"
                : pago.Comprobante;

            ObservacionesLabel.Text = string.IsNullOrWhiteSpace(pago.Observaciones)
                ? "Sin observaciones."
                : pago.Observaciones;

            try
            {
                var prestamo = await _prestamoService
                    .ObtenerPorIdAsync(pago.PrestamoId);

                if (prestamo is not null)
                {
                    ClienteLabel.Text = prestamo.ClienteNombre;
                    AvatarLabel.Text = prestamo.ClienteNombre.Length > 0
                        ? prestamo.ClienteNombre.Substring(0, 1).ToUpperInvariant()
                        : "?";
                    PrestamoInfoLabel.Text =
                        $"Préstamo S/ {prestamo.CapitalInicial:N2}";
                }
                else
                {
                    ClienteLabel.Text = "Préstamo";
                    PrestamoInfoLabel.Text = string.Empty;
                }
            }
            catch
            {
                ClienteLabel.Text = "Préstamo";
                PrestamoInfoLabel.Text = string.Empty;
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
