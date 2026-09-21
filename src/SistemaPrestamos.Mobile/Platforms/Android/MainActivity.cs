using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace SistemaPrestamos.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public const string CanalNotificacionesId = "general";

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        CrearCanalNotificaciones();
    }

    private void CrearCanalNotificaciones()
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(26))
        {
            return;
        }

        var manager = (NotificationManager?)GetSystemService(
            Context.NotificationService);

        manager?.CreateNotificationChannel(new NotificationChannel(
            CanalNotificacionesId,
            "General",
            NotificationImportance.Default));
    }
}
