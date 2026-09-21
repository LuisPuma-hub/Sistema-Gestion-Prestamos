using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

[QueryProperty(nameof(PrestamoIdTexto), "prestamoId")]
public partial class DetalleMorosidadPage : ContentPage
{
    private readonly PrestamoService _prestamoService;
    private readonly MorosidadService _morosidadService;
    private Guid _prestamoId = Guid.Empty;

    public string PrestamoIdTexto { get; set; } = string.Empty;

    public DetalleMorosidadPage(
        PrestamoService prestamoService,
        MorosidadService morosidadService)
    {
        InitializeComponent();
        _prestamoService = prestamoService;
        _morosidadService = morosidadService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Guid.TryParse(PrestamoIdTexto, out var id))
        {
            _prestamoId = id;
            await CargarAsync();
        }
        else
        {
            MostrarError("Identificador de préstamo no válido.");
        }
    }

    private async Task CargarAsync()
    {
        try
        {
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var prestamo = await _prestamoService
                .ObtenerPorIdAsync(_prestamoId);

            ClienteLabel.Text = prestamo?.ClienteNombre ?? "Préstamo";

            var mora = await _morosidadService
                .EvaluarAsync(_prestamoId);

            if (mora is null)
            {
                EstadoLabel2.Text = "Sin mora";
                SemanasLabel.Text = "Semanas vencidas: 0";
                InicioLabel.Text = string.Empty;
                ReactivacionLabel.Text = string.Empty;
                ObservacionesLabel.Text = "El préstamo está al día.";
                ReactivarButton.IsVisible = false;
                return;
            }

            EstadoLabel2.Text = mora.Activa ? "En mora" : "Sin mora activa";
            SemanasLabel.Text = $"Semanas vencidas: {mora.PagosInteresVencidos}";
            InicioLabel.Text = mora.FechaInicio is null
                ? "Inicio de mora: -"
                : $"Inicio de mora: {mora.FechaInicio:dd/MM/yyyy}";
            ReactivacionLabel.Text = mora.FechaReactivacion is null
                ? "Última reactivación: -"
                : $"Última reactivación: {mora.FechaReactivacion:dd/MM/yyyy}";
            ObservacionesLabel.Text = string.IsNullOrWhiteSpace(mora.Observaciones)
                ? "Sin observaciones."
                : mora.Observaciones;

            ReactivarButton.IsVisible = mora.Activa && await EsAdminAsync();
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
            MostrarCargando(false);
        }
    }

    private async void OnReactivarClicked(object? sender, EventArgs e)
    {
        var observaciones = ObservacionesEditor.Text?.Trim();

        var confirmar = await DisplayAlertAsync(
            "Reactivar préstamo",
            "¿Marcar el préstamo como reactivado?",
            "Sí",
            "No");

        if (!confirmar)
        {
            return;
        }

        try
        {
            ReactivarButton.IsEnabled = false;
            MostrarCargando(true);
            ErrorLabel.IsVisible = false;

            var (exito, error) = await _morosidadService.ReactivarAsync(
                _prestamoId,
                string.IsNullOrWhiteSpace(observaciones) ? null : observaciones);

            if (!exito)
            {
                MostrarError(error ?? "No se pudo reactivar.");
                return;
            }

            await DisplayAlertAsync(
                "Préstamo reactivado",
                "La morosidad fue cerrada correctamente.",
                "OK");

            await CargarAsync();
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
            ReactivarButton.IsEnabled = true;
            MostrarCargando(false);
        }
    }

    private static async Task<bool> EsAdminAsync()
    {
        var rol = await SecureStorage.Default.GetAsync("usuario_rol");

        return string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase);
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
