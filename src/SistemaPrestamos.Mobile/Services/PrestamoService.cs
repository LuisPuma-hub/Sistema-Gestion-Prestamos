using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class PrestamoService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PrestamoService(HttpClient httpClient)
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

    public async Task<List<PrestamoDto>> ObtenerTodosAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync("api/Prestamos");

        if (!response.IsSuccessStatusCode)
        {
            return new List<PrestamoDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<PrestamoDto>>(
            contenido,
            _jsonOptions) ?? new List<PrestamoDto>();
    }

    public async Task<PrestamoDto?> ObtenerPorIdAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync($"api/Prestamos/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<PrestamoDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<List<PrestamoDto>> ObtenerPorClienteAsync(
        Guid clienteId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/Prestamos/cliente/{clienteId}");

        if (!response.IsSuccessStatusCode)
        {
            return new List<PrestamoDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<PrestamoDto>>(
            contenido,
            _jsonOptions) ?? new List<PrestamoDto>();
    }

    public async Task<List<PeriodoInteresDto>> ObtenerPeriodosAsync(
        Guid prestamoId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/Prestamos/{prestamoId}/periodos");

        if (!response.IsSuccessStatusCode)
        {
            return new List<PeriodoInteresDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<PeriodoInteresDto>>(
            contenido,
            _jsonOptions) ?? new List<PeriodoInteresDto>();
    }

    public async Task<(bool Exito, string? Error)> CrearAsync(
        Guid clienteId,
        Guid? garanteId,
        decimal capitalInicial,
        DateTime fechaInicio)
    {
        await AplicarTokenAsync();

        var dto = new
        {
            clienteId,
            garanteId,
            capitalInicial,
            fechaInicio
        };

        var response = await _httpClient.PostAsJsonAsync(
            "api/Prestamos",
            dto);

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string? Error)> AprobarAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PatchAsync(
            $"api/Prestamos/{id}/aprobar",
            null);

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string? Error)> AnularAsync(
        Guid id,
        string motivo)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PatchAsJsonAsync(
            $"api/Prestamos/{id}/anular",
            new { motivo });

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
