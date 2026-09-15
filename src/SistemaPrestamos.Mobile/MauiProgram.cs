using Microsoft.Extensions.Logging;
using SistemaPrestamos.Mobile.Services;
using SistemaPrestamos.Mobile.Views;

namespace SistemaPrestamos.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

#if ANDROID
        builder.Services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("http://10.0.2.2:5077/")
        });
#else
        builder.Services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5077/")
        });
#endif

        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddTransient<LoginPage>();

        return builder.Build();
    }
}
