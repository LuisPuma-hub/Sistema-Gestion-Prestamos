using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class PrestamosPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private List<PrestamoDto> _todos = new();

    public PrestamosPage(PrestamoService prestamoService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
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

            _todos = await _prestamoService.ObtenerTodosAsync();

            AplicarFiltros();
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo cargar la lista de préstamos. Verifique su conexión e intente nuevamente.");
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
            filtrados = filtrados.Where(p =>
                string.Equals(p.Estado, filtroEstado, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            filtrados = filtrados.Where(p =>
                p.ClienteNombre.ToLowerInvariant().Contains(texto));
        }

        var lista = filtrados.ToList();

        PrestamosCollection.ItemsSource = lista;
        ResumenLabel.Text = $"Préstamos registrados: {lista.Count}";

        if (_todos.Count == 0)
        {
            MostrarError("No hay préstamos registrados.");
        }
        else if (lista.Count == 0)
        {
            MostrarError("No se encontraron préstamos. Intenta otra búsqueda.");
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

    private async void OnPrestamoTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Guid id)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DetallePrestamoPage)}?id={id}");
        }
    }

    private async void OnNuevoPrestamoClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegistrarPrestamoPage));
    }
}
