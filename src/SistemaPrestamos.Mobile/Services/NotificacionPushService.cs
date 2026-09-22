using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Maui.ApplicationModel;
using Plugin.Firebase.CloudMessaging;

namespace SistemaPrestamos.Mobile.Services;

public class NotificacionPushService
{
    private readonly HttpClient _httpClient;
    private bool _suscripto;

    public NotificacionPushService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task InicializarAsync()
    {
        try
        {
            var estado = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (estado != PermissionStatus.Granted)
            {
                estado = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            if (estado != PermissionStatus.Granted)
            {
                return;
            }

            if (!_suscripto)
            {
                CrossFirebaseCloudMessaging.Current.TokenChanged +=
                    async (_, args) => await EnviarTokenAlBackendAsync(args.Token);

                CrossFirebaseCloudMessaging.Current.NotificationReceived +=
                    (_, args) => MostrarLocalSiVisible(
                        args.Notification.Title,
                        args.Notification.Body);

                _suscripto = true;
            }

            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                await EnviarTokenAlBackendAsync(token);
            }
        }
        catch
        {
            // Push no disponible (ej. sin Google Play Services):
            // la app sigue funcionando sin notificaciones.
        }
    }

    private static void MostrarLocalSiVisible(
        string? titulo,
        string? cuerpo)
    {
#if ANDROID
        if (!OperatingSystem.IsAndroidVersionAtLeast(26))
        {
            return;
        }

        var activity = Platform.CurrentActivity;

        // En 2do plano el sistema ya muestra el aviso
        // con el canal "general": no duplicar.
        if (activity is null || !activity.HasWindowFocus)
        {
            return;
        }

        var contexto = activity.ApplicationContext ?? activity;

        var intent = contexto.PackageManager
            ?.GetLaunchIntentForPackage(contexto.PackageName ?? string.Empty);

        intent?.SetFlags(
            Android.Content.ActivityFlags.ClearTop |
            Android.Content.ActivityFlags.SingleTop);

        var pending = Android.App.PendingIntent.GetActivity(
            contexto,
            0,
            intent,
            Android.App.PendingIntentFlags.Immutable |
            Android.App.PendingIntentFlags.UpdateCurrent);

        var aviso = new Android.App.Notification.Builder(contexto, "general")
            .SetContentTitle(titulo ?? "Préstamos")
            .SetContentText(cuerpo ?? string.Empty)
            .SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon)
            .SetContentIntent(pending)
            .SetAutoCancel(true)
            .Build();

        var manager = (Android.App.NotificationManager?)contexto.GetSystemService(
            Android.Content.Context.NotificationService);

        manager?.Notify(1001, aviso);
#endif
    }

    public async Task EnviarTokenAlBackendAsync(string? token)
    {        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            await SecureStorage.Default.SetAsync("fcm_token", token);

            var auth = await SecureStorage.Default.GetAsync("auth_token");

            if (string.IsNullOrWhiteSpace(auth))
            {
                return;
            }

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "api/Dispositivos");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", auth);

            request.Content = JsonContent.Create(new
            {
                token,
                plataforma = "android"
            });

            await _httpClient.SendAsync(request);
        }
        catch
        {
            // No bloquea el uso de la app.
        }
    }

    public async Task DarDeBajaAsync()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync("fcm_token");
            var auth = await SecureStorage.Default.GetAsync("auth_token");

            if (string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(auth))
            {
                return;
            }

            using var request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"api/Dispositivos/por-token?token={Uri.EscapeDataString(token)}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", auth);

            await _httpClient.SendAsync(request);
        }
        catch
        {
            // Mejor esfuerzo: el logout local continúa igual.
        }
        finally
        {
            SecureStorage.Default.Remove("fcm_token");
        }
    }
}
