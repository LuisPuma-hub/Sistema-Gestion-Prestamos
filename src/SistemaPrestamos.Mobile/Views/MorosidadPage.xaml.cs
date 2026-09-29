using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class MorosidadPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private readonly MorosidadService _morosidadService;
    private readonly ClienteService _clienteService;
    private List<MorosidadDto> _todos = new();
    private List<ClienteDto> _observacion = new();

    // 0 = solo morosos, 1 = todos, 2 = en observación.
    private int _modo = 0;

    public MorosidadPage(
        PrestamoService prestamoService,
        MorosidadService morosidadService,
        ClienteService clienteService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
        _morosidadService = morosidadService;
        _clienteService = clienteService;
        ActualizarChips();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);
        await EvaluarAsync();
    }

    private async void OnEvaluarClicked(object? sender, EventArgs e)
    {
        await EvaluarAsync();
    }

    private async Task EvaluarAsync()
    {
        try
        {
            MostrarCargando(true);
            EstadoLabel.IsVisible = false;
            ReintentarButton.IsVisible = false;
            EvaluarButton.IsEnabled = false;

            var prestamos = await _prestamoService.ObtenerTodosAsync();

            // Se evalúan Activo y Moroso: si solo se mirara
            // Activo, el préstamo desaparecería al pasar a
            // Moroso y ya no se podría reactivar.
            var evaluables = prestamos
                .Where(p =>
                    string.Equals(p.Estado, "Activo", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(p.Estado, "Moroso", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var tareas = evaluables.Select(async p =>
            {
                var mora = await _morosidadService.EvaluarAsync(p.Id);

                if (mora is null)
                {
                    return null;
                }

                mora.PrestamoNombre = p.ClienteNombre;
                return mora;
            });

            var resultados = await Task.WhenAll(tareas);

            _todos = resultados
                .Where(m => m is not null)
                .Cast<MorosidadDto>()
                .ToList();

            var clientes = await _clienteService.ObtenerTodosAsync();

            _observacion = clientes
                .Where(c => string.Equals(
                    c.Estado, "En observación",
                    StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Apellidos)
                .ThenBy(c => c.Nombres)
                .ToList();

            AplicarFiltro();
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo evaluar la morosidad. Verifique su conexión e intente nuevamente.");
        }
        catch (Exception ex)
        {
            MostrarError($"Ocurrió un error: {ex.Message}");
        }
        finally
        {
            MostrarCargando(false);
            EvaluarButton.IsEnabled = true;
        }
    }

    private void AplicarFiltro()
    {
        var enObservacion = _modo == 2;

        MorosidadCollection.IsVisible = !enObservacion;
        ObservacionCollection.IsVisible = enObservacion;

        if (enObservacion)
        {
            ObservacionCollection.ItemsSource = _observacion;

            ResumenLabel.Text =
                $"En observación: {_observacion.Count} cliente" +
                $"{(_observacion.Count == 1 ? "" : "s")}";
            BannerMora.IsVisible = false;

            if (_observacion.Count == 0)
            {
                EstadoLabel.Text = "No hay clientes en observación.";
                EstadoLabel.IsVisible = true;
            }
            else
            {
                EstadoLabel.IsVisible = false;
            }

            return;
        }

        var lista = _modo == 0
            ? _todos.Where(m => m.Activa).ToList()
            : _todos;

        MorosidadCollection.ItemsSource = lista;

        var enMora = _todos.Count(m => m.Activa);
        ResumenLabel.Text = $"En mora: {enMora} cliente{(enMora == 1 ? "" : "s")}";
        BannerMora.IsVisible = enMora > 0;

        if (lista.Count == 0)
        {
            EstadoLabel.Text = _modo == 0
                ? "No hay préstamos en mora."
                : "No hay préstamos activos para evaluar.";
            EstadoLabel.IsVisible = true;
        }
        else
        {
            EstadoLabel.IsVisible = false;
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

    private void OnFiltroChipClicked(object? sender, EventArgs e)
    {
        _modo = sender == ChipObservacion ? 2 : sender == ChipTodos ? 1 : 0;
        ActualizarChips();
        AplicarFiltro();
    }

    private void ActualizarChips()
    {
        PintarChip(ChipMorosos, _modo == 0);
        PintarChip(ChipTodos, _modo == 1);
        PintarChip(ChipObservacion, _modo == 2);
    }

    private static void PintarChip(Button chip, bool seleccionado)
    {
        chip.BackgroundColor = seleccionado
            ? Color.FromArgb("#15307A")
            : Colors.White;
        chip.TextColor = seleccionado
            ? Colors.White
            : Color.FromArgb("#6B7280");
        chip.BorderColor = seleccionado
            ? Color.FromArgb("#15307A")
            : Color.FromArgb("#E5E7EB");
        chip.BorderWidth = 1;
        chip.CornerRadius = 18;
        chip.FontSize = 13;
    }

    private void OnFiltroChanged(object? sender, EventArgs e)
    {
        AplicarFiltro();
    }

    private async void OnMorosidadTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Guid prestamoId)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DetalleMorosidadPage)}?prestamoId={prestamoId}");
        }
    }

    private async void OnActivarClicked(object? sender, EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Guid clienteId)
        {
            return;
        }

        var confirma = await DisplayAlertAsync(
            "Activar cliente",
            "El cliente volverá a Activo y podrá recibir préstamos. ¿Continuar?",
            "Sí, activar",
            "Cancelar");

        if (!confirma)
        {
            return;
        }

        try
        {
            MostrarCargando(true);

            var (exito, error) = await _clienteService
                .CambiarEstadoAsync(clienteId, "Activo");

            if (!exito)
            {
                MostrarError(error ?? "No se pudo activar.");
                return;
            }

            await EvaluarAsync();
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
}
