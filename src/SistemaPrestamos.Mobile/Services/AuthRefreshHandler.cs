using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace SistemaPrestamos.Mobile.Services;

/// <summary>
/// Reintenta una vez cada petición 401 renovando el JWT
/// con el refresh token. Si falla, la app debe pedir login.
/// </summary>
public class AuthRefreshHandler : DelegatingHandler
{
    private readonly Uri _baseAddress;
    private static readonly SemaphoreSlim _candado = new(1, 1);

    public AuthRefreshHandler(Uri baseAddress)
    {
        _baseAddress = baseAddress;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var respuesta = await base.SendAsync(request, cancellationToken);

        if (respuesta.StatusCode != HttpStatusCode.Unauthorized ||
            EsEndpointAuth(request))
        {
            return respuesta;
        }

        var renovado = await IntentarRenovarAsync(cancellationToken);

        if (!renovado)
        {
            return respuesta;
        }

        respuesta.Dispose();

        var token = await SecureStorage.Default.GetAsync("auth_token");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }

    private static bool EsEndpointAuth(HttpRequestMessage request)
    {
        return request.RequestUri?.AbsolutePath
            .Contains("/api/Auth/", StringComparison.OrdinalIgnoreCase)
            is true;
    }

    private async Task<bool> IntentarRenovarAsync(
        CancellationToken cancellationToken)
    {
        await _candado.WaitAsync(cancellationToken);

        try
        {
            var refresh = await SecureStorage.Default.GetAsync("refresh_token");

            if (string.IsNullOrWhiteSpace(refresh))
            {
                return false;
            }

            using var http = new HttpClient { BaseAddress = _baseAddress };

            var response = await http.PostAsJsonAsync(
                "api/Auth/refresh",
                new { refreshToken = refresh },
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var contenido = await response.Content
                .ReadAsStringAsync(cancellationToken);

            var dto = JsonSerializer.Deserialize<Models.LoginResponseDto>(
                contenido,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (dto is null || string.IsNullOrWhiteSpace(dto.Token))
            {
                return false;
            }

            await SecureStorage.Default.SetAsync("auth_token", dto.Token);
            await SecureStorage.Default.SetAsync(
                "refresh_token",
                dto.RefreshToken);

            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            _candado.Release();
        }
    }
}
