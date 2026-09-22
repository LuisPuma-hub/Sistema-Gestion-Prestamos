using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class UsuariosPage : ContentPage
{
    private readonly UsuarioService _usuarioService;
    private List<UsuarioDto> _todos = new();

    public UsuariosPage(UsuarioService usuarioService)
    {
        InitializeComponent();
        _usuarioService = usuarioService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);

        var rol = await SecureStorage.Default.GetAsync("usuario_rol");

        if (!string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlertAsync(
                "Sin permiso",
                "Solo un administrador puede gestionar usuarios.",
                "OK");

            await Shell.Current.GoToAsync("..");
            return;
        }

        await CargarAsync();
    }

    private async Task CargarAsync()
    {
        try
        {
            MostrarCargando(true);
            EstadoLabel.IsVisible = false;
            ReintentarButton.IsVisible = false;

            _todos = await _usuarioService.ObtenerTodosAsync();

            UsuariosCollection.ItemsSource = _todos;

            if (_todos.Count == 0)
            {
                EstadoLabel.Text = "No hay usuarios registrados.";
                EstadoLabel.IsVisible = true;
            }
        }
        catch (HttpRequestException)
        {
            MostrarError("No se pudo cargar. Verifique su conexión e intente nuevamente.");
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

    private async void OnUsuarioTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is not Guid id)
        {
            return;
        }

        var usuario = _todos.FirstOrDefault(u => u.Id == id);

        if (usuario is null)
        {
            return;
        }

        var accion = await DisplayActionSheetAsync(
            usuario.NombreCompleto,
            "Cancelar",
            null,
            "Resetear contraseña",
            "Eliminar usuario");

        if (accion == "Resetear contraseña")
        {
            await ResetearAsync(usuario);
        }
        else if (accion == "Eliminar usuario")
        {
            await EliminarAsync(usuario);
        }
    }

    private async Task ResetearAsync(UsuarioDto usuario)
    {
        var nueva = await DisplayPromptAsync(
            "Resetear contraseña",
            $"Nueva contraseña para {usuario.Email} (mínimo 8 caracteres):",
            "Guardar",
            "Cancelar",
            maxLength: 100);

        if (string.IsNullOrWhiteSpace(nueva))
        {
            return;
        }

        var (exito, error) = await _usuarioService.ResetearClaveAsync(
            usuario.Id,
            nueva.Trim());

        if (!exito)
        {
            await DisplayAlertAsync(
                "No se pudo resetear",
                TextoError.Limpiar(error, "Inténtalo de nuevo."),
                "OK");
            return;
        }

        await DisplayAlertAsync(
            "Contraseña reseteada",
            "Comunícale la nueva contraseña al usuario.",
            "OK");
    }

    private async Task EliminarAsync(UsuarioDto usuario)
    {
        var confirmar = await DisplayAlertAsync(
            "Eliminar usuario",
            $"¿Eliminar a {usuario.NombreCompleto}? No podrás eliminarte a ti mismo.",
            "Sí, eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        var (exito, error) = await _usuarioService.EliminarAsync(usuario.Id);

        if (!exito)
        {
            await DisplayAlertAsync(
                "No se puede eliminar",
                TextoError.Limpiar(error, "Inténtalo de nuevo."),
                "OK");
            return;
        }

        await CargarAsync();
    }

    private async void OnReintentarClicked(object? sender, EventArgs e)
    {
        await CargarAsync();
    }

    private async void OnNuevoUsuarioClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegistrarUsuarioPage));
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
        EstadoLabel.Text = mensaje;
        EstadoLabel.IsVisible = true;
        ReintentarButton.IsVisible = true;
    }
}
