using System.Globalization;
using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class RegistrarPrestamoPage : ContentPage
{
    private const decimal TasaSemanal = 0.05m;

    private readonly ClienteService _clienteService;
    private readonly GaranteService _garanteService;
    private readonly PrestamoService _prestamoService;
    private DateTime _fechaInicio = DateTime.Today;

    public RegistrarPrestamoPage(
        ClienteService clienteService,
        GaranteService garanteService,
        PrestamoService prestamoService)
    {
        InitializeComponent();
        _clienteService = clienteService;
        _garanteService = garanteService;
        _prestamoService = prestamoService;
        MostrarFecha();
    }

    private void MostrarFecha()
    {
        FechaEntry.Text = _fechaInicio.ToString("dd/MM/yyyy");
    }

    private void OnDiaMenosClicked(object? sender, EventArgs e)
    {
        _fechaInicio = _fechaInicio.AddDays(-7);
        MostrarFecha();
    }

    private void OnDiaMasClicked(object? sender, EventArgs e)
    {
        _fechaInicio = _fechaInicio.AddDays(7);
        MostrarFecha();
    }

    private void OnUltimoVencimientoClicked(object? sender, EventArgs e)
    {
        _fechaInicio = DateTime.Today;
        MostrarFecha();
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);
        await CargarClientesAsync();
    }

    private async Task CargarClientesAsync()
    {
        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var clientes = await _clienteService.ObtenerTodosAsync();

            ClientePicker.ItemsSource = clientes
                .Where(c => string.Equals(c.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Apellidos)
                .ThenBy(c => c.Nombres)
                .ToList();

            if (ClientePicker.ItemsSource.Count == 0)
            {
                MostrarError("No hay clientes activos para registrar préstamos.");
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

    private async void OnClienteChanged(object? sender, EventArgs e)
    {
        if (ClientePicker.SelectedItem is not ClienteDto cliente)
        {
            ClienteValidCard.IsVisible = false;
            return;
        }

        ClienteValidCard.IsVisible = true;
        ClienteValidNombre.Text = cliente.NombreCompleto;
        ClienteValidInicial.Text = cliente.Inicial;
        ActualizarBoton();

        ClienteValidDetalle.Text =
            $"{cliente.DocumentoCompleto} - {cliente.Telefono}";

        try
        {
            var garantes = await _garanteService
                .ObtenerPorClienteAsync(cliente.Id);

            GarantePicker.ItemsSource = garantes;
            GarantePicker.SelectedIndex = -1;
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudieron cargar los avales del cliente.");
        }
    }

    private void OnCapitalChanged(object? sender, TextChangedEventArgs e)
    {
        if (decimal.TryParse(
                e.NewTextValue,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out var capital) && capital > 0)
        {
            ResumenInteresLabel.Text =
                $"Interés semanal (5 %): S/ {capital * TasaSemanal:N2}";
        }
        else
        {
            ResumenInteresLabel.Text = "Interés semanal (5 %): S/ 0.00";
        }

        ActualizarBoton();
    }

    private void ActualizarBoton()
    {
        var valido = ClientePicker.SelectedItem is ClienteDto
            && decimal.TryParse(
                CapitalEntry.Text?.Trim(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out var capital)
            && capital > 0;

        GuardarButton.BackgroundColor = valido
            ? Color.FromArgb("#1D4ED8")
            : Color.FromArgb("#BFDBFE");
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (ClientePicker.SelectedItem is not ClienteDto cliente)
        {
            MostrarError("Seleccione un cliente.");
            return;
        }

        if (!decimal.TryParse(
                CapitalEntry.Text?.Trim(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out var capital) || capital <= 0)
        {
            MostrarError("Ingrese un capital válido mayor que cero.");
            return;
        }

        Guid? garanteId = null;

        if (GarantePicker.SelectedItem is GaranteDto garante)
        {
            garanteId = garante.Id;
        }

        try
        {
            GuardarButton.IsEnabled = false;
            MostrarCargando(true);

            var (exito, error) = await _prestamoService.CrearAsync(
                cliente.Id,
                garanteId,
                capital,
                _fechaInicio);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo registrar el préstamo.");
                return;
            }

            await DisplayAlertAsync(
                "Préstamo registrado",
                "El préstamo quedó en estado Pendiente hasta su aprobación.",
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
