using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IPagoRepository _pagoRepository;
    private readonly IPeriodoInteresRepository _periodoRepository;
    private readonly IMorosidadRepository _morosidadRepository;
    private readonly IMensajeWhatsappRepository _mensajeRepository;
    private readonly IGaranteRepository _garanteRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClienteService(
        IClienteRepository clienteRepository,
        IPrestamoRepository prestamoRepository,
        IPagoRepository pagoRepository,
        IPeriodoInteresRepository periodoRepository,
        IMorosidadRepository morosidadRepository,
        IMensajeWhatsappRepository mensajeRepository,
        IGaranteRepository garanteRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _clienteRepository = clienteRepository;
        _prestamoRepository = prestamoRepository;
        _pagoRepository = pagoRepository;
        _periodoRepository = periodoRepository;
        _morosidadRepository = morosidadRepository;
        _mensajeRepository = mensajeRepository;
        _garanteRepository = garanteRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ClienteDto>> ObtenerTodosAsync()
    {
        var clientes = await _clienteRepository.ObtenerTodosAsync();

        return clientes.Select(MapearDto);
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(Guid id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        return cliente is null ? null : MapearDto(cliente);
    }

    public async Task<ClienteDto?> ObtenerPorDocumentoAsync(string numeroDocumento)
    {
        var cliente = await _clienteRepository
            .ObtenerPorDocumentoAsync(numeroDocumento);

        return cliente is null ? null : MapearDto(cliente);
    }

    public async Task<ClienteDto> CrearAsync(CrearClienteDto dto)
    {
        var existente = await _clienteRepository
            .ObtenerPorDocumentoAsync(dto.NumeroDocumento);

        if (existente is not null)
        {
            throw new InvalidOperationException(
                "Ya existe un cliente con ese número de documento.");
        }

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            TipoDocumento = dto.TipoDocumento,
            NumeroDocumento = dto.NumeroDocumento,
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            ReferenciaDireccion = dto.ReferenciaDireccion,
            FotoReciboServicio = dto.FotoReciboServicio,
            Observaciones = dto.Observaciones,
            Estado = "Activo",
            UsuarioRegistraId = ObtenerUsuarioActualId(),
            FechaRegistro = DateTime.UtcNow
        };

        await _clienteRepository.CrearAsync(cliente);
        await _clienteRepository.GuardarCambiosAsync();

        return MapearDto(cliente);
    }

    public async Task<bool> ActualizarAsync(
        Guid id,
        CrearClienteDto dto)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente is null)
        {
            return false;
        }

        cliente.TipoDocumento = dto.TipoDocumento;
        cliente.NumeroDocumento = dto.NumeroDocumento;
        cliente.Nombres = dto.Nombres;
        cliente.Apellidos = dto.Apellidos;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;
        cliente.ReferenciaDireccion = dto.ReferenciaDireccion;
        cliente.FotoReciboServicio = dto.FotoReciboServicio;
        cliente.Observaciones = dto.Observaciones;

        await _clienteRepository.ActualizarAsync(cliente);
        await _clienteRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> ActualizarFotoAsync(Guid id, string ruta)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente is null)
        {
            return false;
        }

        cliente.FotoReciboServicio = ruta;

        await _clienteRepository.ActualizarAsync(cliente);
        await _clienteRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(Guid id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente is null)
        {
            return false;
        }

        var resumen = await ContarRegistrosAsync(id);

        if (resumen.Prestamos > 0)
        {
            throw new InvalidOperationException(
                $"Tiene {resumen.Prestamos} préstamo(s), " +
                $"{resumen.Pagos} pago(s), " +
                $"{resumen.Mensajes} mensaje(s) y " +
                $"{resumen.Garantes} aval(es). " +
                $"Use eliminar todo si desea borrarlos.");
        }

        await _clienteRepository.EliminarAsync(cliente);
        await _clienteRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<ResumenCascada> EliminarCascadaAsync(Guid id)
    {
        var existe = await _clienteRepository.ObtenerPorIdAsync(id);

        if (existe is null)
        {
            throw new InvalidOperationException(
                "Cliente no encontrado.");
        }

        // Partir de un seguimiento limpio: evita conflictos
        // "another instance with the same key is already being tracked".
        _clienteRepository.LimpiarSeguimiento();

        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente is null)
        {
            throw new InvalidOperationException(
                "Cliente no encontrado.");
        }

        var resumen = new ResumenCascada();
        var prestamos = (await _prestamoRepository
            .ObtenerPorClienteAsync(id)).ToList();

        resumen.Prestamos = prestamos.Count;

        foreach (var prestamo in prestamos)
        {
            var pagos = (await _pagoRepository
                .ObtenerPorPrestamoAsync(prestamo.Id)).ToList();

            foreach (var pago in pagos)
            {
                await _pagoRepository.EliminarAsync(pago);
                resumen.Pagos++;
            }

            var periodos = (await _periodoRepository
                .ObtenerPorPrestamoAsync(prestamo.Id)).ToList();

            foreach (var periodo in periodos)
            {
                await _periodoRepository.EliminarAsync(periodo);
                resumen.Periodos++;
            }

            var morosidad = await _morosidadRepository
                .ObtenerPorPrestamoAsync(prestamo.Id);

            if (morosidad is not null)
            {
                await _morosidadRepository.EliminarAsync(morosidad);
                resumen.Morosidades++;
            }

            await _prestamoRepository.EliminarAsync(prestamo);
        }

        var mensajes = (await _mensajeRepository
            .ObtenerPorClienteAsync(id)).ToList();

        foreach (var mensaje in mensajes)
        {
            await _mensajeRepository.EliminarAsync(mensaje);
            resumen.Mensajes++;
        }

        var garantes = (await _garanteRepository
            .ObtenerPorClienteAsync(id)).ToList();

        foreach (var garante in garantes)
        {
            await _garanteRepository.EliminarAsync(garante);
            resumen.Garantes++;
        }

        await _clienteRepository.EliminarAsync(cliente);
        await _clienteRepository.GuardarCambiosAsync();

        return resumen;
    }

    private async Task<ResumenCascada> ContarRegistrosAsync(Guid clienteId)
    {
        var resumen = new ResumenCascada();
        var prestamos = (await _prestamoRepository
            .ObtenerPorClienteAsync(clienteId)).ToList();

        resumen.Prestamos = prestamos.Count;

        foreach (var prestamo in prestamos)
        {
            resumen.Pagos += (await _pagoRepository
                .ObtenerPorPrestamoAsync(prestamo.Id)).Count();
        }

        resumen.Mensajes = (await _mensajeRepository
            .ObtenerPorClienteAsync(clienteId)).Count();

        resumen.Garantes = (await _garanteRepository
            .ObtenerPorClienteAsync(clienteId)).Count();

        return resumen;
    }

    public async Task<bool> CambiarEstadoAsync(Guid id, string estado)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente is null)
        {
            return false;
        }

        cliente.Estado = estado;

        await _clienteRepository.ActualizarAsync(cliente);
        await _clienteRepository.GuardarCambiosAsync();

        return true;
    }

    private static ClienteDto MapearDto(Cliente cliente)
    {
        return new ClienteDto
        {
            Id = cliente.Id,
            TipoDocumento = cliente.TipoDocumento,
            NumeroDocumento = cliente.NumeroDocumento,
            Nombres = cliente.Nombres,
            Apellidos = cliente.Apellidos,
            Telefono = cliente.Telefono,
            Direccion = cliente.Direccion,
            ReferenciaDireccion = cliente.ReferenciaDireccion,
            FotoReciboServicio = cliente.FotoReciboServicio,
            Observaciones = cliente.Observaciones,
            Estado = cliente.Estado,
            UsuarioRegistraId = cliente.UsuarioRegistraId,
            FechaRegistro = cliente.FechaRegistro
        };
    }

    private Guid? ObtenerUsuarioActualId()
    {
        var valor = _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(valor, out var id) ? id : null;
    }
}