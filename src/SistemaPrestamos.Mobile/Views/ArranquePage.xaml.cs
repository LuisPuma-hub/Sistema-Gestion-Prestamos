using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class ArranquePage : ContentPage
{
    private bool _decidido;

    public ArranquePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_decidido)
        {
            return;
        }

        _decidido = true;
        await DecidirAsync();
    }

    private async Task DecidirAsync()
    {
        try
        {
            var refresh = await SecureStorage.Default
                .GetAsync("refresh_token");

            if (string.IsNullOrWhiteSpace(refresh))
            {
                IrLogin();
                return;
            }

            var servicios = Handler?.MauiContext?.Services;

            if (servicios is not IServiceProvider proveedor)
            {
                IrLogin();
                return;
            }

            var conHuella = await proveedor
                .GetRequiredService<BiometriaService>()
                .EstaActivaAsync();

            var ventana = Application.Current?.Windows.FirstOrDefault();

            if (ventana is null)
            {
                return;
            }

            if (conHuella)
            {
                ventana.Page = proveedor
                    .GetRequiredService<BloqueoPage>();
                return;
            }

            var push = proveedor
                .GetRequiredService<NotificacionPushService>();

            _ = push.InicializarAsync();

            ventana.Page = new AppShell();
        }
        catch
        {
            IrLogin();
        }
    }

    private void IrLogin()
    {
        var servicios = Handler?.MauiContext?.Services;

        if (servicios is not IServiceProvider proveedor)
        {
            return;
        }

        var ventana = Application.Current?.Windows.FirstOrDefault();

        if (ventana is not null)
        {
            ventana.Page = proveedor.GetRequiredService<LoginPage>();
        }
    }
}
