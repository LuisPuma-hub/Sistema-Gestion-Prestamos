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
        if (e.Parameter as string is string ruta)
        {
            await Shell.Current.GoToAsync($"//{ruta}");
        }
    }

    private void Pintar(string? actual)
    {
        PintarTab(TextoInicio, IconoInicio, actual == "MainPage");
        PintarTab(TextoClientes, IconoClientes, actual == "Clientes");
        PintarTab(TextoPrestamos, IconoPrestamos, actual == "Prestamos");
        PintarTab(TextoPagos, IconoPagos, actual == "Pagos");
        PintarTab(TextoMora, IconoMora, actual == "Morosidad");
        PintarTab(TextoMas, IconoMas, actual == "Mas");
    }

    private static void PintarTab(Label texto, Label icono, bool seleccionado)
    {
        var color = seleccionado
            ? Color.FromArgb("#512BD4")
            : Color.FromArgb("#9CA3AF");

        texto.TextColor = color;
        texto.FontAttributes = seleccionado
            ? FontAttributes.Bold
            : FontAttributes.None;
        icono.Opacity = seleccionado ? 1 : 0.6;
    }
}
