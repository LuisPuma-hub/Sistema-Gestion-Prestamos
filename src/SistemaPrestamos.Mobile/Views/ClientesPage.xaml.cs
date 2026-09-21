using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class ClientesPage : ContentPage
{
    private readonly ClienteService _clienteService;
    private List<ClienteDto> _todos = new();

    public ClientesPage(ClienteService clienteService)
    {
        InitializeComponent();
        _clienteService = clienteService;
        FiltroEstadoPicker.SelectedIndex = 0;
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
            EstadoLabel.IsVisible = false;
            ReintentarButton.IsVisible = false;

            _todos = await _clienteService.ObtenerTodosAsync();

            AplicarFiltros();
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo cargar la lista de clientes. Verifique su conexión e intente nuevamente.");
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

    private void AplicarFiltros()
    {
        var texto = BuscarBar.Text?.Trim().ToLowerInvariant() ?? string.Empty;
        var filtroEstado = FiltroEstadoPicker.SelectedItem as string ?? "Todos";

        var filtrados = _todos.AsEnumerable();

        if (filtroEstado != "Todos")
        {
            filtrados = filtrados.Where(c =>
                string.Equals(c.Estado, filtroEstado, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            filtrados = filtrados.Where(c =>
                c.Nombres.ToLowerInvariant().Contains(texto) ||
                c.Apellidos.ToLowerInvariant().Contains(texto) ||
                c.NumeroDocumento.ToLowerInvariant().Contains(texto) ||
                c.Telefono.ToLowerInvariant().Contains(texto));
        }

        var lista = filtrados.ToList();

        ClientesCollection.ItemsSource = lista;
        ResumenLabel.Text = $"Clientes registrados: {lista.Count}";

        if (_todos.Count == 0)
        {
            MostrarError("No hay clientes registrados. Comienza registrando tu primer cliente.");
        }
        else if (lista.Count == 0)
        {
            MostrarError("No se encontraron clientes. Intenta realizar otra búsqueda.");
        }
        else
        {
            EstadoLabel.IsVisible = false;
            ReintentarButton.IsVisible = false;
        }
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

    private void OnBuscarTextChanged(object? sender, TextChangedEventArgs e)
    {
        AplicarFiltros();
    }

    private void OnFiltroEstadoChanged(object? sender, EventArgs e)
    {
        AplicarFiltros();
    }

    private async void OnReintentarClicked(object? sender, EventArgs e)
    {
        await CargarAsync();
    }

    private async void OnClienteTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Guid id)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DetalleClientePage)}?id={id}");
        }
    }

    private async void OnNuevoClienteClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegistrarClientePage));
    }
}
