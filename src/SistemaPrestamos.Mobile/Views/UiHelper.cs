using Microsoft.Maui.ApplicationModel;

namespace SistemaPrestamos.Mobile.Views;

// Barra de estado azul con iconos claros.

public static class UiHelper
{
    public static void StatusBarAzul()
    {
#if ANDROID
        try
        {
            var activity = Platform.CurrentActivity;
            var ventana = activity?.Window;

            if (ventana is null)
            {
                return;
            }

            // SetStatusBarColor obsoleto en API 35+, pero sigue
            // funcionando en 36. Sin reemplazo directo simple.
#pragma warning disable CA1422
                ventana.SetStatusBarColor(new Android.Graphics.Color(0x1D, 0x4E, 0xD8));
#pragma warning restore CA1422

            var vista = ventana.DecorView;

            if (vista is not null)
            {
                new AndroidX.Core.View.WindowInsetsControllerCompat(ventana, vista)
                    .AppearanceLightStatusBars = false;
            }
        }
        catch
        {
            // Decorativo: nunca debe romper la app.
        }
#endif
    }
}
