# Restricciones de Base de Datos

## 1. Introducción

El presente documento define las restricciones que deberá cumplir la base de datos del Sistema de Gestión de Préstamos.

Las restricciones tienen como objetivo garantizar la integridad, consistencia, validez y seguridad de la información almacenada.

Estas restricciones complementan las reglas de negocio definidas para el sistema y deberán ser consideradas durante la implementación de la base de datos.

---

## 2. Objetivos

Las restricciones de la base de datos tienen los siguientes objetivos:

- Garantizar la integridad referencial.
- Evitar registros duplicados.
- Evitar valores inválidos.
- Garantizar la consistencia de los datos financieros.
- Evitar relaciones inexistentes entre entidades.
- Controlar los estados permitidos.
- Mantener la trazabilidad de los pagos.
- Proteger la información sensible.
- Evitar valores monetarios negativos cuando no correspondan.
- Garantizar la correcta relación entre préstamos, cuotas y pagos.

---

## 3. Restricciones de Claves Primarias

Todas las tablas deberán disponer de una clave primaria que identifique de forma única cada registro.

### 3.1 usuarios

La clave primaria será:

`id_usuario`

No podrá existir más de un registro con el mismo identificador.

### 3.2 roles

La clave primaria será:

`id_rol`

### 3.3 usuario_roles

La clave primaria será compuesta por:

`id_usuario` + `id_rol`

Esto impedirá asignar el mismo rol dos veces al mismo usuario.

### 3.4 clientes

La clave primaria será:

`id_cliente`

### 3.5 prestamos

La clave primaria será:

`id_prestamo`

### 3.6 cuotas

La clave primaria será:

`id_cuota`

### 3.7 pagos

La clave primaria será:

`id_pago`

### 3.8 pago_cuotas

La clave primaria será compuesta por:

`id_pago` + `id_cuota`

### 3.9 moras

La clave primaria será:

`id_mora`

### 3.10 notificaciones

La clave primaria será:

`id_notificacion`

### 3.11 mensajes_whatsapp

La clave primaria será:

`id_mensaje`

---

## 4. Restricciones de Claves Foráneas

Las claves foráneas deberán garantizar que las relaciones existentes entre las tablas sean válidas.

### 4.1 usuario_roles

`usuario_roles.id_usuario` debe referenciar a:

`usuarios.id_usuario`

`usuario_roles.id_rol` debe referenciar a:

`roles.id_rol`

No se permitirá asignar un rol a un usuario inexistente.

---

### 4.2 clientes

`clientes.id_usuario` debe referenciar a:

`usuarios.id_usuario`

El campo podrá ser NULL cuando el cliente no corresponda directamente a una cuenta de acceso.

---

### 4.3 prestamos

`prestamos.id_cliente` debe referenciar a:

`clientes.id_cliente`

`prestamos.id_usuario_creador` debe referenciar a:

`usuarios.id_usuario`

No se permitirá registrar un préstamo para un cliente inexistente.

---

### 4.4 cuotas

`cuotas.id_prestamo` debe referenciar a:

`prestamos.id_prestamo`

No se permitirá registrar una cuota asociada a un préstamo inexistente.

---

### 4.5 pagos

`pagos.id_prestamo` debe referenciar a:

`prestamos.id_prestamo`

`pagos.id_usuario` debe referenciar a:

`usuarios.id_usuario`

---

### 4.6 pago_cuotas

`pago_cuotas.id_pago` debe referenciar a:

`pagos.id_pago`

`pago_cuotas.id_cuota` debe referenciar a:

`cuotas.id_cuota`

---

### 4.7 moras

`moras.id_cuota` debe referenciar a:

`cuotas.id_cuota`

---

### 4.8 notificaciones

`notificaciones.id_usuario` debe referenciar a:

`usuarios.id_usuario`

`notificaciones.id_prestamo` debe referenciar a:

`prestamos.id_prestamo`

El préstamo relacionado podrá ser NULL cuando la notificación sea general.

---

### 4.9 mensajes_whatsapp

`mensajes_whatsapp.id_cliente` debe referenciar a:

`clientes.id_cliente`

`mensajes_whatsapp.id_prestamo` debe referenciar a:

`prestamos.id_prestamo`

El préstamo relacionado podrá ser NULL cuando el mensaje no corresponda a un préstamo específico.

---

## 5. Restricciones de Unicidad

### 5.1 Usuarios

El campo:

`usuarios.email`

debe ser único.

No podrán existir dos usuarios con el mismo correo electrónico.

---

### 5.2 Roles

El campo:

`roles.nombre`

debe ser único.

---

### 5.3 Clientes

Compuesta:

`UNIQUE(tipo_documento, numero_documento_normalizado)`.

No podrán existir dos clientes con mismo tipo+numero (normalizado TRIM UPPER). Colisión DNI `123` vs CE `123` permitida por ser distinto tipo. Validar `DNI ^[0-9]{8}$, CE ^[A-Z0-9]{9,12}$`.

```sql
ALTER TABLE clientes ADD CONSTRAINT ux_cliente_doc UNIQUE (tipo_documento, numero_documento);
```

---

## 6. Restricciones de Valores Nulos

Los campos fundamentales para el funcionamiento del sistema no deberán aceptar valores NULL.

### Usuarios

No deben aceptar NULL:

- `nombre`
- `apellido`
- `email`
- `password_hash`
- `estado`
- `fecha_creacion`
- `fecha_actualizacion`

### Clientes

No deben aceptar NULL:

- `tipo_documento`
- `numero_documento`
- `nombres`
- `apellidos`
- `telefono`
- `estado`
- `fecha_registro`
- `fecha_actualizacion`

### Préstamos

No deben aceptar NULL:

- `id_cliente`
- `id_usuario_creador`
- `monto_capital`
- `tasa_interes`
- `tipo_tasa`
- `plazo`
- `frecuencia_pago`
- `monto_interes`
- `monto_total`
- `fecha_desembolso`
- `fecha_primer_vencimiento`
- `fecha_vencimiento`
- `estado`

### Cuotas

No deben aceptar NULL:

- `id_prestamo`
- `numero_cuota`
- `fecha_vencimiento`
- `capital_programado`
- `interes_programado`
- `monto_programado`
- `capital_pagado`
- `interes_pagado`
- `monto_pagado`
- `saldo`
- `estado`

### Pagos

No deben aceptar NULL (+ espejo diccionario):

- `id_prestamo`
- `id_usuario`
- `monto_pago`
- `fecha_pago`
- `metodo_pago`
- `estado`
- `fecha_registro`

### Tablas restantes (completan §6)

- `roles.nombre NOT NULL, usuario_roles.* NOT NULL, pago_cuotas.* NOT NULL DEFAULT 0.00, moras.(id_cuota,dias_mora,tasa_mora,monto_mora,fecha_inicio) NOT NULL, notificaciones.(id_usuario,tipo,titulo,mensaje,canal,estado) NOT NULL, mensajes_whatsapp.(id_cliente,tipo_plantilla,numero_destino,contenido,estado,identificador_externo,fecha_creacion) NOT NULL`.

---

## 7. Restricciones de Valores Monetarios

Los valores monetarios deberán utilizar:

`DECIMAL(12,2)`

No deberán utilizarse tipos de datos de punto flotante para almacenar importes financieros.

### 7.1 Capital

`monto_capital` debe ser mayor que cero.

Regla:

`monto_capital > 0`

---

### 7.2 Interés

`monto_interes` no podrá ser negativo.

Regla:

`monto_interes >= 0`

---

### 7.3 Monto total

`monto_total` no podrá ser negativo.

Además, deberá corresponder a:

`monto_capital + monto_interes`

```sql
ALTER TABLE prestamos ADD CONSTRAINT ck_prestamo_total CHECK (monto_total = monto_capital + monto_interes);
```

---

### 7.4 Pagos

`monto_pago` debe ser mayor que cero.

Regla:

`monto_pago > 0`

---

### 7.5 Importes de pago

Los siguientes campos no podrán ser negativos:

- `monto_capital`
- `monto_interes`
- `monto_mora`
- `monto_total`

---

### 7.6 Saldo

El campo `saldo` no podrá ser negativo.

Regla:

`saldo >= 0`

---

## 8. Restricciones de Intereses

El sistema utilizará interés calculado sobre el capital inicial, tasa en porcentaje (0-100).

```sql
-- monto_interes = ROUND(monto_capital * (tasa_interes/100) * plazo, 2)
-- WHERE tipo_tasa='SEMANAL_FIJA' AND frecuencia_pago='SEMANAL'
-- v1: CHECK(tasa_interes=5.00). Ejemplo S/100 -> S/5 por semana.
ALTER TABLE prestamos ADD CONSTRAINT ck_prestamo_tasa_v1 CHECK (tasa_interes = 5.00);
```

### Restricción de no capitalización

Los intereses generados no deberán incorporarse nuevamente al capital para generar nuevos intereses.

Por lo tanto:

`capital_base_interes = monto_capital`

y no:

`capital_base_interes = monto_capital + intereses_generados`

---

## 9. Restricciones de Préstamos

### 9.1 Plazo

El plazo debe ser mayor que cero.

Regla:

`plazo > 0`

---

### 9.2 Tasa de interés

```sql
ALTER TABLE prestamos ADD CONSTRAINT ck_tasa CHECK (tasa_interes >= 0 AND tasa_interes <= 100);
-- v1 fija: CHECK(tasa_interes=5.00)
```

### 9.5 Estado préstamo (ver RN-PRE-011)

El estado del préstamo solo podrá pertenecer al conjunto permitido (RN-PRE-011):

- `PENDIENTE`
- `ACTIVO`
- `MOROSO`
- `CANCELADO`
- `ANULADO`

```sql
ALTER TABLE prestamos ADD CONSTRAINT ck_prestamo_estado CHECK (estado IN ('PENDIENTE','ACTIVO','MOROSO','CANCELADO','ANULADO'));
```

---

### 9.3 Fechas

La fecha de primer vencimiento no deberá ser anterior a la fecha de desembolso.

Regla:

`fecha_primer_vencimiento >= fecha_desembolso`

La fecha de vencimiento final no deberá ser anterior a la fecha del primer vencimiento.

Regla:

`fecha_vencimiento >= fecha_primer_vencimiento`

---

### 9.4 Cliente

Todo préstamo debe estar asociado a un cliente existente.

Además índice único parcial 1 activo (ver Alcance):

```sql
CREATE UNIQUE INDEX ux_prestamo_activo ON prestamos(id_cliente) WHERE estado IN ('ACTIVO','MOROSO');
```

---

## 10. Restricciones de Cuotas

### 10.1 Número de cuota

El número de cuota debe ser mayor que cero.

Regla:

`numero_cuota > 0`

---

### 10.2 Cuotas duplicadas

No deberá existir más de una cuota con el mismo número dentro de un mismo préstamo.

```sql
ALTER TABLE cuotas ADD CONSTRAINT ux_cuota_num UNIQUE (id_prestamo, numero_cuota);
```

---

### 10.3 Capital programado

`capital_programado >= 0`. Prorrateo v1: `ROUND(capital_inicial/plazo,2)` por cuota, última ajusta por redondeo (`SUM(capital_programado)=monto_capital`).

---

### 10.4 Interés programado

`interes_programado >= 0`

---

### 10.5 Monto programado

`monto_programado >= 0`

El monto programado deberá corresponder a:

`capital_programado + interes_programado`

---

### 10.6 Capital pagado

`capital_pagado >= 0`

Además:

`capital_pagado <= capital_programado`

---

### 10.7 Interés pagado

`interes_pagado >= 0`

Además:

`interes_pagado <= interes_programado`

---

### 10.8 Monto pagado

`monto_pagado >= 0`

v1 estricta: cada imputación `<= saldo_cuota` y `SUM(pago_cuotas.monto_total)=pagos.monto_pago` (trigger aborta si excede). Pagos adelantados quedan para v2 con estado ANTICIPADO.

---

### 10.9 Saldo

El saldo deberá calcularse como:

`saldo = monto_programado - monto_pagado`

y deberá cumplirse:

`saldo >= 0`

---

## 11. Restricciones de Pagos

### 11.1 Pago positivo

Todo pago registrado debe tener un importe mayor que cero.

Regla:

`monto_pago > 0`

---

### 11.2 Préstamo válido

Todo pago debe estar asociado a un préstamo existente.

---

### 11.3 Usuario válido

Todo pago debe registrar el usuario responsable de realizar el registro.

---

### 11.4 Distribución del pago

v1 estricta:

```sql
-- SUM(pago_cuotas.monto_total) = pagos.monto_pago (igualdad, no <= sin asignar)
```

---

### 11.5 Componentes del pago

Para cada registro de `pago_cuotas` deberá cumplirse:

`monto_total = monto_capital + monto_interes + monto_mora`

---

## 12. Restricciones de Mora

### 12.1 Días de mora

Los días de mora no podrán ser negativos.

Regla:

`dias_mora >= 0`

---

### 12.2 Tasa de mora

La tasa de mora no podrá ser negativa. En v1 fija `0.00` (ver RN-MOR-004).

Regla:

`tasa_mora >= 0`

---

### 12.3 Monto de mora

El monto de mora no podrá ser negativo.

Regla:

`monto_mora >= 0`

---

### 12.4 Cuota relacionada

Toda mora debe estar asociada a una cuota existente.

---

### 12.5 Mora y cuota pagada

Una cuota completamente pagada no deberá generar nueva mora.

---

## 13. Restricciones de Estados

Los estados deberán limitarse a valores previamente definidos.

### Usuario

- `ACTIVO`
- `INACTIVO`
- `BLOQUEADO`

### Cliente

- `ACTIVO`
- `EN_OBSERVACION`
- `MOROSO`
- `INACTIVO`
- `BLOQUEADO`

### Préstamo (RN-PRE-011)

- `PENDIENTE`
- `ACTIVO`
- `MOROSO`
- `CANCELADO`
- `ANULADO`

### Cuota

- `PENDIENTE`
- `PAGADA`
- `VENCIDA`
- `PARCIAL`

### Pago

- `REGISTRADO`
- `ANULADO`

### Mora

- `ACTIVA`
- `PAGADA`
- `ANULADA`

### Notificación

- `PENDIENTE`
- `ENVIADO`
- `ENTREGADO`
- `LEIDO`
- `FALLIDO`
- `CANCELADO`

### WhatsApp

- `PENDIENTE`
- `ENVIADO`
- `ENTREGADO`
- `LEIDO`
- `FALLIDO`
- `CANCELADO`

---

## 14. Restricciones de Eliminación

Todas las FK financieras `ON DELETE RESTRICT ON UPDATE CASCADE`. Sin DELETE físico en pagos/cuotas (`REVOKE DELETE`, solo `UPDATE estado=ANULADO` con trigger que exige motivo). Test: intento DELETE → 23503.

### 14.1 Préstamos

No deberá eliminarse físicamente un préstamo que tenga cuotas o pagos asociados.

El préstamo deberá cambiar a un estado como:

`CANCELADO`

cuando corresponda.

---

### 14.2 Pagos

Los pagos registrados no deberán eliminarse físicamente cuando formen parte del historial financiero.

En caso de invalidación deberán cambiar a:

`ANULADO`

---

### 14.3 Clientes

Un cliente con préstamos históricos no deberá eliminarse físicamente.

Deberá utilizarse el estado:

`INACTIVO`

---

### 14.4 Usuarios

Los usuarios que hayan realizado operaciones históricas no deberán eliminarse físicamente.

Deberán utilizarse estados como:

`INACTIVO`

o

`BLOQUEADO`

---

## 15. Integridad Referencial

La base de datos deberá impedir:

- Registrar cuotas para préstamos inexistentes.
- Registrar pagos para préstamos inexistentes.
- Registrar pagos asociados a usuarios inexistentes.
- Registrar moras para cuotas inexistentes.
- Registrar notificaciones para usuarios inexistentes.
- Registrar mensajes para clientes inexistentes.
- Registrar roles para usuarios inexistentes.

Las operaciones de eliminación deberán considerar las dependencias existentes antes de eliminar registros.

---

## 16. Restricciones de Contraseñas

Argon2id (m=64MB,t=3,p=4) o bcrypt $2a$12 + salt único. Columna `VARCHAR(255) CHECK(length>=60)`. Nunca loggear. Cambio con verificación actual + rate 5/15min.

---

## 17. Restricciones de Auditoría

Solo agregados (usuarios, clientes, prestamos, pagos) llevan `fecha_creacion/actualizacion/id_usuario`. Tablas unión/evento (usuario_roles, pago_cuotas, moras) son append-only inmutables con `fecha_registro` + trigger anti UPDATE/DELETE + tabla `auditoria(tabla,id_registro,operacion,diff JSONB,id_usuario,fecha)`.

---

## 18. Restricciones de Consistencia Financiera

El sistema deberá garantizar que:

`monto_total = monto_capital + monto_interes`

Para las cuotas:

`monto_programado = capital_programado + interes_programado`

Para la distribución de pagos:

`monto_total = monto_capital + monto_interes + monto_mora`

Para el saldo:

`saldo = monto_programado - monto_pagado`

Validación DB (GENERATED) + trigger; sin `salvo que` abierto (ver §10.8 v1 estricta).

---

## 19. Restricciones de Integridad de Pagos

v1 estricta: impedir pago por encima del saldo (`SUM=monto_pago`, imputación <= saldo). Sin `salvo que` sin regla (ver §10.8).

Antes de registrar un pago deberán verificarse:

1. Que el préstamo exista.
2. Que el préstamo permita recibir pagos.
3. Que la cuota exista.
4. Que la cuota pertenezca al préstamo correspondiente.
5. Que el monto sea mayor que cero.
6. Que la distribución del pago sea válida.
7. Que no se supere el saldo permitido.
8. Que se actualice correctamente el estado de la cuota.
9. Que se actualice correctamente el saldo del préstamo.

---

## 20. Transacciones

Las operaciones financieras deberán ejecutarse mediante transacciones.

Por ejemplo, el registro de un pago deberá considerar como una única operación:

1. Crear el registro del pago.
2. Registrar la distribución del pago.
3. Actualizar la cuota.
4. Actualizar el saldo.
5. Actualizar el estado de la cuota.
6. Actualizar el estado del préstamo cuando corresponda.

Si alguna operación falla, la transacción deberá revertirse para evitar información financiera inconsistente.

---

## 21. Resumen de Restricciones

| Categoría | Restricción principal |
|---|---|
| Claves primarias | Identificación única de registros |
| Claves foráneas | Integridad referencial |
| Unicidad | Evitar duplicados |
| Valores NULL | Control de campos obligatorios |
| Valores monetarios | No permitir importes inválidos |
| Préstamos | Control de capital, interés, plazo y fechas |
| Cuotas | Control de programación y saldos |
| Pagos | Control de importes y distribución |
| Mora | Control de días, tasas e importes |
| Estados | Valores controlados |
| Eliminación | Protección del historial financiero |
| Contraseñas | Almacenamiento mediante hash |
| Auditoría | Conservación de trazabilidad |
| Transacciones | Integridad de operaciones financieras |

---

## 22. Consideraciones Finales

Matriz ejecutable: cada regla lleva su SQL (`PRIMARY KEY/CHECK/FK/TRIGGER/APP-ONLY+test`). Se elimina la revisión diferida: lo definido aquí es normativo. Regenerar `02-Diagrama-ER.png` por CI desde el `.puml` (fuente versionable).