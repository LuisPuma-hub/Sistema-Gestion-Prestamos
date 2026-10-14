using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Application.Services;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class CompromisoService : ICompromisoService
{
    private readonly INotaCompromisoRepository _compromisoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IPrestamoRepository? _prestamoRepository;

    public CompromisoService(
        INotaCompromisoRepository compromisoRepository,
        IClienteRepository clienteRepository,
        IPrestamoRepository? prestamoRepository = null)
    {
        _compromisoRepository = compromisoRepository;
        _clienteRepository = clienteRepository;
        _prestamoRepository = prestamoRepository;
    }

    public async Task<CompromisoDto> CrearAsync(
        Guid clienteId,
        CrearCompromisoDto dto,
        Guid? creadoPor)
    {
        var cliente = await _clienteRepository
            .ObtenerPorIdAsync(clienteId);

        if (cliente is null)
            throw new InvalidOperationException(
                "El cliente no existe.");

        if (string.IsNullOrWhiteSpace(dto.Texto) ||
            dto.Texto.Trim().Length > 200)
            throw new InvalidOperationException(
                "El texto es obligatorio (máx. 200 caracteres).");

        Guid? prestamoId = null;

        if (dto.PrestamoId.HasValue &&
            dto.PrestamoId.Value != Guid.Empty)
        {
            if (_prestamoRepository is null)
                throw new InvalidOperationException(
                    "No se puede validar el préstamo.");

            var prestamo = await _prestamoRepository
                .ObtenerPorIdAsync(dto.PrestamoId.Value);

            if (prestamo is null ||
                prestamo.ClienteId != clienteId)
                throw new InvalidOperationException(
                    "El préstamo no es del cliente.");

            prestamoId = prestamo.Id;
        }

        var fecha = dto.FechaCompromiso == default
            ? DateTime.UtcNow.Date
            : DateTime.SpecifyKind(
                dto.FechaCompromiso.Date, DateTimeKind.Utc);

        var limaHoy = ProgramadorService.AhoraLima(
            DateTime.UtcNow).Date;

        if (fecha.Date < limaHoy.AddDays(-30))
            throw new InvalidOperationException(
                "La fecha no puede ser tan pasada.");

        var compromiso = new NotaCompromiso
        {
            Id = Guid.NewGuid(),
            ClienteId = clienteId,
            PrestamoId = prestamoId,
            Texto = dto.Texto.Trim(),
            FechaCompromiso = fecha,
            Estado = "Pendiente",
            CreadoPor = creadoPor,
            FechaCreacion = DateTime.UtcNow
        };

        await _compromisoRepository.CrearAsync(compromiso);
        await _compromisoRepository.GuardarCambiosAsync();

        return Mapear(compromiso, cliente);
    }

    public async Task<IEnumerable<CompromisoDto>> ListarPorClienteAsync(
        Guid clienteId)
    {
        var lista = await _compromisoRepository
            .ObtenerPorClienteAsync(clienteId);

        return lista.Select(x => Mapear(x, x.Cliente));
    }

    public async Task<bool> AnularAsync(Guid id)
    {
        var compromiso = await _compromisoRepository
            .ObtenerPorIdAsync(id);

        if (compromiso is null)
            return false;

        if (compromiso.Estado != "Pendiente")
            throw new InvalidOperationException(
                "Solo se puede anular un compromiso pendiente.");

        compromiso.Estado = "Anulada";

        await _compromisoRepository.ActualizarAsync(compromiso);
        await _compromisoRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<int> CumplirPorPagoAsync(
        Guid clienteId, Guid prestamoId, DateTime fecha)
    {
        var dia = DateTime.SpecifyKind(fecha.Date, DateTimeKind.Utc);

        var pendientes = (await _compromisoRepository
                .ObtenerPorClienteAsync(clienteId))
            .Where(x =>
                x.Estado == "Pendiente" &&
                x.FechaCompromiso.Date == dia &&
                (x.PrestamoId == null || x.PrestamoId == prestamoId))
            .ToList();

        foreach (var c in pendientes)
        {
            c.Estado = "Cumplida";
            c.FechaCumplimiento = DateTime.UtcNow;

            await _compromisoRepository.ActualizarAsync(c);
        }

        if (pendientes.Count > 0)
            await _compromisoRepository.GuardarCambiosAsync();

        return pendientes.Count;
    }

    public async Task<IEnumerable<CompromisoDto>> PendientesDeHoyAsync(
        DateTime hoy)
    {
        var pendientes = await _compromisoRepository
            .ObtenerPendientesAsync();

        return pendientes
            .Where(x => x.FechaCompromiso.Date == hoy.Date)
            .Select(x => Mapear(x, x.Cliente));
    }

    public async Task<int> MarcarVencidosAsync(DateTime hoy)
    {
        var pendientes = (await _compromisoRepository
                .ObtenerPendientesAsync())
            .Where(x => x.FechaCompromiso.Date < hoy.Date)
            .ToList();

        foreach (var c in pendientes)
        {
            c.Estado = "Vencida";

            await _compromisoRepository.ActualizarAsync(c);
        }

        if (pendientes.Count > 0)
            await _compromisoRepository.GuardarCambiosAsync();

        return pendientes.Count;
    }

    public async Task<int> RevertirPorAnulacionAsync(
        Guid clienteId, Guid prestamoId, DateTime fecha)
    {
        var dia = DateTime.SpecifyKind(fecha.Date, DateTimeKind.Utc);

        var limaHoy = ProgramadorService.AhoraLima(
            DateTime.UtcNow).Date;

        var cumplidos = (await _compromisoRepository
                .ObtenerPorClienteAsync(clienteId))
            .Where(x =>
                x.Estado == "Cumplida" &&
                x.FechaCompromiso.Date == dia &&
                (x.PrestamoId == null || x.PrestamoId == prestamoId))
            .ToList();

        foreach (var c in cumplidos)
        {
            c.Estado = c.FechaCompromiso.Date < limaHoy
                ? "Vencida"
                : "Pendiente";
            c.FechaCumplimiento = null;

            await _compromisoRepository.ActualizarAsync(c);
        }

        if (cumplidos.Count > 0)
            await _compromisoRepository.GuardarCambiosAsync();

        return cumplidos.Count;
    }

    private static CompromisoDto Mapear(
        NotaCompromiso c, Cliente? cliente)
    {
        return new CompromisoDto
        {
            Id = c.Id,
            ClienteId = c.ClienteId,
            ClienteNombre = cliente is null
                ? string.Empty
                : $"{cliente.Nombres} {cliente.Apellidos}".Trim(),
            PrestamoId = c.PrestamoId,
            Texto = c.Texto,
            FechaCompromiso = c.FechaCompromiso,
            Estado = c.Estado,
            FechaCumplimiento = c.FechaCumplimiento
        };
    }
}
