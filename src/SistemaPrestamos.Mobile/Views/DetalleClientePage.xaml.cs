using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(IdTexto), "id")]
public partial class DetalleClientePage : ContentPage
{
    private readonly ClienteService _clienteService;
    private readonly GaranteService _garanteService;
    private ClienteDto? _cliente;
    private bool _editando;

    public string IdTexto { get; set; } = string.Empty;

    public DetalleClientePage(
        ClienteService clienteService,
        GaranteService garanteService)
    {
        InitializeComponent();
        _clienteService = clienteService;
        _garanteService = garanteService;
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
            MostrarError("Identificador de cliente no válido.");
        }
    }

    private async Task CargarAsync(Guid id)
    {
        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            _cliente = await _clienteService.ObtenerPorIdAsync(id);

            if (_cliente is null)
            {
                MostrarError("Cliente no encontrado.");
                return;
            }

            NombreLabel.Text = _cliente.NombreCompleto;
            EstadoLabel.Text = $"Estado: {_cliente.Estado}";

            TipoDocumentoPicker.SelectedItem = _cliente.TipoDocumento;
            NumeroDocumentoEntry.Text = _cliente.NumeroDocumento;
            NombresEntry.Text = _cliente.Nombres;
            ApellidosEntry.Text = _cliente.Apellidos;
            TelefonoEntry.Text = _cliente.Telefono;
            DireccionEntry.Text = _cliente.Direccion;
            ReferenciaEntry.Text = _cliente.ReferenciaDireccion;
            ObservacionesEditor.Text = _cliente.Observaciones;
            FechaLabel.Text = $"Registrado: {_cliente.FechaRegistro:dd/MM/yyyy}";

            EstadoPicker.SelectedItem = _cliente.Estado;

            var garantes = await _garanteService
                .ObtenerPorClienteAsync(_cliente.Id);

            var primero = garantes.FirstOrDefault();

            GaranteLabel.Text = primero is null
                ? "Sin aval registrado."
                : $"{primero.NombreCompleto} - {primero.Telefono}";

            SetEdicion(false);
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

    private void SetEdicion(bool editando)
    {
        _editando = editando;

        TipoDocumentoPicker.IsEnabled = editando;
        NumeroDocumentoEntry.IsEnabled = editando;
        NombresEntry.IsEnabled = editando;
        ApellidosEntry.IsEnabled = editando;
        TelefonoEntry.IsEnabled = editando;
        DireccionEntry.IsEnabled = editando;
        ReferenciaEntry.IsEnabled = editando;
        ObservacionesEditor.IsEnabled = editando;

        EditarButton.IsVisible = !editando;
        GuardarButton.IsVisible = editando;
    }

    private void OnEditarClicked(object? sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        SetEdicion(true);
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        if (_cliente is null)
        {
            return;
        }

        ErrorLabel.IsVisible = false;

        var tipoDocumento = TipoDocumentoPicker.SelectedItem as string ?? string.Empty;
        var numeroDocumento = NumeroDocumentoEntry.Text?.Trim() ?? string.Empty;
        var nombres = NombresEntry.Text?.Trim() ?? string.Empty;
        var apellidos = ApellidosEntry.Text?.Trim() ?? string.Empty;
        var telefono = TelefonoEntry.Text?.Trim() ?? string.Empty;
        var direccion = DireccionEntry.Text?.Trim() ?? string.Empty;
        var referencia = ReferenciaEntry.Text?.Trim();
        var observaciones = ObservacionesEditor.Text?.Trim();

        if (string.IsNullOrWhiteSpace(numeroDocumento) ||
            string.IsNullOrWhiteSpace(nombres) ||
            string.IsNullOrWhiteSpace(apellidos) ||
            string.IsNullOrWhiteSpace(telefono) ||
            string.IsNullOrWhiteSpace(direccion))
        {
            MostrarError("Complete los datos obligatorios.");
            return;
        }

        try
        {
            MostrarCargando(true);

            var (exito, error) = await _clienteService.ActualizarAsync(
                _cliente.Id,
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
                MostrarError(error ?? "No se pudo actualizar el cliente.");
                return;
            }

            await DisplayAlertAsync(
                "Cliente actualizado",
                "Los datos fueron guardados correctamente.",
                "OK");

            await CargarAsync(_cliente.Id);
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

    private async void OnCambiarEstadoClicked(object? sender, EventArgs e)    {
        if (_cliente is null)
        {
            return;
        }

        var estado = EstadoPicker.SelectedItem as string;

        if (string.IsNullOrWhiteSpace(estado))
        {
            MostrarError("Seleccione un estado.");
            return;
        }

        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var (exito, error) = await _clienteService.CambiarEstadoAsync(
                _cliente.Id,
                estado);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo cambiar el estado.");
                return;
            }

            await DisplayAlertAsync(
                "Estado actualizado",
                $"El cliente ahora está: {estado}.",
                "OK");

            await CargarAsync(_cliente.Id);
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

    private async void OnWhatsappClicked(object? sender, EventArgs e)
    {
        if (_cliente is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"{nameof(HistorialWhatsappPage)}?clienteId={_cliente.Id}");
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
