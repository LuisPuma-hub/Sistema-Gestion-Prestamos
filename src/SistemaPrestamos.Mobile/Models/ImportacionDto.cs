namespace SistemaPrestamos.Mobile.Models;

public class ImportacionFilaDto
{
    public int IdFila { get; set; }

    public int Linea { get; set; }

    public bool Valida { get; set; }

    public bool ConError => !Valida;

    public string? Error { get; set; }

    public string NumeroDocumento { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public decimal CapitalInicial { get; set; }

    public decimal SaldoFinal { get; set; }

    public int PeriodosGenerados { get; set; }

    public decimal MontoAjuste { get; set; }

    public bool ClienteNuevo { get; set; }

    public string Titulo =>
        $"Fila {IdFila}" +
        (string.IsNullOrWhiteSpace(NombreCompleto)
            ? string.Empty
            : $" - {NombreCompleto}");

    public string Detalle =>
        Valida
            ? $"DNI {NumeroDocumento} - S/ {CapitalInicial:N2} -> " +
              $"S/ {SaldoFinal:N2} ({PeriodosGenerados} sem.)"
            : $"Línea {Linea}: {Error}";
}

public class ImportacionPreviewDto
{
    public int TotalFilas { get; set; }

    public int Validas { get; set; }

    public int ConError { get; set; }

    public DateTime FechaCorte { get; set; }

    public List<ImportacionFilaDto> Filas { get; set; } = new();

    public string Resumen =>
        $"{TotalFilas} filas: {Validas} válidas, " +
        $"{ConError} con error.";
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

    public string Resumen =>
        $"{PrestamosCreados} préstamos creados " +
        $"({ClientesCreados} clientes nuevos, " +
        $"{ClientesReutilizados} reutilizados).";
}
