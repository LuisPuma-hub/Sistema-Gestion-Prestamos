using SistemaPrestamos.Mobile.Models;
using SistemaPrestamos.Mobile.Services;

namespace SistemaPrestamos.Mobile.Views;

public partial class ImportacionPage : ContentPage
{
    private readonly ImportacionService _importacionService;

    private FileResult? _archivo;
    private ImportacionPreviewDto? _preview;

    public ImportacionPage(ImportacionService importacionService)
    {
        InitializeComponent();
        _importacionService = importacionService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _ = Animaciones.EntradaAsync(Content);

        var rol = await SecureStorage.Default.GetAsync("usuario_rol");

        if (!string.Equals(
                rol,
                "Administrador",
                StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlertAsync(
                "Sin permiso",
                "Solo un administrador puede importar préstamos.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnElegirClicked(object? sender, EventArgs e)
    {
        try
        {
            var tipos = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "*/*" } },
                    { DevicePlatform.WinUI, new[] { ".csv" } },
                    { DevicePlatform.MacCatalyst, new[] { "public.comma-separated-values-text" } },
                    { DevicePlatform.iOS, new[] { "public.comma-separated-values-text" } }
                });

            _archivo = await FilePicker.Default.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Elija el CSV de préstamos",
                    FileTypes = tipos
                });

            if (_archivo is null)
                return;

            if (!string.Equals(
                    Path.GetExtension(_archivo.FileName),
                    ".csv",
                    StringComparison.OrdinalIgnoreCase))
            {
                await DisplayAlertAsync(
                    "Archivo no válido",
                    "Elija un archivo con extensión .csv",
                    "OK");
                return;
            }

            ArchivoLabel.Text = _archivo.FileName;
            PrevisualizarButton.IsEnabled = true;

            LimpiarPreview();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "No se pudo abrir",
                $"No se pudo elegir el archivo: {ex.Message}",
                "OK");
        }
    }

    private async void OnPlantillaClicked(object? sender, EventArgs e)
    {
        const string plantilla =
            "idFila,tipoDoc,nroDoc,nombres,apellidos,telefono," +
            "direccion,capitalInicial,fechaInicio,saldoCapitalActual\n" +
            "1,DNI,70000001,Juan,Perez,987654321,Av. Lima 123," +
            "500.00,2026-08-01,400.00\n" +
            "2,DNI,70000002,Maria,Lopez,912345678,Jr. Rosas 45," +
            "300.00,2026-09-01,\n";

        try
        {
            var ruta = Path.Combine(
                FileSystem.CacheDirectory,
                "plantilla_prestamos.csv");

            await File.WriteAllTextAsync(ruta, plantilla);

            await Share.Default.RequestAsync(
                new ShareFileRequest
                {
                    Title = "Plantilla de importación",
                    File = new ShareFile(ruta)
                });
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "No se pudo generar",
                $"No se pudo crear la plantilla: {ex.Message}",
                "OK");
        }
    }

    private async void OnPrevisualizarClicked(object? sender, EventArgs e)
    {
        if (_archivo is null)
            return;

        try
        {
            MostrarCargando(true);
            LimpiarPreview();

            var (preview, error) =
                await _importacionService.PrevisualizarAsync(
                    () => _archivo.OpenReadAsync(),
                    _archivo.FileName);

            if (error is not null || preview is null)
            {
                MostrarEstado(error ?? "No se pudo previsualizar.");
                return;
            }

            _preview = preview;

            ResumenLabel.Text = preview.Resumen;
            ResumenCard.IsVisible = true;
            FilasCollection.ItemsSource = preview.Filas;

            ConfirmarButton.IsVisible =
                preview.TotalFilas > 0 && preview.ConError == 0;

            if (preview.ConError > 0)
                MostrarEstado("Corrija el archivo y vuelva a previsualizar.");
        }
        catch (HttpRequestException)
        {
            MostrarEstado("Sin conexión con el servidor. Verifique su red.");
        }
        catch (Exception ex)
        {
            MostrarEstado($"Ocurrió un error: {ex.Message}");
        }
        finally
        {
            MostrarCargando(false);
        }
    }

    private async void OnConfirmarClicked(object? sender, EventArgs e)
    {
        if (_archivo is null || _preview is null)
            return;

        var confirma = await DisplayAlertAsync(
            "Confirmar importación",
            $"Se crearán {_preview.Validas} préstamos. " +
            "Esta acción no se puede deshacer. ¿Continuar?",
            "Sí, importar",
            "Cancelar");

        if (!confirma)
            return;

        try
        {
            MostrarCargando(true);
            ConfirmarButton.IsEnabled = false;

            var (resultado, error) =
                await _importacionService.ConfirmarAsync(
                    () => _archivo.OpenReadAsync(),
                    _archivo.FileName);

            if (error is not null || resultado is null)
            {
                await DisplayAlertAsync(
                    "No se pudo importar",
                    error ?? "Inténtelo de nuevo.",
                    "OK");
                return;
            }

            await DisplayAlertAsync(
                "Importación completada",
                resultado.Resumen,
                "OK");

            LimpiarTodo();
        }
        catch (HttpRequestException)
        {
            await DisplayAlertAsync(
                "Sin conexión",
                "Verifique su red e inténtelo de nuevo.",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Ocurrió un error",
                ex.Message,
                "OK");
        }
        finally
        {
            MostrarCargando(false);
            ConfirmarButton.IsEnabled = true;
        }
    }

    private async void OnVolverClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void LimpiarPreview()
    {
        _preview = null;
        ResumenCard.IsVisible = false;
        FilasCollection.ItemsSource = null;
        ConfirmarButton.IsVisible = false;
        EstadoLabel.IsVisible = false;
    }

    private void LimpiarTodo()
    {
        _archivo = null;
        ArchivoLabel.Text = "Ningún archivo elegido.";
        PrevisualizarButton.IsEnabled = false;
        LimpiarPreview();
    }

    private void MostrarCargando(bool cargando)
    {
        CargandoIndicator.IsVisible = cargando;
        CargandoIndicator.IsRunning = cargando;
    }

    private void MostrarEstado(string mensaje)
    {
        EstadoLabel.Text = mensaje;
        EstadoLabel.IsVisible = true;
    }
}
