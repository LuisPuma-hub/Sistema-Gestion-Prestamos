using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class PagosPage : ContentPage
{
    private readonly PagoService _pagoService;
    private readonly PrestamoService _prestamoService;
    private readonly ClienteService _clienteService;
    private List<PagoDto> _todos = new();
    private Dictionary<Guid, string> _telefonos = new();

    public PagosPage(
        PagoService pagoService,
        PrestamoService prestamoService,
        ClienteService clienteService)
    {
        InitializeComponent();
        _pagoService = pagoService;
        _prestamoService = prestamoService;
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
            EstadoLabel.IsVisible = false;
            ReintentarButton.IsVisible = false;
            var pagos = await _pagoService.ObtenerTodosAsync();
            var prestamos = await _prestamoService.ObtenerTodosAsync();

            var nombres = prestamos.ToDictionary(
                p => p.Id,
                p => p.ClienteNombre);

            var duenos = prestamos.ToDictionary(
                p => p.Id,
                p => p.ClienteId);

            foreach (var pago in pagos)
            {
                pago.PrestamoNombre = nombres.TryGetValue(
                    pago.PrestamoId,
                    out var nombre)
                        ? nombre
                        : "Préstamo";
            }

            try
            {
                var clientes = await _clienteService.ObtenerTodosAsync();

                var tels = clientes.ToDictionary(
                    c => c.Id,
                    c => c.Telefono ?? string.Empty);

                _telefonos = duenos
                    .Where(kv => tels.ContainsKey(kv.Value))
                    .ToDictionary(kv => kv.Key, kv => tels[kv.Value]);
            }
            catch
            {
                _telefonos = new Dictionary<Guid, string>();
            }

            _todos = pagos
                .OrderByDescending(p => p.FechaPago)
                .ToList();

            AplicarFiltros();
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo cargar la lista de pagos. Verifique su conexión e intente nuevamente.");
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

        var filtrados = _todos.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(texto))
        {
            filtrados = filtrados.Where(p =>
                p.PrestamoNombre.ToLowerInvariant().Contains(texto) ||
                (_telefonos.TryGetValue(p.PrestamoId, out var tel) &&
                 tel.ToLowerInvariant().Contains(texto)));
        }

        var lista = filtrados.ToList();

        PagosCollection.ItemsSource = lista;
        ResumenLabel.Text = lista.Count.ToString();

        if (_todos.Count == 0)
        {
            MostrarError("No hay pagos registrados.");
        }
        else if (lista.Count == 0)
        {
            MostrarError("No se encontraron pagos. Intenta otra búsqueda.");
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

    private async void OnReintentarClicked(object? sender, EventArgs e)
    {
        await CargarAsync();
    }

    private async void OnPagoTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Guid id)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DetallePagoPage)}?id={id}");
        }
    }

    private async void OnNuevoPagoClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegistrarPagoPage));
    }
}
