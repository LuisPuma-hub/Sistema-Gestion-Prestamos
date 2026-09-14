# Reglas de Morosidad

## RN-MOR-001

Cada periodo de interés tendrá una fecha de vencimiento.

## RN-MOR-002

Un periodo se considerará vencido si y solo si a las 23:59:59 America/Lima de su `fecha_vencimiento` se cumple `pagado_interes < interes_periodo`.

- Sin periodo de gracia en v1 (gracia = 0h).
- Pagos con `fecha_valor <= fecha_vencimiento` cuentan aunque se registren hasta 2 días después (ventana de registro; distinta del backdate máx. 7 días de RN-PAG-002); fuera de ese plazo no salvan el vencimiento.
- Un abono parcial (PARCIAL) sigue contando como vencido.

## RN-MOR-003

Los intereses vencidos continuarán formando parte de la deuda.

## RN-MOR-004

Los intereses vencidos no generarán nuevos intereses.

En v1 `tasa_mora = 0.00` y `monto_mora = 0` (sin recargo; la mora solo se registra y cuenta para el umbral RN-MOR-005). El orden `mora → interés → capital` se mantiene por compatibilidad futura, con mora siempre 0 en v1.

## RN-MOR-005

Regla determinista (sin discrecionalidad): el préstamo pasará a MOROSO si y solo si `COUNT(periodos_vencidos_impagos) >= 3`.

- Evaluación por job diario `DetectarMorosidad` a las 00:05 America/Lima.
- Al cumplirse: cambia a MOROSO + inserta `Historial_Morosidad` + dispara notificación/WhatsApp de morosidad.
- 1-2 periodos vencidos = ATRASADO, no MOROSO. Se elimina el término ambiguo "pago a destiempo" y "no necesariamente".

## RN-MOR-006

El administrador podrá reactivar manualmente el préstamo MOROSO a ACTIVO solo si se paga al menos 1 periodo vencido o se registra motivo (10-200 chars) + actor + fecha. No resetea el contador de vencidos (ver RN-PRE-012).

## RN-MOR-007

La reactivación no eliminará la deuda existente.

## RN-MOR-008

La condición de morosidad deberá quedar registrada en `Historial_Morosidad(id, prestamo_id, fecha_evento, de_estado, a_estado, periodos_vencidos_count, actor, motivo)`.

## RN-MOR-009

Independencia parcial:

- Préstamo MOROSO dispara trigger automático que pone al cliente en MOROSO.
- Cliente MOROSO no cambia el estado de sus préstamos.
- El cliente solo vuelve a ACTIVO cuando tiene 0 préstamos en MOROSO.
- Esto permite filtrar cobranza por ambos estados sin inconsistencias (préstamo MOROSO + cliente ACTIVO queda prohibido).