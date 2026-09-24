using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class ClientesPage : ContentPage
{
    private readonly ClienteService _clienteService;
    private List<ClienteDto> _todos = new();
    private string _filtroEstado = "Todos";

    public ClientesPage(ClienteService clienteService)
    {
        InitializeComponent();
        _clienteService = clienteService;
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
        var filtroEstado = _filtroEstado;

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
        ResumenLabel.Text = lista.Count.ToString();

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

    private void OnFiltroChipClicked(object? sender, EventArgs e)
    {
        if (sender == ChipTodos)
        {
            _filtroEstado = "Todos";
        }
        else if (sender == ChipActivo)
        {
            _filtroEstado = "Activo";
        }
        else if (sender == ChipObservacion)
        {
            _filtroEstado = "En observación";
        }
        else if (sender == ChipMoroso)
        {
            _filtroEstado = "Moroso";
        }

        ActualizarChips();
        AplicarFiltros();
    }

    private void ActualizarChips()
    {
        PintarChip(ChipTodos, _filtroEstado == "Todos");
        PintarChip(ChipActivo, _filtroEstado == "Activo");
        PintarChip(ChipObservacion, _filtroEstado == "En observación");
        PintarChip(ChipMoroso, _filtroEstado == "Moroso");
    }

    private static void PintarChip(Button chip, bool seleccionado)
    {
        chip.BackgroundColor = seleccionado
            ? Color.FromArgb("#1D4ED8")
            : Colors.White;
        chip.TextColor = seleccionado
            ? Colors.White
            : Color.FromArgb("#6B7280");
        chip.BorderColor = seleccionado
            ? Color.FromArgb("#1D4ED8")
            : Color.FromArgb("#E5E7EB");
        chip.BorderWidth = 1;
        chip.CornerRadius = 18;
        chip.FontSize = 13;
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
