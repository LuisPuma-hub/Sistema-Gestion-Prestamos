namespace SistemaPrestamos.Mobile.Views;

public partial class EnlaceReseteoPage : ContentPage
{
    private readonly string _token;

    public EnlaceReseteoPage(string email, string token, DateTime expiraEn)
    {
        InitializeComponent();

        _token = token;

        DescripcionLabel.Text =
            $"Comparte este código con {email} " +
            $"(vence {expiraEn.ToLocalTime():HH:mm}):";

        TokenLabel.Text = token;
    }

    private async void OnCopiarClicked(object? sender, EventArgs e)
    {
        await Clipboard.Default.SetTextAsync(_token);

        await DisplayAlertAsync(
            "Copiado",
            "Código copiado al portapapeles.",
            "OK");
    }

    private async void OnCerrarClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
