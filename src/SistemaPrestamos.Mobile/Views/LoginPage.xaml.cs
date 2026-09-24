using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;
    private readonly NotificacionPushService _pushService;

    public LoginPage(
        AuthService authService,
        NotificacionPushService pushService)
    {
        InitializeComponent();
        _authService = authService;
        _pushService = pushService;
    }

    private async void OnLoginClicked(
        object? sender,
        EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        ErrorLabel.Text = string.Empty;

        var email = EmailEntry.Text?.Trim() ?? string.Empty;
        var password = PasswordEntry.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email))
        {
            MostrarError("Ingrese su correo electr�nico.");
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            MostrarError("Ingrese su contraseña.");
            return;
        }

        try
        {
            LoginButton.IsEnabled = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var resultado = await _authService.LoginAsync(
                email,
                password);

            if (resultado is null)
            {
                MostrarError("Correo o contraseña incorrectos.");
                return;
            }

            var ventana = Application.Current?
                .Windows
                .FirstOrDefault();

            if (ventana is not null)
            {
                ventana.Page = new AppShell();
            }

            _ = _pushService.InicializarAsync();
        }
        catch (HttpRequestException)
        {
            MostrarError(
                "No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            MostrarError(
                $"Ocurri� un error al iniciar sesi�n: {ex.Message}");
        }
        finally
        {
            LoginButton.IsEnabled = true;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private void OnOjoClicked(object? sender, TappedEventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        OjoIcon.Source = PasswordEntry.IsPassword ? "ic_eye.png" : "ic_eye_off.png";
    }

    private void OnEmailBoxTapped(object? sender, TappedEventArgs e)
    {
        EmailEntry.Focus();
    }

    private void OnClaveBoxTapped(object? sender, TappedEventArgs e)
    {
        PasswordEntry.Focus();
    }

    private void MostrarError(string mensaje)
    {
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }

    private async void OnOlvidoClicked(object? sender, TappedEventArgs e)
    {
        await DisplayAlertAsync(
            "Recuperar contraseña",
            "Contacta a tu administrador o llama al 01-234-5678 para restablecerla.",
            "OK");
    }
}
