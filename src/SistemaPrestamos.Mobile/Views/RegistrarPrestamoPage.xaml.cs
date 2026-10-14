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
    private bool _esPlazoFijo;
    private bool _frecuenciaSemanal;
    private bool _plazoEnSemanas;

    private static readonly DateTime _fechaMinima = new(2020, 1, 1);

    public RegistrarPrestamoPage(
        ClienteService clienteService,
        GaranteService garanteService,
        PrestamoService prestamoService)
    {
        InitializeComponent();
        _clienteService = clienteService;
        _garanteService = garanteService;
        _prestamoService = prestamoService;
        FechaPicker.MinimumDate = _fechaMinima;
        FechaPicker.MaximumDate = DateTime.Today;
        FechaPicker.Date = _fechaInicio;
    }

    private void OnFechaElegida(object? sender, DateChangedEventArgs e)
    {
        _fechaInicio = e.NewDate?.Date ?? _fechaInicio;
        ActualizarResumenDiaria();
    }

    private void OnModalidadClicked(object? sender, EventArgs e)
    {
        OnTipoClicked(sender, e);
    }

    private void OnTipoClicked(object? sender, EventArgs e)
    {
        _esPlazoFijo = sender == TipoPlazoFijoButton;
        PlazoLayout.IsVisible = _esPlazoFijo;
        EsquemaLayout.IsVisible = !_esPlazoFijo;

        TipoSemanalButton.BackgroundColor = Color.FromArgb(
            _esPlazoFijo ? "#F3F4F6" : "#15307A");
        TipoSemanalButton.TextColor = _esPlazoFijo
            ? Color.FromArgb("#374151")
            : Colors.White;

        TipoPlazoFijoButton.BackgroundColor = Color.FromArgb(
            _esPlazoFijo ? "#15307A" : "#F3F4F6");
        TipoPlazoFijoButton.TextColor = _esPlazoFijo
            ? Colors.White
            : Color.FromArgb("#374151");

        ActualizarResumenDiaria();
        ActualizarBoton();
    }

    private void OnFrecuenciaClicked(object? sender, EventArgs e)
    {
        _frecuenciaSemanal = sender == FrecSemanalButton;

        FrecDiariaButton.BackgroundColor = Color.FromArgb(
            _frecuenciaSemanal ? "#F3F4F6" : "#15307A");
        FrecDiariaButton.TextColor = _frecuenciaSemanal
            ? Color.FromArgb("#374151")
            : Colors.White;

        FrecSemanalButton.BackgroundColor = Color.FromArgb(
            _frecuenciaSemanal ? "#15307A" : "#F3F4F6");
        FrecSemanalButton.TextColor = _frecuenciaSemanal
            ? Colors.White
            : Color.FromArgb("#374151");

        // Con frecuencia semanal el plazo va en semanas.
        PlazoUnidadGrid.IsVisible = !_frecuenciaSemanal;

        ActualizarResumenDiaria();
        ActualizarBoton();
    }

    private void OnPlazoUnidadClicked(object? sender, EventArgs e)
    {
        _plazoEnSemanas = sender == PlazoSemanasButton;

        PlazoDiasButton.BackgroundColor = Color.FromArgb(
            _plazoEnSemanas ? "#F3F4F6" : "#15307A");
        PlazoDiasButton.TextColor = _plazoEnSemanas
            ? Color.FromArgb("#374151")
            : Colors.White;

        PlazoSemanasButton.BackgroundColor = Color.FromArgb(
            _plazoEnSemanas ? "#15307A" : "#F3F4F6");
        PlazoSemanasButton.TextColor = _plazoEnSemanas
            ? Colors.White
            : Color.FromArgb("#374151");

        ActualizarResumenDiaria();
        ActualizarBoton();
    }

    private void OnPlazoChanged(object? sender, TextChangedEventArgs e)
    {
        ActualizarResumenDiaria();
        ActualizarBoton();
    }

    private static bool EsDomingo(DateTime fecha)
    {
        return fecha.DayOfWeek == DayOfWeek.Sunday;
    }

    private static DateTime SiguienteCobro(DateTime fecha)
    {
        var dia = fecha.Date;

        while (EsDomingo(dia))
            dia = dia.AddDays(1);

        return dia;
    }

    private int? ValorPlazo()
    {
        if (!int.TryParse(
                PlazoEntry.Text?.Trim(),
                out var valor) || valor <= 0)
            return null;

        return valor;
    }

    private int? CuotasElegidas()
    {
        var valor = ValorPlazo();

        if (valor is null)
            return null;

        if (_frecuenciaSemanal)
            return valor > 52 ? null : valor;

        var n = _plazoEnSemanas ? valor.Value * 7 : valor.Value;

        return _plazoEnSemanas
            ? (valor > 52 ? null : n)
            : (valor > 365 ? null : n);
    }

    private void ActualizarResumenDiaria()
    {
        if (!_esPlazoFijo)
            return;

        var n = CuotasElegidas();

        if (n is null)
        {
            ResumenDiarioLabel.Text = _frecuenciaSemanal
                ? "Indica las semanas (máx. 52)."
                : "Indica el plazo (máx. 365 días o 52 semanas).";
            return;
        }

        if (!decimal.TryParse(
                CapitalEntry.Text?.Trim(),
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out var capital) || capital <= 0)
        {
            ResumenDiarioLabel.Text =
                "Indica el capital para ver el cálculo.";
            return;
        }

        var semanas = _frecuenciaSemanal ? n.Value : n.Value / 7m;

        var interes = Math.Round(
            capital * TasaSemanal * semanas,
            2,
            MidpointRounding.AwayFromZero);

        var total = capital + interes;
        var cuota = Math.Ceiling(total / n.Value * 10m) / 10m;

        DateTime dia;
        DateTime fin;

        if (_frecuenciaSemanal)
        {
            dia = _fechaInicio;
            for (var i = 1; i < n.Value; i++)
                dia = SiguienteCobro(dia.AddDays(7));
            fin = SiguienteCobro(dia.AddDays(7));

            ResumenDiarioLabel.Text =
                $"Total S/ {total:N2} en {n.Value} cuotas semanales " +
                $"de S/ {cuota:N2} (última ajustada). Fin: " +
                $"{fin:dd/MM/yyyy}.";
        }
        else
        {
            dia = SiguienteCobro(_fechaInicio);

            for (var i = 1; i < n.Value; i++)
                dia = SiguienteCobro(dia.AddDays(1));

            fin = SiguienteCobro(dia.AddDays(1));

            ResumenDiarioLabel.Text =
                $"Total S/ {total:N2} en {n.Value} cuotas de " +
                $"S/ {cuota:N2} (última ajustada). Fin: " +
                $"{fin:dd/MM/yyyy}. Domingos no se cobran.";
        }
    }

    private void OnEsquemaClicked(object? sender, EventArgs e)
    {
        EsquemaPicker.SelectedIndex =
            sender == EsquemaSaldoButton ? 1 : 0;

        var esSaldo = EsquemaPicker.SelectedIndex == 1;

        EsquemaFijoButton.BackgroundColor = Color.FromArgb(
            esSaldo ? "#F3F4F6" : "#15307A");
        EsquemaFijoButton.TextColor = esSaldo
            ? Color.FromArgb("#374151")
            : Colors.White;

        EsquemaSaldoButton.BackgroundColor = Color.FromArgb(
            esSaldo ? "#15307A" : "#F3F4F6");
        EsquemaSaldoButton.TextColor = esSaldo
            ? Colors.White
            : Color.FromArgb("#374151");
    }

    private void OnDiaMenosClicked(object? sender, EventArgs e)
    {
        FijarFecha(_fechaInicio.AddDays(-1));
    }

    private void OnDiaMasClicked(object? sender, EventArgs e)
    {
        FijarFecha(_fechaInicio.AddDays(1));
    }

    private void OnUltimoVencimientoClicked(object? sender, EventArgs e)
    {
        FijarFecha(DateTime.Today);
    }

    private void FijarFecha(DateTime fecha)
    {
        if (fecha < _fechaMinima)
            fecha = _fechaMinima;

        if (fecha > DateTime.Today)
            fecha = DateTime.Today;

        _fechaInicio = fecha.Date;
        FechaPicker.Date = _fechaInicio;
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

        ActualizarResumenDiaria();
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
            && capital > 0
            && (!_esPlazoFijo || CuotasElegidas() is not null);

        GuardarButton.BackgroundColor = valido
            ? Color.FromArgb("#15307A")
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
                _fechaInicio,
                EsquemaPicker.SelectedIndex == 1 ? "Saldo" : "Fijo",
                _esPlazoFijo ? "PlazoFijo" : "Semanal",
                _esPlazoFijo ? ValorPlazo() : null,
                _frecuenciaSemanal
                    ? "Semanas"
                    : (_plazoEnSemanas ? "Semanas" : "Dias"),
                _frecuenciaSemanal ? "Semanal" : "Diaria");

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
