using Microsoft.Maui.ApplicationModel;

namespace SistemaPrestamos.Mobile.Services;

public class BiometriaService
{
    private const string ClaveFlag = "biometria_activa";

    public async Task<bool> EstaDisponibleAsync()
    {
        try
        {
#if ANDROID
            return await Task.FromResult(
                Platforms.Android.BiometricoAndroid.EstaDisponible(
                    Platform.CurrentActivity));
#else
            await Task.CompletedTask;
            return false;
#endif
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> EstaActivaAsync()
    {
        var flag = await SecureStorage.Default.GetAsync(ClaveFlag);
        return flag == "1";
    }

    public async Task ActivarAsync()
    {
        await SecureStorage.Default.SetAsync(ClaveFlag, "1");
    }

    public async Task DesactivarAsync()
    {
        SecureStorage.Default.Remove(ClaveFlag);
    }

    public async Task<(bool Ok, string? Error)> AutenticarAsync(
        string motivo)
    {
        try
        {
#if ANDROID
            var activity = Platform.CurrentActivity;

            if (activity is null)
            {
                return (false, "Abre la app e inténtalo de nuevo.");
            }

            if (!Platforms.Android.BiometricoAndroid.EstaDisponible(activity))
            {
                return (false, "Este equipo no tiene biometría disponible.");
            }

            return await Platforms.Android.BiometricoAndroid.AutenticarAsync(
                activity,
                "Sistema de Préstamos",
                motivo);
#else
            await Task.CompletedTask;
            return (false, "Biometría no disponible en esta plataforma.");
#endif
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
