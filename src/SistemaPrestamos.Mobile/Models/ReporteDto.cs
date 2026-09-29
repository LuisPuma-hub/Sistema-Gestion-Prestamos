namespace SistemaPrestamos.Mobile.Models;

public class CobranzaDto
{
    public DateTime Fecha { get; set; }

    public decimal Total { get; set; }

    public decimal AInteres { get; set; }

    public decimal ACapital { get; set; }

    public int NumPagos { get; set; }
}

public class CarteraFilaDto
{
    public Guid PrestamoId { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public decimal CapitalPendiente { get; set; }

    public decimal InteresesPendientes { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = string.Empty;
}

public class CarteraDto
{
    public decimal CapitalTotal { get; set; }

    public decimal InteresesTotal { get; set; }

    public decimal DeudaTotal { get; set; }

    public int NumPrestamos { get; set; }

    public List<CarteraFilaDto> Filas { get; set; } = new();
}

public class IngresoDiaDto
{
    public DateTime Fecha { get; set; }

    public decimal Intereses { get; set; }

    public decimal Capital { get; set; }

    public decimal Total { get; set; }

    public int NumPagos { get; set; }

    public string Dia =>
        Fecha.ToLocalTime().ToString("dd/MM");
}
