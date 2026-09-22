using Microsoft.EntityFrameworkCore;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;
using SistemaPrestamos.Infrastructure.Data;

namespace SistemaPrestamos.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly PrestamosDbContext _context;

    public ClienteRepository(PrestamosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
    {
        return await _context.Clientes
            .AsNoTracking()
            .OrderBy(x => x.Apellidos)
            .ThenBy(x => x.Nombres)
            .ToListAsync();
    }

    public async Task<Cliente?> ObtenerPorIdAsync(Guid id)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Cliente?> ObtenerPorDocumentoAsync(
        string numeroDocumento)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(
                x => x.NumeroDocumento == numeroDocumento);
    }

    public async Task<Cliente> CrearAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        return cliente;
    }

    public Task ActualizarAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        return Task.CompletedTask;
    }

    public Task EliminarAsync(Cliente cliente)
    {
        _context.Clientes.Remove(cliente);
        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}