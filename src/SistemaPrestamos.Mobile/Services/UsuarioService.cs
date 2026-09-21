using System.Net.Http.Headers;
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
}
