namespace SistemaPrestamos.Mobile.Views;

public partial class BarraTabs : ContentView
{
    public static readonly BindableProperty TabActualProperty =
        BindableProperty.Create(
            nameof(TabActual),
            typeof(string),
            typeof(BarraTabs),
            string.Empty,
            propertyChanged: (b, _, nuevo) =>
                ((BarraTabs)b).Pintar((string?)nuevo));

    public string TabActual
    {
        get => (string)GetValue(TabActualProperty);
        set => SetValue(TabActualProperty, value);
    }

    public BarraTabs()
    {
        InitializeComponent();
        Pintar(TabActual);
    }

    private async void OnTabTapped(object? sender, TappedEventArgs e)
    {
        if (sender is VisualElement vista)
        {
            await Animaciones.ReboteAsync(vista);
        }

        if (e.Parameter as string is string ruta)
        {
            await Shell.Current.GoToAsync($"//{ruta}");
        }
    }

    private void Pintar(string? actual)
    {
        PintarTab(TextoInicio, IconoInicio, "ic_home", actual == "MainPage");
        PintarTab(TextoClientes, IconoClientes, "ic_users", actual == "Clientes");
        PintarTab(TextoPrestamos, IconoPrestamos, "ic_card", actual == "Prestamos");
        PintarTab(TextoPagos, IconoPagos, "ic_dollar", actual == "Pagos");
        PintarTab(TextoMora, IconoMora, "ic_alert", actual == "Morosidad");
        PintarTab(TextoMas, IconoMas, "ic_more", actual == "Mas");
    }

    private static void PintarTab(Label texto, Image icono, string baseIcono, bool seleccionado)
    {
        var color = seleccionado
            ? Color.FromArgb("#15307A")
            : Color.FromArgb("#9CA3AF");

        texto.TextColor = color;
        texto.FontAttributes = seleccionado
            ? FontAttributes.Bold
            : FontAttributes.None;
        icono.Source = seleccionado ? $"{baseIcono}_p.png" : $"{baseIcono}.png";
        icono.Opacity = seleccionado ? 1 : 0.6;
    }
}
