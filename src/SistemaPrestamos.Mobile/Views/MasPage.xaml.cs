namespace SistemaPrestamos.Mobile.Views;

public partial class MasPage : ContentPage
{
    public MasPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);

        var rol = await SecureStorage.Default.GetAsync("usuario_rol");

        var esAdmin = string.Equals(
            rol,
            "Administrador",
            StringComparison.OrdinalIgnoreCase);

        UsuariosCard.IsVisible = esAdmin;
        NotificacionesCard.IsVisible = esAdmin;
    }

    private async void OnPerfilClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PerfilPage));
    }

    private async void OnUsuariosClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UsuariosPage));
    }

    private async void OnNotificacionesClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NotificacionesPage));
    }
}
