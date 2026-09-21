using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class WhatsappService : IWhatsappService
{
    private const string VersionApi = "v22.0";

    private readonly HttpClient _httpClient;
    private readonly IMensajeWhatsappRepository _mensajeRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly string _phoneNumberId;
    private readonly string _token;

    public WhatsappService(
        HttpClient httpClient,
        IMensajeWhatsappRepository mensajeRepository,
        IClienteRepository clienteRepository,
        IPrestamoRepository prestamoRepository,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _mensajeRepository = mensajeRepository;
        _clienteRepository = clienteRepository;
        _prestamoRepository = prestamoRepository;

        _phoneNumberId = configuration["WhatsApp:PhoneNumberId"]
            ?? throw new InvalidOperationException(
                "WhatsApp:PhoneNumberId no configurado.");

        _token = configuration["WhatsApp:Token"]
            ?? throw new InvalidOperationException(
                "WhatsApp:Token no configurado.");
    }

    public async Task<MensajeWhatsapp> EnviarPlantillaAsync(
        Guid? clienteId,
        Guid? prestamoId,
        string numeroDestino,
        string nombrePlantilla,
        string contenido,
        string idioma = "es_PE")
    {
        var numero = NormalizarNumero(numeroDestino);

        var cuerpo = new
        {
            messaging_product = "whatsapp",
            to = numero,
            type = "template",
            template = new
            {
                name = nombrePlantilla,
                language = new { code = idioma }
            }
        };

        var idExterno = await EnviarAsync(cuerpo);

        return await GuardarAsync(
            clienteId,
            prestamoId,
            nombrePlantilla,
            numero,
            contenido,
            idExterno);
    }

    public async Task<MensajeWhatsapp> EnviarTextoAsync(
        Guid? clienteId,
        Guid? prestamoId,
        string numeroDestino,
        string texto)
    {
        var numero = NormalizarNumero(numeroDestino);

        var cuerpo = new
        {
            messaging_product = "whatsapp",
            to = numero,
            type = "text",
            text = new { body = texto }
        };

        var idExterno = await EnviarAsync(cuerpo);

        return await GuardarAsync(
            clienteId,
            prestamoId,
            "texto_libre",
            numero,
            texto,
            idExterno);
    }

    public async Task<MensajeWhatsapp> EnviarRecordatorioAsync(
        Guid clienteId,
        Guid? prestamoId)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);

        if (cliente is null)
        {
            throw new InvalidOperationException(
                "El cliente no existe.");
        }

        if (string.IsNullOrWhiteSpace(cliente.Telefono))
        {
            throw new InvalidOperationException(
                "El cliente no tiene teléfono registrado.");
        }

        string texto;

        if (prestamoId.HasValue)
        {
            var prestamo = await _prestamoRepository
                .ObtenerPorIdAsync(prestamoId.Value);

            if (prestamo is null)
            {
                throw new InvalidOperationException(
                    "El préstamo no existe.");
            }

            texto = $"Hola {cliente.Nombres}, le recordamos que su préstamo " +
                $"tiene un capital pendiente de S/ {prestamo.CapitalPendiente:N2}. " +
                $"Por favor acérquese a realizar su pago semanal. Gracias.";
        }
        else
        {
            texto = $"Hola {cliente.Nombres}, le recordamos que tiene pagos " +
                $"pendientes en el Sistema de Préstamos. Gracias.";
        }

        return await EnviarTextoAsync(
            clienteId,
            prestamoId,
            cliente.Telefono,
            texto);
    }

    public async Task<IEnumerable<MensajeWhatsapp>> ObtenerHistorialAsync(
        Guid clienteId)
    {
        return await _mensajeRepository.ObtenerPorClienteAsync(clienteId);
    }

    private async Task<string?> EnviarAsync(object cuerpo)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"https://graph.facebook.com/{VersionApi}/{_phoneNumberId}/messages");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _token);

        request.Content = JsonContent.Create(cuerpo);

        var response = await _httpClient.SendAsync(request);
        var contenido = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"WhatsApp rechazó el envío ({(int)response.StatusCode}): {contenido}");
        }

        try
        {
            using var doc = JsonDocument.Parse(contenido);

            return doc.RootElement
                .GetProperty("messages")[0]
                .GetProperty("id")
                .GetString();
        }
        catch
        {
            return null;
        }
    }

    private async Task<MensajeWhatsapp> GuardarAsync(
        Guid? clienteId,
        Guid? prestamoId,
        string tipo,
        string numero,
        string contenido,
        string? idExterno)
    {
        var mensaje = new MensajeWhatsapp
        {
            Id = Guid.NewGuid(),
            ClienteId = clienteId,
            PrestamoId = prestamoId,
            TipoPlantilla = tipo,
            NumeroDestino = numero,
            Contenido = contenido,
            Estado = "Enviado",
            IdentificadorExterno = idExterno,
            FechaEnvio = DateTime.UtcNow,
            FechaCreacion = DateTime.UtcNow
        };

        await _mensajeRepository.CrearAsync(mensaje);
        await _mensajeRepository.GuardarCambiosAsync();

        return mensaje;
    }

    private static string NormalizarNumero(string numero)
    {
        var digitos = new string(
            numero.Where(char.IsDigit).ToArray());

        // Celular peruano de 9 dígitos sin código país.
        if (digitos.Length == 9 && digitos.StartsWith("9"))
        {
            digitos = "51" + digitos;
        }

        if (digitos.Length < 8)
        {
            throw new InvalidOperationException(
                "Número de destino no válido.");
        }

        return digitos;
    }
}
