using SistemaPrestamos.Mobile.Models;
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
        _opcionAval = "Sin aval";
        PintarRadios();
    }

    private string _opcionAval = "Sin aval";

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarClientesAsync();
    }

    private async Task CargarClientesAsync()
    {
        try
        {
            var clientes = await _clienteService.ObtenerTodosAsync();

            AvalClientePicker.ItemsSource = clientes
                .OrderBy(c => c.Apellidos)
                .ThenBy(c => c.Nombres)
                .ToList();
        }
        catch
        {
            // Sin lista de clientes igual se puede registrar.
        }
    }

    private void OnAvalOpcionTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter as string is string opcion)
        {
            _opcionAval = opcion;
            PintarRadios();
        }

        AvalClientePicker.IsVisible = _opcionAval == "Cliente existente";
        AvalNuevoLayout.IsVisible = _opcionAval == "Nuevo aval";
    }

    private void PintarRadios()
    {
        PintarRadio(OpcionSinAval, RadioSinAval, RadioSinAvalDot, _opcionAval == "Sin aval");
        PintarRadio(OpcionExistente, RadioExistente, RadioExistenteDot, _opcionAval == "Cliente existente");
        PintarRadio(OpcionNuevo, RadioNuevo, RadioNuevoDot, _opcionAval == "Nuevo aval");
    }

    private static void PintarRadio(Border tarjeta, Border radio, BoxView punto, bool seleccionado)
    {
        tarjeta.Stroke = seleccionado
            ? Color.FromArgb("#512BD4")
            : Color.FromArgb("#E5E7EB");
        tarjeta.StrokeThickness = seleccionado ? 2 : 1;
        radio.Stroke = seleccionado
            ? Color.FromArgb("#512BD4")
            : Color.FromArgb("#D1D5DB");
        radio.StrokeThickness = 2;
        punto.IsVisible = seleccionado;
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

        var opcionAval = _opcionAval;

        string avalNombres = string.Empty;
        string avalApellidos = string.Empty;
        string avalTelefono = string.Empty;
        string? avalDireccion = null;
        var conAval = false;

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

        if (opcionAval == "Cliente existente")
        {
            if (AvalClientePicker.SelectedItem is not ClienteDto elegido)
            {
                MostrarError("Seleccione un cliente como aval.");
                return;
            }

            conAval = true;
            avalNombres = elegido.Nombres;
            avalApellidos = elegido.Apellidos;
            avalTelefono = elegido.Telefono;
            avalDireccion = string.IsNullOrWhiteSpace(elegido.Direccion)
                ? null
                : elegido.Direccion;
        }
        else if (opcionAval == "Nuevo aval")
        {
            avalNombres = AvalNombresEntry.Text?.Trim() ?? string.Empty;
            avalApellidos = AvalApellidosEntry.Text?.Trim() ?? string.Empty;
            avalTelefono = AvalTelefonoEntry.Text?.Trim() ?? string.Empty;
            var dir = AvalDireccionEntry.Text?.Trim();
            avalDireccion = string.IsNullOrWhiteSpace(dir) ? null : dir;

            conAval = !string.IsNullOrWhiteSpace(avalNombres) ||
                !string.IsNullOrWhiteSpace(avalApellidos) ||
                !string.IsNullOrWhiteSpace(avalTelefono);

            if (conAval && (string.IsNullOrWhiteSpace(avalNombres) ||
                string.IsNullOrWhiteSpace(avalApellidos) ||
                string.IsNullOrWhiteSpace(avalTelefono)))
            {
                MostrarError("Complete nombres, apellidos y teléfono del aval.");
                return;
            }
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
        ErrorLabel.Text = TextoError.Limpiar(mensaje);
        ErrorLabel.IsVisible = true;
    }
}
