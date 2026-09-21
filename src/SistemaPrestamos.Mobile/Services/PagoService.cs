using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class PagoService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PagoService(HttpClient httpClient)
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

    public async Task<List<PagoDto>> ObtenerTodosAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync("api/Pagos");

        if (!response.IsSuccessStatusCode)
        {
            return new List<PagoDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<PagoDto>>(
            contenido,
            _jsonOptions) ?? new List<PagoDto>();
    }

    public async Task<PagoDto?> ObtenerPorIdAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync($"api/Pagos/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<PagoDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<List<PagoDto>> ObtenerPorPrestamoAsync(
        Guid prestamoId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/Pagos/prestamo/{prestamoId}");

        if (!response.IsSuccessStatusCode)
        {
            return new List<PagoDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<PagoDto>>(
            contenido,
            _jsonOptions) ?? new List<PagoDto>();
    }

    public async Task<(bool Exito, string? Error)> RegistrarAsync(
        Guid prestamoId,
        decimal monto,
        DateTime fechaPago,
        string? comprobante,
        string? observaciones)
    {
        await AplicarTokenAsync();

        var dto = new
        {
            prestamoId,
            monto,
            fechaPago,
            comprobante,
            observaciones
        };

        var response = await _httpClient.PostAsJsonAsync(
            "api/Pagos",
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
}
