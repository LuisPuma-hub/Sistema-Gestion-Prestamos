using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile;

public partial class MainPage : ContentPage
{
    private readonly ClienteService _clienteService;
    private readonly PrestamoService _prestamoService;
    private readonly PagoService _pagoService;
    private readonly MorosidadService _morosidadService;

    public MainPage(
        ClienteService clienteService,
        PrestamoService prestamoService,
        PagoService pagoService,
        MorosidadService morosidadService)
    {
        InitializeComponent();
        _clienteService = clienteService;
        _prestamoService = prestamoService;
        _pagoService = pagoService;
        _morosidadService = morosidadService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAsync();
    }

    private async void OnRefreshing(object? sender, EventArgs e)
    {
        await CargarAsync();
        RefreshView.IsRefreshing = false;
    }

    private async Task CargarAsync()
    {
        try
        {
            ErrorLabel.IsVisible = false;

            var nombre = await SecureStorage.Default.GetAsync("usuario_nombre");

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                SaludoLabel.Text = $"Hola, {nombre}";
            }

            var clientesTask = _clienteService.ObtenerTodosAsync();
            var prestamosTask = _prestamoService.ObtenerTodosAsync();
            var pagosTask = _pagoService.ObtenerTodosAsync();

            await Task.WhenAll(clientesTask, prestamosTask, pagosTask);

            var clientes = await clientesTask;
            var prestamos = await prestamosTask;
            var pagos = await pagosTask;

            var activos = prestamos
                .Where(p => string.Equals(p.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
                .ToList();

            ClientesValorLabel.Text = clientes.Count.ToString();
            PrestamosValorLabel.Text = activos.Count.ToString();
            CapitalValorLabel.Text =
                $"S/ {activos.Sum(p => p.CapitalPendiente):N2}";

            var hoy = DateTime.Today;
            var cobradosHoy = pagos
                .Where(p => p.FechaPago.Date == hoy)
                .ToList();

            PagosHoyValorLabel.Text =
                $"{cobradosHoy.Count} pagos - S/ {cobradosHoy.Sum(p => p.Monto):N2}";

            var morasTask = activos.Select(p =>
                _morosidadService.EvaluarAsync(p.Id));

            var moras = await Task.WhenAll(morasTask);

            MoraValorLabel.Text = moras
                .Count(m => m is not null && m.Activa)
                .ToString();
        }
        catch (HttpRequestException)
        {
            ErrorLabel.Text = "Sin conexión. Desliza para reintentar.";
            ErrorLabel.IsVisible = true;
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = $"No se pudo cargar: {ex.Message}";
            ErrorLabel.IsVisible = true;
        }
    }

    private async void OnClientesClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("//Clientes");
    }

    private async void OnPrestamosClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("//Prestamos");
    }

    private async void OnPagosClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("//Pagos");
    }

    private async void OnMorosidadClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("//Morosidad");
    }
}
