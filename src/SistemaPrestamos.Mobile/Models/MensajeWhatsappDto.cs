namespace SistemaPrestamos.Mobile.Models;

public class MensajeWhatsappDto
{
    public Guid Id { get; set; }

    public Guid? ClienteId { get; set; }

    public Guid? PrestamoId { get; set; }

    public string TipoPlantilla { get; set; } = string.Empty;

    public string NumeroDestino { get; set; } = string.Empty;

    public string Contenido { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string? IdentificadorExterno { get; set; }

    public DateTime? FechaEnvio { get; set; }

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
