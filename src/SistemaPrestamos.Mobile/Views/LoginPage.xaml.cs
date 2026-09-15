using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;

    public LoginPage(AuthService authService)
    {
        InitializeComponent();
        _authService = authService;
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
            MostrarError("Ingrese su correo electrónico.");
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
        }
        catch (HttpRequestException)
        {
            MostrarError(
                "No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            MostrarError(
                $"Ocurrió un error al iniciar sesión: {ex.Message}");
        }
        finally
        {
            LoginButton.IsEnabled = true;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private void MostrarError(string mensaje)
    {
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }
}
