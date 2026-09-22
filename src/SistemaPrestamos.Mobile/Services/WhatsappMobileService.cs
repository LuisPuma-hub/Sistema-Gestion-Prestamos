using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class WhatsappMobileService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public WhatsappMobileService(HttpClient httpClient)
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

    public async Task<List<MensajeWhatsappDto>> ObtenerHistorialAsync(
        Guid clienteId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/Whatsapp/historial/cliente/{clienteId}");

        if (!response.IsSuccessStatusCode)
        {
            return new List<MensajeWhatsappDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<MensajeWhatsappDto>>(
            contenido,
            _jsonOptions) ?? new List<MensajeWhatsappDto>();
    }

    public async Task<(bool Exito, string? Error)> EnviarRecordatorioAsync(
        Guid clienteId,
        Guid? prestamoId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PostAsJsonAsync(
            "api/Whatsapp/recordatorio",
            new { clienteId, prestamoId });

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string? Error)> EnviarPlantillaAsync(
        Guid clienteId,
        Guid? prestamoId,
        string plantilla)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PostAsJsonAsync(
            "api/Whatsapp/enviar-plantilla",
            new { clienteId, prestamoId, plantilla });

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
