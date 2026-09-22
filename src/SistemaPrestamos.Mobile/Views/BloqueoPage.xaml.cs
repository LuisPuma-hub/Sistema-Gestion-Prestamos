using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class BloqueoPage : ContentPage
{
    private readonly BiometriaService _biometriaService;

    public BloqueoPage(BiometriaService biometriaService)
    {
        InitializeComponent();
        _biometriaService = biometriaService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);

        var nombre = await SecureStorage.Default.GetAsync("usuario_nombre");

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            NombreLabel.Text = nombre;
        }
    }

    private async void OnHuellaClicked(object? sender, EventArgs e)
    {
        await DesbloquearAsync();
    }

    private async Task DesbloquearAsync()
    {
        ErrorLabel.IsVisible = false;
        CargandoIndicator.IsVisible = true;
        CargandoIndicator.IsRunning = true;

        try
        {
            var (ok, error) = await _biometriaService.AutenticarAsync(
                "Confirma tu identidad para entrar.");

            if (!ok)
            {
                ErrorLabel.Text = error ?? "No se pudo desbloquear.";
                ErrorLabel.IsVisible = true;
                return;
            }

            Entrar();
        }
        finally
        {
            CargandoIndicator.IsVisible = false;
            CargandoIndicator.IsRunning = false;
        }
    }

    private async void OnClaveClicked(object? sender, EventArgs e)
    {
        var servicios = Handler?.MauiContext?.Services;

        if (servicios is IServiceProvider proveedor)
        {
            var auth = proveedor.GetRequiredService<AuthService>();
            var push = proveedor.GetRequiredService<NotificacionPushService>();

            var ventana = Application.Current?.Windows.FirstOrDefault();

            if (ventana is not null)
            {
                ventana.Page = new LoginPage(auth, push);
            }
        }
    }

    private void Entrar()
    {
        var ventana = Application.Current?.Windows.FirstOrDefault();

        if (ventana is not null)
        {
            ventana.Page = new AppShell();
        }
    }
}
