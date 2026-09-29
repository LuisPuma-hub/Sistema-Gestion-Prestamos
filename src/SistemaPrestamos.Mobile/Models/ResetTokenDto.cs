namespace SistemaPrestamos.Mobile.Models;

public class ResetTokenDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiraEn { get; set; }
}
