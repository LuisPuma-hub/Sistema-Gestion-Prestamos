using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Interfaces;

public interface IAuditoriaRepository
{
    Task<IEnumerable<Auditoria>> ObtenerRecientesAsync(int top);
}
