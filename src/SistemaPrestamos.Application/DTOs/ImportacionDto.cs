namespace SistemaPrestamos.Application.DTOs;

public class ImportacionFilaDto
{
    public int IdFila { get; set; }

    public int Linea { get; set; }

    public bool Valida { get; set; }

    public string? Error { get; set; }

    public string NumeroDocumento { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public decimal CapitalInicial { get; set; }

    public decimal SaldoFinal { get; set; }

    public int PeriodosGenerados { get; set; }

    public decimal MontoAjuste { get; set; }

    public bool ClienteNuevo { get; set; }
}

public class ImportacionPreviewDto
{
    public int TotalFilas { get; set; }

    public int Validas { get; set; }

    public int ConError { get; set; }

    public DateTime FechaCorte { get; set; }

    public List<ImportacionFilaDto> Filas { get; set; } = new();
}

public class ImportacionResultadoDto
{
    public int PrestamosCreados { get; set; }

    public int ClientesCreados { get; set; }

    public int ClientesReutilizados { get; set; }

    public int PeriodosGenerados { get; set; }

    public int PagosAjuste { get; set; }

    public decimal MontoAjusteTotal { get; set; }

    public DateTime FechaCorte { get; set; }
}
