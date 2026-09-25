using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class NotificacionesService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public NotificacionesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task AplicarTokenAsync()
    {
        var token = await SecureStorage.Default.GetAsync("auth_token");

        _httpClient.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<ReglaDto>> ObtenerReglasAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync("api/ReglasNotificacion");

        if (!response.IsSuccessStatusCode)
        {
            return new List<ReglaDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<ReglaDto>>(
            contenido,
            _jsonOptions) ?? new List<ReglaDto>();
    }

    public async Task<ReglaDto?> ObtenerReglaAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/ReglasNotificacion/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ReglaDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<(bool Exito, string? Error)> CrearReglaAsync(
        string nombre,
        string evento,
        string canal,
        string hora,
        int diasSemana,
        string? plantilla)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PostAsJsonAsync(
            "api/ReglasNotificacion",
            new { nombre, evento, canal, hora, diasSemana, plantilla });

        return await LeerResultadoAsync(response);
    }

    public async Task<(bool Exito, string? Error)> ActualizarReglaAsync(
        Guid id,
        string nombre,
        string evento,
        string canal,
        string hora,
        int diasSemana,
        string? plantilla,
        bool activa)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PutAsJsonAsync(
            $"api/ReglasNotificacion/{id}",
            new
            {
                nombre, evento, canal, hora, diasSemana, plantilla, activa
            });

        return await LeerResultadoAsync(response);
    }

    public async Task<(bool Exito, string? Error)> CambiarEstadoAsync(
        Guid id,
        bool activa)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PatchAsJsonAsync(
            $"api/ReglasNotificacion/{id}/estado",
            new { activa });

        return await LeerResultadoAsync(response);
    }

    public async Task<(bool Exito, string? Error)> EliminarReglaAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.DeleteAsync(
            $"api/ReglasNotificacion/{id}");

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string Mensaje)> ProbarReglaAsync(
        Guid id,
        Guid? clienteId = null,
        Guid? prestamoId = null)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PostAsJsonAsync(
            $"api/ReglasNotificacion/{id}/probar",
            new { clienteId, prestamoId });

        var contenido = await response.Content.ReadAsStringAsync();

        try
        {
            using var doc = JsonDocument.Parse(contenido);

            var mensaje = doc.RootElement.TryGetProperty("mensaje", out var m)
                ? m.GetString() ?? string.Empty
                : contenido;

            var exito = doc.RootElement.TryGetProperty("exito", out var e) &&
                e.GetBoolean();

            return (exito, mensaje);
        }
        catch
        {
            return (false, contenido);
        }
    }

    public async Task<DiagnosticoDto?> ObtenerDiagnosticoAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            "api/ReglasNotificacion/diagnostico");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<DiagnosticoDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<List<EnvioDto>> ObtenerEnviosAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            "api/ReglasNotificacion/envios?top=100");

        if (!response.IsSuccessStatusCode)
        {
            return new List<EnvioDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<EnvioDto>>(
            contenido,
            _jsonOptions) ?? new List<EnvioDto>();
    }

    private static async Task<(bool Exito, string? Error)> LeerResultadoAsync(
        HttpResponseMessage response)
    {
        var contenido = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        try
        {
            using var doc = JsonDocument.Parse(contenido);

            if (doc.RootElement.TryGetProperty("mensaje", out var m))
            {
                return (false, m.GetString());
            }
        }
        catch
        {
        }

        return (false, string.IsNullOrWhiteSpace(contenido)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : contenido);
    }
}
