using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SistemaPrestamos.Mobile.Models;

namespace SistemaPrestamos.Mobile.Services;

public class ClienteService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ClienteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task AplicarTokenAsync()
    {
        var token = await SecureStorage.Default.GetAsync("auth_token");

        if (string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<ClienteDto>> ObtenerTodosAsync()
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync("api/Clientes");

        if (!response.IsSuccessStatusCode)
        {
            return new List<ClienteDto>();
        }

        var contenido = await response.Content.ReadAsStringAsync();

        var clientes = JsonSerializer.Deserialize<List<ClienteDto>>(
            contenido,
            _jsonOptions);

        return clientes ?? new List<ClienteDto>();
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync($"api/Clientes/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ClienteDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<ClienteDto?> ObtenerPorDocumentoAsync(
        string numeroDocumento)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.GetAsync(
            $"api/Clientes/documento/{numeroDocumento}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contenido = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ClienteDto>(
            contenido,
            _jsonOptions);
    }

    public async Task<(bool Exito, string? Error)> CrearAsync(
        string tipoDocumento,
        string numeroDocumento,
        string nombres,
        string apellidos,
        string telefono,
        string? direccion,
        string? referenciaDireccion,
        string? observaciones)
    {
        await AplicarTokenAsync();

        var dto = new
        {
            tipoDocumento,
            numeroDocumento,
            nombres,
            apellidos,
            telefono,
            direccion,
            referenciaDireccion,
            observaciones
        };

        var responseCrear = await _httpClient.PostAsJsonAsync(
            "api/Clientes",
            dto);

        if (responseCrear.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var errorCrear = await responseCrear.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(errorCrear)
            ? $"Error del servidor: {(int)responseCrear.StatusCode}"
            : errorCrear);
    }

    public async Task<(bool Exito, string? Error)> ActualizarAsync(
        Guid id,
        string tipoDocumento,
        string numeroDocumento,
        string nombres,
        string apellidos,
        string telefono,
        string? direccion,
        string? referenciaDireccion,
        string? observaciones)
    {
        await AplicarTokenAsync();

        var dto = new
        {
            tipoDocumento,
            numeroDocumento,
            nombres,
            apellidos,
            telefono,
            direccion,
            referenciaDireccion,
            observaciones
        };

        var response = await _httpClient.PutAsJsonAsync(
            $"api/Clientes/{id}",
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

    public string? ObtenerUrlFoto(Guid clienteId, string? ruta)
    {
        if (string.IsNullOrWhiteSpace(ruta) ||
            _httpClient.BaseAddress is null)
        {
            return null;
        }

        return new Uri(
            _httpClient.BaseAddress,
            $"api/Clientes/{clienteId}/foto").ToString();
    }

    public async Task<byte[]?> DescargarFotoAsync(Guid clienteId)
    {
        await AplicarTokenAsync();

        try
        {
            var response = await _httpClient.GetAsync(
                $"api/Clientes/{clienteId}/foto");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
        catch
        {
            return null;
        }
    }

    public async Task<(bool Exito, string? Error)> SubirFotoAsync(
        Guid id,
        Stream imagen,
        string nombreArchivo)
    {
        await AplicarTokenAsync();

        try
        {
            using var contenido = new MultipartFormDataContent();
            using var flujo = new StreamContent(imagen);

            contenido.Add(flujo, "archivo", nombreArchivo);

            var response = await _httpClient.PostAsync(
                $"api/Clientes/{id}/foto",
                contenido);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var error = await response.Content.ReadAsStringAsync();

            return (false, string.IsNullOrWhiteSpace(error)
                ? $"Error del servidor: {(int)response.StatusCode}"
                : error);
        }
        catch (HttpRequestException)
        {
            return (false, "No se pudo conectar con el servidor.");
        }
    }

    public async Task<(bool Exito, string? Error)> EliminarAsync(Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.DeleteAsync($"api/Clientes/{id}");

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();

        return (false, string.IsNullOrWhiteSpace(error)
            ? $"Error del servidor: {(int)response.StatusCode}"
            : error);
    }

    public async Task<(bool Exito, string? Resumen, string? Error)> EliminarCascadaAsync(
        Guid id)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.DeleteAsync(
            $"api/Clientes/{id}/cascada");

        var contenido = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return (false, null, string.IsNullOrWhiteSpace(contenido)
                ? $"Error del servidor: {(int)response.StatusCode}"
                : contenido);
        }

        return (true, contenido, null);
    }

    public async Task<(bool Exito, string? Error)> CambiarEstadoAsync(
        Guid id,
        string estado)
    {
        await AplicarTokenAsync();

        var response = await _httpClient.PatchAsJsonAsync(
            $"api/Clientes/{id}/estado",
            new { estado });

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
