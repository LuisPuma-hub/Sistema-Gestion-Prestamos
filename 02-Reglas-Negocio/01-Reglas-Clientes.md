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

> Excepción implementada (RN-MOR-009): un préstamo que pasa a MOROSO pone al cliente en MOROSO automáticamente, y el cliente vuelve a ACTIVO solo cuando tiene 0 préstamos en MOROSO.

## RN-CLI-007

Un cliente podrá tener historial de múltiples préstamos.

## RN-CLI-008

Un cliente Activo podrá tener varios préstamos vigentes simultáneamente.

El freno es el estado, no la cantidad: con cliente en Mora (u otro estado no Activo) no se crea ni se aprueba ningún préstamo hasta regularizar.

> Implementado y verificado: `CrearAsync`/`AprobarAsync` e importación exigen cliente Activo.

## RN-CLI-009

Un cliente podrá actuar como aval de otro cliente.