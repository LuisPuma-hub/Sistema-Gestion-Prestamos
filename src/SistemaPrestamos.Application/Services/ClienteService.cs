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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClienteService(
        IClienteRepository clienteRepository,
        IPrestamoRepository prestamoRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _clienteRepository = clienteRepository;
        _prestamoRepository = prestamoRepository;
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

    public async Task<bool> EliminarAsync(Guid id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);

        if (cliente is null)
        {
            return false;
        }

        var prestamos = await _prestamoRepository
            .ObtenerPorClienteAsync(id);

        if (prestamos.Any())
        {
            throw new InvalidOperationException(
                "No se puede eliminar un cliente con préstamos registrados.");
        }

        await _clienteRepository.EliminarAsync(cliente);
        await _clienteRepository.GuardarCambiosAsync();

        return true;
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