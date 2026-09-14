# Reglas de Pagos

## RN-PAG-001

Todo pago deberá estar asociado a un préstamo.

## RN-PAG-002

Todo pago deberá registrar:

- `prestamo_id` (obligatorio), `monto` DECIMAL(10,2) > 0 y <= deuda_total, moneda PEN.
- `fecha_valor` DATE (permite backdate máx. 7 días, nunca futura; ver ventana de 2 días para salvar vencimiento en RN-MOR-002) y `fecha_registro` NOW() del servidor.
- `metodo` ENUM canónico v1: EFECTIVO, YAPE, TRANSFERENCIA, PLIN, DEPOSITO, OTRO (con `OTRO` se recomienda `referencia`; `referencia` sigue `NULL` según diccionario).
- `registrado_por`, `idempotencia-key` UUID (para evitar doble-clic), `comprobante_url` opcional.
- Sin borrado físico (ver RN-PAG-011).

## RN-PAG-003

Los pagos se aplicarán primero a los intereses pendientes, en orden FIFO por `fecha_vencimiento` ascendente (P1, luego P2, ...).

- `intereses_pendientes = SUM(periodos.interes - pagado_interes WHERE estado != PAGADO)`.
- Se guardará `Pago_Detalle(pago_id, periodo_id, a_interes, a_capital)`.
- Un periodo solo pasa a PAGADO si `pagado_interes == interes_total`; un abono parcial lo deja en PARCIAL y sigue contando como vencido para morosidad (RN-MOR-002).

## RN-PAG-004

Una vez cubiertos los intereses pendientes, cualquier excedente se aplicará al capital.

## RN-PAG-005

El capital pendiente no disminuirá por pagos aplicados exclusivamente a intereses.

## RN-PAG-006

Un pago podrá cubrir varios periodos de interés.

## RN-PAG-007

El sistema deberá guardar el detalle de cómo fue distribuido cada pago.

### Ejemplo

Capital inicial: S/ 100

Intereses pendientes: S/ 10

Pago recibido: S/ 30

Distribución:

- Interés: S/ 10
- Capital: S/ 20

Resultado:

Capital pendiente = S/ 80

## RN-PAG-008

Si el pago solamente cubre intereses, el capital permanecerá sin cambios.

## RN-PAG-009

En v1 el sistema rechazará en UI y API todo pago con `monto > deuda_total (capital_pendiente + intereses_pendientes)` con error E-PAG-09 "Monto excede deuda S/X".

No se permite excedente ni vuelto en v1. Si a futuro se requiere, se definirá nueva versión con campo `monto_devuelto` + firma.

## RN-PAG-010

Los valores monetarios deberán manejarse con precisión decimal.

- C#: `decimal`, PG: `NUMERIC(12,2)`, redondeo HALF_UP a 2 decimales. Prohibido `float/double` para dinero.

## RN-PAG-011

Los pagos no se editan ni se borran físicamente.

- La corrección se hace por anulación: crea `Pago_Anulacion` que revierte la distribución y recalcula saldos en una transacción, con motivo obligatorio (10-200 chars) + auditoría (actor, fecha).