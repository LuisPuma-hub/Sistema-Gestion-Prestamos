# Reglas de Clientes

## RN-CLI-001

Cada cliente deberá tener un tipo de documento:

- DNI
- CE

## RN-CLI-002

El número de documento deberá ser único.

## RN-CLI-003

El cliente deberá registrar:

- Nombres.
- Apellidos.
- Número de celular.

## RN-CLI-004

Los recibos de luz y agua serán documentos asociados al cliente.

## RN-CLI-005

El cliente podrá tener observaciones registradas por el administrador.

## RN-CLI-006

El estado del cliente será independiente del estado del préstamo.

Estados iniciales:

- ACTIVO
- EN_OBSERVACION
- MOROSO
- INACTIVO
- BLOQUEADO

## RN-CLI-007

Un cliente podrá tener historial de múltiples préstamos.

## RN-CLI-008

Inicialmente solamente podrá tener un préstamo activo simultáneamente.

Esta restricción podrá modificarse posteriormente.

## RN-CLI-009

Un cliente podrá actuar como aval de otro cliente.