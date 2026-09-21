namespace SistemaPrestamos.Application.Interfaces;

public interface INotificacionService
{
    Task<int> EnviarAUsuarioAsync(
        Guid usuarioId,
        string titulo,
        string cuerpo);

    Task EnviarATokenAsync(
        string token,
        string titulo,
        string cuerpo);
}
