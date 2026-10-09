using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class ReporteService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ReporteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CobranzaDto?> CobranzaHoyAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            "api/Reportes/cobranza");

        if (!response.IsSuccessStatusCode)
            return null;

        return JsonSerializer.Deserialize<CobranzaDto>(
            await response.Content.ReadAsStringAsync(),
            _jsonOptions);
    }

    public async Task<CarteraDto?> CarteraAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            "api/Reportes/cartera");

        if (!response.IsSuccessStatusCode)
            return null;

        return JsonSerializer.Deserialize<CarteraDto>(
            await response.Content.ReadAsStringAsync(),
            _jsonOptions);
    }

    public async Task<List<IngresoDiaDto>> IngresosUltimosDiasAsync(
        int dias = 7)
    {
        await AplicarTokenAsync();

        var hasta = DateTime.UtcNow.Date;
        var desde = hasta.AddDays(-(dias - 1));

        var response = await _httpClient.GetAsync(
            $"api/Reportes/ingresos?desde={desde:yyyy-MM-dd}" +
            $"&hasta={hasta:yyyy-MM-dd}");

        if (!response.IsSuccessStatusCode)
            return new List<IngresoDiaDto>();

        return JsonSerializer.Deserialize<List<IngresoDiaDto>>(
            await response.Content.ReadAsStringAsync(),
            _jsonOptions) ?? new List<IngresoDiaDto>();
    }

    public async Task<CapitalDto?> CapitalAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            "api/Reportes/capital");

        if (!response.IsSuccessStatusCode)
            return null;

        return JsonSerializer.Deserialize<CapitalDto>(
            await response.Content.ReadAsStringAsync(),
            _jsonOptions);
    }

    public async Task<List<FondoMovimientoDto>> MovimientosAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            "api/Reportes/fondo");

        if (!response.IsSuccessStatusCode)
            return new List<FondoMovimientoDto>();

        return JsonSerializer.Deserialize<List<FondoMovimientoDto>>(
            await response.Content.ReadAsStringAsync(),
            _jsonOptions) ?? new List<FondoMovimientoDto>();
    }

    public async Task<(bool Exito, string? Error)> RegistrarMovimientoAsync(
        string tipo,
        decimal monto,
        string? motivo)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PostAsJsonAsync(
            "api/Reportes/fondo",
            new
            {
                tipo,
                monto,
                fecha = DateTime.UtcNow,
                motivo
            });

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<string?> DescargarCarteraCsvAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            "api/Reportes/cartera/csv");

        if (!response.IsSuccessStatusCode)
            return null;

        var bytes = await response.Content.ReadAsByteArrayAsync();
        var ruta = Path.Combine(
            FileSystem.CacheDirectory, "cartera.csv");

        await File.WriteAllBytesAsync(ruta, bytes);

        return ruta;
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
