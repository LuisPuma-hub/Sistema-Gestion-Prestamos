using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(IdTexto), "id")]
public partial class DetallePagoPage : ContentPage
{
    private readonly PagoService _pagoService;

    public string IdTexto { get; set; } = string.Empty;

    public DetallePagoPage(PagoService pagoService)
    {
        InitializeComponent();
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
            FechaLabel.Text = $"Pagado: {pago.FechaPago:dd/MM/yyyy}";

            InteresLabel.Text = $"A interés: S/ {pago.MontoInteres:N2}";
            CapitalLabel.Text = $"A capital: S/ {pago.MontoCapital:N2}";
            PendienteLabel.Text = $"Capital pendiente restante: S/ {pago.CapitalPendiente:N2}";

            ComprobanteLabel.Text = string.IsNullOrWhiteSpace(pago.Comprobante)
                ? "Comprobante: -"
                : $"Comprobante: {pago.Comprobante}";

            ObservacionesLabel.Text = string.IsNullOrWhiteSpace(pago.Observaciones)
                ? "Sin observaciones."
                : pago.Observaciones;
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
