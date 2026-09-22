using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class PerfilPage : ContentPage
{
    private readonly UsuarioService _usuarioService;
    private readonly AuthService _authService;
    private readonly BiometriaService _biometriaService;
    private bool _cargandoSwitch;

    public PerfilPage(
        UsuarioService usuarioService,
        AuthService authService,
        BiometriaService biometriaService)
    {
        InitializeComponent();
        _usuarioService = usuarioService;
        _authService = authService;
        _biometriaService = biometriaService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarAsync();
    }

    private async Task CargarAsync()
    {
        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var nombre = await SecureStorage.Default.GetAsync("usuario_nombre") ?? string.Empty;
            var email = await SecureStorage.Default.GetAsync("usuario_email") ?? string.Empty;
            var rol = await SecureStorage.Default.GetAsync("usuario_rol") ?? string.Empty;
            var idTexto = await SecureStorage.Default.GetAsync("usuario_id");

            NombreLabel.Text = nombre;
            EmailLabel.Text = email;
            RolLabel.Text = rol;

            if (Guid.TryParse(idTexto, out var id))
            {
                var usuario = await _usuarioService.ObtenerPorIdAsync(id);

                if (usuario is not null)
                {
                    NombreLabel.Text = usuario.NombreCompleto;
                    EmailLabel.Text = usuario.Email;
                    RolLabel.Text = usuario.Rol;
                }
            }

            var disponible = await _biometriaService.EstaDisponibleAsync();

            BiometriaGrid.IsVisible = disponible;

            if (disponible)
            {
                _cargandoSwitch = true;
                BiometriaSwitch.IsToggled =
                    await _biometriaService.EstaActivaAsync();
                _cargandoSwitch = false;
            }
        }
        catch (HttpRequestException)
        {
            MostrarError("Sin conexión: se muestran los datos guardados.");
        }
        catch (Exception ex)
        {
            MostrarError($"Ocurrió un error: {ex.Message}");
        }
        finally
        {
            MostrarCargando(false);
        }
    }

    private async void OnCerrarSesionClicked(object? sender, EventArgs e)
    {
        var confirmar = await DisplayAlertAsync(
            "Cerrar sesión",
            "¿Deseas cerrar tu sesión?",
            "Sí",
            "No");

        if (!confirmar)
        {
            return;
        }

        await _authService.CerrarSesionAsync();
        var ventana = Application.Current?.Windows.FirstOrDefault();

        if (ventana is not null)
        {
            var servicios = Handler?.MauiContext?.Services;

            if (servicios is IServiceProvider proveedor)
            {
                ventana.Page = proveedor.GetRequiredService<LoginPage>();
                return;
            }
        }

        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnBiometriaToggled(object? sender, ToggledEventArgs e)
    {
        if (_cargandoSwitch)
        {
            return;
        }

        if (!e.Value)
        {
            await _biometriaService.DesactivarAsync();
            return;
        }

        var (ok, error) = await _biometriaService.AutenticarAsync(
            "Confirma tu huella para activar el desbloqueo.");

        if (ok)
        {
            await _biometriaService.ActivarAsync();

            await DisplayAlertAsync(
                "Huella activada",
                "Desde ahora podrás entrar con tu huella.",
                "OK");
        }
        else
        {
            _cargandoSwitch = true;
            BiometriaSwitch.IsToggled = false;
            _cargandoSwitch = false;

            await DisplayAlertAsync(
                "No se activó",
                error ?? "Inténtalo de nuevo.",
                "OK");
        }
    }

    private async void OnCambiarClaveClicked(object? sender, EventArgs e)
    {
        var actual = await DisplayPromptAsync(
            "Cambiar contraseña",
            "Contraseña actual:",
            "Siguiente",
            "Cancelar",
            maxLength: 100,
            keyboard: Keyboard.Default);

        if (string.IsNullOrWhiteSpace(actual))
        {
            return;
        }

        var nueva = await DisplayPromptAsync(
            "Cambiar contraseña",
            "Nueva contraseña (mínimo 8 caracteres):",
            "Guardar",
            "Cancelar",
            maxLength: 100,
            keyboard: Keyboard.Default);

        if (string.IsNullOrWhiteSpace(nueva))
        {
            return;
        }

        try
        {
            CambiarClaveButton.IsEnabled = false;
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var (exito, error) = await _usuarioService.CambiarMiClaveAsync(
                actual,
                nueva.Trim());

            if (!exito)
            {
                MostrarError(error ?? "No se pudo cambiar la contraseña.");
                return;
            }

            await DisplayAlertAsync(
                "Contraseña actualizada",
                "Usa la nueva la próxima vez que entres.",
                "OK");
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
            CambiarClaveButton.IsEnabled = true;
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
        ErrorLabel.Text = TextoError.Limpiar(mensaje);
        ErrorLabel.IsVisible = true;
    }
}
