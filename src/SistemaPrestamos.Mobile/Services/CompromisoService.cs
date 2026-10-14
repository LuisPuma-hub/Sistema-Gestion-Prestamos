using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class CompromisoService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CompromisoService(HttpClient httpClient)
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

    public async Task<List<CompromisoDto>> ListarPorClienteAsync(
        Guid clienteId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/Compromisos/cliente/{clienteId}");

        if (!response.IsSuccessStatusCode)
            return new List<CompromisoDto>();

        return JsonSerializer.Deserialize<List<CompromisoDto>>(
            await response.Content.ReadAsStringAsync(),
            _jsonOptions) ?? new List<CompromisoDto>();
    }

    public async Task<(bool Exito, string? Error)> CrearAsync(
        Guid clienteId,
        string texto,
        DateTime fecha,
        Guid? prestamoId = null)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PostAsJsonAsync(
            $"api/Compromisos/{clienteId}",
            new { texto, fechaCompromiso = fecha, prestamoId });

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string? Error)> AnularAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.DeleteAsync(
            $"api/Compromisos/{id}");

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }
}
