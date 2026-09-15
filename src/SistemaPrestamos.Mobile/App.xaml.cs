using Microsoft.Extensions.DependencyInjection;
using SistemaPrestamos.Mobile.Services;
using SistemaPrestamos.Mobile.Views;

namespace SistemaPrestamos.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(
        IActivationState? activationState)
    {
        var authService = Handler!
            .MauiContext!
            .Services
            .GetRequiredService<AuthService>();

        return new Window(
            new LoginPage(authService));
    }
}
