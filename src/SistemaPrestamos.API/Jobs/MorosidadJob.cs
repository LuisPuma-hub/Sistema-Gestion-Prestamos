using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Application.Services;

namespace SistemaPrestamos.API.Jobs;

/// <summary>
/// Evalúa la morosidad de los préstamos vigentes todos los días
/// a las 00:05 America/Lima (RN-MOR-005).
/// </summary>
public class MorosidadJob : BackgroundService
{
    private static readonly TimeSpan Intervalo = TimeSpan.FromMinutes(1);

    private readonly IServiceProvider _servicios;
    private readonly ILogger<MorosidadJob> _logger;

    private DateTime? _ultimoDiaEjecutado;

    public MorosidadJob(
        IServiceProvider servicios,
        ILogger<MorosidadJob> logger)
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
                var lima = ProgramadorService.AhoraLima(
                    DateTime.UtcNow);

                // Diaria 00:05 Lima, con puesta al día: si el
                // servicio dormía a esa hora (plan Free), se
                // ejecuta una vez al despertar pasada esa hora.
                var pasadaLaHora =
                    lima.Hour > 0 ||
                    (lima.Hour == 0 && lima.Minute >= 5);

                if (!pasadaLaHora ||
                    _ultimoDiaEjecutado == lima.Date)
                    continue;

                _ultimoDiaEjecutado = lima.Date;

                using var alcance = _servicios.CreateScope();

                var morosidad = alcance.ServiceProvider
                    .GetRequiredService<IMorosidadService>();

                var evaluados = await morosidad.EvaluarVigentesAsync(
                    DateTime.UtcNow);

                _logger.LogInformation(
                    "Job de morosidad evaluó {Total} préstamos.",
                    evaluados);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error en el job de morosidad.");
            }
        }
    }
}
