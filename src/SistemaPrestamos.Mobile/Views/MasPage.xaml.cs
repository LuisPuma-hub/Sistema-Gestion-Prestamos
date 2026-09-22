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

        var rol = await SecureStorage.Default.GetAsync("usuario_rol");

        UsuariosCard.IsVisible = string.Equals(
            rol,
            "Administrador",
            StringComparison.OrdinalIgnoreCase);
    }

    private async void OnPerfilClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PerfilPage));
    }

    private async void OnUsuariosClicked(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UsuariosPage));
    }
}
