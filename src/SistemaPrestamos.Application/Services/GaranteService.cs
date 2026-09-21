using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class GaranteService : IGaranteService
{
    private readonly IGaranteRepository _garanteRepository;

    public GaranteService(IGaranteRepository garanteRepository)
    {
        _garanteRepository = garanteRepository;
    }

    public async Task<IEnumerable<GaranteDto>> ObtenerPorClienteAsync(
        Guid clienteId)
    {
        var garantes = await _garanteRepository
            .ObtenerPorClienteAsync(clienteId);

        return garantes.Select(MapearDto);
    }

    public async Task<GaranteDto?> ObtenerPorIdAsync(Guid id)
    {
        var garante = await _garanteRepository.ObtenerPorIdAsync(id);

        return garante is null ? null : MapearDto(garante);
    }

    public async Task<GaranteDto> CrearAsync(CrearGaranteDto dto)
    {
        Validar(dto);

        var garante = new Garante
        {
            Id = Guid.NewGuid(),
            Nombres = dto.Nombres.Trim(),
            Apellidos = dto.Apellidos.Trim(),
            Telefono = dto.Telefono.Trim(),
            Direccion = string.IsNullOrWhiteSpace(dto.Direccion)
                ? null
                : dto.Direccion.Trim(),
            ClienteId = dto.ClienteId,
            FechaRegistro = DateTime.UtcNow
        };

        await _garanteRepository.CrearAsync(garante);
        await _garanteRepository.GuardarCambiosAsync();

        return MapearDto(garante);
    }

    public async Task<bool> ActualizarAsync(Guid id, CrearGaranteDto dto)
    {
        var garante = await _garanteRepository.ObtenerPorIdAsync(id);

        if (garante is null)
        {
            return false;
        }

        Validar(dto);

        garante.Nombres = dto.Nombres.Trim();
        garante.Apellidos = dto.Apellidos.Trim();
        garante.Telefono = dto.Telefono.Trim();
        garante.Direccion = string.IsNullOrWhiteSpace(dto.Direccion)
            ? null
            : dto.Direccion.Trim();
        garante.ClienteId = dto.ClienteId;

        await _garanteRepository.ActualizarAsync(garante);
        await _garanteRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(Guid id)
    {
        var garante = await _garanteRepository.ObtenerPorIdAsync(id);

        if (garante is null)
        {
            return false;
        }

        await _garanteRepository.EliminarAsync(garante);
        await _garanteRepository.GuardarCambiosAsync();

        return true;
    }

    private static void Validar(CrearGaranteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombres) ||
            string.IsNullOrWhiteSpace(dto.Apellidos) ||
            string.IsNullOrWhiteSpace(dto.Telefono))
        {
            throw new InvalidOperationException(
                "Nombres, apellidos y teléfono del garante son obligatorios.");
        }
    }

    private static GaranteDto MapearDto(Garante garante)
    {
        return new GaranteDto
        {
            Id = garante.Id,
            Nombres = garante.Nombres,
            Apellidos = garante.Apellidos,
            Telefono = garante.Telefono,
            Direccion = garante.Direccion,
            ClienteId = garante.ClienteId,
            FechaRegistro = garante.FechaRegistro
        };
    }
}
