# Sistema de Gestión de Préstamos

Sistema móvil para la gestión y control de préstamos, clientes, pagos, morosidad y comunicaciones con los clientes.

El sistema está orientado a facilitar la administración de préstamos mediante una aplicación móvil conectada a un backend centralizado, permitiendo registrar operaciones financieras, consultar información y automatizar recordatorios mediante notificaciones móviles y WhatsApp.

---

## 1. Descripción del Proyecto

El **Sistema de Gestión de Préstamos** permite administrar el ciclo completo de los préstamos otorgados a los clientes.

El sistema contempla:

- Registro y administración de clientes.
- Registro de avales.
- Registro y gestión de múltiples préstamos por cliente.
- Cálculo de intereses semanales.
- Registro de pagos.
- Control de capital pendiente.
- Seguimiento de pagos vencidos.
- Gestión de morosidad.
- Notificaciones mediante Firebase Cloud Messaging.
- Mensajería automatizada mediante WhatsApp Business Platform de Meta.
- Gestión de usuarios, roles y permisos.
- Configuración de parámetros del sistema.
- Registro y trazabilidad de operaciones importantes.

El backend será responsable de validar las operaciones y aplicar las reglas financieras del sistema.

---

## 2. Objetivo

Desarrollar una solución móvil que permita gestionar préstamos de manera organizada, segura y trazable, reduciendo el trabajo manual asociado al registro de clientes, cálculo de intereses, control de pagos, seguimiento de morosidad y comunicación con los clientes.

---

## 3. Alcance

El sistema contempla los siguientes módulos principales:

### 3.1 Gestión de usuarios

Permite:

- Iniciar sesión.
- Cerrar sesión.
- Recuperar contraseña.
- Cambiar contraseña.
- Consultar sesiones activas.
- Cerrar otras sesiones.
- Administrar roles y permisos según autorización.

### 3.2 Gestión de clientes

Permite:

- Registrar clientes.
- Editar información.
- Consultar información.
- Consultar préstamos asociados.
- Consultar pagos.
- Consultar estado de morosidad.
- Registrar observaciones.
- Asociar un aval.
- Utilizar un cliente existente como aval.

### 3.3 Gestión de préstamos

Permite:

- Registrar préstamos.
- Aprobar préstamos.
- Consultar préstamos.
- Consultar detalle del préstamo.
- Gestionar múltiples préstamos por cliente.
- Consultar capital pendiente.
- Consultar intereses.
- Consultar estado del préstamo.

### 3.4 Gestión de pagos

Permite:

- Registrar pagos.
- Registrar pagos parciales.
- Aplicar primero el pago correspondiente al interés.
- Aplicar el excedente al capital.
- Generar información del pago.
- Consultar historial de pagos.
- Mantener trazabilidad de las operaciones.

### 3.5 Gestión de morosidad

Permite:

- Detectar pagos de intereses vencidos.
- Registrar el estado de morosidad.
- Identificar clientes morosos.
- Identificar clientes con más de dos intereses vencidos.
- Permitir la reactivación cuando corresponda.

### 3.6 Notificaciones

Permite enviar notificaciones relacionadas con:

- Próximos pagos.
- Pagos vencidos.
- Morosidad.
- Eventos importantes del préstamo.

Las notificaciones programadas inicialmente consideran los horarios:

- 08:00
- 16:00

### 3.7 WhatsApp

Permite:

- Enviar recordatorios.
- Enviar mensajes de morosidad.
- Enviar confirmaciones.
- Enviar mensajes administrativos.
- Utilizar plantillas aprobadas por Meta.
- Registrar el historial de mensajes.
- Controlar el estado de envío.

---

## 4. Reglas Financieras Principales

Las reglas financieras deben ser aplicadas principalmente por el backend.

### 4.1 Interés semanal

El interés establecido inicialmente es:

**5 % semanal sobre el capital inicial del préstamo.**

Ejemplo:

```text
Capital inicial: S/ 100.00
Interés semanal: 5 %

Interés semanal = 100 × 0.05
Interés semanal = S/ 5.00
```

El interés se calcula sobre el capital inicial y no sobre el saldo reducido.

### 4.2 Interés no compuesto

Los intereses no se capitalizan.

El interés pendiente no se agrega al capital para calcular nuevos intereses.

### 4.3 Frecuencia

La frecuencia establecida es semanal.

Ejemplo:

```text
Inicio del préstamo: lunes

Primer interés:
lunes siguiente

Segundo interés:
lunes posterior

Y así sucesivamente mientras corresponda.
```

### 4.4 Aplicación de pagos

Cuando se registra un pago:

1. Se aplica primero al interés pendiente.
2. Si existe un excedente, se aplica al capital.
3. Se actualizan los saldos.
4. Se registra la operación.
5. Se actualiza el estado correspondiente.

### 4.5 Pagos parciales

El sistema permite registrar pagos parciales.

Por ejemplo:

```text
Interés pendiente: S/ 5.00
Pago realizado:    S/ 3.00

Interés pendiente: S/ 2.00
```

### 4.6 Múltiples préstamos

Un cliente puede tener más de un préstamo.

Cada préstamo debe mantener sus propios:

- Datos.
- Capital inicial.
- Intereses.
- Pagos.
- Estado.
- Historial.
- Fechas.

---

## 5. Morosidad

El sistema realiza seguimiento de los intereses vencidos.

Como regla principal:

> Un cliente puede ser considerado en una situación crítica de morosidad cuando posee más de dos pagos de intereses vencidos.

El sistema debe permitir:

- Identificar pagos vencidos.
- Contabilizar intereses vencidos.
- Mostrar el estado de morosidad.
- Generar notificaciones.
- Generar mensajes de WhatsApp.
- Permitir la reactivación cuando corresponda.

---

## 6. Roles del Sistema

Los roles principales considerados son:

### Administrador

Tiene permisos para:

- Gestionar clientes.
- Gestionar préstamos.
- Gestionar pagos.
- Gestionar morosidad.
- Gestionar notificaciones.
- Gestionar WhatsApp.
- Administrar configuraciones autorizadas.
- Administrar usuarios y permisos.
- Consultar información general.
- Supervisar operaciones.

### Cobrador

Tiene permisos relacionados principalmente con:

- Consulta de clientes.
- Registro de operaciones autorizadas.
- Consulta de préstamos.
- Registro de pagos.
- Consulta de morosidad.
- Consulta de información necesaria para la cobranza.

Los permisos definitivos son controlados por el backend.

---

## 7. Arquitectura

La solución utiliza una arquitectura basada en una aplicación móvil, una API REST y una base de datos centralizada.

```text
┌──────────────────────────────┐
│       Aplicación Móvil       │
│          .NET MAUI           │
│             C#               │
└──────────────┬───────────────┘
               │
               │ HTTPS / REST / JSON
               ▼
┌──────────────────────────────┐
│       ASP.NET Core Web API   │
│             C#               │
│                              │
│  Reglas de negocio           │
│  Autenticación               │
│  Autorización                │
│  Validaciones                │
│  Operaciones financieras     │
│  Integraciones               │
└──────────────┬───────────────┘
               │
               │ Entity Framework Core
               ▼
┌──────────────────────────────┐
│          PostgreSQL          │
│          Base de Datos       │
└──────────────────────────────┘
```

Integraciones externas:

```text
ASP.NET Core Web API
        │
        ├── Firebase Cloud Messaging
        │
        └── WhatsApp Business Platform de Meta
```

---

## 8. Stack Tecnológico

| Componente | Tecnología |
|---|---|
| IDE | Visual Studio Code (Stable 64 bits, único IDE oficial) |
| Lenguaje | C# |
| Aplicación móvil | .NET MAUI |
| Backend | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Base de datos | PostgreSQL |
| Notificaciones | Firebase Cloud Messaging (FCM) |
| WhatsApp | WhatsApp Business Platform de Meta |
| Comunicación | REST / JSON |
| Seguridad de comunicación | HTTPS |
| Control de versiones | Git |
| Repositorio | GitHub |
| Asistente de desarrollo | GitHub Copilot (inline en VS Code) + Muse Spark vía OpenCode (agente de ingeniería) |

---

## 9. Estructura de Documentación

La documentación del proyecto se encuentra organizada de la siguiente manera:

```text
Sistema-Gestion-Prestamos/
│
├── 01-Requisitos/
│
├── 02-Reglas-Negocio/
│
├── 03-Arquitectura/
│
├── 04-Base-Datos/
│
├── 05-API/
│
├── 06-Diseno-UX-UI/
│
├── 07-Notificaciones/
│
├── 08-WhatsApp/
│
├── 09-Seguridad/
│
├── 10-Pruebas/
│
└── 11-Proyecto/
```

Cada directorio contiene documentación específica del sistema.

---

## 10. Seguridad

La seguridad es responsabilidad tanto de la aplicación móvil como del backend.

Entre las principales medidas contempladas se encuentran:

- Autenticación mediante correo electrónico y contraseña.
- Almacenamiento seguro de contraseñas mediante hash.
- Comunicación mediante HTTPS.
- Control de sesiones.
- Recuperación de contraseña.
- Cambio de contraseña.
- Control de roles.
- Control de permisos.
- Autorización en el backend.
- Protección de operaciones financieras.
- Validación de datos.
- Protección contra acceso no autorizado.
- Registro de operaciones importantes.
- Protección de credenciales de servicios externos.

La aplicación móvil no debe considerarse una autoridad para las operaciones financieras.

El backend debe validar y ejecutar las reglas críticas.

---

## 11. Requisitos Previos

Para desarrollar el proyecto se requiere, como mínimo y de forma obligatoria:

- Visual Studio Code (versión Stable de 64 bits, 1.90 o superior) como único IDE oficial. No se usará Visual Studio 2026 ni otra edición de Visual Studio.
- Extensiones obligatorias de Visual Studio Code: C# (`ms-dotnettools.csharp`), C# Dev Kit (`ms-dotnettools.csdevkit`), .NET MAUI (`ms-dotnettools.dotnet-maui`).
- .NET SDK LTS vigente (versión exacta fijada en `global.json`) con workload MAUI instalado (`dotnet workload install maui`).
- Android SDK + JDK compatible con MAUI (plataforma inicial: Android).
- C#.
- .NET MAUI.
- ASP.NET Core.
- PostgreSQL.
- Git.
- Cuenta de GitHub.
- Cuenta/proyecto de Firebase para FCM.
- Configuración de WhatsApp Business Platform de Meta para la integración correspondiente.

Los SDK y versiones definitivas deberán mantenerse documentados en la configuración del proyecto.

---

## 12. Configuración del Proyecto

Las credenciales y configuraciones sensibles no deben almacenarse directamente en el código fuente.

Ejemplos de configuraciones:

```text
ConnectionStrings
Database
Firebase
WhatsApp
Authentication
Notifications
Application
```

Ejemplo conceptual:

```text
Database:
    Host
    Port
    Database
    Username
    Password

Firebase:
    ProjectId
    Credentials

WhatsApp:
    BusinessAccountId
    PhoneNumberId
    AccessToken
    WebhookVerifyToken
```

Los valores reales deben mantenerse fuera del repositorio cuando contengan información sensible.

### 12.1 Secretos locales (user-secrets)

En desarrollo, `appsettings.json` trae valores vacíos a propósito. La API no arranca sin `Jwt:Key` (`InvalidOperationException`). Configura los secretos una vez con:

```powershell
dotnet user-secrets init # solo la primera vez (ya tiene UserSecretsId)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=sistema_gestion_prestamos;Username=postgres;Password=TU_PASSWORD"
dotnet user-secrets set "Jwt:Key" "TU_CLAVE_DE_AL_MENOS_32_CARACTERES"
dotnet user-secrets set "Firebase:ServiceAccountPath" "C:\ruta\firebase-adminsdk.json"
dotnet user-secrets set "Firebase:ProjectId" "tu-proyecto"
dotnet user-secrets set "WhatsApp:PhoneNumberId" "TU_PHONE_NUMBER_ID"
dotnet user-secrets set "WhatsApp:Token" "TU_TOKEN_META"
```

Referencia completa en `src/SistemaPrestamos.API/appsettings.example.json`.

| Clave | Default si falta | Efecto |
|---|---|---|
| `Jwt:ExpirationMinutes` | 120 | Vigencia del access token. |
| `Jwt:RefreshExpirationDays` | 7 | Vigencia del refresh token (no está en todos los `appsettings`; si falta, son 7 días). |

### 12.2 HTTPS por entorno

`UseHttpsRedirection` solo se activa fuera de `Development` (el emulador Android usa `http://10.0.2.2:5077`). En producción, servir siempre detrás de HTTPS: los JWT y contraseñas viajan en cada request.

---

## 13. Base de Datos

La base de datos utilizada por el sistema es PostgreSQL.

El acceso a los datos se realiza mediante Entity Framework Core.

Las operaciones de base de datos deben considerar:

- Integridad referencial.
- Claves primarias.
- Claves foráneas.
- Restricciones.
- Índices.
- Transacciones.
- Validación de datos.
- Auditoría cuando corresponda.

Las modificaciones estructurales de la base de datos deben gestionarse mediante migraciones de Entity Framework Core.

Documentación relacionada:

- [Diagrama ER](../04-Base-Datos/01-Diagrama-ER.puml)
- [Diccionario de Datos](../04-Base-Datos/03-Diccionario-Datos.md)
- [Restricciones de Base de Datos](../04-Base-Datos/04-Restricciones-BD.md)

---

## 14. API

El backend expone una API REST para permitir la comunicación entre la aplicación móvil y el servidor.

La comunicación utiliza:

```text
HTTPS
REST
JSON
```

La API será responsable de:

- Autenticar usuarios.
- Autorizar operaciones.
- Gestionar clientes.
- Gestionar préstamos.
- Registrar pagos.
- Calcular operaciones financieras.
- Gestionar morosidad.
- Gestionar notificaciones.
- Gestionar WhatsApp.
- Validar información.
- Aplicar reglas de negocio.

Documentación relacionada:

- [Endpoints](../05-API/01-Endpoints.md)
- [Autenticación](../05-API/02-Autenticacion.md)
- [Modelos Request/Response](../05-API/03-Modelos-Request-Response.md)

---

## 15. Notificaciones

Las notificaciones móviles utilizan Firebase Cloud Messaging.

El sistema contempla notificaciones para:

- Próximos pagos.
- Pagos vencidos.
- Morosidad.
- Eventos relacionados con los préstamos.

Los horarios iniciales definidos son:

```text
08:00
16:00
```

La programación debe realizarse desde el backend o mediante un mecanismo de ejecución programada que permita enviar las notificaciones aunque la aplicación no esté abierta.

Documentación relacionada:

- [Firebase FCM](../07-Notificaciones/01-Firebase-FCM.md)
- [Programación de Notificaciones](../07-Notificaciones/02-Programacion-Notificaciones.md)

---

## 16. Integración con WhatsApp

La comunicación automatizada utiliza WhatsApp Business Platform de Meta.

La integración contempla:

- Plantillas.
- Variables dinámicas.
- Recordatorios.
- Mensajes de morosidad.
- Confirmaciones.
- Mensajes administrativos.
- Registro de mensajes.
- Estados de envío.
- Webhooks.
- Reintentos controlados.
- Prevención de mensajes duplicados.

Documentación relacionada:

- [Integración con Meta](../08-WhatsApp/01-Integracion-Meta.md)
- [Plantillas](../08-WhatsApp/02-Plantillas.md)
- [Flujo de Mensajería](../08-WhatsApp/03-Flujo-Mensajeria.md)

---

## 17. Pruebas

El proyecto contempla pruebas funcionales, técnicas y financieras.

Las pruebas deben validar principalmente:

- Autenticación.
- Autorización.
- Registro de clientes.
- Registro de préstamos.
- Cálculo del 5 % semanal.
- Interés sobre capital inicial.
- Interés no compuesto.
- Pagos parciales.
- Aplicación del interés antes del capital.
- Múltiples préstamos.
- Morosidad.
- Notificaciones.
- WhatsApp.
- Seguridad.
- Integridad de datos.

Documentación relacionada:

- [Plan de Pruebas](../10-Pruebas/01-Plan-Pruebas.md)
- [Casos de Prueba](../10-Pruebas/02-Casos-Prueba.md)
- [Pruebas de Reglas Financieras](../10-Pruebas/03-Pruebas-Reglas-Financieras.md)

---

## 18. Control de Versiones

El proyecto utiliza Git para el control de versiones y GitHub como repositorio remoto.

Flujo recomendado:

```text
1. Crear o seleccionar una rama de trabajo.
2. Realizar los cambios.
3. Revisar los archivos modificados.
4. Ejecutar pruebas.
5. Realizar commit.
6. Enviar los cambios al repositorio.
7. Revisar los cambios.
8. Integrar la rama cuando corresponda.
```

Los commits deben describir claramente el cambio realizado.

Ejemplos:

```text
feat: agregar registro de clientes
fix: corregir cálculo de interés semanal
docs: actualizar reglas de pagos
test: agregar pruebas de morosidad
refactor: reorganizar servicio de préstamos
```

---

## 19. Principios de Desarrollo

El desarrollo debe seguir los siguientes principios:

### 19.1 Separación de responsabilidades

Cada componente debe tener responsabilidades claramente definidas.

### 19.2 Validación en backend

Las operaciones críticas deben ser validadas en el servidor.

### 19.3 Seguridad por diseño

La seguridad debe considerarse desde el diseño y no únicamente al finalizar el desarrollo.

### 19.4 Integridad financiera

Los cálculos y operaciones relacionadas con dinero deben utilizar tipos apropiados para valores monetarios, evitando errores derivados de operaciones de punto flotante.

### 19.5 Trazabilidad

Las operaciones financieras y administrativas importantes deben poder ser rastreadas.

### 19.6 Mantenibilidad

El código debe mantenerse organizado y facilitar futuras modificaciones.

### 19.7 Control de versiones

Todo cambio relevante debe gestionarse mediante Git.

### 19.8 Protección de secretos

Nunca se deben publicar:

- Contraseñas.
- Tokens.
- Claves privadas.
- Access tokens.
- Credenciales de base de datos.
- Credenciales de Firebase.
- Credenciales de Meta.

---

## 20. Estado del Proyecto

Estado general de documentación:

```text
Requisitos              ██████████  Completo
Reglas de Negocio       ██████████  Completo
Arquitectura            ██████████  Completo
Base de Datos           ██████████  Documentado
API                     ██████████  Documentado
UX/UI                   ██████████  Documentado
Notificaciones          ██████████  Documentado
WhatsApp                ██████████  Documentado
Seguridad               ██████████  Documentado
Pruebas                 ██████████  Documentado
Proyecto                ███████░░░  En documentación
```

> El estado indicado corresponde a la documentación del proyecto. No implica necesariamente que todas las funcionalidades hayan sido implementadas en código.

---

## 21. Configuración General del Sistema

La configuración inicial contemplada es:

| Parámetro | Valor |
|---|---|
| Moneda | Soles (S/) |
| Zona horaria | America/Lima |
| Formato de fecha | DD/MM/YYYY |
| Interés semanal inicial | 5 % |
| Frecuencia | Semanal |
| Interés compuesto | No |
| Múltiples préstamos por cliente | Sí |
| Notificación 1 | 08:00 |
| Notificación 2 | 16:00 |
| Morosidad crítica | Más de 2 intereses vencidos |

Los parámetros financieros deben estar protegidos y cualquier modificación autorizada debe mantener trazabilidad.

---

## 22. Documentación Relacionada

### Requisitos

- [Descripción General](../01-Requisitos/01-Descripcion-General.md)
- [Alcance](../01-Requisitos/02-Alcance.md)
- [Requisitos Funcionales](../01-Requisitos/03-Requisitos-Funcionales.md)
- [Requisitos No Funcionales](../01-Requisitos/04-Requisitos-No-Funcionales.md)

### Reglas de Negocio

- [Reglas de Clientes](../02-Reglas-Negocio/01-Reglas-Clientes.md)
- [Reglas de Préstamos](../02-Reglas-Negocio/02-Reglas-Prestamos.md)
- [Reglas de Pagos](../02-Reglas-Negocio/03-Reglas-Pagos.md)
- [Reglas de Morosidad](../02-Reglas-Negocio/04-Reglas-Morosidad.md)
- [Reglas de Notificaciones](../02-Reglas-Negocio/05-Reglas-Notificaciones.md)
- [Reglas de WhatsApp](../02-Reglas-Negocio/06-Reglas-WhatsApp.md)

### Arquitectura

- [Arquitectura General](../03-Arquitectura/01-Arquitectura-General.md)
- [Stack Tecnológico](../03-Arquitectura/02-Stack-Tecnologico.md)
- [Arquitectura Backend](../03-Arquitectura/03-Arquitectura-Backend.md)
- [Arquitectura Mobile](../03-Arquitectura/04-Arquitectura-Mobile.md)
- [Integraciones](../03-Arquitectura/05-Integraciones.md)

### UX/UI

- [Flujo de Navegación](../06-Diseno-UX-UI/01-Flujo-Navegacion.md)

### Seguridad

- [Autenticación](../09-Seguridad/01-Autenticacion.md)
- [Autorización](../09-Seguridad/02-Autorizacion.md)
- [Protección de Datos](../09-Seguridad/03-Proteccion-Datos.md)

---

## 23. Contribución

Para realizar cambios en el proyecto:

1. Crear una rama de trabajo.
2. Implementar el cambio.
3. Mantener la arquitectura definida.
4. Actualizar la documentación cuando sea necesario.
5. Ejecutar las pruebas correspondientes.
6. Revisar posibles impactos en las reglas financieras.
7. Realizar el commit.
8. Enviar los cambios al repositorio.
9. Revisar antes de integrar los cambios.

Los cambios que afecten cálculos financieros, pagos, morosidad, intereses o configuraciones críticas deben recibir una revisión adicional.

---

## 24. Manejo de Secretos

Está prohibido subir al repositorio información sensible.

No deben incluirse en Git:

```text
.env
appsettings.Production.json
Credenciales reales
Access Tokens
Private Keys
Passwords
Connection Strings con credenciales
Firebase Credentials
Meta Access Tokens
```

Cuando sea necesario compartir configuraciones, utilizar valores de ejemplo o variables de entorno.

Ejemplo:

```text
DB_PASSWORD=<REPLACE_WITH_SECRET>
META_ACCESS_TOKEN=<REPLACE_WITH_SECRET>
FIREBASE_CREDENTIALS=<REPLACE_WITH_SECRET>
```

---

## 25. Licencia

La licencia del proyecto aún no ha sido definida.

```text
License: TBD
```

Antes de realizar una distribución pública del software se deberá definir la licencia correspondiente.

---

## 26. Soporte y Contacto

Información de contacto del proyecto:

```text
Responsable: TBD
Correo: TBD
Repositorio: TBD
```

Esta información debe actualizarse cuando se establezcan los datos definitivos del proyecto.

---

## 27. Roadmap

El desarrollo futuro se encuentra documentado en:

[ROADMAP.md](./ROADMAP.md)

El roadmap permite organizar las funcionalidades pendientes, mejoras y futuras versiones del sistema.

---

## 28. Historial de Cambios

Los cambios importantes del proyecto se registran en:

[CHANGELOG.md](./CHANGELOG.md)

---

## 29. Versión de la Documentación

```text
Proyecto:
Sistema de Gestión de Préstamos

Versión:
0.1.0

Estado:
En desarrollo

Documentación:
11-Proyecto

Fecha:
TBD
```

---

## 30. Nota Final

Este README constituye el punto de entrada principal para comprender el proyecto.

Para conocer el funcionamiento detallado del sistema se recomienda revisar la documentación específica de cada módulo, especialmente:

- Requisitos.
- Reglas de negocio.
- Arquitectura.
- Base de datos.
- API.
- UX/UI.
- Notificaciones.
- WhatsApp.
- Seguridad.
- Pruebas.

Las reglas financieras documentadas deben considerarse como reglas críticas del sistema y cualquier modificación debe ser analizada antes de implementarse.