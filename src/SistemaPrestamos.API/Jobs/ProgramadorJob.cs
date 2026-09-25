using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Application.Services;

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

    public static DateTime? UltimoTickUtc { get; private set; }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        using var temporizador = new PeriodicTimer(Intervalo);

        while (await temporizador.WaitForNextTickAsync(stoppingToken))
        {
            UltimoTickUtc = DateTime.UtcNow;

            try
            {
                using var alcance = _servicios.CreateScope();

                var programador = alcance.ServiceProvider
                    .GetRequiredService<IProgramadorService>();

                var lima = ProgramadorService.AhoraLima(DateTime.UtcNow);

                _logger.LogInformation(
                    "Tick programador {HoraLima:HH:mm:ss} dow {Dow}.",
                    lima,
                    lima.DayOfWeek);

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
