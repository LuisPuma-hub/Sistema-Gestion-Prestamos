using System.Net.Http.Headers;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class ImportacionService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ImportacionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(ImportacionPreviewDto? Preview, string? Error)>
        PrevisualizarAsync(Func<Task<Stream>> abrirFlujo, string nombreArchivo)
    {
        return await EnviarAsync<ImportacionPreviewDto>(
            "api/Importacion/prestamos/preview",
            abrirFlujo,
            nombreArchivo);
    }

    public async Task<(ImportacionResultadoDto? Resultado, string? Error)>
        ConfirmarAsync(Func<Task<Stream>> abrirFlujo, string nombreArchivo)
    {
        return await EnviarAsync<ImportacionResultadoDto>(
            "api/Importacion/prestamos/confirm",
            abrirFlujo,
            nombreArchivo);
    }

    private async Task<(T? Dato, string? Error)> EnviarAsync<T>(
        string url,
        Func<Task<Stream>> abrirFlujo,
        string nombreArchivo)
    {
        await AplicarTokenAsync();

        await using var flujo = await abrirFlujo();

        using var contenido = new MultipartFormDataContent();
        using var archivo = new StreamContent(flujo);

        contenido.Add(archivo, "archivo", nombreArchivo);

        var response = await _httpClient.PostAsync(url, contenido);
        var cuerpo = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return (default, TextoError.Limpiar(
                cuerpo,
                $"Error del servidor: {(int)response.StatusCode}"));
        }

        try
        {
            var dato = JsonSerializer.Deserialize<T>(cuerpo, _jsonOptions);

            return dato is null
                ? (default, "Respuesta vacía del servidor.")
                : (dato, null);
        }
        catch (JsonException)
        {
            return (default, "Respuesta no válida del servidor.");
        }
    }

    private async Task AplicarTokenAsync()
    {
        var token = await SecureStorage.Default.GetAsync("auth_token");

        _httpClient.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
    }
}
