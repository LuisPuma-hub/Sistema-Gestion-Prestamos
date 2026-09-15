using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
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
            FotoReciboServicio = dto.FotoReciboServicio,
            Observaciones = dto.Observaciones,
            Estado = "Activo",
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
        cliente.FotoReciboServicio = dto.FotoReciboServicio;
        cliente.Observaciones = dto.Observaciones;

        await _clienteRepository.ActualizarAsync(cliente);
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
            FotoReciboServicio = cliente.FotoReciboServicio,
            Observaciones = cliente.Observaciones,
            Estado = cliente.Estado,
            FechaRegistro = cliente.FechaRegistro
        };
    }
}