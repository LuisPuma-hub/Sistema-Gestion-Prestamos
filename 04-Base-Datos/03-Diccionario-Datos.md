# Diccionario de Datos

## 1. Introducción

El presente documento define el diccionario de datos del Sistema de Gestión de Préstamos. Su objetivo es establecer la estructura, significado y características de las entidades y atributos que conforman la base de datos del sistema.

El diccionario permite mantener una referencia técnica común para el desarrollo del backend, aplicación móvil, API, pruebas y mantenimiento del sistema.

---

## 2. Convenciones

| Abreviatura | Significado |
|---|---|
| PK | Clave primaria |
| FK | Clave foránea |
| UK | Restricción de unicidad |
| NOT NULL | No permite valores nulos (NN) |
| NULL | Permite valores nulos |

Columna `Nulo?` usa `NOT NULL / NULL` (no `NO/SÍ`). Todo `NOT NULL` financiero lleva `DEFAULT 0.00` salvo PK/FK.

### Tipos de datos principales (PostgreSQL)

| Tipo | Descripción |
|---|---|
| BIGINT GENERATED ALWAYS AS IDENTITY | Identificador (max 9e18, esperado <1M en 5 años) |
| INT | Número entero |
| VARCHAR | Cadena longitud variable |
| NUMERIC(12,2) | Dinero (prohibido float/double, C# decimal HALF_UP) |
| DATE | Fecha civil |
| TIMESTAMPTZ | Instante con zona (default now(), TimeZone='America/Lima', Npgsql DateTimeKind.Utc) |
| TEXT | Texto largo |
| JSONB | Diff auditoría |

---

## 3. Tabla: usuarios

Almacena la información de las personas que tienen acceso al sistema.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_usuario | BIGINT | PK | NO | Identificador único del usuario |
| nombre | VARCHAR(100) | — | NO | Nombres del usuario |
| apellido | VARCHAR(100) | — | NO | Apellidos del usuario |
| email | VARCHAR(150) | UK | NO | Correo electrónico utilizado para el acceso |
| password_hash | VARCHAR(255) | — | NO | Contraseña almacenada mediante hash |
| telefono | VARCHAR(20) | — | SÍ | Número telefónico del usuario |
| estado | VARCHAR(20) | — | NO | Estado de la cuenta |
| fecha_creacion | DATETIME | — | NO | Fecha y hora de creación |
| fecha_actualizacion | DATETIME | — | NO | Fecha y hora de última actualización |

---

## 4. Tabla: roles

Almacena los roles disponibles dentro del sistema.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_rol | BIGINT | PK | NO | Identificador único del rol |
| nombre | VARCHAR(50) | UK | NO | Nombre del rol |
| descripcion | VARCHAR(255) | — | SÍ | Descripción de las funciones del rol |

---

## 5. Tabla: usuario_roles

Relaciona usuarios con los roles que tienen asignados.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_usuario | BIGINT | PK, FK | NO | Usuario asociado |
| id_rol | BIGINT | PK, FK | NO | Rol asignado al usuario |

### Clave primaria

La clave primaria está compuesta por `id_usuario` e `id_rol`.

Esto evita registrar dos veces el mismo rol para un mismo usuario.

---

## 6. Tabla: clientes

Almacena la información de los clientes que solicitan préstamos.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_cliente | BIGINT | PK | NO | Identificador único del cliente |
| id_usuario | BIGINT | FK | SÍ | Deprecated: ver id_usuario_registra / id_usuario_login en ER v1.1 |
| id_usuario_registra | BIGINT | FK NOT NULL | NO | Usuario que registró al cliente |
| id_usuario_login | BIGINT | FK UNIQUE | SÍ | Login futuro opcional |
| tipo_documento | VARCHAR(20) CHECK(DNI,CE) | — | NO | Tipo de documento de identidad |
| numero_documento | VARCHAR(20) | UK compuesta (tipo,numero_normalizado) | NO | Número normalizado TRIM UPPER; DNI ^[0-9]{8}$, CE ^[A-Z0-9]{9,12}$ |
| nombres | VARCHAR(100) | — | NO | Nombres del cliente |
| apellidos | VARCHAR(100) | — | NO | Apellidos del cliente |
| telefono | VARCHAR(20) | — | NO | Número telefónico |
| email | VARCHAR(150) | — | SÍ | Correo electrónico |
| direccion | VARCHAR(255) | — | SÍ | Dirección del cliente |
| referencia_direccion | VARCHAR(255) | — | SÍ | Referencia adicional de ubicación |
| estado | VARCHAR(20) | — | NO | Estado del cliente |
| fecha_registro | DATETIME | — | NO | Fecha de registro |
| fecha_actualizacion | DATETIME | — | NO | Fecha de última actualización |

---

## 7. Tabla: prestamos

Almacena la información principal de cada préstamo otorgado.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_prestamo | BIGINT | PK | NO | Identificador único del préstamo |
| id_cliente | BIGINT | FK | NO | Cliente que recibe el préstamo |
| id_usuario_creador | BIGINT | FK | NO | Usuario que registra el préstamo |
| monto_capital | DECIMAL(12,2) | — | NO | Capital inicial prestado |
| tasa_interes | NUMERIC(5,2) CHECK 0-100 DEFAULT 5.00 | — | NO | En porcentaje en DB (5.00 = 5%); en API se expone tanto por uno 0.05 (`tasa_api=tasa_db/100`). v1 fija 5.00 (CHECK=5.00). Fórmula: monto_interes=ROUND(capital*(tasa/100)*plazo,2) |
| tipo_tasa | VARCHAR(20) CHECK(SEMANAL_FIJA) | — | NO | v1 solo SEMANAL_FIJA |
| plazo | INT CHECK>0 | — | NO | Cantidad de periodos en SEMANAS |
| frecuencia_pago | VARCHAR(20) CHECK(SEMANAL) | — | NO | v1 solo SEMANAL |
| monto_interes | DECIMAL(12,2) | — | NO | Interés total = interes_semanal * plazo |
| monto_total | DECIMAL(12,2) | — | NO | Capital + monto_interes |
| capital_pendiente | DECIMAL(12,2) DEFAULT monto_capital | — | NO | Saldo capital; vista v_prestamo_saldo lo deriva si se prefiere |
| fecha_desembolso | DATE | — | NO | Fecha en que se entrega el préstamo |
| fecha_primer_vencimiento | DATE | — | NO | Fecha del primer vencimiento |
| fecha_vencimiento | DATE | — | NO | Fecha del último vencimiento |
| estado | VARCHAR(20) | — | NO | Estado actual del préstamo |
| observaciones | TEXT | — | SÍ | Observaciones adicionales |
| fecha_creacion | DATETIME | — | NO | Fecha de registro |
| fecha_actualizacion | DATETIME | — | NO | Fecha de actualización |

### Regla financiera

El campo `monto_interes` representa el interés calculado sobre el capital inicial.

Los intereses generados no forman parte de la base para calcular nuevos intereses.

---

## 8. Tabla: cuotas

Almacena las cuotas programadas para cada préstamo.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_cuota | BIGINT | PK | NO | Identificador único de la cuota |
| id_prestamo | BIGINT | FK | NO | Préstamo al que pertenece |
| numero_cuota | INT | — | NO | Número de cuota dentro del préstamo |
| fecha_vencimiento | DATE | — | NO | Fecha límite de pago |
| capital_programado | DECIMAL(12,2) | — | NO | Prorrateo v1: `ROUND(capital_inicial/plazo,2)` por cuota; la última ajusta por redondeo para que `SUM=capital_inicial`. `interes_programado` = interés semanal fijo en todas |
| interes_programado | DECIMAL(12,2) | — | NO | Interés programado |
| monto_programado | DECIMAL(12,2) | — | NO | Total programado de la cuota |
| capital_pagado | DECIMAL(12,2) DEFAULT 0.00 | — | NO | Capital efectivamente pagado |
| interes_pagado | DECIMAL(12,2) DEFAULT 0.00 | — | NO | Interés efectivamente pagado |
| monto_pagado | DECIMAL(12,2) DEFAULT 0.00 | — | NO | Total pagado de la cuota |
| saldo | DECIMAL(12,2) GENERATED (monto_programado-monto_pagado) | — | NO | Saldo pendiente (incluye mora aparte) |
| estado | VARCHAR(20) | — | NO | Estado de la cuota |
| fecha_pago | DATETIME | — | SÍ | Fecha del último pago asociado |

---

## 9. Tabla: pagos

Registra los pagos realizados por los clientes.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_pago | BIGINT | PK | NO | Identificador único del pago |
| id_prestamo | BIGINT | FK | NO | Préstamo al que se aplica el pago |
| id_usuario | BIGINT | FK | NO | Usuario que registra el pago |
| monto_pago | DECIMAL(12,2) | — | NO | Importe total del pago |
| fecha_pago | DATETIME | — | NO | Fecha y hora del pago |
| metodo_pago | VARCHAR(30) CHECK(EFECTIVO,YAPE,TRANSFERENCIA,PLIN,DEPOSITO,OTRO) | — | NO | Método canónico v1 (ver RN-PAG-002) |
| referencia | VARCHAR(100) | — | SÍ | Número o código de referencia |
| observaciones | TEXT | — | SÍ | Información adicional |
| estado | VARCHAR(20) | — | NO | Estado del pago |
| fecha_registro | DATETIME | — | NO | Fecha de registro |

---

## 10. Tabla: pago_cuotas

Permite distribuir un pago entre una o varias cuotas.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_pago | BIGINT | PK, FK | NO | Pago asociado |
| id_cuota | BIGINT | PK, FK | NO | Cuota a la que se aplica |
| monto_capital | DECIMAL(12,2) | — | NO | Parte del pago aplicada al capital |
| monto_interes | DECIMAL(12,2) | — | NO | Parte del pago aplicada al interés |
| monto_mora | DECIMAL(12,2) | — | NO | Parte del pago aplicada a mora |
| monto_total | DECIMAL(12,2) | — | NO | Total aplicado a la cuota |

### Clave primaria

La clave primaria está compuesta por `id_pago` e `id_cuota`.

---

## 11. Tabla: moras

Registra la morosidad generada por cuotas vencidas.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_mora | BIGINT | PK | NO | Identificador único de la mora |
| id_cuota | BIGINT | FK | NO | Cuota que originó la mora |
| dias_mora | INT | — | NO | Cantidad de días de retraso |
| tasa_mora | DECIMAL(5,2) | — | NO | v1 fija 0.00 (sin recargo, ver RN-MOR-004) |
| monto_mora | DECIMAL(12,2) | — | NO | Importe de mora calculado |
| fecha_inicio | DATE | — | NO | Fecha desde la cual se considera la mora |
| fecha_calculo | DATETIME | — | NO | Fecha y hora del cálculo |
| estado | VARCHAR(20) | — | NO | Estado de la mora |

---

## 12. Tabla: notificaciones

Almacena las notificaciones enviadas o programadas para los usuarios.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_notificacion | BIGINT | PK | NO | Identificador único |
| id_usuario | BIGINT | FK | NO | Usuario destinatario |
| id_prestamo | BIGINT | FK | SÍ | Préstamo relacionado |
| tipo | VARCHAR(30) CHECK(PAYMENT_REMINDER,PAYMENT_OVERDUE,DELINQUENCY,CRITICAL_DELINQUENCY,PAYMENT_REGISTERED,LOAN_REACTIVATED) | — | NO | Tipo canónico v1 (ver 07-Notificaciones/01 §14). `COBRO_PROXIMO/INTERES_VENCIDO/MOROSO/PAGO_REGISTRADO/REACTIVADO` deprecados, no usar |
| titulo | VARCHAR(150) | — | NO | Título de la notificación |
| mensaje | TEXT | — | NO | Contenido |
| canal | VARCHAR(30) CHECK(PUSH,WHATSAPP) | — | NO | Canal de envío |
| estado | VARCHAR(20) | — | NO | Estado de la notificación |
| fecha_programada | DATETIME | — | SÍ | Fecha prevista de envío |
| fecha_envio | DATETIME | — | SÍ | Fecha real de envío |
| fecha_lectura | DATETIME | — | SÍ | Fecha en que fue leída |

---

## 13. Tabla: mensajes_whatsapp

Registra los mensajes enviados mediante la integración con WhatsApp.

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_mensaje | BIGINT | PK | NO | Identificador único del mensaje |
| id_cliente | BIGINT | FK | NO | Cliente destinatario |
| id_prestamo | BIGINT | FK | SÍ | Préstamo relacionado |
| tipo_plantilla | VARCHAR(100) FK → plantillas_whatsapp.nombre | — | NO | Plantilla utilizada |
| numero_destino | VARCHAR(20) | — | NO | Número de WhatsApp destinatario |
| contenido | TEXT | — | NO | Contenido del mensaje |
| estado | VARCHAR(30) | — | NO | Estado del envío |
| identificador_externo | VARCHAR(150) | — | SÍ | Identificador proporcionado por el servicio externo |
| fecha_programada | DATETIME | — | SÍ | Fecha prevista de envío |
| fecha_envio | DATETIME | — | SÍ | Fecha real de envío |
| fecha_creacion | DATETIME | — | NO | Fecha de creación del registro |

---

## 13b. Tabla: dispositivos (mapeo API)

| Campo | Tipo | Clave | NULL | Descripción |
|---|---|---|---|---|
| id_dispositivo | BIGINT | PK | NO | Identificador único |
| id_usuario | BIGINT | FK | NO | Usuario dueño |
| fcm_token | TEXT | UK | NO | = `DispositivoToken.token` (API) |
| plataforma | VARCHAR(20) | — | NO | `android` / `ios` |
| device_id | UUID | UK parcial | NO | = `deviceId` (API); único solo entre `activo=TRUE` (un activo por dispositivo; inactivos conservan historial) |
| app_version | VARCHAR(20) | — | SÍ | = `appVersion` (API) |
| ultimo_uso | TIMESTAMPTZ | — | SÍ | Última actividad |
| activo | BOOLEAN | — | NO | Default TRUE; FALSE si token inválido |

(Avales, solicitudes, documentos, plantillas y auditoría viven en `01-Diagrama-ER.puml`; se documentarán en diccionario en la próxima pasada.)

---

## 14. Relaciones principales

| Entidad origen | Relación | Entidad destino |
|---|---|---|
| usuarios | 1:N | usuario_roles |
| roles | 1:N | usuario_roles |
| usuarios | 1:0..1 | clientes |
| clientes | 1:N | prestamos |
| usuarios | 1:N | prestamos |
| prestamos | 1:N | cuotas |
| prestamos | 1:N | pagos |
| usuarios | 1:N | pagos |
| pagos | 1:N | pago_cuotas |
| cuotas | 1:N | pago_cuotas |
| cuotas | 1:N | moras |
| usuarios | 1:N | notificaciones |
| prestamos | 1:N | notificaciones |
| clientes | 1:N | mensajes_whatsapp |
| prestamos | 1:N | mensajes_whatsapp |

---

## 15. Estados principales (catálogo único versionado, con CHECK)

### Usuario

- `ACTIVO`
- `INACTIVO`
- `BLOQUEADO`

### Cliente

- `ACTIVO`
- `EN_OBSERVACION`
- `MOROSO` (automático por trigger desde préstamo, ver RN-MOR-009)
- `INACTIVO`
- `BLOQUEADO` (manual admin)

### Préstamo (RN-PRE-011)

- `PENDIENTE`
- `ACTIVO`
- `MOROSO`
- `CANCELADO` (saldado capital+intereses)
- `ANULADO` (error pre-primer-pago)

Se eliminan `PAGADO/VENCIDO` como estado de préstamo (son de cuota).

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

### Mensaje WhatsApp (unificado con 08-WhatsApp/01)

- `PENDIENTE`
- `ENVIADO`
- `ENTREGADO`
- `LEIDO`
- `FALLIDO`
- `CANCELADO`

Se eliminan `ENVIADA/LEIDA/ERROR/ENVIANDO/PROCESANDO` como sinónimos.

---

## 16. Consideraciones

1. Los identificadores principales utilizan `BIGINT` para permitir el crecimiento de la información.
2. Los valores monetarios utilizan `DECIMAL(12,2)` para evitar errores de precisión propios de los tipos de punto flotante.
3. Las contraseñas nunca deben almacenarse en texto plano.
4. Los pagos deben conservar un registro histórico.
5. La distribución de pagos se realiza mediante `pago_cuotas`.
6. Los intereses se calculan sobre el capital inicial según las reglas financieras establecidas.
7. La mora debe mantenerse separada del interés ordinario.
8. Las claves foráneas deben garantizar la integridad referencial.
9. Los estados deberán validarse mediante restricciones de base de datos o mediante catálogos, según la implementación definitiva.
10. Los campos de fecha y hora deben almacenarse de forma consistente con la zona horaria definida para el sistema.