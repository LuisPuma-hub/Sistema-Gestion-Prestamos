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
    private readonly IPeriodoInteresRepository _periodoRepository;
    private readonly IMorosidadService _morosidadService;
    private readonly IPagoRepository _pagoRepository;
    private readonly string _phoneNumberId;
    private readonly string _token;

    public WhatsappService(
        HttpClient httpClient,
        IMensajeWhatsappRepository mensajeRepository,
        IClienteRepository clienteRepository,
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoRepository,
        IMorosidadService morosidadService,
        IPagoRepository pagoRepository,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _mensajeRepository = mensajeRepository;
        _clienteRepository = clienteRepository;
        _prestamoRepository = prestamoRepository;
        _periodoRepository = periodoRepository;
        _morosidadService = morosidadService;
        _pagoRepository = pagoRepository;

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
        string idioma = "es_PE",
        List<string>? parametros = null)
    {
        var numero = NormalizarNumero(numeroDestino);

        object plantilla = parametros is { Count: > 0 }
            ? new
            {
                name = nombrePlantilla,
                language = new { code = idioma },
                components = new[]
                {
                    new
                    {
                        type = "body",
                        parameters = parametros.Select(p => new
                        {
                            type = "text",
                            text = p
                        }).ToArray()
                    }
                }
            }
            : new
            {
                name = nombrePlantilla,
                language = new { code = idioma }
            };

        var cuerpo = new
        {
            messaging_product = "whatsapp",
            to = numero,
            type = "template",
            template = plantilla
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

    public static IReadOnlyList<(string Nombre, string Descripcion)> PlantillasDisponibles { get; } =
    [
        ("recordatorio_pago_v2", "Recordatorio de pago semanal"),
        ("aviso_mora", "Aviso de morosidad"),
        ("confirmacion_pago", "Confirmación de pago recibido"),
        ("prestamo_aprobado", "Aviso de préstamo aprobado")
    ];

    public async Task<MensajeWhatsapp> EnviarRecordatorioAsync(
        Guid clienteId,
        Guid? prestamoId)
    {
        if (!prestamoId.HasValue)
        {
            throw new InvalidOperationException(
                "El recordatorio con plantilla requiere el préstamo.");
        }

        var (cliente, prestamo) = await ObtenerClienteYPrestamoAsync(
            clienteId,
            prestamoId.Value);

        var periodos = await _periodoRepository
            .ObtenerPorPrestamoAsync(prestamo.Id);

        var proximo = periodos
            .Where(p => p.InteresPendiente > 0)
            .OrderBy(p => p.FechaVencimiento)
            .FirstOrDefault();

        var semanal = prestamo.CapitalInicial * prestamo.TasaInteresSemanal;

        return await EnviarPlantillaAsync(
            clienteId,
            prestamo.Id,
            cliente.Telefono,
            "recordatorio_pago_v2",
            $"Recordatorio a {cliente.Nombres}.",
            "es_PE",
            [
                cliente.Nombres,
                semanal.ToString("N2", System.Globalization.CultureInfo.InvariantCulture),
                (proximo?.FechaVencimiento ?? prestamo.FechaInicio.AddDays(7))
                    .ToString("dd/MM/yyyy")
            ]);
    }

    public async Task<MensajeWhatsapp> EnviarPlantillaCatalogoAsync(
        Guid clienteId,
        Guid? prestamoId,
        string plantilla)
    {
        var nombre = PlantillasDisponibles
            .FirstOrDefault(p => string.Equals(
                p.Nombre,
                plantilla,
                StringComparison.OrdinalIgnoreCase))
            .Nombre;

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new InvalidOperationException(
                "Plantilla no disponible.");
        }

        if (nombre == "recordatorio_pago_v2")
        {
            return await EnviarRecordatorioAsync(clienteId, prestamoId);
        }

        if (nombre == "aviso_mora")
        {
            return await EnviarAvisoMoraAsync(clienteId, prestamoId);
        }

        if (nombre == "prestamo_aprobado")
        {
            return await EnviarPrestamoAprobadoAsync(clienteId, prestamoId);
        }

        return await EnviarConfirmacionPagoAsync(clienteId, prestamoId);
    }

    private async Task<MensajeWhatsapp> EnviarAvisoMoraAsync(
        Guid clienteId,
        Guid? prestamoId)
    {
        if (!prestamoId.HasValue)
        {
            throw new InvalidOperationException(
                "El aviso de mora requiere el préstamo.");
        }

        var (cliente, prestamo) = await ObtenerClienteYPrestamoAsync(
            clienteId,
            prestamoId.Value);

        var mora = await _morosidadService.EvaluarAsync(
            prestamo.Id,
            DateTime.UtcNow);

        var periodos = await _periodoRepository
            .ObtenerPorPrestamoAsync(prestamo.Id);

        var pendiente = periodos.Sum(p => p.InteresPendiente);

        return await EnviarPlantillaAsync(
            clienteId,
            prestamo.Id,
            cliente.Telefono,
            "aviso_mora",
            $"Aviso de mora a {cliente.Nombres}.",
            "es_PE",
            [
                cliente.Nombres,
                (mora?.PagosInteresVencidos ?? 0).ToString(),
                pendiente.ToString("N2", System.Globalization.CultureInfo.InvariantCulture)
            ]);
    }

    private async Task<MensajeWhatsapp> EnviarPrestamoAprobadoAsync(
        Guid clienteId,
        Guid? prestamoId)
    {
        if (!prestamoId.HasValue)
        {
            throw new InvalidOperationException(
                "Se requiere el préstamo.");
        }

        var (cliente, prestamo) = await ObtenerClienteYPrestamoAsync(
            clienteId,
            prestamoId.Value);

        var periodos = await _periodoRepository
            .ObtenerPorPrestamoAsync(prestamo.Id);

        var primero = periodos
            .OrderBy(p => p.FechaVencimiento)
            .FirstOrDefault();

        return await EnviarPlantillaAsync(
            clienteId,
            prestamo.Id,
            cliente.Telefono,
            "prestamo_aprobado",
            $"Préstamo aprobado a {cliente.Nombres}.",
            "es_PE",
            [
                cliente.Nombres,
                prestamo.CapitalInicial.ToString("N2", System.Globalization.CultureInfo.InvariantCulture),
                (primero?.FechaVencimiento ?? prestamo.FechaInicio.AddDays(7))
                    .ToString("dd/MM/yyyy")
            ]);
    }

    private async Task<MensajeWhatsapp> EnviarConfirmacionPagoAsync(
        Guid clienteId,
        Guid? prestamoId)
    {
        var (cliente, prestamo) = prestamoId.HasValue
            ? await ObtenerClienteYPrestamoAsync(clienteId, prestamoId.Value)
            : (await ObtenerClienteAsync(clienteId), null as Prestamo);

        decimal ultimoMonto = 0;
        decimal saldo = prestamo?.CapitalPendiente ?? 0;

        if (prestamo is not null)
        {
            var pagos = await _pagoRepository
                .ObtenerPorPrestamoAsync(prestamo.Id);

            var ultimo = pagos
                .OrderByDescending(p => p.FechaPago)
                .FirstOrDefault();

            ultimoMonto = ultimo?.Monto ?? 0;
        }

        return await EnviarPlantillaAsync(
            clienteId,
            prestamo?.Id,
            cliente.Telefono,
            "confirmacion_pago",
            $"Confirmación de pago a {cliente.Nombres}.",
            "es_PE",
            [
                cliente.Nombres,
                ultimoMonto.ToString("N2", System.Globalization.CultureInfo.InvariantCulture),
                saldo.ToString("N2", System.Globalization.CultureInfo.InvariantCulture)
            ]);
    }

    private async Task<(Cliente cliente, Prestamo prestamo)> ObtenerClienteYPrestamoAsync(
        Guid clienteId,
        Guid prestamoId)
    {
        var cliente = await ObtenerClienteAsync(clienteId);

        var prestamo = await _prestamoRepository
            .ObtenerPorIdAsync(prestamoId);

        if (prestamo is null)
        {
            throw new InvalidOperationException(
                "El préstamo no existe.");
        }

        return (cliente, prestamo);
    }

    private async Task<Cliente> ObtenerClienteAsync(Guid clienteId)
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

        return cliente;
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
