using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(IdTexto), "id")]
public partial class DetalleClientePage : ContentPage
{
    private readonly ClienteService _clienteService;
    private readonly GaranteService _garanteService;
    private readonly PrestamoService _prestamoService;
    private ClienteDto? _cliente;
    private GaranteDto? _garanteActual;
    private byte[]? _fotoBytes;
    private bool _editando;

    public string IdTexto { get; set; } = string.Empty;

    public DetalleClientePage(
        ClienteService clienteService,
        GaranteService garanteService,
        PrestamoService prestamoService)
    {
        InitializeComponent();
        _clienteService = clienteService;
        _garanteService = garanteService;
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

            EliminarButton.IsVisible = await EsAdminAsync();

            var garantes = await _garanteService
                .ObtenerPorClienteAsync(_cliente.Id);

            _garanteActual = garantes.FirstOrDefault();

            GaranteLabel.Text = _garanteActual is null
                ? "Sin aval registrado."
                : $"{_garanteActual.NombreCompleto} - {_garanteActual.Telefono}";

            QuitarAvalButton.IsVisible = _garanteActual is not null;

            MostrarFoto();

            if (_garanteActual is not null)
            {
                AvalNombresEntry.Text = _garanteActual.Nombres;
                AvalApellidosEntry.Text = _garanteActual.Apellidos;
                AvalTelefonoEntry.Text = _garanteActual.Telefono;
                AvalDireccionEntry.Text = _garanteActual.Direccion;
            }

            AvalOpcionPicker.SelectedIndex = -1;
            AvalClientePicker.IsVisible = false;
            AvalNuevoLayout.IsVisible = false;
            GuardarAvalButton.IsVisible = false;

            var todos = await _clienteService.ObtenerTodosAsync();

            AvalClientePicker.ItemsSource = todos
                .Where(c => c.Id != _cliente.Id)
                .OrderBy(c => c.Apellidos)
                .ThenBy(c => c.Nombres)
                .ToList();
            AvalClientePicker.SelectedIndex = -1;

            var prestamos = await _prestamoService
                .ObtenerPorClienteAsync(_cliente.Id);

            var listaPrestamos = prestamos
                .OrderByDescending(p => p.FechaInicio)
                .ToList();

            PrestamosCollection.ItemsSource = listaPrestamos;

            PrestamosResumenLabel.Text = listaPrestamos.Count == 0
                ? "Sin préstamos registrados."
                : $"Préstamos: {listaPrestamos.Count}";

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

    private void OnAvalOpcionChanged(object? sender, EventArgs e)
    {
        var opcion = AvalOpcionPicker.SelectedItem as string;

        AvalClientePicker.IsVisible = opcion == "Cliente existente";
        AvalNuevoLayout.IsVisible = opcion == "Nuevo aval";
        GuardarAvalButton.IsVisible =
            opcion == "Cliente existente" || opcion == "Nuevo aval";
    }

    private async void OnGuardarAvalClicked(object? sender, EventArgs e)
    {
        if (_cliente is null)
        {
            return;
        }

        var opcion = AvalOpcionPicker.SelectedItem as string;

        string nombres;
        string apellidos;
        string telefono;
        string? direccion = null;

        if (opcion == "Cliente existente")
        {
            if (AvalClientePicker.SelectedItem is not ClienteDto elegido)
            {
                MostrarError("Seleccione un cliente como aval.");
                return;
            }

            nombres = elegido.Nombres;
            apellidos = elegido.Apellidos;
            telefono = elegido.Telefono;
            direccion = string.IsNullOrWhiteSpace(elegido.Direccion)
                ? null
                : elegido.Direccion;
        }
        else if (opcion == "Nuevo aval")
        {
            nombres = AvalNombresEntry.Text?.Trim() ?? string.Empty;
            apellidos = AvalApellidosEntry.Text?.Trim() ?? string.Empty;
            telefono = AvalTelefonoEntry.Text?.Trim() ?? string.Empty;
            var dir = AvalDireccionEntry.Text?.Trim();
            direccion = string.IsNullOrWhiteSpace(dir) ? null : dir;

            if (string.IsNullOrWhiteSpace(nombres) ||
                string.IsNullOrWhiteSpace(apellidos) ||
                string.IsNullOrWhiteSpace(telefono))
            {
                MostrarError("Complete nombres, apellidos y teléfono del aval.");
                return;
            }
        }
        else
        {
            MostrarError("Seleccione una opción de aval.");
            return;
        }

        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            if (_garanteActual is not null)
            {
                var (okEdit, errorEdit) = await _garanteService.ActualizarAsync(
                    _garanteActual.Id,
                    nombres,
                    apellidos,
                    telefono,
                    direccion,
                    _cliente.Id);

                if (!okEdit)
                {
                    MostrarError(errorEdit ?? "No se pudo guardar el aval.");
                    return;
                }
            }
            else
            {
                var (okCrear, errorCrear) = await _garanteService.CrearAsync(
                    nombres,
                    apellidos,
                    telefono,
                    direccion,
                    _cliente.Id);

                if (!okCrear)
                {
                    MostrarError(errorCrear ?? "No se pudo guardar el aval.");
                    return;
                }
            }

            await DisplayAlertAsync(
                "Aval guardado",
                "El aval fue registrado correctamente.",
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

    private async void OnQuitarAvalClicked(object? sender, EventArgs e)
    {
        if (_cliente is null || _garanteActual is null)
        {
            return;
        }

        var confirmar = await DisplayAlertAsync(
            "Quitar aval",
            $"¿Quitar a {_garanteActual.NombreCompleto} como aval?",
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

            var (exito, error) = await _garanteService
                .EliminarAsync(_garanteActual.Id);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo quitar el aval.");
                return;
            }

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

    private async void OnPrestamoTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Guid id)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DetallePrestamoPage)}?id={id}");
        }
    }

    private async void MostrarFoto()
    {
        FotoImage.IsVisible = false;
        FotoImage.Source = null;

        if (_cliente is null ||
            string.IsNullOrWhiteSpace(_cliente.FotoReciboServicio))
        {
            FotoLabel.Text = "Sin foto registrada.";
            return;
        }

        FotoLabel.Text = "Cargando foto...";

        var bytes = await _clienteService.DescargarFotoAsync(_cliente.Id);

        if (bytes is null || bytes.Length == 0)
        {
            FotoLabel.Text = "No se pudo cargar la foto.";
            return;
        }

        FotoImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
        FotoImage.IsVisible = true;
        FotoLabel.Text = "Recibo registrado. Tócalo para ampliar.";
        _fotoBytes = bytes;
    }

    private void OnFotoTapped(object? sender, TappedEventArgs e)
    {
        if (_fotoBytes is null || _fotoBytes.Length == 0)
        {
            return;
        }

        FotoCompletaImage.Source = ImageSource.FromStream(
            () => new MemoryStream(_fotoBytes));
        FotoOverlay.IsVisible = true;
    }

    private void OnCerrarFoto(object? sender, EventArgs e)
    {
        FotoOverlay.IsVisible = false;
        FotoCompletaImage.Source = null;
    }

    private async void OnCamaraClicked(object? sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                MostrarError("Este equipo no tiene cámara disponible.");
                return;
            }

            var foto = await MediaPicker.Default.CapturePhotoAsync();

            if (foto is not null)
            {
                await SubirFotoAsync(foto);
            }
        }
        catch (Exception ex)
        {
            MostrarError($"No se pudo tomar la foto: {ex.Message}");
        }
    }

    private async void OnGaleriaClicked(object? sender, EventArgs e)
    {
        try
        {
            var fotos = await MediaPicker.Default.PickPhotosAsync();
            var foto = fotos?.FirstOrDefault();

            if (foto is not null)
            {
                await SubirFotoAsync(foto);
            }
        }
        catch (Exception ex)
        {
            MostrarError($"No se pudo elegir la foto: {ex.Message}");
        }
    }

    private async Task SubirFotoAsync(FileResult foto)
    {
        if (_cliente is null)
        {
            return;
        }

        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            await using var flujo = await foto.OpenReadAsync();

            var (exito, error) = await _clienteService.SubirFotoAsync(
                _cliente.Id,
                flujo,
                foto.FileName);

            if (!exito)
            {
                await DisplayAlertAsync(
                    "No se pudo guardar",
                    TextoError.Limpiar(error, "Inténtalo de nuevo."),
                    "OK");
                return;
            }

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

    private async void OnEliminarClicked(object? sender, EventArgs e)
    {
        if (_cliente is null)
        {
            return;
        }

        var confirmar = await DisplayAlertAsync(
            "Eliminar cliente",
            $"¿Eliminar a {_cliente.NombreCompleto}? Solo es posible si no tiene préstamos.",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        var seguro = await DisplayAlertAsync(
            "Confirmar",
            "Esta acción no se puede deshacer. ¿Continuar?",
            "Eliminar",
            "Volver");

        if (!seguro)
        {
            return;
        }

        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var (exito, error) = await _clienteService
                .EliminarAsync(_cliente.Id);

            if (!exito)
            {
                await OfrecerCascadaAsync(
                    error ?? "No se pudo eliminar el cliente.");
                return;
            }

            await DisplayAlertAsync(
                "Cliente eliminado",
                "El cliente fue eliminado correctamente.",
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
            MostrarCargando(false);
        }
    }

    private async Task OfrecerCascadaAsync(string error)
    {
        var limpio = TextoError.Limpiar(error);

        var quiereTodo = await DisplayAlertAsync(
            "No se puede eliminar",
            $"{limpio}\n\n¿Eliminar TODO incluyendo sus registros?",
            "Ver qué se borrará",
            "Cancelar");

        if (!quiereTodo || _cliente is null)
        {
            return;
        }

        var confirmar = await DisplayAlertAsync(
            "Eliminar todo",
            "Se borrarán DEFINITIVAMENTE:\n" +
            "• Sus préstamos\n" +
            "• Sus pagos\n" +
            "• Sus períodos de interés\n" +
            "• Su morosidad\n" +
            "• Sus mensajes de WhatsApp\n" +
            "• Sus avales\n" +
            "• El cliente\n\n" +
            "Esta acción no se puede deshacer. ¿Continuar?",
            "Sí, borrar todo",
            "Volver");

        if (!confirmar)
        {
            return;
        }

        try
        {
            MostrarCargando(true);

            var (exito, _, errorCascada) = await _clienteService
                .EliminarCascadaAsync(_cliente.Id);

            if (!exito)
            {
                await DisplayAlertAsync(
                    "No se pudo eliminar",
                    TextoError.Limpiar(errorCascada),
                    "OK");
                return;
            }

            await DisplayAlertAsync(
                "Todo eliminado",
                "El cliente y todos sus registros fueron eliminados.",
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
            MostrarCargando(false);
        }
    }

    private static async Task<bool> EsAdminAsync()
    {
        var rol = await SecureStorage.Default.GetAsync("usuario_rol");

        return string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase);
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
