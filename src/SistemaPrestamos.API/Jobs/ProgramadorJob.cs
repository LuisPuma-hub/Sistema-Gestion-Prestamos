using SistemaPrestamos.Application.Interfaces;

namespace SistemaPrestamos.API.Jobs;

/// <summary>
/// Revisa cada minuto si hay reglas programadas que tocan
/// y las ejecuta. El estado UltimaEjecucion evita repeticiones.
/// </summary>
public class ProgramadorJob : BackgroundService
{
    private static readonly TimeSpan Intervalo = TimeSpan.FromMinutes(1);

    private readonly IServiceProvider _servicios;
    private readonly ILogger<ProgramadorJob> _logger;

    public ProgramadorJob(
        IServiceProvider servicios,
        ILogger<ProgramadorJob> logger)
    {
        _servicios = servicios;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        using var temporizador = new PeriodicTimer(Intervalo);

        while (await temporizador.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var alcance = _servicios.CreateScope();

                var programador = alcance.ServiceProvider
                    .GetRequiredService<IProgramadorService>();

                var enviados = await programador.EjecutarPendientesAsync(
                    DateTime.UtcNow);

                if (enviados > 0)
                {
                    _logger.LogInformation(
                        "Programador ejecutó {Total} envíos.",
                        enviados);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error en el programador de notificaciones.");
            }
        }
    }
}
