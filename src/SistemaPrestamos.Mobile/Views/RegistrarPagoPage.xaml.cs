using System.Globalization;
using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(PrestamoIdTexto), "prestamoId")]
public partial class RegistrarPagoPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private readonly PagoService _pagoService;
    private List<DateTime> _fechasPago = new();
    private int _indiceFecha;
    private DateTime _fechaPago = DateTime.Today;

    public string PrestamoIdTexto { get; set; } = string.Empty;

    public RegistrarPagoPage(
        PrestamoService prestamoService,
        PagoService pagoService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
        _pagoService = pagoService;
        MostrarFecha();
    }

    private void MostrarFecha()
    {
        _fechaPago = _fechasPago.Count == 0
            ? DateTime.Today
            : _fechasPago[_indiceFecha];

        FechaEntry.Text = _fechaPago.ToString("dd/MM/yyyy");
    }

    private void OnDiaMenosClicked(object? sender, EventArgs e)
    {
        if (_indiceFecha > 0)
        {
            _indiceFecha--;
            MostrarFecha();
        }
    }

    private void OnDiaMasClicked(object? sender, EventArgs e)
    {
        if (_indiceFecha < _fechasPago.Count - 1)
        {
            _indiceFecha++;
            MostrarFecha();
        }
    }

    private void OnUltimoVencimientoClicked(object? sender, EventArgs e)
    {
        if (_fechasPago.Count > 0)
        {
            _indiceFecha = _fechasPago.Count - 1;
            MostrarFecha();
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarPrestamosAsync();
    }

    private async Task CargarPrestamosAsync()
    {
        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var prestamos = await _prestamoService.ObtenerTodosAsync();

            var activos = prestamos
                .Where(p => string.Equals(p.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.ClienteNombre)
                .ToList();

            PrestamoPicker.ItemsSource = activos;

            if (activos.Count == 0)
            {
                MostrarError("No hay préstamos activos para registrar pagos.");
                return;
            }

            if (Guid.TryParse(PrestamoIdTexto, out var id))
            {
                var match = activos.FirstOrDefault(p => p.Id == id);

                if (match is not null)
                {
                    PrestamoPicker.SelectedItem = match;
                    await MostrarInfoAsync(match);
                }
            }

            PrestamoPicker.SelectedIndexChanged -= OnPrestamoChanged;
            PrestamoPicker.SelectedIndexChanged += OnPrestamoChanged;
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

    private async void OnPrestamoChanged(object? sender, EventArgs e)
    {
        if (PrestamoPicker.SelectedItem is PrestamoDto prestamo)
        {
            await MostrarInfoAsync(prestamo);
        }
    }

    private async Task MostrarInfoAsync(PrestamoDto prestamo)
    {
        var semanal = prestamo.InteresSemanal;

        PrestamoInfoLabel.Text =
            $"Semanal: 5 % de S/ {prestamo.CapitalInicial:N2} = S/ {semanal:N2}";

        try
        {
            var periodos = await _prestamoService
                .ObtenerPeriodosAsync(prestamo.Id);

            var pendientes = periodos
                .Where(p => p.InteresPendiente > 0)
                .OrderBy(p => p.FechaVencimiento)
                .ThenBy(p => p.FechaInicio)
                .ToList();

            var acumulado = pendientes.Sum(p => p.InteresPendiente);

            // Se prellena la semana más antigua por pagar (FIFO),
            // no el acumulado total.
            var cuota = pendientes.FirstOrDefault()?.InteresPendiente
                ?? semanal;

            if (cuota > 0)
            {
                MontoEntry.Text = cuota.ToString(
                    "N2",
                    CultureInfo.CurrentCulture);
            }

            PrestamoInfoLabel.Text += acumulado > 0
                ? $" | Acumulado: S/ {acumulado:N2} ({pendientes.Count} sem.)"
                : " | Sin interés pendiente.";

            // Días de pago: solo vencimientos (cada 7 días),
            // sin fechas futuras.
            var vtos = periodos
                .Select(p => p.FechaVencimiento.Date)
                .Distinct()
                .Where(f => f <= DateTime.Today)
                .OrderBy(f => f)
                .ToList();

            if (vtos.Count == 0)
            {
                vtos.Add(DateTime.Today);
            }

            _fechasPago = vtos;
            _indiceFecha = vtos.Count - 1;
            MostrarFecha();
        }
        catch (HttpRequestException)
        {
            // No bloquea el registro; el usuario digita el monto.
        }
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (PrestamoPicker.SelectedItem is not PrestamoDto prestamo)
        {
            MostrarError("Seleccione un préstamo.");
            return;
        }

        if (!decimal.TryParse(
                MontoEntry.Text?.Trim(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out var monto) || monto <= 0)
        {
            MostrarError("Ingrese un monto válido mayor que cero.");
            return;
        }

        var comprobante = ComprobanteEntry.Text?.Trim();
        var observaciones = ObservacionesEditor.Text?.Trim();

        try
        {
            GuardarButton.IsEnabled = false;
            MostrarCargando(true);

            var (exito, error) = await _pagoService.RegistrarAsync(
                prestamo.Id,
                monto,
                _fechaPago,
                string.IsNullOrWhiteSpace(comprobante) ? null : comprobante,
                string.IsNullOrWhiteSpace(observaciones) ? null : observaciones);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo registrar el pago.");
                return;
            }

            await DisplayAlertAsync(
                "Pago registrado",
                "El pago fue aplicado correctamente.",
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
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }
}
