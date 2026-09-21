using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        string email,
        string password)
    {
        var loginDto = new LoginDto
        {
            Email = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync(
            "api/Auth/login",
            loginDto);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        var resultado = JsonSerializer.Deserialize<LoginResponseDto>(
            contenido,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (resultado is null ||
            string.IsNullOrWhiteSpace(resultado.Token))
        {
            return null;
        }

        await SecureStorage.Default.SetAsync(
            "auth_token",
            resultado.Token);

        await SecureStorage.Default.SetAsync(
            "usuario_id",
            resultado.UsuarioId.ToString());

        await SecureStorage.Default.SetAsync(
            "usuario_nombre",
            $"{resultado.Nombres} {resultado.Apellidos}");

        await SecureStorage.Default.SetAsync(
            "usuario_rol",
            resultado.Rol);

        await SecureStorage.Default.SetAsync(
            "usuario_email",
            resultado.Email);

        await SecureStorage.Default.SetAsync(
            "refresh_token",
            resultado.RefreshToken);

        return resultado;
    }

    public async Task<string?> ObtenerTokenAsync()
    {
        return await SecureStorage.Default.GetAsync("auth_token");
    }

    public async Task CerrarSesionAsync()
    {
        try
        {
            var refresh = await SecureStorage.Default
                .GetAsync("refresh_token");

            if (!string.IsNullOrWhiteSpace(refresh))
            {
                await _httpClient.PostAsJsonAsync(
                    "api/Auth/logout",
                    new { refreshToken = refresh });
            }
        }
        catch
        {
            // Si no hay red, igual se limpia local.
        }

        SecureStorage.Default.Remove("auth_token");
        SecureStorage.Default.Remove("refresh_token");
        SecureStorage.Default.Remove("usuario_id");
        SecureStorage.Default.Remove("usuario_nombre");
        SecureStorage.Default.Remove("usuario_rol");
        SecureStorage.Default.Remove("usuario_email");
    }
}
