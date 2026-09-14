# Reglas de Préstamos

## RN-PRE-001

Un préstamo deberá estar asociado a un cliente.

## RN-PRE-002

El préstamo se origina a partir de una solicitud aprobada.

## RN-PRE-003

La tasa de interés por defecto en v1 será 5% semanal fija (TASA_SEMANAL_DEFAULT = 0.05).

- Moneda: PEN (S/, ISO 4217).
- Almacenamiento DB: `NUMERIC(5,2)` en porcentaje (`5.00` = 5%). Exposición API: decimal en tanto por uno (`0.05`). Conversión: `tasa_api = tasa_db / 100`. No mezclar unidades.
- Campo `Prestamo.tasa_semanal`: se fija al crear el préstamo, es inmutable después.
- En v1 no es editable por pantalla; cualquier cambio de tasa por defecto requiere nueva versión de esta regla y solo aplica a préstamos nuevos (snapshot por préstamo).
- Cálculo por periodo: `interes_periodo = ROUND(monto_inicial * tasa_semanal, 2, HALF_UP)`.
- Rango permitido a futuro: [0.00 - 0.10]. TAE informativa: (1.05^52 - 1) ≈ 1164%. Validar tope legal vigente antes de operar.

## RN-PRE-004

El interés se calculará siempre sobre el monto inicial.

Ejemplo:

Monto inicial: S/ 100  
Interés: 5%  
Interés semanal: S/ 5

## RN-PRE-005

El interés no disminuirá cuando disminuya el capital.

Ejemplo:

Monto inicial: S/ 100  
Capital pendiente: S/ 50  
Interés semanal: S/ 5

## RN-PRE-006

Los intereses no generarán intereses adicionales.

No existe interés sobre interés.

## RN-PRE-007

El periodo de interés será semanal.

Amortización v1 (prorrateo): cada cuota lleva `interes_programado` = interés semanal fijo y `capital_programado = ROUND(capital_inicial/plazo,2)` (última cuota ajusta por redondeo). Los abonos a capital reducen `capital_pendiente` del préstamo; las cuotas futuras conservan su `interes_programado` (interés sobre inicial, no sobre saldo).

## RN-PRE-008

Regla general: cada periodo semanal vence a los 7 días calendario de su inicio, a las 23:59:59 America/Lima.

- `fecha_vencimiento = fecha_inicio + 7 días, hora 23:59:59 America/Lima` (tipo `datetimeoffset`).
- Ejemplo (instancia): si inicia lunes 2026-09-07 00:00 -05:00, vence lunes 2026-09-14 23:59:59 -05:00.
- Feriados y fines de semana no desplazan el vencimiento. Año bisiesto no altera el cómputo (7 días naturales).

## RN-PRE-009

El préstamo permanecerá activo (estado ACTIVO o MOROSO) mientras exista capital pendiente > 0 o intereses pendientes > 0.

## RN-PRE-010

El préstamo se considerará CANCELADO solo cuando se cumpla simultáneamente:

- `capital_pendiente == 0 AND intereses_pendientes == 0`.

Si un pago intentara saldar capital dejando intereses pendientes, el sistema deberá redistribuir primero a intereses (RN-PAG-003) o rechazar la operación. En ningún caso un préstamo con intereses pendientes podrá pasar a CANCELADO.

## RN-PRE-011

Estados del préstamo (catálogo único y cerrado):

- ACTIVO
- MOROSO
- CANCELADO
- ANULADO

Definiciones:

- ANULADO = rechazo (`PENDIENTE→ANULADO`) o error de registro (`ACTIVO/MOROSO→ANULADO`, solo antes del primer pago). Ambos requieren motivo + actor, revierten periodos (soft-delete) y prohíben pagos posteriores (ver RN-PRE-012). Se distinguen por `de_estado` en `Historial_Estado`; no se crea estado ni campo nuevo.
- CANCELADO = saldado según RN-PRE-010, automático por trigger, terminal.
- Transiciones permitidas: PENDIENTE -> ACTIVO (aprobar, solo ADMIN), PENDIENTE -> ANULADO (rechazar, con motivo), ACTIVO -> MOROSO (automático, job), MOROSO -> ACTIVO (manual, RN-PRE-012), ACTIVO/MOROSO -> CANCELADO (automático al saldar), ACTIVO/MOROSO -> ANULADO (manual pre-primer-pago). Ninguna otra transición está permitida.

## RN-PRE-012

Un préstamo MOROSO podrá ser reactivado manualmente por el administrador a estado ACTIVO, bajo condiciones:

- Requiere pago de al menos 1 periodo vencido, o motivo obligatorio (10-200 chars) + `reactivado_por` + fecha.
- No resetea el contador de periodos vencidos; el job diario podrá devolverlo a MOROSO si aún cumple RN-MOR-005.
- Registra en `Historial_Estado(de, a, motivo, actor, fecha)`. Prohibido reactivar CANCELADO/ANULADO.

## RN-PRE-013

La reactivación no eliminará pagos, intereses ni historial.

## RN-PRE-014

La base de datos permitirá múltiples préstamos históricos para un mismo cliente.