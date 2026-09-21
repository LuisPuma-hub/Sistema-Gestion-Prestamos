using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(ClienteIdTexto), "clienteId")]
[QueryProperty(nameof(PrestamoIdTexto), "prestamoId")]
public partial class HistorialWhatsappPage : ContentPage
{
    private readonly WhatsappMobileService _whatsappService;
    private readonly ClienteService _clienteService;

    public string ClienteIdTexto { get; set; } = string.Empty;
    public string PrestamoIdTexto { get; set; } = string.Empty;

    public HistorialWhatsappPage(
        WhatsappMobileService whatsappService,
        ClienteService clienteService)
    {
        InitializeComponent();
        _whatsappService = whatsappService;
        _clienteService = clienteService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAsync();
    }

    private async Task CargarAsync()
    {
        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;
            EstadoLabel.IsVisible = false;

            if (!Guid.TryParse(ClienteIdTexto, out var clienteId))
            {
                MostrarError("Identificador de cliente no válido.");
                return;
            }

            var cliente = await _clienteService
                .ObtenerPorIdAsync(clienteId);

            ClienteLabel.Text = cliente is null
                ? "Historial"
                : $"Historial - {cliente.NombreCompleto}";

            var historial = await _whatsappService
                .ObtenerHistorialAsync(clienteId);

            HistorialCollection.ItemsSource = historial
                .OrderByDescending(m => m.FechaCreacion)
                .ToList();

            if (historial.Count == 0)
            {
                EstadoLabel.Text = "Sin mensajes enviados a este cliente.";
                EstadoLabel.IsVisible = true;
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

    private async void OnRecordatorioClicked(object? sender, EventArgs e)
    {
        if (!Guid.TryParse(ClienteIdTexto, out var clienteId))
        {
            MostrarError("Identificador de cliente no válido.");
            return;
        }

        Guid? prestamoId = Guid.TryParse(PrestamoIdTexto, out var p)
            ? p
            : null;

        var confirmar = await DisplayAlertAsync(
            "Enviar recordatorio",
            "¿Enviar recordatorio de pago por WhatsApp?",
            "Sí",
            "No");

        if (!confirmar)
        {
            return;
        }

        try
        {
            RecordatorioButton.IsEnabled = false;
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var (exito, error) = await _whatsappService
                .EnviarRecordatorioAsync(clienteId, prestamoId);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo enviar el recordatorio.");
                return;
            }

            await DisplayAlertAsync(
                "Recordatorio enviado",
                "El mensaje fue enviado por WhatsApp.",
                "OK");

            await CargarAsync();
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
            RecordatorioButton.IsEnabled = true;
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
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }
}
