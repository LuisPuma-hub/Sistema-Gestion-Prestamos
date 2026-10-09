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

        _ = Animaciones.EntradaAsync(Content);
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
                AvatarLabel.Text = usuario.NombreCompleto.Length > 0
                    ? usuario.NombreCompleto.Substring(0, 1).ToUpperInvariant()
                    : "?";
                EmailLabel.Text = usuario.Email;
                EmailDetalleLabel.Text = usuario.Email;
                RolLabel.Text = usuario.Rol;
                RolDetalleLabel.Text = usuario.Rol;
            }
            else
            {
                AvatarLabel.Text = nombre.Length > 0
                    ? nombre.Substring(0, 1).ToUpperInvariant()
                    : "?";
                EmailDetalleLabel.Text = email;
                RolDetalleLabel.Text = rol;
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

        try
        {
            var serviciosPush = Handler?.MauiContext?.Services;

            if (serviciosPush is IServiceProvider proveedorPush)
            {
                // Primero: dar de baja el push (aún hay auth).
                await proveedorPush
                    .GetRequiredService<NotificacionPushService>()
                    .DarDeBajaAsync();
            }
        }
        catch
        {
            // Sigue el logout local igual.
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

    private async void OnBateriaClicked(object? sender, EventArgs e)
    {
#if ANDROID
        if (!OperatingSystem.IsAndroidVersionAtLeast(23))
        {
            await DisplayAlertAsync(
                "No disponible",
                "Su Android es anterior y no permite esta opción. " +
                "Actívelo manual: Ajustes → Apps → CrediVnzl → " +
                "Batería → Sin restricciones.",
                "OK");
            return;
        }

        try
        {
            var contexto = Platform.CurrentActivity ??
                throw new InvalidOperationException("Sin actividad.");

            var power = (Android.OS.PowerManager?)contexto.GetSystemService(
                Android.Content.Context.PowerService);

            if (power is not null &&
                power.IsIgnoringBatteryOptimizations(contexto.PackageName))
            {
                await DisplayAlertAsync(
                    "Avisos siempre activos",
                    "Este celular ya permite los avisos con la app cerrada.",
                    "OK");
                return;
            }

            var intent = new Android.Content.Intent(
                Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations,
                Android.Net.Uri.Parse("package:" + contexto.PackageName));

            contexto.StartActivity(intent);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "No se pudo abrir",
                $"Actívelo manual: Ajustes → Apps → CrediVnzl → Batería → Sin restricciones. ({ex.Message})",
                "OK");
        }
#else
        await DisplayAlertAsync(
            "No aplica",
            "Esta opción es solo para Android.",
            "OK");
#endif
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
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
