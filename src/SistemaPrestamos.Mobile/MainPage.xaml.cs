using SistemaPrestamos.Mobile.Services;
using SistemaPrestamos.Mobile.Views;

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

        _ = Animaciones.EntradaAsync(Content);

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

            FechaLabel.Text = DateTime.Today.ToString(
                "dddd, dd 'de' MMMM",
                new System.Globalization.CultureInfo("es-PE"));

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var primero = nombre.Trim().Split(' ')[0];
                SaludoLabel.Text = $"Hola, {primero}";
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
                .Where(p => !p.Anulado && p.FechaPago.ToLocalTime().Date == hoy)
                .ToList();

            PagosHoyValorLabel.Text =
                $"S/ {cobradosHoy.Sum(p => p.Monto):N2}";

            var morasTask = activos.Select(p =>
                _morosidadService.EvaluarAsync(p.Id));

            var moras = await Task.WhenAll(morasTask);

            // Los ya Moroso no se evalúan arriba (filtro Activo),
            // pero sí cuentan.
            var morososDirectos = prestamos.Count(p =>
                string.Equals(
                    p.Estado, "Moroso",
                    StringComparison.OrdinalIgnoreCase));

            MoraValorLabel.Text = (morososDirectos + moras
                .Count(m => m is not null && m.Activa))
                .ToString();

            var nombresPrestamo = prestamos.ToDictionary(
                p => p.Id,
                p => p.ClienteNombre);

            // Actividad reciente: últimos 4 eventos entre pagos
            // (por FechaRegistro) y aprobaciones.
            var eventos = new List<(DateTime Fecha, ActividadItem Item)>();

            foreach (var pago in pagos
                         .Where(p => !p.Anulado)
                         .OrderByDescending(p => p.FechaRegistro)
                         .Take(4))
            {
                var nombreCli = nombresPrestamo.TryGetValue(
                    pago.PrestamoId,
                    out var n) ? n : "Cliente";

                eventos.Add((pago.FechaRegistro, new ActividadItem
                {
                    Inicial = InicialDe(nombreCli),
                    Nombre = nombreCli,
                    Detalle = "Pago recibido",
                    ColorDetalle = "#16A34A",
                    Monto = $"S/ {pago.Monto:N2}",
                    Tiempo = TextoHace(pago.FechaRegistro)
                }));
            }

            foreach (var prestamo in prestamos
                         .Where(p => p.FechaAprobacion.HasValue)
                         .OrderByDescending(p => p.FechaAprobacion!.Value)
                         .Take(4))
            {
                eventos.Add((prestamo.FechaAprobacion!.Value, new ActividadItem
                {
                    Inicial = InicialDe(prestamo.ClienteNombre),
                    Nombre = prestamo.ClienteNombre,
                    Detalle = "Préstamo aprobado",
                    ColorDetalle = "#15307A",
                    Monto = $"S/ {prestamo.CapitalInicial:N2}",
                    Tiempo = TextoHace(prestamo.FechaAprobacion.Value)
                }));
            }

            ActividadCollection.ItemsSource = eventos
                .OrderByDescending(x => x.Fecha)
                .Take(4)
                .Select(x => x.Item)
                .ToList();
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

    private sealed class ActividadItem
    {
        public string Inicial { get; set; } = "?";
        public string Nombre { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public string ColorDetalle { get; set; } = "#16A34A";
        public string Monto { get; set; } = string.Empty;
        public string Tiempo { get; set; } = string.Empty;
    }

    private static string InicialDe(string nombre)
    {
        return nombre.Length > 0
            ? nombre.Substring(0, 1).ToUpperInvariant()
            : "?";
    }

    private static string TextoHace(DateTime fecha)
    {
        var hace = DateTime.Now - fecha.ToLocalTime();

        if (hace.TotalMinutes < 1)
            return "ahora mismo";

        if (hace.TotalHours < 1)
            return $"hace {(int)hace.TotalMinutes} min";

        if (hace.TotalDays < 1)
            return $"hace {(int)hace.TotalHours} h";

        return $"hace {(int)hace.TotalDays} días";
    }

    private async void OnRefrescarClicked(object? sender, TappedEventArgs e)
    {
        await CargarAsync();
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
