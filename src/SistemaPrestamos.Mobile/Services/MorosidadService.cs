using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class MorosidadService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public MorosidadService(HttpClient httpClient)
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

    public async Task<MorosidadDto?> ObtenerPorPrestamoAsync(
        Guid prestamoId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/Morosidades/prestamo/{prestamoId}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<MorosidadDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<MorosidadDto?> EvaluarAsync(Guid prestamoId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PostAsync(
            $"api/Morosidades/prestamo/{prestamoId}/evaluar",
            null);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(contenido))
        {
            return null;
        }

        return JsonSerializer.Deserialize<MorosidadDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<(bool Exito, string? Error)> ReactivarAsync(
        Guid prestamoId,
        string? observaciones)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PatchAsJsonAsync(
            $"api/Morosidades/prestamo/{prestamoId}/reactivar",
            new { observaciones });

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }
}
