using Android.App;
using Android.Hardware.Biometrics;
using Android.OS;

// Biometría del framework: requiere Android 29/30+,
// verificado en runtime con IsAndroidVersionAtLeast.
#pragma warning disable CA1416

namespace SistemaPrestamos.Mobile.Platforms.Android;

public static class BiometricoAndroid
{
    private const BiometricManagerAuthenticators Permitidos =
        BiometricManagerAuthenticators.BiometricStrong
        | BiometricManagerAuthenticators.DeviceCredential;

    public static bool EstaDisponible(Activity? activity)
    {
        if (activity is null)
        {
            return false;
        }

        try
        {
            if (!OperatingSystem.IsAndroidVersionAtLeast(29))
            {
                return false;
            }

            var manager = activity.GetSystemService(
                Java.Lang.Class.FromType(typeof(BiometricManager)))
                    as BiometricManager;

            if (manager is null)
            {
                return false;
            }

            return manager.CanAuthenticate((int)Permitidos)
                == BiometricCode.Success;
        }
        catch
        {
            return false;
        }
    }

    public static Task<(bool Ok, string? Error)> AutenticarAsync(
        Activity activity,
        string titulo,
        string motivo)
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(28))
        {
            return Task.FromResult<(bool Ok, string? Error)>(
                (false, "Biometría no disponible en este equipo."));
        }

        var tcs = new TaskCompletionSource<(bool Ok, string? Error)>();

        try
        {
            var builder = new BiometricPrompt.Builder(activity)
                .SetTitle(titulo)
                .SetSubtitle(motivo);

            if (OperatingSystem.IsAndroidVersionAtLeast(30))
            {
                builder.SetAllowedAuthenticators((int)Permitidos);
            }
            else
            {
#pragma warning disable CA1416
                builder.SetDeviceCredentialAllowed(true);
#pragma warning restore CA1416
            }

            var prompt = builder.Build();

            prompt.Authenticate(
                new CancellationSignal(),
                activity.MainExecutor!,
                new Callback(resultado => tcs.TrySetResult(resultado)));
        }
        catch (Exception ex)
        {
            tcs.TrySetResult((false, ex.Message));
        }

        return tcs.Task;
    }

    private sealed class Callback : BiometricPrompt.AuthenticationCallback
    {
        private readonly Action<(bool Ok, string? Error)> _fin;

        public Callback(Action<(bool Ok, string? Error)> fin)
        {
            _fin = fin;
        }

        public override void OnAuthenticationError(
            BiometricErrorCode errorCode,
            Java.Lang.ICharSequence? errString)
        {
            _fin((false, errString?.ToString() ?? $"Error {(int)errorCode}"));
        }

        public override void OnAuthenticationSucceeded(
            BiometricPrompt.AuthenticationResult? result)
        {
            _fin((true, null));
        }

        public override void OnAuthenticationFailed()
        {
            // Huella no reconocida: el sistema reintenta solo.
        }
    }
}
