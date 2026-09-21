using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class RegistrarClientePage : ContentPage
{
    private readonly ClienteService _clienteService;
    private readonly GaranteService _garanteService;

    public RegistrarClientePage(
        ClienteService clienteService,
        GaranteService garanteService)
    {
        InitializeComponent();
        _clienteService = clienteService;
        _garanteService = garanteService;
        TipoDocumentoPicker.SelectedIndex = 0;
    }

    private async void OnGuardarClicked(
        object? sender,
        EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        ErrorLabel.Text = string.Empty;

        var tipoDocumento = TipoDocumentoPicker.SelectedItem as string ?? string.Empty;
        var numeroDocumento = NumeroDocumentoEntry.Text?.Trim() ?? string.Empty;
        var nombres = NombresEntry.Text?.Trim() ?? string.Empty;
        var apellidos = ApellidosEntry.Text?.Trim() ?? string.Empty;
        var telefono = TelefonoEntry.Text?.Trim() ?? string.Empty;
        var direccion = DireccionEntry.Text?.Trim() ?? string.Empty;
        var referencia = ReferenciaEntry.Text?.Trim();
        var observaciones = ObservacionesEditor.Text?.Trim();

        var avalNombres = AvalNombresEntry.Text?.Trim() ?? string.Empty;
        var avalApellidos = AvalApellidosEntry.Text?.Trim() ?? string.Empty;
        var avalTelefono = AvalTelefonoEntry.Text?.Trim() ?? string.Empty;
        var avalDireccion = AvalDireccionEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(numeroDocumento))
        {
            MostrarError("Ingrese el número de documento.");
            return;
        }

        if (string.IsNullOrWhiteSpace(nombres))
        {
            MostrarError("Ingrese los nombres.");
            return;
        }

        if (string.IsNullOrWhiteSpace(apellidos))
        {
            MostrarError("Ingrese los apellidos.");
            return;
        }

        if (string.IsNullOrWhiteSpace(telefono))
        {
            MostrarError("Ingrese el teléfono.");
            return;
        }

        if (string.IsNullOrWhiteSpace(direccion))
        {
            MostrarError("Ingrese la dirección.");
            return;
        }

        var conAval = !string.IsNullOrWhiteSpace(avalNombres) ||
            !string.IsNullOrWhiteSpace(avalApellidos) ||
            !string.IsNullOrWhiteSpace(avalTelefono);

        if (conAval && (string.IsNullOrWhiteSpace(avalNombres) ||
            string.IsNullOrWhiteSpace(avalApellidos) ||
            string.IsNullOrWhiteSpace(avalTelefono)))
        {
            MostrarError("Complete nombres, apellidos y teléfono del aval.");
            return;
        }

        try
        {
            GuardarButton.IsEnabled = false;
            CargandoIndicator.IsVisible = true;
            CargandoIndicator.IsRunning = true;

            var (exito, error) = await _clienteService.CrearAsync(
                tipoDocumento,
                numeroDocumento,
                nombres,
                apellidos,
                telefono,
                direccion,
                string.IsNullOrWhiteSpace(referencia) ? null : referencia,
                string.IsNullOrWhiteSpace(observaciones) ? null : observaciones);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo registrar el cliente.");
                return;
            }

            if (conAval)
            {
                var cliente = await _clienteService
                    .ObtenerPorDocumentoAsync(numeroDocumento);

                if (cliente is not null)
                {
                    var (avalOk, avalError) = await _garanteService.CrearAsync(
                        avalNombres,
                        avalApellidos,
                        avalTelefono,
                        string.IsNullOrWhiteSpace(avalDireccion) ? null : avalDireccion,
                        cliente.Id);

                    if (!avalOk)
                    {
                        MostrarError(avalError ?? "Cliente creado, pero no se pudo registrar el aval.");
                        return;
                    }
                }
            }

            await DisplayAlertAsync(
                "Cliente registrado",
                "El cliente fue registrado correctamente.",
                "OK");

            await Shell.Current.GoToAsync("..");
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
            GuardarButton.IsEnabled = true;
            CargandoIndicator.IsVisible = false;
            CargandoIndicator.IsRunning = false;
        }
    }

    private void MostrarError(string mensaje)
    {
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }
}
