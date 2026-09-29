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

> Implementado: `MorosidadJob` evalúa los préstamos vigentes cada día a las 00:05 Lima, además de la evaluación al consultar. Al cumplirse, el préstamo pasa a MOROSO y el cliente a MOROSO (RN-MOR-009). Los avisos los envía después el `ProgramadorService`, no la evaluación. Al saldar (CANCELADO), la mora se cierra y el cliente se sincroniza.

## RN-MOR-006

El administrador podrá reactivar manualmente el préstamo MOROSO a ACTIVO solo si se paga al menos 1 periodo vencido o se registra motivo (10-200 chars) + actor + fecha. No resetea el contador de vencidos (ver RN-PRE-012).

> Implementado: `ReactivarAsync` devuelve el préstamo a ACTIVO y sincroniza al cliente (a observación si no quedan morosos). Además, el pago que deja < 3 vencidos reactiva solo (vía pago, RN-PRE-012) y cierra la mora. Las observaciones son opcionales en v1: pendiente endurecer motivo obligatorio 10-200 o pago previo.

## RN-MOR-007

La reactivación no eliminará la deuda existente.

## RN-MOR-008

La condición de morosidad deberá quedar registrada en `Historial_Morosidad(id, prestamo_id, fecha_evento, de_estado, a_estado, periodos_vencidos_count, actor, motivo)`.

> Implementado de forma equivalente en v1: tabla `morosidades` (`PagosInteresVencidos`, `FechaInicio`, `FechaReactivacion`, `Activa`) + auditoría automática de cada cambio. Sin tabla `Historial_Morosidad` separada.

## RN-MOR-009

Independencia parcial:

- Préstamo MOROSO dispara trigger automático que pone al cliente en MOROSO.
- Cliente MOROSO no cambia el estado de sus préstamos.
- El cliente solo vuelve a ACTIVO cuando tiene 0 préstamos en MOROSO.
- Esto permite filtrar cobranza por ambos estados sin inconsistencias (préstamo MOROSO + cliente ACTIVO queda prohibido).

> Implementado: al salir de mora el cliente pasa a `En observación` (no directo a `Activo`); el pase a `Activo` lo hace el administrador manualmente. El cambio manual a `Activo` con préstamos en mora se rechaza.