namespace SistemaPrestamos.Mobile.Views;

public static class Animaciones
{
    public static async Task EntradaAsync(VisualElement? vista)
    {
        UiHelper.StatusBarMorada();

        if (vista is null)
        {
            return;
        }

        try
        {
            vista.Opacity = 0;
            vista.TranslationY = 12;
            await Task.WhenAll(
                vista.FadeToAsync(1, 220, Easing.CubicOut),
                vista.TranslateToAsync(0, 0, 220, Easing.CubicOut));
        }
        catch
        {
            vista.Opacity = 1;
            vista.TranslationY = 0;
        }
    }

    public static async Task ReboteAsync(VisualElement? vista)
    {
        if (vista is null)
        {
            return;
        }

        try
        {
            await vista.ScaleToAsync(0.93, 80, Easing.CubicIn);
            await vista.ScaleToAsync(1, 120, Easing.CubicOut);
        }
        catch
        {
            vista.Scale = 1;
        }
    }
}
