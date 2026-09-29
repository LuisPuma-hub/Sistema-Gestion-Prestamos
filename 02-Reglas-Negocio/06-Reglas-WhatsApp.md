# Reglas de WhatsApp

## RN-WHA-001

WhatsApp no será utilizado como sistema de chat.

## RN-WHA-002

El sistema utilizará WhatsApp Business Platform de Meta o una solución compatible.

## RN-WHA-003

Los mensajes automáticos utilizarán plantillas cuando sean requeridas por las políticas de WhatsApp.

## RN-WHA-004

Se podrán utilizar plantillas para:

- Recordatorio de pago.
- Cobro próximo.
- Cobro vencido.
- Confirmación de pago.
- Información del préstamo.
- Marketing.

> Catálogo vigente en v1 (único válido): `recordatorio_pago_v2`, `aviso_mora`, `confirmacion_pago`, `prestamo_aprobado` (idioma `es_PE`). Se envían `prestamo_aprobado` al aprobar y `confirmacion_pago` al pagar; los recordatorios y avisos de mora los envía el `ProgramadorService`.

## RN-WHA-005

Los mensajes podrán incluir información dinámica.

Ejemplo:

Hola {{nombre}}, te recordamos que tienes un pago de {{monto}} correspondiente a tu préstamo.

## RN-WHA-006

El sistema deberá registrar el resultado del envío.

Estados posibles:

- PENDIENTE
- ENVIADO
- ENTREGADO
- LEIDO
- FALLIDO

> Implementado en v1: se persiste `Enviado` (o `Fallido`) con el identificador externo de Meta. El envío es best-effort (no bloquea la operación y no reintenta). La importación masiva suprime los envíos.

## RN-WHA-007

El sistema deberá guardar el identificador externo proporcionado por Meta cuando corresponda.

## RN-WHA-008

Los mensajes de marketing deberán cumplir las políticas y requisitos de consentimiento correspondientes.