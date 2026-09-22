using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class UsuarioService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UsuarioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(Guid id)
    {
        var token = await SecureStorage.Default.GetAsync("auth_token");

        _httpClient.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"api/Usuarios/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<UsuarioDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<List<UsuarioDto>> ObtenerTodosAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync("api/Usuarios");

        if (!response.IsSuccessStatusCode)
        {
            return new List<UsuarioDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<UsuarioDto>>(
            contenido,
            _jsonOptions) ?? new List<UsuarioDto>();
    }

    private async Task AplicarTokenAsync()
    {
        var token = await SecureStorage.Default.GetAsync("auth_token");

        _httpClient.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<(bool Exito, string? Error)> CrearAsync(
        string nombres,
        string apellidos,
        string email,
        string password,
        string rol)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PostAsJsonAsync(
            "api/Usuarios",
            new { nombres, apellidos, email, password, rol });

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string? Error)> EliminarAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.DeleteAsync($"api/Usuarios/{id}");

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string? Error)> ResetearClaveAsync(
        Guid id,
        string nueva)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PatchAsJsonAsync(
            $"api/Usuarios/{id}/clave",
            new { nueva });

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string? Error)> CambiarMiClaveAsync(
        string actual,
        string nueva)
    {
        var token = await SecureStorage.Default.GetAsync("auth_token");

        _httpClient.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsJsonAsync(
            "api/Auth/cambiar-clave",
            new { actual, nueva });

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
