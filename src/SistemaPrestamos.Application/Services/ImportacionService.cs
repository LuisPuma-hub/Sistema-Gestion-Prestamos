using System.Globalization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using SistemaPrestamos.Application.DTOs;
using SistemaPrestamos.Application.Interfaces;
using SistemaPrestamos.Domain.Entities;

namespace SistemaPrestamos.Application.Services;

public class ImportacionService : IImportacionService
{
    public const int MaxFilas = 500;

    private const decimal TasaInteresSemanal = 0.05m;

    private const string ObservacionAjuste =
        "CARGA INICIAL - importación masiva sin historial";

    private static readonly string[] CabeceraEsperada =
    [
        "idFila", "tipoDoc", "nroDoc", "nombres", "apellidos",
        "telefono", "direccion", "capitalInicial", "fechaInicio",
        "saldoCapitalActual"
    ];

    private static readonly string[] FormatosFecha =
    [
        "yyyy-MM-dd", "yyyy/MM/dd", "dd/MM/yyyy",
        "d/M/yyyy", "dd-MM-yyyy"
    ];

    private readonly IClienteRepository _clienteRepository;
    private readonly IPrestamoRepository _prestamoRepository;
    private readonly IPeriodoInteresRepository _periodoRepository;
    private readonly IPagoRepository _pagoRepository;
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public ImportacionService(
        IClienteRepository clienteRepository,
        IPrestamoRepository prestamoRepository,
        IPeriodoInteresRepository periodoRepository,
        IPagoRepository pagoRepository,
        IHttpContextAccessor? httpContextAccessor = null)
    {
        _clienteRepository = clienteRepository;
        _prestamoRepository = prestamoRepository;
        _periodoRepository = periodoRepository;
        _pagoRepository = pagoRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ImportacionPreviewDto> PrevisualizarAsync(
        string contenidoCsv,
        DateTime? fechaCorte = null)
    {
        var corte = NormalizarCorte(fechaCorte);
        var filas = await ValidarArchivoAsync(contenidoCsv, corte);

        return new ImportacionPreviewDto
        {
            TotalFilas = filas.Count,
            Validas = filas.Count(x => x.Error is null),
            ConError = filas.Count(x => x.Error is not null),
            FechaCorte = corte,
            Filas = filas.Select(MapearFila).ToList()
        };
    }

    public async Task<ImportacionResultadoDto> ConfirmarAsync(
        string contenidoCsv,
        DateTime? fechaCorte = null)
    {
        var corte = NormalizarCorte(fechaCorte);
        var filas = await ValidarArchivoAsync(contenidoCsv, corte);

        var conError = filas.Count(x => x.Error is not null);

        if (conError > 0)
            throw new InvalidOperationException(
                $"La importación tiene {conError} fila(s) con error. " +
                "Corrija el archivo y vuelva a previsualizar.");

        var ahora = DateTime.UtcNow;
        var resultado = new ImportacionResultadoDto
        {
            FechaCorte = corte
        };

        foreach (var fila in filas)
        {
            var cliente = fila.ClienteExistente ?? new Cliente
            {
                Id = Guid.NewGuid(),
                TipoDocumento = fila.TipoDoc,
                NumeroDocumento = fila.NroDoc,
                Nombres = fila.Nombres,
                Apellidos = fila.Apellidos,
                Telefono = fila.Telefono,
                Direccion = fila.Direccion,
                Estado = "Activo",
                UsuarioRegistraId = ObtenerUsuarioActualId(),
                FechaRegistro = ahora
            };

            if (fila.ClienteExistente is null)
            {
                await _clienteRepository.CrearAsync(cliente);
                resultado.ClientesCreados++;
            }
            else
            {
                resultado.ClientesReutilizados++;
            }

            var prestamo = new Prestamo
            {
                Id = Guid.NewGuid(),
                ClienteId = cliente.Id,
                CapitalInicial = fila.CapitalInicial,
                TasaInteresSemanal = TasaInteresSemanal,
                EsquemaInteres = "Saldo",
                CapitalPendiente = fila.CapitalInicial,
                FechaInicio = fila.FechaInicio,
                FechaAprobacion = ahora,
                Estado = "Activo"
            };

            await _prestamoRepository.CrearAsync(prestamo);
            resultado.PrestamosCreados++;

            // Réplica de PrestamoService + PeriodoInteresService:
            // periodos semanales desde fechaInicio hasta el corte,
            // SIN avisos de WhatsApp durante la importación.
            // La importación siempre crea esquema Saldo.
            var interesSemanal = Math.Round(
                fila.CapitalInicial * TasaInteresSemanal,
                2,
                MidpointRounding.AwayFromZero);

            var periodos = new List<PeriodoInteres>();
            var inicio = fila.FechaInicio.Date;

            while (inicio <= corte)
            {
                var periodo = new PeriodoInteres
                {
                    Id = Guid.NewGuid(),
                    PrestamoId = prestamo.Id,
                    FechaInicio = DateTime.SpecifyKind(
                        inicio, DateTimeKind.Utc),
                    FechaVencimiento = DateTime.SpecifyKind(
                        inicio.AddDays(7), DateTimeKind.Utc),
                    InteresGenerado = interesSemanal,
                    InteresPagado = 0,
                    InteresPendiente = interesSemanal,
                    Estado = "Pendiente",
                    FechaPagoCompleto = null,
                    FechaRegistro = ahora
                };

                await _periodoRepository.CrearAsync(periodo);
                periodos.Add(periodo);

                inicio = inicio.AddDays(7);
            }

            resultado.PeriodosGenerados += periodos.Count;

            if (fila.MontoAjuste > 0)
            {
                foreach (var periodo in periodos)
                {
                    periodo.InteresPagado = periodo.InteresGenerado;
                    periodo.InteresPendiente = 0;
                    periodo.Estado = "Pagado";
                    periodo.FechaPagoCompleto = corte;
                }

                prestamo.CapitalPendiente = fila.Saldo;

                var pago = new Pago
                {
                    Id = Guid.NewGuid(),
                    PrestamoId = prestamo.Id,
                    Monto = fila.MontoAjuste,
                    MontoInteres = fila.InteresesTotales,
                    MontoCapital = fila.CapitalInicial - fila.Saldo,
                    FechaPago = corte,
                    Comprobante = null,
                    Observaciones = ObservacionAjuste,
                    Estado = "Registrado",
                    Detalle =
                        $"Ajuste CARGA INICIAL: {periodos.Count} sem. " +
                        $"S/ {fila.InteresesTotales:N2} int. + " +
                        $"S/ {fila.CapitalInicial - fila.Saldo:N2} cap.",
                    FechaRegistro = ahora
                };

                await _pagoRepository.CrearAsync(pago);

                resultado.PagosAjuste++;
                resultado.MontoAjusteTotal += fila.MontoAjuste;
            }
        }

        // Un solo GuardarCambios: atomicidad total. Si algo falla,
        // EF revierte todo (rollback) y no queda nada a medias.
        // La auditoría se genera automáticamente en el DbContext.
        await _prestamoRepository.GuardarCambiosAsync();

        return resultado;
    }

    private async Task<List<FilaValidada>> ValidarArchivoAsync(
        string contenidoCsv,
        DateTime corte)
    {
        if (string.IsNullOrWhiteSpace(contenidoCsv))
            throw new InvalidOperationException(
                "El archivo está vacío.");

        var lineas = contenidoCsv
            .Replace("\r\n", "\n")
            .Replace('\r', '\n')
            .Split('\n')
            .Select((texto, indice) => new
            {
                Texto = texto.TrimEnd(),
                Linea = indice + 1
            })
            .Where(x => x.Texto.Length > 0)
            .ToList();

        if (lineas.Count < 2)
            throw new InvalidOperationException(
                "El archivo debe tener cabecera y al menos una fila.");

        var separador = lineas[0].Texto.Count(x => x == ';') >
                        lineas[0].Texto.Count(x => x == ',')
            ? ';'
            : ',';

        var cabecera = DividirLinea(lineas[0].Texto, separador)
            .Select(x => x.Trim().ToLowerInvariant())
            .ToList();

        if (!cabecera.SequenceEqual(
                CabeceraEsperada,
                StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "La cabecera no coincide con la plantilla esperada: " +
                string.Join(",", CabeceraEsperada) + ".");

        var datos = lineas.Skip(1).ToList();

        if (datos.Count > MaxFilas)
            throw new InvalidOperationException(
                $"El archivo supera el máximo de {MaxFilas} filas.");

        var filas = new List<FilaValidada>();

        foreach (var item in datos)
        {
            var fila = ValidarFila(
                item.Texto, item.Linea, separador, corte);

            filas.Add(fila);
        }

        var duplicadosId = filas
            .GroupBy(x => x.IdFila)
            .Where(g => g.Key > 0 && g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        foreach (var fila in filas)
        {
            if (fila.Error is not null)
                continue;

            if (duplicadosId.Contains(fila.IdFila))
            {
                fila.Error =
                    $"El idFila {fila.IdFila} está repetido en el archivo.";
            }

            // RN-CLI-008: varias filas del mismo documento están
            // permitidas si el cliente está Activo; el freno es
            // el estado (mora), no la cantidad.
        }

        foreach (var fila in filas)
        {
            if (fila.Error is not null)
                continue;

            await ValidarContraBaseAsync(fila);
        }

        return filas;
    }

    private static FilaValidada ValidarFila(
        string texto,
        int linea,
        char separador,
        DateTime corte)
    {
        var fila = new FilaValidada
        {
            Linea = linea
        };

        var celdas = DividirLinea(texto, separador);

        while (celdas.Count < 10)
            celdas.Add(string.Empty);

        if (celdas.Count > 10)
        {
            fila.Error =
                "La fila debe tener 10 columnas según la plantilla.";
            return fila;
        }

        for (var i = 0; i < celdas.Count; i++)
            celdas[i] = celdas[i].Trim();

        if (!int.TryParse(celdas[0], out var idFila) || idFila <= 0)
        {
            fila.Error = "idFila debe ser un entero mayor que cero.";
            return fila;
        }

        fila.IdFila = idFila;

        var tipoDoc = celdas[1].Trim().ToUpperInvariant();

        if (tipoDoc != "DNI" && tipoDoc != "CE")
        {
            fila.Error = "tipoDoc debe ser DNI o CE.";
            return fila;
        }

        fila.TipoDoc = tipoDoc;

        var nroDoc = celdas[2].Trim();

        if (tipoDoc == "DNI" &&
            (nroDoc.Length != 8 || !nroDoc.All(char.IsDigit)))
        {
            fila.Error = "El DNI debe tener 8 dígitos.";
            return fila;
        }

        if (tipoDoc == "CE" &&
            (nroDoc.Length == 0 || nroDoc.Length > 20 ||
             !nroDoc.All(x => char.IsLetterOrDigit(x) || x == '-')))
        {
            fila.Error =
                "El CE debe tener de 1 a 20 caracteres alfanuméricos.";
            return fila;
        }

        fila.NroDoc = nroDoc;

        if (string.IsNullOrWhiteSpace(celdas[3]) ||
            celdas[3].Length > 100)
        {
            fila.Error = "Nombres es obligatorio (máx. 100).";
            return fila;
        }

        if (string.IsNullOrWhiteSpace(celdas[4]) ||
            celdas[4].Length > 100)
        {
            fila.Error = "Apellidos es obligatorio (máx. 100).";
            return fila;
        }

        fila.Nombres = celdas[3].Trim();
        fila.Apellidos = celdas[4].Trim();

        var telefono = NormalizarTelefono(celdas[5]);

        if (telefono is null)
        {
            fila.Error =
                "Teléfono inválido: celular peruano de 9 dígitos " +
                "que empiece con 9.";
            return fila;
        }

        fila.Telefono = telefono;
        fila.Direccion = celdas[6].Trim();

        if (fila.Direccion.Length > 255)
        {
            fila.Error = "Dirección supera 255 caracteres.";
            return fila;
        }

        if (!ParseDecimal(celdas[7], out var capital) || capital <= 0)
        {
            fila.Error = "capitalInicial debe ser mayor que cero.";
            return fila;
        }

        fila.CapitalInicial = capital;

        if (!DateTime.TryParseExact(
                celdas[8].Trim(),
                FormatosFecha,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var fechaInicio))
        {
            fila.Error =
                "fechaInicio inválida (use AAAA-MM-DD).";
            return fila;
        }

        fechaInicio = DateTime.SpecifyKind(
            fechaInicio.Date, DateTimeKind.Utc);

        if (fechaInicio.Date > corte.Date)
        {
            fila.Error = "fechaInicio no puede ser futura.";
            return fila;
        }

        if ((corte.Date - fechaInicio.Date).TotalDays > 5 * 365)
        {
            fila.Error =
                "fechaInicio muy antigua (máx. 5 años).";
            return fila;
        }

        fila.FechaInicio = fechaInicio;

        decimal saldo = capital;

        if (!string.IsNullOrWhiteSpace(celdas[9]))
        {
            if (!ParseDecimal(celdas[9], out saldo) || saldo < 0)
            {
                fila.Error =
                    "saldoCapitalActual debe ser cero o mayor.";
                return fila;
            }

            if (saldo > capital)
            {
                fila.Error =
                    "saldoCapitalActual no puede superar " +
                    "capitalInicial.";
                return fila;
            }

            if (saldo == 0)
            {
                fila.Error =
                    "saldoCapitalActual en cero implica préstamo " +
                    "cancelado: esta importación es solo de activos " +
                    "con saldo pendiente.";
                return fila;
            }
        }

        fila.Saldo = saldo;

        var periodos = ContarPeriodos(fechaInicio.Date, corte.Date);

        fila.Periodos = periodos;
        fila.InteresesTotales =
            periodos * Math.Round(
                capital * TasaInteresSemanal,
                2,
                MidpointRounding.AwayFromZero);
        fila.MontoAjuste = saldo == capital
            ? 0
            : fila.InteresesTotales + (capital - saldo);

        return fila;
    }

    private async Task ValidarContraBaseAsync(FilaValidada fila)
    {
        var existente = await _clienteRepository
            .ObtenerPorDocumentoAsync(fila.NroDoc);

        if (existente is null)
            return;

        if (!string.Equals(
                existente.TipoDocumento?.Trim(),
                fila.TipoDoc,
                StringComparison.OrdinalIgnoreCase) ||
            !Iguales(existente.Nombres, fila.Nombres) ||
            !Iguales(existente.Apellidos, fila.Apellidos))
        {
            fila.Error =
                $"El documento {fila.NroDoc} ya existe con otro " +
                "nombre o tipo de documento.";
            return;
        }

        if (!string.Equals(
                existente.Estado?.Trim(),
                "Activo",
                StringComparison.OrdinalIgnoreCase))
        {
            fila.Error =
                $"El cliente {fila.NroDoc} existe pero no está " +
                $"activo (estado: {existente.Estado}).";
            return;
        }

        fila.ClienteExistente = existente;
    }

    private static ImportacionFilaDto MapearFila(FilaValidada fila)
    {
        return new ImportacionFilaDto
        {
            IdFila = fila.IdFila,
            Linea = fila.Linea,
            Valida = fila.Error is null,
            Error = fila.Error,
            NumeroDocumento = fila.NroDoc,
            NombreCompleto = fila.Error is null
                ? $"{fila.Nombres} {fila.Apellidos}"
                : string.Empty,
            CapitalInicial = fila.CapitalInicial,
            SaldoFinal = fila.Saldo,
            PeriodosGenerados = fila.Periodos,
            MontoAjuste = fila.MontoAjuste,
            ClienteNuevo = fila.Error is null &&
                           fila.ClienteExistente is null
        };
    }

    private static DateTime NormalizarCorte(DateTime? fechaCorte)
    {
        var baseFecha = fechaCorte?.Date ?? DateTime.UtcNow.Date;

        return DateTime.SpecifyKind(baseFecha, DateTimeKind.Utc);
    }

    private static int ContarPeriodos(DateTime inicio, DateTime corte)
    {
        var conteo = 0;
        var actual = inicio;

        while (actual <= corte)
        {
            conteo++;
            actual = actual.AddDays(7);
        }

        return conteo;
    }

    private static string? NormalizarTelefono(string valor)
    {
        var digitos = new string(
            valor.Where(char.IsDigit).ToArray());

        if (digitos.StartsWith("51") && digitos.Length == 11)
            digitos = digitos[2..];

        if (digitos.Length == 9 &&
            digitos.StartsWith("9") &&
            digitos.All(char.IsDigit))
            return digitos;

        return null;
    }

    private static bool ParseDecimal(string valor, out decimal numero)
    {
        var limpio = valor.Trim()
            .Replace("S/", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Trim();

        if (decimal.TryParse(
                limpio,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out numero))
        {
            numero = Math.Round(
                numero, 2, MidpointRounding.AwayFromZero);
            return true;
        }

        if (decimal.TryParse(
                limpio,
                NumberStyles.Number,
                new CultureInfo("es-PE"),
                out numero))
        {
            numero = Math.Round(
                numero, 2, MidpointRounding.AwayFromZero);
            return true;
        }

        numero = 0;
        return false;
    }

    private static bool Iguales(string? a, string? b)
    {
        return string.Equals(
            NormalizarNombre(a),
            NormalizarNombre(b),
            StringComparison.Ordinal);
    }

    private static string NormalizarNombre(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return string.Empty;

        return Regex.Replace(valor.Trim(), @"\s+", " ")
            .ToUpperInvariant();
    }

    private static List<string> DividirLinea(
        string linea,
        char separador)
    {
        var celdas = new List<string>();
        var actual = new System.Text.StringBuilder();
        var entreComillas = false;

        for (var i = 0; i < linea.Length; i++)
        {
            var c = linea[i];

            if (c == '"')
            {
                if (entreComillas &&
                    i + 1 < linea.Length &&
                    linea[i + 1] == '"')
                {
                    actual.Append('"');
                    i++;
                }
                else
                {
                    entreComillas = !entreComillas;
                }
            }
            else if (c == separador && !entreComillas)
            {
                celdas.Add(actual.ToString());
                actual.Clear();
            }
            else
            {
                actual.Append(c);
            }
        }

        celdas.Add(actual.ToString());

        return celdas;
    }

    private sealed class FilaValidada
    {
        public int IdFila { get; set; }

        public int Linea { get; set; }

        public string? Error { get; set; }

        public string TipoDoc { get; set; } = string.Empty;

        public string NroDoc { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public decimal CapitalInicial { get; set; }

        public DateTime FechaInicio { get; set; }

        public decimal Saldo { get; set; }

        public int Periodos { get; set; }

        public decimal InteresesTotales { get; set; }

        public decimal MontoAjuste { get; set; }

        public Cliente? ClienteExistente { get; set; }
    }

    private Guid? ObtenerUsuarioActualId()
    {
        var valor = _httpContextAccessor?.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(valor, out var id) ? id : null;
    }
}
