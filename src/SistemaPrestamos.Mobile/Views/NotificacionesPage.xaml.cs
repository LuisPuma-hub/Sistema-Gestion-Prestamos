using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class NotificacionesPage : ContentPage
{
    private readonly NotificacionesService _notificacionesService;

    public NotificacionesPage(NotificacionesService notificacionesService)
    {
        InitializeComponent();
        _notificacionesService = notificacionesService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);

        await CargarAsync();
    }

    private async Task CargarAsync()
    {
        EstadoLabel.IsVisible = false;

        try
        {
            var reglas = await _notificacionesService.ObtenerReglasAsync();

            ReglasCollection.ItemsSource = reglas;

            if (reglas.Count == 0)
            {
                EstadoLabel.Text = "Sin reglas. Crea la primera con +.";
                EstadoLabel.IsVisible = true;
            }

            try
            {
                var diagnostico = await _notificacionesService
                    .ObtenerDiagnosticoAsync();

                ServidorLabel.Text = diagnostico is null
                    ? "Servidor: sin conexión."
                    : $"Servidor Lima {diagnostico.HoraLima} • " +
                      $"job {(diagnostico.UltimoTickUtc ?? "sin ticks")} • " +
                      $"{diagnostico.ReglasActivas} activas.";
            }
            catch
            {
                ServidorLabel.Text = "Servidor: sin conexión.";
            }
        }
        catch (HttpRequestException)
        {
            EstadoLabel.Text = "No se pudo conectar con el servidor.";
            EstadoLabel.IsVisible = true;
        }
    }

    private async void OnReglaTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Guid id)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(EditarReglaPage)}?reglaId={id}");
        }
    }

    private async void OnActivaToggled(object? sender, ToggledEventArgs e)
    {
        if (sender is not Switch sw ||
            sw.BindingContext is not ReglaDto regla)
        {
            return;
        }

        var (exito, error) = await _notificacionesService.CambiarEstadoAsync(
            regla.Id,
            e.Value);

        if (!exito)
        {
            await DisplayAlertAsync(
                "Aviso",
                string.IsNullOrWhiteSpace(error)
                    ? "No se pudo cambiar el estado."
                    : error,
                "OK");

            await CargarAsync();
        }
    }

    private async void OnNuevaReglaClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EditarReglaPage));
    }

    private async void OnHistorialClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HistorialEnviosPage));
    }

    private async void OnVolverClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
