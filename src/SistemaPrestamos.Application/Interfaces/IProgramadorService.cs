namespace SistemaPrestamos.Application.Interfaces;

public interface IProgramadorService
{
    Task<int> EjecutarPendientesAsync(DateTime ahoraUtc);
}
