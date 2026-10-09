namespace SistemaPrestamos.Mobile.Models;

public class EnvioDto
{
    public Guid Id { get; set; }

    public string Evento { get; set; } = string.Empty;

    public string Canal { get; set; } = string.Empty;

    public string Destinatario { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string? Detalle { get; set; }

    public DateTime FechaCreacion { get; set; }

    // Backend guarda UTC. Mostrar siempre en hora Lima (UTC-5 fijo).
    public DateTime FechaLima
    {
        get
        {
            var utc = FechaCreacion.Kind == DateTimeKind.Utc
                ? FechaCreacion
                : DateTime.SpecifyKind(FechaCreacion, DateTimeKind.Utc);

            try
            {
                TimeZoneInfo zona;
                try
                {
                    zona = TimeZoneInfo.FindSystemTimeZoneById("America/Lima");
                }
                catch (TimeZoneNotFoundException)
                {
                    zona = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                }
                return TimeZoneInfo.ConvertTimeFromUtc(utc, zona);
            }
            catch
            {
                return utc.AddHours(-5);
            }
        }
    }
}
