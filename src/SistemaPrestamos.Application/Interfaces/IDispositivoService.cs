namespace SistemaPrestamos.Application.Interfaces;

public interface IDispositivoService
{
    Task RegistrarAsync(Guid usuarioId, string token, string plataforma);
}
