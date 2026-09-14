# 09 - Autenticación

## 1. Objetivo

Este documento define el mecanismo de autenticación del **Sistema de Gestión de Préstamos**.

La autenticación permitirá verificar la identidad de los usuarios antes de permitirles acceder a las funcionalidades del sistema.

El mecanismo deberá garantizar:

- Identificación segura de usuarios.
- Protección de credenciales.
- Control de sesiones.
- Recuperación de contraseñas.
- Protección contra accesos no autorizados.
- Comunicación segura entre la aplicación móvil y el backend.
- Integración con el sistema de autorización basado en roles y permisos.

---

# 2. Alcance

La autenticación será utilizada por los usuarios internos del sistema, principalmente:

- Administrador.
- Cobrador.

La autenticación será utilizada para acceder a:

- Dashboard.
- Clientes.
- Préstamos.
- Pagos.
- Morosidad.
- Notificaciones.
- WhatsApp.
- Perfil.
- Configuración.

El cliente del préstamo no tendrá acceso directo al sistema administrativo en esta primera versión.

---

# 3. Componentes involucrados

La autenticación estará integrada con:

| Componente | Responsabilidad |
|---|---|
| .NET MAUI | Interfaz de inicio de sesión |
| ASP.NET Core Web API | Autenticación y validación |
| PostgreSQL | Persistencia de usuarios |
| Entity Framework Core | Acceso a datos |
| HTTPS | Protección de comunicaciones |
| Sistema de autorización | Control de permisos |
| Sistema de auditoría | Registro de accesos |

---

# 4. Flujo general de autenticación

El flujo será:

```text
Usuario
   ↓
Aplicación .NET MAUI
   ↓
Formulario de Login
   ↓
Enviar credenciales mediante HTTPS
   ↓
ASP.NET Core Web API
   ↓
Validar usuario
   ↓
Validar contraseña
   ↓
¿Credenciales correctas?
   ┌────┴────┐
  No        Sí
  ↓          ↓
Error     Generar sesión
             ↓
       Retornar resultado
             ↓
       Aplicación móvil
             ↓
          Dashboard
```

---

# 5. Inicio de sesión

El usuario deberá proporcionar:

- Correo electrónico.
- Contraseña.

Formulario conceptual:

```text
┌──────────────────────────────────┐
│        SISTEMA DE PRÉSTAMOS      │
│                                  │
│ Correo electrónico               │
│ [____________________________]   │
│                                  │
│ Contraseña                       │
│ [____________________________]   │
│                                  │
│ [        INICIAR SESIÓN        ] │
│                                  │
│ ¿Olvidaste tu contraseña?        │
└──────────────────────────────────┘
```

---

# 6. Credenciales

Las credenciales estarán compuestas por:

```text
Correo electrónico
+
Contraseña
```

El correo electrónico deberá identificar de forma única al usuario.

No se deberán almacenar contraseñas en texto plano.

---

# 7. Protección de contraseñas

Las contraseñas deberán almacenarse utilizando mecanismos seguros de hashing proporcionados por las herramientas de autenticación de ASP.NET Core.

El sistema deberá:

- No almacenar contraseñas en texto plano.
- No mostrar contraseñas en registros.
- No enviar contraseñas por correo electrónico.
- No incluir contraseñas en respuestas de la API.
- No almacenar contraseñas en la aplicación móvil.
- Utilizar algoritmos seguros de derivación de contraseñas.

La validación deberá realizarse comparando la contraseña proporcionada con el valor almacenado de forma segura.

---

# 8. Validación del usuario

Cuando el usuario intente iniciar sesión, el backend deberá verificar:

1. Que el correo exista.
2. Que el usuario esté activo.
3. Que la contraseña sea correcta.
4. Que la cuenta pueda iniciar sesión.
5. Que no exista alguna restricción de seguridad aplicable.

Si alguna validación falla, el sistema deberá rechazar el acceso.

---

# 9. Respuesta de autenticación

Cuando las credenciales sean correctas, el backend deberá retornar (alineado a `05-API/02 §12` y `03-Modelos-Request-Response.md`):

```json
{"success": true, "user": {"id": "uuid", "email": "..."}, "accessToken": "jwt RS256", "refreshToken": "opaque", "expiresIn": 900, "roles": ["ADMIN"]}
```

`role:string` singular queda prohibido (rompe multi-rol). La estructura definitiva es la de la API.

---

# 10. Token de autenticación

La aplicación móvil utilizará un mecanismo de token para realizar solicitudes autenticadas al backend.

El token permitirá que el backend identifique al usuario que realiza cada solicitud.

Flujo:

```text
Login
  ↓
Credenciales válidas
  ↓
Backend genera token
  ↓
Aplicación recibe token
  ↓
Aplicación conserva sesión
  ↓
Solicitudes posteriores
  ↓
Token enviado al backend
```

---

# 11. Uso del token

Las solicitudes protegidas deberán incluir el mecanismo de autenticación correspondiente.

Conceptualmente:

```text
Aplicación
    ↓
Solicitud HTTPS
    ↓
Token de autenticación
    ↓
ASP.NET Core Web API
    ↓
Validar token
    ↓
¿Válido?
 ┌───┴───┐
No       Sí
↓         ↓
401      Procesar solicitud
```

---

# 12. Expiración de sesión

Access 15min, Refresh 7d con rotación obligatoria (ver `05-API/02 §8/§10/§15`). La duración se configura solo por env, no por API.

---

# 13. Cierre de sesión

El usuario podrá cerrar su sesión desde la aplicación.

Flujo:

```text
Usuario
  ↓
Cerrar sesión
  ↓
Eliminar información de sesión
  ↓
Volver al Login
```

La aplicación deberá eliminar de forma segura la información de autenticación almacenada localmente.

---

# 14. Sesiones activas

El sistema deberá contemplar la gestión de sesiones activas.

Desde el perfil del usuario se podrá mostrar información relacionada con:

- Sesión actual.
- Otras sesiones activas.
- Fecha de inicio.
- Última actividad.
- Dispositivo cuando corresponda.

---

# 15. Cerrar otras sesiones

El usuario podrá disponer de una opción para cerrar otras sesiones activas.

Flujo:

```text
Perfil
  ↓
Sesiones activas
  ↓
Cerrar otras sesiones
  ↓
Confirmar
  ↓
Backend
  ↓
Invalidar sesiones correspondientes
```

La sesión actual deberá mantenerse activa, salvo que el usuario seleccione cerrar todas las sesiones.

---

# 16. Recuperación de contraseña

El sistema deberá permitir recuperar el acceso cuando el usuario olvide su contraseña.

Flujo:

```text
Login
  ↓
¿Olvidaste tu contraseña?
  ↓
Ingresar correo
  ↓
Backend
  ↓
Generar proceso de recuperación
  ↓
Enviar mecanismo de recuperación
  ↓
Usuario verifica acceso
  ↓
Crear nueva contraseña
  ↓
Confirmar cambio
  ↓
Volver al Login
```

El mecanismo concreto de envío podrá implementarse mediante correo electrónico u otro canal seguro definido posteriormente.

---

# 17. Restablecimiento de contraseña

La nueva contraseña deberá cumplir las reglas de seguridad establecidas.

Después de cambiar la contraseña:

- La contraseña anterior dejará de ser válida.
- El cambio deberá quedar registrado.
- Se podrán invalidar sesiones anteriores según la política de seguridad.
- El usuario podrá iniciar sesión nuevamente con la nueva contraseña.

---

# 18. Requisitos mínimos de contraseña

Normativa (no ejemplo): mín 10, 1 mayús, 1 min, 1 dígito, no top-1000, Argon2id/bcrypt. Al cambiar: verificar actual siempre, invalida otros refresh, mantiene sesión actual, notifica cambio.

---

# 19. Protección contra intentos repetidos

Normativo: `POST /api/v1/auth/login: 5/min/IP + 5/15min/email → 429 Retry-After + bloqueo 15min tras 5 fallidos`. Mensaje genérico `"Las credenciales proporcionadas no son válidas"` (no revela si falla email o password).

---

# 20. Mensajes de error

Los mensajes de autenticación no deberán revelar información innecesaria.

No se recomienda indicar:

```text
El correo no existe.
```

y después:

```text
La contraseña es incorrecta.
```

Se recomienda utilizar un mensaje general:

```text
Las credenciales proporcionadas no son válidas.
```

Esto reduce la posibilidad de revelar información sobre usuarios registrados.

---

# 21. Comunicación segura

Toda comunicación entre la aplicación móvil y el backend deberá utilizar:

```text
HTTPS
```

Flujo:

```text
.NET MAUI
    ↓
HTTPS
    ↓
ASP.NET Core Web API
    ↓
PostgreSQL
```

Las credenciales y tokens no deberán transmitirse mediante conexiones inseguras.

---

# 22. Protección de tokens

Los tokens de autenticación deberán almacenarse de forma segura en la aplicación móvil.

No deberán almacenarse en:

- Archivos de texto sin protección.
- Logs.
- Código fuente.
- Parámetros visibles.
- Repositorios Git.

La implementación deberá utilizar mecanismos seguros de almacenamiento disponibles para .NET MAUI.

---

# 23. Expiración y renovación

El sistema deberá contemplar el ciclo de vida del token.

Conceptualmente:

```text
Token válido
     ↓
Uso normal
     ↓
Token próximo a expirar
     ↓
Renovación o nuevo login
     ↓
Nuevo token
```

La estrategia definitiva podrá utilizar expiración y renovación controlada de tokens según la configuración de seguridad adoptada.

---

# 24. Usuario autenticado

Después de autenticarse correctamente, el backend deberá identificar al usuario durante las solicitudes posteriores.

Información conceptual:

```text
Usuario
├── Id
├── Nombre
├── Correo
├── Rol
└── Estado
```

La identidad del usuario no deberá depender de datos enviados arbitrariamente por la aplicación móvil.

---

# 25. Identificación del usuario en las operaciones

Las operaciones importantes deberán asociarse al usuario autenticado.

Ejemplos:

```text
Registrar cliente
Registrar préstamo
Registrar pago
Enviar comunicación
Modificar configuración
Cambiar contraseña
```

El backend deberá determinar el usuario a partir de la sesión autenticada.

---

# 26. Autenticación y autorización

La autenticación y la autorización son conceptos diferentes.

### Autenticación

Responde:

> ¿Quién es el usuario?

### Autorización

Responde:

> ¿Qué puede hacer el usuario?

Flujo:

```text
Usuario
  ↓
Autenticación
  ↓
Identidad confirmada
  ↓
Autorización
  ↓
Evaluar permisos
  ↓
Permitir / Denegar
```

La autenticación será definida en este documento.

La autorización será desarrollada en:

```text
09-Seguridad/02-Autorizacion.md
```

---

# 27. Roles

Los roles iniciales serán:

| Rol | Descripción |
|---|---|
| Administrador | Gestiona y configura el sistema |
| Cobrador | Realiza operaciones permitidas de gestión y cobranza |

Los permisos asociados a cada rol serán definidos en el documento de autorización.

---

# 28. Estado de la cuenta

Cada usuario deberá tener un estado (enum único UPPER_SNAKE en DB, API y docs):

| Estado | Descripción |
|---|---|
| ACTIVO | Puede iniciar sesión |
| INACTIVO | No puede iniciar sesión |
| BLOQUEADO | Acceso temporalmente restringido → 403 ACCOUNT_BLOCKED |

El backend deberá verificar el estado antes de permitir el acceso.

---

# 29. Desactivación de usuarios

Cuando un administrador desactive un usuario:

```text
Usuario activo
      ↓
Administrador desactiva
      ↓
Estado = INACTIVO
      ↓
Usuario no puede iniciar sesión
```

Las sesiones existentes podrán ser invalidadas según la política definida.

---

# 30. Reactivación de usuarios

Un administrador autorizado podrá reactivar un usuario cuando corresponda.

Flujo:

```text
Usuario inactivo
      ↓
Administrador
      ↓
Reactivar
      ↓
Estado = ACTIVO
      ↓
Puede iniciar sesión
```

La operación deberá quedar registrada en auditoría.

---

# 31. Auditoría de autenticación

El sistema deberá registrar eventos relevantes de seguridad.

Ejemplos:

- Inicio de sesión exitoso.
- Inicio de sesión fallido.
- Cierre de sesión.
- Cambio de contraseña.
- Recuperación de contraseña.
- Bloqueo de cuenta.
- Desbloqueo.
- Activación de usuario.
- Desactivación de usuario.
- Cierre de otras sesiones.

---

# 32. Información de auditoría

Para cada evento de autenticación se podrá registrar:

| Campo | Descripción |
|---|---|
| ID | Identificador |
| Usuario | Usuario relacionado |
| Evento | Tipo de evento |
| Fecha | Fecha y hora |
| Resultado | Éxito o error |
| Dirección IP | Cuando corresponda |
| Dispositivo | Cuando corresponda |
| Descripción | Información adicional |

No deberán almacenarse contraseñas ni secretos.

---

# 33. Protección de datos sensibles

El sistema deberá evitar registrar información sensible innecesaria.

No deberá registrarse:

```text
Contraseña
Token completo
Credenciales de Meta
Secretos de configuración
```

Cuando sea necesario registrar identificadores técnicos, deberán aplicarse mecanismos adecuados para evitar exponer información confidencial.

---

# 34. Autenticación en la aplicación móvil

La aplicación .NET MAUI deberá manejar correctamente los estados de autenticación.

Estados conceptuales:

```text
SIN_AUTENTICAR
       ↓
AUTENTICANDO
       ↓
AUTENTICADO
       ↓
SESION_ACTIVA
       ↓
SESION_EXPIRADA
       ↓
SIN_AUTENTICAR
```

---

# 35. Protección de pantallas

Las pantallas administrativas no deberán estar disponibles para usuarios no autenticados.

Ejemplo:

```text
Usuario no autenticado
        ↓
Intentar acceder a Dashboard
        ↓
Redirigir a Login
```

Incluso si un usuario intenta acceder directamente a una ruta interna de la aplicación, deberá verificarse su autenticación.

---

# 36. Protección en el backend

La seguridad no deberá depender únicamente de la aplicación móvil.

El backend deberá validar:

- Token.
- Usuario.
- Estado de cuenta.
- Permisos.
- Datos recibidos.
- Operación solicitada.

Ejemplo:

```text
Aplicación
    ↓
Solicitud
    ↓
ASP.NET Core Web API
    ↓
Autenticación
    ↓
Autorización
    ↓
Validación
    ↓
Procesar operación
```

---

# 37. Validación de solicitudes

Toda solicitud protegida deberá ser validada. Todas las rutas usan prefijo `/api/v1` (ver `05-API/01`).

Por ejemplo:

```text
POST /api/v1/pagos
```

El backend deberá verificar:

1. Usuario autenticado.
2. Usuario autorizado.
3. Datos correctos.
4. Préstamo válido.
5. Cliente válido.
6. Reglas de negocio.
7. Operación permitida.

---

# 38. No confiar en datos del cliente

La aplicación móvil no deberá considerarse una fuente confiable para decisiones de seguridad.

Por ejemplo, no deberá confiarse únicamente en:

```text
role = "Administrador"
```

enviado desde la aplicación.

El backend deberá obtener la identidad y los permisos desde el contexto autenticado.

---

# 39. Protección de la API

Los endpoints sensibles deberán estar protegidos (prefijo `/api/v1`, ver tabla `05-API/01 §17`).

```text
/api/v1/clientes
/api/v1/prestamos
/api/v1/pagos
/api/v1/prestamos/morosos
/api/v1/notificaciones
/api/v1/whatsapp
/api/v1/configuracion
```

Cada endpoint deberá definir los requisitos de autenticación y autorización correspondientes.

---

# 40. Protección de operaciones financieras

Las operaciones financieras requerirán especial protección.

Ejemplos:

- Registrar pago.
- Modificar préstamo.
- Modificar interés.
- Modificar configuración financiera.
- Reactivar cliente.
- Modificar saldo mediante operaciones autorizadas.

Estas operaciones deberán ejecutarse exclusivamente mediante el backend y quedar registradas cuando corresponda.

---

# 41. Configuración financiera

Los parámetros financieros deberán estar protegidos.

Ejemplos:

```text
Interés semanal = 5 %
Frecuencia = Semanal
Múltiples préstamos = Habilitado
```

Un usuario sin permisos suficientes no deberá modificar estos valores.

Los cambios deberán contar con:

- Autenticación.
- Autorización.
- Validación.
- Auditoría.

---

# 42. Protección de credenciales de terceros

Las credenciales utilizadas para servicios externos, como WhatsApp Business Platform, no deberán almacenarse en la aplicación móvil.

Deberán mantenerse en el backend y gestionarse mediante mecanismos seguros de configuración.

Ejemplo conceptual:

```text
.NET MAUI
     X
     │
     │ No contiene secretos de Meta
     │
     ▼
ASP.NET Core Web API
     │
     ▼
Configuración segura
     │
     ▼
WhatsApp Business Platform
```

---

# 43. Inicio automático de sesión

La aplicación podrá ofrecer mecanismos para mantener la sesión cuando sea apropiado.

Sin embargo, esta funcionalidad deberá respetar:

- Seguridad del dispositivo.
- Expiración de sesión.
- Protección del token.
- Cierre de sesión.
- Revocación de sesiones.

No se deberá almacenar nuevamente la contraseña para mantener la sesión.

---

# 44. Sesión expirada

Cuando el backend indique que la sesión ya no es válida:

```text
Solicitud
   ↓
Respuesta de autenticación no válida
   ↓
Aplicación detecta sesión expirada
   ↓
Eliminar sesión local
   ↓
Mostrar Login
```

El usuario deberá volver a autenticarse cuando sea necesario.

---

# 45. Error de conexión

Si la aplicación no puede comunicarse con el backend:

```text
.NET MAUI
   ↓
Solicitud
   ↓
Sin conexión
   ↓
Mostrar mensaje
```

Ejemplo:

```text
No se pudo conectar con el servidor.
Verifica tu conexión e inténtalo nuevamente.
```

La aplicación no deberá asumir que una operación fue realizada correctamente si no recibió confirmación del backend.

---

# 46. Protección contra acceso no autorizado

El sistema deberá impedir:

- Acceso sin autenticación.
- Uso de tokens inválidos.
- Uso de tokens expirados.
- Acceso con cuentas inactivas.
- Acceso a funciones sin permisos.
- Modificación no autorizada de información.
- Uso de credenciales de otro usuario.

---

# 47. Flujo de seguridad completo

```text
Usuario
   ↓
Login
   ↓
HTTPS
   ↓
ASP.NET Core Web API
   ↓
Validar credenciales
   ↓
Validar estado
   ↓
Generar autenticación
   ↓
Aplicación recibe sesión
   ↓
Solicitud protegida
   ↓
Validar autenticación
   ↓
Validar autorización
   ↓
Validar datos
   ↓
Procesar operación
   ↓
Registrar auditoría
```

---

# 48. Pruebas de autenticación

Se deberán realizar pruebas para:

### Login

- Credenciales correctas.
- Correo inexistente.
- Contraseña incorrecta.
- Campos vacíos.
- Usuario inactivo.
- Usuario bloqueado.

### Sesión

- Token válido.
- Token inválido.
- Token expirado.
- Cierre de sesión.
- Cierre de otras sesiones.

### Contraseña

- Cambio de contraseña.
- Recuperación.
- Contraseña anterior inválida después del cambio.
- Contraseña insegura.

### Seguridad

- Acceso sin token.
- Token manipulado.
- Acceso a endpoint no autorizado.
- Intentos repetidos.
- Acceso con usuario desactivado.

---

# 49. Criterios de aceptación

El módulo de autenticación será considerado correctamente implementado cuando:

- [ ] El usuario pueda iniciar sesión mediante correo y contraseña.
- [ ] Las contraseñas no se almacenen en texto plano.
- [ ] El backend valide las credenciales.
- [ ] Los usuarios inactivos no puedan iniciar sesión.
- [ ] La comunicación utilice HTTPS.
- [ ] Las solicitudes protegidas requieran autenticación.
- [ ] El sistema utilice tokens o un mecanismo equivalente de sesión segura.
- [ ] Los tokens tengan expiración controlada.
- [ ] El usuario pueda cerrar sesión.
- [ ] El usuario pueda recuperar su contraseña.
- [ ] El usuario pueda cambiar su contraseña.
- [ ] Existan mecanismos para gestionar sesiones activas.
- [ ] Se registren eventos importantes de autenticación.
- [ ] No se almacenen contraseñas en logs.
- [ ] Los secretos de terceros permanezcan en el backend.
- [ ] El backend no confíe en roles enviados desde la aplicación.
- [ ] Las operaciones financieras estén protegidas.
- [ ] Los permisos se validen en el backend.
- [ ] Las sesiones expiradas sean rechazadas.

---

# 50. Dependencias

Este módulo depende de:

- .NET MAUI.
- ASP.NET Core Web API.
- PostgreSQL.
- Entity Framework Core.
- HTTPS.
- Sistema de autorización.
- Sistema de auditoría.
- Sistema de gestión de usuarios.
- Sistema de recuperación de contraseña.

---

# 51. Consideraciones futuras

En futuras versiones podrán incorporarse:

- Autenticación multifactor (MFA).
- Verificación mediante correo electrónico.
- Autenticación biométrica en dispositivos compatibles.
- Políticas avanzadas de contraseñas.
- Detección de actividad sospechosa.
- Control avanzado de dispositivos.
- Alertas de seguridad.
- Gestión avanzada de sesiones.
- Revocación inmediata de tokens.
- Integración con proveedores externos de identidad.

---

# 52. Resumen

La autenticación permitirá proteger el acceso al Sistema de Gestión de Préstamos y garantizar que únicamente los usuarios autorizados puedan utilizar las funcionalidades administrativas.

El proceso estará basado en:

- Correo electrónico.
- Contraseña.
- Sesiones autenticadas.
- Tokens.
- HTTPS.
- Validación en el backend.
- Protección de credenciales.
- Expiración de sesiones.
- Recuperación de contraseña.
- Auditoría.

La aplicación .NET MAUI será responsable de proporcionar la interfaz de autenticación, mientras que **ASP.NET Core Web API** será responsable de validar las credenciales y proteger los recursos del sistema.

La autenticación será complementada por un sistema de autorización que determinará qué funcionalidades puede utilizar cada usuario según su rol y permisos.

La seguridad deberá mantenerse principalmente en el backend, evitando confiar en información enviada directamente desde la aplicación móvil.