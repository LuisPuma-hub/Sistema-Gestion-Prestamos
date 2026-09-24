using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class HistorialEnviosPage : ContentPage
{
    private readonly NotificacionesService _notificacionesService;

    public HistorialEnviosPage(NotificacionesService notificacionesService)
    {
        InitializeComponent();
        _notificacionesService = notificacionesService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);

        EstadoLabel.IsVisible = false;

        try
        {
            var envios = await _notificacionesService.ObtenerEnviosAsync();

            EnviosCollection.ItemsSource = envios;

            if (envios.Count == 0)
            {
                EstadoLabel.Text = "Aún no hay envíos registrados.";
                EstadoLabel.IsVisible = true;
            }
        }
        catch (HttpRequestException)
        {
            EstadoLabel.Text = "No se pudo conectar con el servidor.";
            EstadoLabel.IsVisible = true;
        }
    }

    private async void OnVolverClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
