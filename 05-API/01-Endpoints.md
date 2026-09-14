# 01. Endpoints de la API

## 1. Introducción

Este documento define los endpoints principales de la API REST del Sistema de Gestión de Préstamos.

La API será utilizada principalmente por la aplicación móvil para realizar operaciones relacionadas con usuarios, clientes, préstamos, pagos, morosidad, notificaciones y mensajería mediante WhatsApp.

La comunicación entre la aplicación cliente y el backend se realizará obligatoriamente mediante HTTPS con `Content-Type: application/json; charset=utf-8`, utilizando el formato JSON.

Solo se permite HTTP en `localhost` sin datos reales para desarrollo. En cualquier otro entorno el backend debe redirigir 301 HTTP→HTTPS y enviar HSTS. Nunca se enviarán password ni Bearer por HTTP.

---

## 2. URL Base

Base URL: `https://api.{dominio}/api/v1`.

La estructura general de la API será:

`/api/v1`

Ejemplo:

`/api/v1/clientes`

La versión `v1` permitirá realizar futuras modificaciones importantes en la API sin afectar necesariamente a versiones anteriores.

---

## 3. Convenciones HTTP

| Método | Uso                                                  |
| ------ | ---------------------------------------------------- |
| GET    | Obtener información                                  |
| POST   | Crear un nuevo registro                              |
| PUT    | Actualizar un registro completo                      |
| PATCH  | Actualizar parcialmente un registro                  |
| DELETE | Eliminar o desactivar un registro cuando corresponda |

---

## 4. Códigos de respuesta HTTP

| Código | Descripción                                   |
| ------ | --------------------------------------------- |
| 200    | Operación realizada correctamente             |
| 201    | Recurso creado correctamente                  |
| 202    | Aceptado para procesamiento async (jobId)     |
| 204    | Operación correcta sin contenido de respuesta |
| 400    | Solicitud inválida (sintaxis / JSON malformado) |
| 401    | Usuario no autenticado (Bearer faltante/inválido/expirado) |
| 403    | Usuario sin permisos o cuenta BLOQUEADA (ACCOUNT_BLOCKED) |
| 404    | Recurso no encontrado                         |
| 409    | Conflicto con información existente           |
| 422    | Error de validación semántica (reglas negocio) |
| 429    | Rate limit excedido (Retry-After)             |
| 500    | Error interno del servidor                    |
| 503    | Servicio no disponible / dependencia caída    |

### 4.1 Envelope obligatorio

Éxito: `{"success":true,"data":{...},"meta":{...}}`. Error: `{"success":false,"error":{"code":"VALIDATION_ERROR","message":"...","details":[...]},"traceId":"uuid"}` con `Content-Type: application/problem+json` permitido.

`400` = request malformada. `422` = bien formada pero viola regla (ej. monto > deuda, DNI duplicado). Matriz mínima por endpoint: `POST /auth/login: 200|400|401|429`, `POST /pagos: 201|400|401|403|404|409|422|429`.

---

# 5. Endpoints de Autenticación

## 5.1 Iniciar sesión

**POST**

`/api/v1/auth/login`

Permite autenticar a un usuario dentro del sistema.

---

## 5.2 Cerrar sesión

**POST**

`/api/v1/auth/logout`

Permite cerrar la sesión activa del usuario.

---

## 5.3 Renovar sesión

**POST**

`/api/v1/auth/refresh`

Permite renovar el token de acceso cuando corresponda.

---

## 5.4 Obtener usuario autenticado

**GET**

`/api/v1/auth/me`

Devuelve la información del usuario que tiene la sesión activa.

---

# 6. Endpoints de Usuarios

## 6.1 Listar usuarios

**GET**

`/api/v1/usuarios`

Permite obtener la lista de usuarios registrados.

---

## 6.2 Obtener usuario por ID

**GET**

`/api/v1/usuarios/{id_usuario}`

Permite obtener la información de un usuario específico.

---

## 6.3 Registrar usuario

**POST**

`/api/v1/usuarios`

Permite registrar un nuevo usuario en el sistema.

---

## 6.4 Actualizar usuario

**PUT**

`/api/v1/usuarios/{id_usuario}`

Permite actualizar la información de un usuario.

---

## 6.5 Cambiar estado de usuario

**PATCH**

`/api/v1/usuarios/{id_usuario}/estado`

Permite cambiar el estado de un usuario.

Estados posibles:

* `ACTIVO`
* `INACTIVO`
* `BLOQUEADO`

---

## 6.6 Obtener roles de usuario

**GET**

`/api/v1/usuarios/{id_usuario}/roles`

Permite obtener los roles asignados a un usuario.

---

## 6.7 Asignar rol

**POST**

`/api/v1/usuarios/{id_usuario}/roles`

Permite asignar un rol a un usuario.

---

## 6.8 Eliminar rol

**DELETE**

`/api/v1/usuarios/{id_usuario}/roles/{id_rol}`

Permite retirar un rol asignado a un usuario.

---

# 7. Endpoints de Roles

## 7.1 Listar roles

**GET**

`/api/v1/roles`

Permite obtener todos los roles disponibles.

---

## 7.2 Obtener rol

**GET**

`/api/v1/roles/{id_rol}`

Permite obtener la información de un rol específico.

---

## 7.3 Crear rol

**POST**

`/api/v1/roles`

Permite registrar un nuevo rol.

---

## 7.4 Actualizar rol

**PUT**

`/api/v1/roles/{id_rol}`

Permite actualizar la información de un rol.

---

# 8. Endpoints de Clientes

## 8.1 Listar clientes

**GET**

`/api/v1/clientes`

Permite obtener la lista de clientes. Requiere Bearer. Roles: ADMIN, COBRADOR.

Query tipados: `?estado=ACTIVO|INACTIVO|BLOQUEADO|MOROSO&tipoDocumento=DNI|CE&numeroDocumento=string&nombre=string&telefono=string&page=1&pageSize=20&sort=-createdAt`.

Defaults: `page=1, pageSize=20, max 100`. Respuesta paginada `{data:[],meta:{total,page,pageSize}}`. Búsqueda `contains` case-insensitive, diacríticos ignorados.

---

## 8.2 Buscar clientes

**GET**

`/api/v1/clientes/buscar`

Permite realizar una búsqueda de clientes.

Ejemplo:

`/api/v1/clientes/buscar?q=Luis`

---

## 8.3 Obtener cliente por ID

**GET**

`/api/v1/clientes/{id_cliente}`

Permite obtener toda la información de un cliente.

---

## 8.4 Registrar cliente

**POST**

`/api/v1/clientes`

Permite registrar un nuevo cliente.

---

## 8.5 Actualizar cliente

**PUT**

`/api/v1/clientes/{id_cliente}`

Permite actualizar la información de un cliente.

---

## 8.6 Cambiar estado del cliente

**PATCH**

`/api/v1/clientes/{id_cliente}/estado`

Permite cambiar el estado del cliente.

Estados principales:

* `ACTIVO`
* `INACTIVO`

---

## 8.7 Obtener préstamos del cliente

**GET**

`/api/v1/clientes/{id_cliente}/prestamos`

Permite obtener todos los préstamos asociados a un cliente.

Un cliente puede tener múltiples préstamos.

---

# 9. Endpoints de Préstamos

## 9.1 Listar préstamos

**GET**

`/api/v1/prestamos`

Permite obtener todos los préstamos registrados. Requiere Bearer. Roles: ADMIN, COBRADOR.

Query: `?clienteId=uuid&estado=PENDIENTE|ACTIVO|MOROSO|CANCELADO|ANULADO&desde=YYYY-MM-DD&hasta=YYYY-MM-DD&page=1&pageSize=20`. Defaults `page=1, pageSize=20, max 100`.

---

## 9.2 Obtener préstamo

**GET**

`/api/v1/prestamos/{id_prestamo}`

Permite obtener la información detallada de un préstamo.

---

## 9.3 Registrar solicitud de préstamo

**POST**

`/api/v1/prestamos`

Permite registrar un nuevo préstamo. En v1 `solicitudId` (solicitud `APROBADA`, relación 1:1) es obligatorio.

El préstamo inicialmente podrá tener el estado:

`PENDIENTE`

---

## 9.4 Aprobar préstamo

**PATCH**

`/api/v1/prestamos/{id_prestamo}/aprobar`

Permite aprobar un préstamo pendiente.

Al aprobarse, el préstamo pasará al estado:

`ACTIVO`

---

## 9.5 Rechazar o cancelar préstamo

**PATCH**

`/api/v1/prestamos/{id_prestamo}/rechazar` para `PENDIENTE→ANULADO` por rechazo (con `{motivo: 10-200 chars}`). `CANCELADO` queda reservado a saldado RN-PRE-010, nunca a rechazo. El ANULADO por rechazo se distingue del ANULADO por error mediante `de_estado` en `Historial_Estado` (`PENDIENTE` vs `ACTIVO/MOROSO`).

**PATCH** `/api/v1/prestamos/{id_prestamo}/anular` para `ACTIVO/MOROSO→ANULADO` solo antes del primer pago (con motivo + actor). Roles: solo ADMIN.

---

## 9.6 Actualizar estado del préstamo

**PATCH**

`/api/v1/prestamos/{id_prestamo}/estado`

Queda prohibida la transición libre. Solo para casos admin explícitos auditados; en v1 usar `/aprobar`, `/rechazar`, `/anular`, `/reactivar`. La transición `ACTIVO→MOROSO` solo la hace el scheduler, nunca API manual.

Estados únicos (RN-PRE-011):

* `PENDIENTE`
* `ACTIVO`
* `MOROSO`
* `CANCELADO`
* `ANULADO`

Se eliminan `PAGADO/VENCIDO` como estados de préstamo (son estados de cuota). Cualquier otro valor → 422.

## 9.6b Reactivar préstamo

**PATCH** `/api/v1/prestamos/{id_prestamo}/reactivar [ADMIN] {motivo: 10-200 chars} → MOROSO→ACTIVO` (no resetea vencidos, ver RN-PRE-012). Sin este endpoint el evento `LOAN_REACTIVATED` no es implementable.

---

## 9.7 Obtener cuotas del préstamo

**GET**

`/api/v1/prestamos/{id_prestamo}/cuotas`

Permite obtener todas las cuotas asociadas a un préstamo.

---

## 9.8 Obtener pagos del préstamo

**GET**

`/api/v1/prestamos/{id_prestamo}/pagos`

Permite obtener el historial de pagos.

---

## 9.9 Obtener resumen financiero

**GET**

`/api/v1/prestamos/{id_prestamo}/resumen`

Permite obtener un resumen financiero del préstamo.

El resumen podrá incluir:

* Capital inicial.
* Capital pagado.
* Capital pendiente.
* Intereses generados.
* Intereses pagados.
* Intereses pendientes.
* Mora pendiente.
* Total pagado.
* Saldo pendiente.
* Estado del préstamo.

---

# 10. Endpoints de Cuotas

Rutas literales tienen prioridad sobre `/{id}`. `{id_cuota: uuid v4}`.

## 10.1 Listar cuotas

**GET**

`/api/v1/cuotas?prestamoId=uuid&estado=PENDIENTE|VENCIDA|PAGADA|PARCIAL&page=1&pageSize=20`

Permite obtener cuotas registradas. Reemplaza a `/cuotas/pendientes`, `/cuotas/vencidas` y al duplicado `GET /prestamos/{id}/cuotas` (§10.5 eliminado, usar §9.7 como única fuente por préstamo).

## 10.2 Obtener cuota

**GET**

`/api/v1/cuotas/{id_cuota}`

Permite obtener el detalle de una cuota.

---

## 10.3 Obtener cuotas pendientes

Eliminado. Usar `GET /api/v1/cuotas?estado=PENDIENTE`.

---

## 10.4 Obtener cuotas vencidas

Eliminado. Usar `GET /api/v1/cuotas?estado=VENCIDA`.

---

## 10.5 Obtener cuotas por préstamo

Eliminado (duplicado de §9.7). Usar `GET /api/v1/prestamos/{id_prestamo}/cuotas`.

---

# 11. Endpoints de Pagos

## 11.1 Listar pagos

**GET**

`/api/v1/pagos`

Permite obtener el historial general de pagos.

---

## 11.2 Obtener pago

**GET**

`/api/v1/pagos/{id_pago}`

Permite obtener el detalle de un pago.

---

## 11.3 Registrar pago

**POST**

`/api/v1/pagos`

Permite registrar un pago realizado por un cliente.

El backend será responsable de aplicar correctamente el pago según las reglas financieras.

Norma única (ver RN-PAG-003, no duplicar):

1. Mora.
2. Interés pendiente (FIFO por vencimiento).
3. Capital pendiente.

Pagos parciales permitidos, no se reordena.

---

## 11.4 Obtener distribución de un pago

**GET**

`/api/v1/pagos/{id_pago}/detalle`

Permite obtener cómo fue distribuido el pago entre capital, interés y mora.

---

## 11.5 Anular pago

**PATCH**

`/api/v1/pagos/{id_pago}/anular`

Permite anular un pago registrado.

El pago no deberá eliminarse físicamente.

Su estado cambiará a:

`ANULADO`

---

# 12. Endpoints de Morosidad

## 12.1 Listar moras

**GET**

`/api/v1/moras`

Permite obtener los registros de morosidad.

---

## 12.2 Obtener mora

**GET**

`/api/v1/moras/{id_mora}`

Permite obtener el detalle de una mora.

---

## 12.3 Obtener préstamos morosos

**GET**

`/api/v1/prestamos/morosos`

Permite obtener los préstamos que presentan retrasos.

---

## 12.4 Obtener morosidad de un préstamo

**GET**

`/api/v1/prestamos/{id_prestamo}/moras`

Permite obtener la información de morosidad relacionada con un préstamo.

---

## 12.5 Recalcular morosidad

**POST** `[ADMIN]`

`/api/v1/moras/recalcular`

Permite ejecutar el proceso de actualización de morosidad según RN-MOR-005. Rate 1/min → 429 si se excede. Retorna `202 {jobId}` (async, idempotente por día). Solo ADMIN.

---

## 12.6 Webhook WhatsApp

**GET+POST** `/api/v1/whatsapp/webhook` (verificación `hub.mode/challenge` + `X-Hub-Signature-256`, responde 200 <5s, procesa async). Endpoints públicos: `POST /auth/login`, `POST /auth/refresh`, `GET/POST /api/v1/whatsapp/webhook`. Resto requiere Bearer → 401 sin token.

---

# 13. Endpoints de Notificaciones

## 13.1 Listar notificaciones

**GET**

`/api/v1/notificaciones`

Permite obtener las notificaciones del usuario autenticado.

---

## 13.2 Obtener notificación

**GET**

`/api/v1/notificaciones/{id_notificacion}`

Permite obtener el detalle de una notificación.

---

## 13.3 Marcar como leída

**PATCH**

`/api/v1/notificaciones/{id_notificacion}/leer`

Permite marcar una notificación como leída.

---

## 13.4 Registrar token del dispositivo

**POST** (Bearer)

`/api/v1/dispositivos/token {"token":"fcm:...","plataforma":"android","deviceId":"uuid","appVersion":"1.0"}`

Permite registrar o actualizar el token de Firebase Cloud Messaging del dispositivo.

---

## 13.5 Eliminar token del dispositivo

**DELETE** `/api/v1/dispositivos/token?deviceId=uuid` (sin body; el body en DELETE es problemático).

Permite eliminar o invalidar el token asociado (marca `activo=false`).

---

# 14. Endpoints de WhatsApp

## 14.1 Listar mensajes

**GET**

`/api/v1/whatsapp/mensajes`

Permite obtener el historial de mensajes enviados.

---

## 14.2 Obtener mensaje

**GET**

`/api/v1/whatsapp/mensajes/{id_mensaje}`

Permite obtener el detalle de un mensaje.

---

## 14.3 Enviar mensaje

**POST** `[ADMIN, COBRADOR asignado]`

`/api/v1/whatsapp/mensajes {"clienteId":"uuid","prestamoId":"uuid","template":"wh_pago_recordatorio_v1","lang":"es_PE","params":{"1":"Juan","2":"15/09/2026"}}`

Permite enviar un mensaje mediante plantilla aprobada Meta (100% plantilla en v1, sintaxis `{{1}}` posicional).

---

## 14.4 Programar mensaje

**POST**

`/api/v1/whatsapp/mensajes/programar {same + "scheduledAt":"2026-09-15T08:00:00-05:00"}` (ISO8601 America/Lima).

Permite programar el envío de un mensaje para una fecha y hora determinada.

---

## 14.5 Obtener plantillas

**GET**

`/api/v1/whatsapp/plantillas`

Permite obtener las plantillas disponibles para el envío de mensajes.

---

# 15. Endpoints de Dashboard y Reportes

## 15.1 Obtener resumen general

**GET**

`/api/v1/dashboard/resumen`

Permite obtener indicadores generales del sistema.

Podrá incluir:

* Total de clientes.
* Préstamos activos.
* Préstamos pendientes.
* Préstamos vencidos.
* Capital prestado.
* Capital recuperado.
* Intereses generados.
* Pagos recibidos.
* Morosidad total.

---

## 15.2 Obtener préstamos próximos a vencer

**GET**

`/api/v1/dashboard/proximos-vencimientos?dias=7` (default 7, max 30).

Permite obtener los préstamos o cuotas próximos a vencer (ventana `0 <= vencimiento-hoy <= dias`, America/Lima).

---

## 15.3 Obtener clientes morosos

**GET**

`/api/v1/dashboard/clientes-morosos`

Permite obtener los clientes que presentan pagos pendientes o vencidos.

---

## 15.4 Obtener actividad reciente

**GET**

`/api/v1/dashboard/actividad-reciente`

Permite obtener las operaciones recientes del sistema.

Por ejemplo:

* Nuevo cliente registrado.
* Préstamo creado.
* Préstamo aprobado.
* Pago registrado.
* Pago anulado.
* Cambio de estado.

---

# 16. Resumen General de Endpoints

| Módulo         | Endpoint principal | Operación                   |
| -------------- | ------------------ | --------------------------- |
| Autenticación  | `/auth`            | Inicio y control de sesión  |
| Usuarios       | `/usuarios`        | Gestión de usuarios         |
| Roles          | `/roles`           | Gestión de roles            |
| Clientes       | `/clientes`        | Gestión de clientes         |
| Préstamos      | `/prestamos`       | Gestión de préstamos        |
| Cuotas         | `/cuotas`          | Consulta de cuotas          |
| Pagos          | `/pagos`           | Registro y control de pagos |
| Moras          | `/moras`           | Gestión de morosidad        |
| Notificaciones | `/notificaciones`  | Gestión de notificaciones   |
| Dispositivos   | `/dispositivos`    | Gestión de tokens FCM       |
| WhatsApp       | `/whatsapp`        | Mensajería y plantillas     |
| Dashboard      | `/dashboard`       | Indicadores y reportes      |

---

# 17. Consideraciones Generales

1. Todos los endpoints deberán utilizar HTTPS (ver §1), incluso en desarrollo con cert local.
2. Públicos sin auth: `POST /auth/login`, `POST /auth/refresh`, `GET/POST /api/v1/whatsapp/webhook`. Todo lo demás requiere `Authorization: Bearer` → 401 si falta/inválido.
3. El backend será responsable de validar los datos recibidos.
4. Las operaciones financieras deberán ejecutarse mediante transacciones.
5. Los pagos no deberán eliminarse físicamente.
6. Los préstamos deberán conservar su historial.
7. Los usuarios solo podrán acceder a los recursos permitidos según matriz `09-Seguridad/02-Autorizacion.md` (ADMIN todo; COBRADOR solo asignados, no `/moras/recalcular` ni config plantillas).
8. Las respuestas deberán utilizar envelope §4.1 + schemas `03-Modelos-Request-Response.md`.
9. Los errores deberán devolver código §4 + `traceId`.
10. Las reglas financieras críticas deberán ejecutarse en el backend y no depender únicamente de la aplicación móvil.
11. La API deberá versionarse utilizando el prefijo `/api/v1` (solo major por path).
12. Toda lista incorpora paginación `?page&pageSize` + filtros §8.1/§9.1/§10.1 cuando aplique.
