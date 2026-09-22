using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class RegistrarUsuarioPage : ContentPage
{
    private readonly UsuarioService _usuarioService;

    public RegistrarUsuarioPage(UsuarioService usuarioService)
    {
        InitializeComponent();
        _usuarioService = usuarioService;
        RolPicker.SelectedIndex = 0;
    }

    private async void OnGuardarClicked(object? sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        var nombres = NombresEntry.Text?.Trim() ?? string.Empty;
        var apellidos = ApellidosEntry.Text?.Trim() ?? string.Empty;
        var email = EmailEntry.Text?.Trim() ?? string.Empty;
        var password = PasswordEntry.Text ?? string.Empty;
        var rol = RolPicker.SelectedItem as string ?? "Cobrador";

        if (string.IsNullOrWhiteSpace(nombres) ||
            string.IsNullOrWhiteSpace(apellidos) ||
            string.IsNullOrWhiteSpace(email))
        {
            MostrarError("Complete nombres, apellidos y correo.");
            return;
        }

        if (password.Length < 8)
        {
            MostrarError("La contraseña debe tener al menos 8 caracteres.");
            return;
        }

        try
        {
            GuardarButton.IsEnabled = false;
            MostrarCargando(true);

            var (exito, error) = await _usuarioService.CrearAsync(
                nombres,
                apellidos,
                email,
                password,
                rol);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo crear el usuario.");
                return;
            }

            await DisplayAlertAsync(
                "Usuario creado",
                "El usuario fue registrado correctamente.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo conectar con el servidor.");
        }
        catch (Exception ex)
        {
            MostrarError($"Ocurrió un error: {ex.Message}");
        }
        finally
        {
            GuardarButton.IsEnabled = true;
            MostrarCargando(false);
        }
    }

    private void MostrarCargando(bool cargando)
    {
        CargandoIndicator.IsVisible = cargando;
        CargandoIndicator.IsRunning = cargando;
    }

    private void MostrarError(string mensaje)
    {
        ErrorLabel.Text = mensaje;
        ErrorLabel.IsVisible = true;
    }
}
