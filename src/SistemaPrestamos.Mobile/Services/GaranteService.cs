using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class GaranteService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public GaranteService(HttpClient httpClient)
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

    public async Task<List<GaranteDto>> ObtenerPorClienteAsync(Guid clienteId)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/Garantes/cliente/{clienteId}");

        if (!response.IsSuccessStatusCode)
        {
            return new List<GaranteDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<GaranteDto>>(
            contenido,
            _jsonOptions) ?? new List<GaranteDto>();
    }

    public async Task<(bool Exito, string? Error)> CrearAsync(
        string nombres,
        string apellidos,
        string telefono,
        string? direccion,
        Guid? clienteId)
    {
        await AplicarTokenAsync();

        var dto = new
        {
            nombres,
            apellidos,
            telefono,
            direccion,
            clienteId
        };

        var response = await _httpClient.PostAsJsonAsync(
            "api/Garantes",
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
