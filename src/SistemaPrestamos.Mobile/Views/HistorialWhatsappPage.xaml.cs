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

        _ = Animaciones.EntradaAsync(Content);
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
                : cliente.NombreCompleto;

            AvatarLabel.Text = cliente is null || cliente.NombreCompleto.Length == 0
                ? "?"
                : cliente.NombreCompleto.Substring(0, 1).ToUpperInvariant();

            ClienteTelefonoLabel.Text = cliente?.Telefono ?? string.Empty;

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

    private static readonly Dictionary<string, string> _plantillas = new()
    {
        ["Recordatorio de pago"] = "recordatorio_pago_v2",
        ["Aviso de mora"] = "aviso_mora",
        ["Confirmación de pago"] = "confirmacion_pago",
        ["Préstamo aprobado"] = "prestamo_aprobado"
    };

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnEnviarMensajeClicked(object? sender, EventArgs e)
    {
        if (!Guid.TryParse(ClienteIdTexto, out var clienteId))
        {
            MostrarError("Identificador de cliente no válido.");
            return;
        }

        var opcion = await DisplayActionSheetAsync(
            "Plantilla a enviar",
            "Cancelar",
            null,
            _plantillas.Keys.ToArray());

        if (string.IsNullOrWhiteSpace(opcion) ||
            !_plantillas.TryGetValue(opcion, out var plantilla))
        {
            return;
        }

        Guid? prestamoId = Guid.TryParse(PrestamoIdTexto, out var p)
            ? p
            : null;

        if ((plantilla is "recordatorio_pago_v2" or "aviso_mora"
                or "prestamo_aprobado") && prestamoId is null)
        {
            await DisplayAlertAsync(
                "Falta el préstamo",
                "Esta plantilla necesita un préstamo. Ábrela desde el detalle del préstamo.",
                "OK");
            return;
        }

        var confirmar = await DisplayAlertAsync(
            opcion,
            $"¿Enviar '{opcion}' por WhatsApp?",
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
                .EnviarPlantillaAsync(clienteId, prestamoId, plantilla);

            if (!exito)
            {
                await DisplayAlertAsync(
                    "No se pudo enviar",
                    TextoError.Limpiar(error, "Inténtalo de nuevo."),
                    "OK");
                return;
            }

            await DisplayAlertAsync(
                "Mensaje enviado",
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
        ErrorLabel.Text = TextoError.Limpiar(mensaje);
        ErrorLabel.IsVisible = true;
    }
}
