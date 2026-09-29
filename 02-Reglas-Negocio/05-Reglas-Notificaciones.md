# Reglas de Notificaciones

## RN-NOT-001

Las notificaciones estarán dirigidas inicialmente al administrador.

## RN-NOT-002

El administrador podrá configurar los horarios.

Ejemplo:

- 08:00
- 16:00

## RN-NOT-003

El sistema deberá permitir agregar o modificar horarios posteriormente.

## RN-NOT-004

Las notificaciones deberán funcionar aunque:

- La aplicación esté cerrada.
- El usuario no esté dentro de la aplicación.
- El dispositivo esté en segundo plano.

## RN-NOT-005

Se utilizará Firebase Cloud Messaging.

## RN-NOT-006

Las notificaciones podrán informar:

- Cobros próximos.
- Cobros vencidos.
- Clientes morosos.
- Pagos registrados.
- Situaciones importantes de cobranza.

> Implementado: scheduler minutal (`ProgramadorJob`) que ejecuta reglas por hora de Lima con ventana de 15 min. Eventos: `VenceHoy`, `VenceManana`, `MoraNueva`, `MoraPersistente`, `ResumenDiario`, `MoraCobrador`, `CobradoDia`, `PrestamoPorAprobar`, `ResumenVencimientos`. Semillas: vencen hoy 08:00, vencen mañana 16:00, resumen cobrador 07:30. Log antispam en `EnvioNotificacion` + endpoint de diagnóstico. La importación masiva no dispara notificaciones.

## RN-NOT-007

El backend deberá controlar la programación de las notificaciones.

## RN-NOT-008

La aplicación móvil será responsable de recibir y mostrar la notificación.