using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class PrestamosPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private List<PrestamoDto> _todos = new();
    private string _filtroEstado = "Todos";

    public PrestamosPage(PrestamoService prestamoService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
        ActualizarChips();
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
        var filtroEstado = _filtroEstado;

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
        ResumenLabel.Text = lista.Count.ToString();

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

    private void OnFiltroChipClicked(object? sender, EventArgs e)
    {
        if (sender == ChipTodos)
        {
            _filtroEstado = "Todos";
        }
        else if (sender == ChipPendiente)
        {
            _filtroEstado = "Pendiente";
        }
        else if (sender == ChipActivo)
        {
            _filtroEstado = "Activo";
        }
        else if (sender == ChipCancelado)
        {
            _filtroEstado = "Cancelado";
        }

        ActualizarChips();
        AplicarFiltros();
    }

    private void ActualizarChips()
    {
        PintarChip(ChipTodos, _filtroEstado == "Todos");
        PintarChip(ChipPendiente, _filtroEstado == "Pendiente");
        PintarChip(ChipActivo, _filtroEstado == "Activo");
        PintarChip(ChipCancelado, _filtroEstado == "Cancelado");
    }

    private static void PintarChip(Button chip, bool seleccionado)
    {
        chip.BackgroundColor = seleccionado
            ? Color.FromArgb("#512BD4")
            : Colors.White;
        chip.TextColor = seleccionado
            ? Colors.White
            : Color.FromArgb("#6B7280");
        chip.BorderColor = seleccionado
            ? Color.FromArgb("#512BD4")
            : Color.FromArgb("#E5E7EB");
        chip.BorderWidth = 1;
        chip.CornerRadius = 18;
        chip.FontSize = 13;
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
