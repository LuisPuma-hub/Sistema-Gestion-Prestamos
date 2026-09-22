using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.LifecycleEvents;
using SistemaPrestamos.Mobile.Services;
using SistemaPrestamos.Mobile.Views;

namespace SistemaPrestamos.Mobile;

public static class MauiProgram
{
    // Emulador Android. Para celular físico por WiFi:
    // "http://192.168.18.25:5077/".
    private const string ApiBaseUrl = "http://10.0.2.2:5077/";

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

        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android => android.OnCreate((activity, _) =>
                Plugin.Firebase.Core.Platforms.Android.CrossFirebase.Initialize(
                    activity,
                    () => Platform.CurrentActivity ?? activity)));
#endif
        });

#if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
            "SinSubrayado",
            (handler, _) => handler.PlatformView.Background = null);
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

#if ANDROID
        builder.Services.AddSingleton(sp =>
            CrearHttpClient(ApiBaseUrl));
#else
        builder.Services.AddSingleton(sp =>
            CrearHttpClient("http://localhost:5077/"));
#endif

        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<BiometriaService>();
        builder.Services.AddSingleton<NotificacionPushService>();
        builder.Services.AddSingleton<UsuarioService>();
        builder.Services.AddSingleton<ClienteService>();
        builder.Services.AddSingleton<GaranteService>();
        builder.Services.AddSingleton<PrestamoService>();
        builder.Services.AddSingleton<PagoService>();
        builder.Services.AddSingleton<MorosidadService>();
        builder.Services.AddSingleton<WhatsappMobileService>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ClientesPage>();
        builder.Services.AddTransient<RegistrarClientePage>();
        builder.Services.AddTransient<DetalleClientePage>();
        builder.Services.AddTransient<PrestamosPage>();
        builder.Services.AddTransient<RegistrarPrestamoPage>();
        builder.Services.AddTransient<DetallePrestamoPage>();
        builder.Services.AddTransient<PagosPage>();
        builder.Services.AddTransient<RegistrarPagoPage>();
        builder.Services.AddTransient<DetallePagoPage>();
        builder.Services.AddTransient<MorosidadPage>();
        builder.Services.AddTransient<DetalleMorosidadPage>();
        builder.Services.AddTransient<HistorialWhatsappPage>();
        builder.Services.AddTransient<PerfilPage>();
        builder.Services.AddTransient<MasPage>();
        builder.Services.AddTransient<UsuariosPage>();
        builder.Services.AddTransient<RegistrarUsuarioPage>();
        builder.Services.AddTransient<BloqueoPage>();
        builder.Services.AddTransient<ArranquePage>();

        return builder.Build();
    }

    private static HttpClient CrearHttpClient(string baseUrl)
    {
        var handler = new AuthRefreshHandler(new Uri(baseUrl))
        {
            InnerHandler = new HttpClientHandler()
        };

        return new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }
}
