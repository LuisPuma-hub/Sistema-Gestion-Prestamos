# Autenticación y Gestión de Sesiones

## 1. Introducción

Este documento define el mecanismo de autenticación y gestión de sesiones del Sistema de Gestión de Préstamos.

La autenticación permitirá identificar de forma segura a los usuarios que acceden al sistema y controlar las operaciones que pueden realizar según el rol que tengan asignado.

La aplicación móvil se comunicará con el backend mediante una API REST protegida mediante tokens de autenticación.

El sistema utilizará una arquitectura basada en:

- Autenticación mediante correo electrónico y contraseña.
- Access Token para acceder a los recursos protegidos.
- Refresh Token para renovar la sesión.
- Control de autorización mediante roles.
- Almacenamiento seguro de contraseñas mediante hash.
- Comunicación segura mediante HTTPS en producción.

---

## 2. Objetivos

El sistema de autenticación deberá permitir:

1. Identificar correctamente a cada usuario.
2. Validar las credenciales proporcionadas.
3. Generar tokens de acceso.
4. Controlar el acceso a endpoints protegidos.
5. Renovar sesiones sin solicitar nuevamente las credenciales cuando corresponda.
6. Permitir cerrar sesiones.
7. Invalidar sesiones comprometidas cuando sea necesario.
8. Controlar el acceso según los roles asignados.
9. Proteger las contraseñas de los usuarios.
10. Evitar el acceso no autorizado a la información financiera.

---

## 3. Tipo de Autenticación

El sistema utilizará autenticación basada en tokens.

El flujo general será:

`Usuario → Aplicación móvil → API → Validación de credenciales → Generación de tokens`

Después de iniciar sesión correctamente, el backend devolverá los tokens necesarios para acceder a los recursos protegidos.

---

## 4. Credenciales de Acceso

Para iniciar sesión, el usuario deberá proporcionar:

- Correo electrónico.
- Contraseña.

Ejemplo de solicitud:

`POST /api/v1/auth/login`

Datos enviados:

`email`

`password`

El backend verificará que:

1. El usuario exista.
2. El usuario se encuentre activo.
3. La contraseña proporcionada sea correcta.
4. El usuario tenga permisos para acceder al sistema.

---

## 5. Contraseñas

Las contraseñas nunca deberán almacenarse en texto plano.

La base de datos almacenará únicamente:

`password_hash`

El hash deberá generarse utilizando un algoritmo seguro, como:

- Argon2.
- bcrypt.

El backend será responsable de comparar la contraseña proporcionada con el hash almacenado.

La contraseña original nunca deberá devolverse mediante la API.

---

## 6. Access Token

El Access Token será utilizado para acceder a los endpoints protegidos de la API.

Después de un inicio de sesión exitoso, el backend generará un token que representará la sesión autenticada del usuario.

El cliente deberá enviar el token en cada solicitud protegida mediante el encabezado:

`Authorization: Bearer ACCESS_TOKEN`

Ejemplo:

`Authorization: Bearer eyJ...`

El token permitirá al backend identificar al usuario autenticado.

---

## 7. Contenido del Access Token

El Access Token será JWT RS256 (clave privada en backend, pública para validar).

Claims normativos: `iss=api-prestamos, aud=mobile, sub=userId(uuid), roles:[...], jti, iat, exp`. Skew 60s. Access 15min (`JWT_ACCESS_TTL=900s` por env, requiere redeploy para cambiar).

Por ejemplo:

- Identificador del usuario (`sub`).
- Correo electrónico.
- Roles o permisos.
- Fecha de emisión (`iat`).
- Fecha de expiración (`exp`).

No deberán almacenarse dentro del token:

- Contraseñas.
- Información financiera sensible.
- Datos personales innecesarios.

La información crítica deberá obtenerse desde el backend cuando sea necesario.

---

## 8. Expiración del Access Token

El Access Token deberá tener una duración limitada.

Esto reduce el riesgo en caso de que un token sea comprometido.

Duración normativa: `15 minutos` (configurable solo por env `JWT_ACCESS_TTL`, no por API sin ADMIN).

Cuando el Access Token expire, el cliente deberá utilizar el Refresh Token para solicitar uno nuevo.

---

## 9. Refresh Token

El Refresh Token permitirá obtener un nuevo Access Token sin que el usuario tenga que iniciar sesión nuevamente.

El endpoint será:

`POST /api/v1/auth/refresh`

El flujo será:

1. El Access Token expira.
2. La aplicación detecta que necesita renovar la sesión.
3. La aplicación envía el Refresh Token.
4. El backend valida el Refresh Token.
5. Si es válido, genera un nuevo Access Token.
6. La aplicación continúa utilizando la sesión.

---

## 10. Duración del Refresh Token

Tabla normativa (configurable solo por env, cambios requieren redeploy):

- Access Token: 15 minutos.
- Refresh Token: 7 días, opaque de 256-bit (no JWT), enviado en body `{refreshToken}`.

Rotación obligatoria + reuse-detection (ver §15). Por razones de seguridad, el Refresh Token podrá ser invalidado antes de su fecha de expiración.

---

## 11. Inicio de Sesión

El endpoint utilizado será:

`POST /api/v1/auth/login`

El proceso será:

1. El usuario ingresa su correo electrónico.
2. El usuario ingresa su contraseña.
3. La aplicación envía las credenciales mediante HTTPS.
4. El backend valida los datos.
5. El backend verifica el estado del usuario.
6. El backend verifica la contraseña.
7. Se generan los tokens de autenticación.
8. El backend devuelve la información necesaria para iniciar sesión.

Si las credenciales son correctas, el usuario podrá acceder a los recursos autorizados según su rol.

---

## 12. Respuesta de Inicio de Sesión

La respuesta incluirá (contrato exacto en `03-Modelos-Request-Response.md` / OpenAPI 3.1):

`LoginResponse{user{id,email}, accessToken:jwt, refreshToken:opaque, expiresIn:900, tokenType:Bearer, roles:["ADMIN"]}`.

Ejemplo conceptual:

`usuario`

`accessToken`

`refreshToken`

`expiresIn`

`roles`

La estructura exacta será definida en el documento:

`03-Modelos-Request-Response.md`

---

## 13. Usuario Autenticado

Para obtener la información del usuario que tiene la sesión activa se utilizará:

`GET /api/v1/auth/me`

Este endpoint requerirá un Access Token válido.

La respuesta podrá incluir:

- Identificador.
- Nombre.
- Apellido.
- Correo electrónico.
- Teléfono.
- Estado.
- Roles asignados.

No deberá incluir:

- Contraseña.
- Hash de contraseña.
- Refresh Token.
- Información interna sensible.

---

## 14. Renovación de Sesión

Cuando el Access Token expire, la aplicación podrá utilizar el endpoint:

`POST /api/v1/auth/refresh`

El backend deberá validar:

1. Que el Refresh Token exista.
2. Que no esté expirado.
3. Que no haya sido revocado.
4. Que corresponda a un usuario válido.
5. Que el usuario se encuentre activo.

Si la validación es correcta, el backend generará un nuevo Access Token + nuevo Refresh Token (rotación obligatoria, ver §15).

---

## 15. Rotación de Refresh Tokens

Rotación obligatoria con reuse-detection: si se reusa un refresh revocado, se revoca la familia completa.

El proceso será:

1. El cliente envía el Refresh Token actual.
2. El backend valida el token.
3. El backend invalida el Refresh Token anterior.
4. El backend genera un nuevo Refresh Token.
5. El backend genera un nuevo Access Token.
6. El cliente reemplaza ambos tokens almacenados.

Esto reduce el riesgo asociado con la reutilización prolongada de un mismo Refresh Token.

---

## 16. Cierre de Sesión

El endpoint utilizado será:

`POST /api/v1/auth/logout`

El proceso será:

1. El usuario solicita cerrar sesión.
2. La aplicación informa al backend.
3. El backend invalida el Refresh Token o la sesión correspondiente.
4. La aplicación elimina los tokens almacenados localmente.
5. El usuario es redirigido a la pantalla de inicio de sesión.

El cierre de sesión deberá impedir que el Refresh Token utilizado continúe generando nuevos Access Tokens.

---

## 17. Cierre de Sesión en Todos los Dispositivos

El sistema permitirá cerrar todas las sesiones (requerido por `09-Seguridad/01 §15`).

Endpoints:

`POST /api/v1/auth/logout-all → revoca todos los refresh del usuario. GET /api/v1/auth/sesiones → lista {jti, deviceId, lastSeen}`.

Esta operación deberá invalidar todos los Refresh Tokens activos asociados al usuario.

Esta funcionalidad será útil cuando:

- El usuario sospeche que su cuenta fue comprometida.
- Se cambie la contraseña.
- Un administrador bloquee al usuario.
- Se detecte actividad sospechosa.

---

## 18. Endpoints Protegidos

Los endpoints relacionados con información sensible requerirán autenticación.

Ejemplos:

`GET /api/v1/clientes`

`POST /api/v1/prestamos`

`POST /api/v1/pagos`

`GET /api/v1/dashboard/resumen`

La aplicación deberá enviar:

`Authorization: Bearer ACCESS_TOKEN`

Si el token no es válido o ha expirado, el backend deberá devolver:

`401 Unauthorized`

---

## 19. Autorización por Roles

La autenticación responde a la pregunta:

**¿Quién es el usuario?**

La autorización responde a la pregunta:

**¿Qué puede hacer el usuario?**

El sistema inicialmente considerará los siguientes roles:

### Administrador

El administrador tendrá acceso completo a las funciones autorizadas del sistema.

Podrá:

- Gestionar usuarios.
- Gestionar roles.
- Registrar clientes.
- Aprobar préstamos.
- Registrar pagos.
- Consultar información financiera.
- Gestionar notificaciones.
- Gestionar mensajes de WhatsApp.
- Consultar reportes.
- Configurar aspectos permitidos del sistema.

### Prestamista o Cobrador

Inicialmente, el rol de prestamista podrá estar asociado también a las funciones de cobranza.

Matriz mínima (detalle en `09-Seguridad/02-Autorizacion.md`): `ADMIN: todo. COBRADOR: CRUD clientes asignados, crear pago, no aprobar préstamo, no /moras/recalcular, no config plantillas. POST /whatsapp/mensajes solo ADMIN o COBRADOR asignado`.

---

## 20. Usuario Inactivo o Bloqueado

Un usuario con estado:

`INACTIVO`

o:

`BLOQUEADO`

no deberá poder iniciar sesión.

Si un usuario es bloqueado mientras tiene una sesión activa, el backend revocará sus Refresh inmediatamente y denegará el siguiente request con access (middleware consulta estado o caché 60s) → 403 ACCOUNT_BLOCKED. El access stateless solo vive por su expiración corta (15min) o blacklist jti 15min.

---

## 21. Almacenamiento de Tokens en la Aplicación Móvil

Los tokens no deberán almacenarse en texto plano en ubicaciones inseguras.

La aplicación móvil deberá utilizar almacenamiento seguro proporcionado por el sistema operativo.

El objetivo es reducir el riesgo de exposición de:

- Access Token.
- Refresh Token.
- Información de sesión.

La implementación específica dependerá de la tecnología utilizada para desarrollar la aplicación móvil.

---

## 22. Comunicación Segura

En producción, toda comunicación entre la aplicación móvil y el backend deberá realizarse mediante:

`HTTPS`

No deberán enviarse:

- Contraseñas.
- Tokens.
- Información financiera.
- Datos personales.

mediante conexiones HTTP sin cifrado.

---

## 23. Manejo de Errores de Autenticación

El backend deberá devolver códigos HTTP apropiados.

### Credenciales incorrectas

Código:

`401 Unauthorized`

### Token inválido

Código:

`401 Unauthorized`

### Token expirado

Código:

`401 Unauthorized`

La aplicación podrá intentar renovar la sesión utilizando el Refresh Token.

### Usuario sin permisos

Código:

`403 Forbidden`

### Usuario bloqueado

Código:

`403 Forbidden`

### Solicitud inválida

Código:

`400 Bad Request`

---

## 24. Protección contra Intentos Repetidos

Rate-limit normativo: `POST /auth/login: 5/min/IP + 5/15min/email → 429 Retry-After + bloqueo temporal 15min tras 5 fallidos`. Log intento + alerta. Bloqueo temporal con registro de evento de seguridad.

---

## 25. Cambio de Contraseña

Endpoint: `PATCH /api/v1/auth/password` (requiere Bearer + password actual).

Como funcionalidad del sistema, un usuario podrá cambiar su contraseña. Reglas: min 10, mayús+minús+número, no top-1000, Argon2id/bcrypt.

El proceso deberá requerir:

1. Contraseña actual.
2. Nueva contraseña.
3. Confirmación de la nueva contraseña.

El backend deberá:

1. Validar la contraseña actual.
2. Validar las reglas de seguridad de la nueva contraseña.
3. Generar el nuevo hash.
4. Actualizar el registro.
5. Opcionalmente invalidar las sesiones activas.

---

## 26. Recuperación de Contraseña (v2 — diseñado, fuera del MVP, ver ROADMAP)

Endpoints: `POST /api/v1/auth/forgot {email} → siempre 200 genérico. POST /api/v1/auth/reset {token, newPassword}`.

Token reset opaque, 15-30min TTL, single-use, hash en DB. Cambio exige las mismas reglas de §25.

El proceso podría ser:

1. El usuario solicita recuperar su contraseña.
2. El sistema genera un token temporal.
3. Se envía un mecanismo de recuperación.
4. El usuario valida su identidad.
5. El usuario establece una nueva contraseña.
6. El token de recuperación queda invalidado.

El token de recuperación deberá:

- Tener una duración limitada.
- Poder utilizarse una sola vez.
- Almacenarse de manera segura.

---

## 27. Flujo General de Autenticación

El flujo principal será:

`Usuario`

↓

`Ingresa correo y contraseña`

↓

`Aplicación móvil`

↓

`POST /api/v1/auth/login`

↓

`Backend valida credenciales`

↓

`Credenciales válidas`

↓

`Generación de Access Token y Refresh Token`

↓

`Aplicación almacena los tokens de forma segura`

↓

`Usuario accede a recursos protegidos`

↓

`Access Token expira`

↓

`POST /api/v1/auth/refresh`

↓

`Nuevo Access Token`

↓

`Continuación de la sesión`

---

## 28. Flujo de Cierre de Sesión

El proceso será:

`Usuario solicita cerrar sesión`

↓

`POST /api/v1/auth/logout`

↓

`Backend invalida Refresh Token`

↓

`Aplicación elimina tokens locales`

↓

`Sesión finalizada`

---

## 29. Consideraciones de Seguridad

El sistema deberá cumplir las siguientes consideraciones:

1. Las contraseñas nunca se almacenarán en texto plano.
2. Los tokens tendrán una fecha de expiración.
3. Los Access Tokens tendrán una duración corta.
4. Los Refresh Tokens podrán ser revocados.
5. Los endpoints sensibles requerirán autenticación.
6. Los permisos dependerán del rol del usuario.
7. Los usuarios bloqueados no podrán acceder al sistema.
8. La comunicación utilizará HTTPS en producción.
9. Los tokens se almacenarán de forma segura en la aplicación móvil.
10. Se deberán limitar los intentos repetidos de inicio de sesión.
11. No se deberán incluir datos sensibles innecesarios dentro de los tokens.
12. Los errores no deberán revelar información que facilite ataques.

---

## 30. Resumen

La autenticación del Sistema de Gestión de Préstamos estará basada en tokens.

El sistema utilizará:

- Correo electrónico y contraseña para el inicio de sesión.
- Hash seguro para proteger contraseñas.
- Access Token para acceder a recursos protegidos.
- Refresh Token para renovar sesiones.
- Control de acceso basado en roles.
- HTTPS para proteger la comunicación.
- Almacenamiento seguro de tokens en la aplicación móvil.
- Mecanismos de expiración y revocación de sesiones.

La autenticación y autorización serán responsabilidad principalmente del backend, mientras que la aplicación móvil será responsable de gestionar la sesión y almacenar los tokens de manera segura.