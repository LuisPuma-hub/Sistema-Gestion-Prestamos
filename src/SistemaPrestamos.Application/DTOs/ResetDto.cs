namespace SistemaPrestamos.Application.DTOs;

public class ResetTokenDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiraEn { get; set; }
}

public class CanjearResetDto
{
    public string Token { get; set; } = string.Empty;

    public string Nueva { get; set; } = string.Empty;
}

public class AnularPagoDto
{
    public string Motivo { get; set; } = string.Empty;
}
