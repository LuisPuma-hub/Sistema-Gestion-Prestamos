using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class MorosidadPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private readonly MorosidadService _morosidadService;
    private List<MorosidadDto> _todos = new();
    private bool _soloMorosos = true;

    public MorosidadPage(
        PrestamoService prestamoService,
        MorosidadService morosidadService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
        _morosidadService = morosidadService;
        ActualizarChips();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
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

            var activos = prestamos
                .Where(p => string.Equals(p.Estado, "Activo", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var tareas = activos.Select(async p =>
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
        var lista = _soloMorosos
            ? _todos.Where(m => m.Activa).ToList()
            : _todos;

        MorosidadCollection.ItemsSource = lista;

        var enMora = _todos.Count(m => m.Activa);
        ResumenLabel.Text = $"En mora: {enMora} cliente{(enMora == 1 ? "" : "s")}";
        BannerMora.IsVisible = enMora > 0;

        if (lista.Count == 0)
        {
            EstadoLabel.Text = _soloMorosos
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
        _soloMorosos = sender != ChipTodos;
        ActualizarChips();
        AplicarFiltro();
    }

    private void ActualizarChips()
    {
        PintarChip(ChipMorosos, _soloMorosos);
        PintarChip(ChipTodos, !_soloMorosos);
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
}
