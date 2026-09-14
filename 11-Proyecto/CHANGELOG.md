# Changelog

Todos los cambios importantes realizados en el proyecto **Sistema de Gestión de Préstamos** se documentarán en este archivo.

El formato utilizado está basado en el concepto de **Versionado Semántico (Semantic Versioning)**:

```text
MAJOR.MINOR.PATCH
```

Donde:

- **MAJOR**: cambios importantes que pueden modificar la estructura o funcionamiento principal del sistema.
- **MINOR**: incorporación de nuevas funcionalidades compatibles con la versión anterior.
- **PATCH**: correcciones de errores y cambios menores.

---

# 1. [0.1.0] - Versión Inicial

**Estado:** En desarrollo

## Documentación

Se estableció la estructura general de documentación del proyecto.

Se documentaron las siguientes áreas:

- Requisitos.
- Reglas de negocio.
- Arquitectura.
- Base de datos.
- API.
- Diseño UX/UI.
- Notificaciones.
- Integración con WhatsApp.
- Seguridad.
- Pruebas.
- Documentación general del proyecto.

## Requisitos

Se documentaron:

- Descripción general del sistema.
- Alcance del proyecto.
- Requisitos funcionales.
- Requisitos no funcionales.

## Reglas de Negocio

Se establecieron las reglas principales relacionadas con:

- Clientes.
- Avales.
- Préstamos.
- Pagos.
- Intereses.
- Morosidad.
- Notificaciones.
- WhatsApp.

## Arquitectura

Se definió la arquitectura tecnológica basada en:

- C#.
- .NET MAUI.
- ASP.NET Core Web API.
- Entity Framework Core.
- PostgreSQL.
- Firebase Cloud Messaging.
- WhatsApp Business Platform de Meta.
- REST / JSON.
- HTTPS.
- Git.
- GitHub.

## Diseño UX/UI

Se documentaron los wireframes y pantallas principales del sistema.

Entre ellas:

- Login.
- Dashboard.
- Clientes.
- Registro de cliente.
- Detalle de cliente.
- Préstamos.
- Registro de préstamo.
- Detalle de préstamo.
- Pagos.
- Morosidad.
- Notificaciones.
- WhatsApp.
- Perfil.
- Configuración.

## Notificaciones

Se documentó la utilización de:

**Firebase Cloud Messaging (FCM)**

Se establecieron inicialmente los siguientes horarios para las notificaciones programadas:

```text
08:00
16:00
```

## WhatsApp

Se documentó la integración prevista con:

**WhatsApp Business Platform de Meta**

Se contemplan:

- Plantillas.
- Variables dinámicas.
- Recordatorios.
- Mensajes de morosidad.
- Confirmaciones.
- Mensajes administrativos.
- Estados de envío.
- Webhooks.
- Historial de mensajes.

## Seguridad

Se documentaron mecanismos relacionados con:

- Autenticación.
- Autorización.
- Roles.
- Permisos.
- Protección de credenciales.
- Comunicación HTTPS.
- Protección de operaciones financieras.
- Control de sesiones.
- Protección de datos.

## Pruebas

Se establecieron pruebas para validar:

- Autenticación.
- Autorización.
- Clientes.
- Préstamos.
- Pagos.
- Intereses.
- Morosidad.
- Notificaciones.
- WhatsApp.
- Seguridad.

También se documentaron pruebas específicas para las reglas financieras.

---

# 2. Reglas Financieras Documentadas

Se establecieron las siguientes reglas financieras principales:

## Interés semanal

El interés inicial establecido es:

```text
5 % semanal
```

El cálculo se realiza sobre el **capital inicial del préstamo**.

Ejemplo:

```text
Capital inicial = S/ 100.00
Interés = 5 %

Interés semanal = S/ 5.00
```

## Interés no compuesto

Los intereses no se capitalizan.

El interés pendiente no se suma al capital para generar nuevos intereses.

## Pagos

Los pagos se aplican en el siguiente orden:

```text
1. Interés pendiente
2. Capital pendiente
```

Si el pago no cubre completamente el interés:

```text
Pago → Interés
```

Si existe un excedente después de cubrir el interés:

```text
Pago → Interés
     → Capital
```

## Pagos parciales

Se permite registrar pagos parciales.

## Múltiples préstamos

Un cliente puede tener múltiples préstamos.

Cada préstamo mantiene su propia información financiera e historial de pagos.

## Morosidad

Se estableció como condición de morosidad crítica:

```text
Más de 2 intereses vencidos
```

---

# 3. Convenciones del Changelog

A partir de las siguientes versiones se utilizarán las siguientes categorías:

## Added

Para funcionalidades nuevas.

Ejemplo:

```text
### Added
- Registro de clientes.
- Registro de préstamos.
```

## Changed

Para funcionalidades existentes que fueron modificadas.

Ejemplo:

```text
### Changed
- Se modificó la pantalla de registro de préstamos.
```

## Fixed

Para errores corregidos.

Ejemplo:

```text
### Fixed
- Se corrigió el cálculo del interés semanal.
```

## Security

Para cambios relacionados con seguridad.

Ejemplo:

```text
### Security
- Se mejoró la validación de permisos.
```

## Removed

Para funcionalidades eliminadas.

Ejemplo:

```text
### Removed
- Se eliminó una funcionalidad obsoleta.
```

## Documentation

Para cambios exclusivamente relacionados con documentación.

Ejemplo:

```text
### Documentation
- Se actualizó la documentación de la API.
```

---

# 4. Historial de Versiones

| Versión | Estado | Descripción |
|---|---|---|
| 0.1.0 | En desarrollo | Estructura inicial y documentación del proyecto |

---

# 5. Próximas Versiones

Las siguientes versiones se definirán conforme avance la implementación.

Ejemplo de planificación:

```text
0.2.0
Gestión de usuarios y autenticación.

0.3.0
Gestión de clientes.

0.4.0
Gestión de préstamos.

0.5.0
Gestión de pagos.

0.6.0
Gestión de morosidad.

0.7.0
Notificaciones mediante FCM.

0.8.0
Integración con WhatsApp.

0.9.0
Pruebas e integración general.

1.0.0
Primera versión estable.
```

Estas versiones son una **referencia de planificación** y podrán modificarse según el avance real del proyecto.

---

# 6. Criterios para Registrar Cambios

Se debe agregar una entrada al `CHANGELOG.md` cuando se realice un cambio significativo, como:

- Incorporación de una funcionalidad.
- Modificación de una regla de negocio.
- Corrección de un error.
- Modificación de una operación financiera.
- Cambio de arquitectura.
- Cambio importante de base de datos.
- Cambio en una integración externa.
- Modificación de seguridad.
- Eliminación de una funcionalidad.

No es necesario registrar cambios mínimos que no tengan impacto relevante en el funcionamiento o documentación del proyecto.

---

# 7. Cambios Financieros

Los cambios relacionados con operaciones financieras deben documentarse con especial cuidado.

Se consideran cambios críticos:

- Modificación del porcentaje de interés.
- Modificación de la frecuencia del interés.
- Cambio en la base de cálculo.
- Cambio en el orden de aplicación de pagos.
- Cambio en la gestión de pagos parciales.
- Cambio en las reglas de morosidad.
- Cambio en el tratamiento del capital.
- Cambio en el cálculo de saldos.

Ejemplo:

```text
## [0.5.1] - 2026-XX-XX

### Changed
- Se modificó la regla de aplicación de pagos.

### Documentation
- Se actualizaron las pruebas de reglas financieras.
```

Todo cambio financiero debe estar acompañado por las pruebas correspondientes.

---

# 8. Relación con Git

El `CHANGELOG.md` complementa el historial técnico proporcionado por Git.

Git permite conocer:

```text
Quién realizó un cambio.
Cuándo realizó el cambio.
Qué archivos modificó.
Qué código modificó.
```

El `CHANGELOG.md` permite conocer:

```text
Qué cambio importante recibió el proyecto.
Qué funcionalidad se agregó.
Qué problema se corrigió.
En qué versión se incorporó el cambio.
```

Por lo tanto:

```text
Git
↓
Historial técnico detallado

CHANGELOG.md
↓
Resumen organizado de cambios importantes
```

---

# 9. Reglas para Versionado

Se utilizará el siguiente criterio:

## MAJOR

Se incrementará cuando exista un cambio incompatible o una modificación importante de la arquitectura o funcionamiento principal.

Ejemplo:

```text
1.5.0 → 2.0.0
```

## MINOR

Se incrementará cuando se agregue una nueva funcionalidad importante sin romper la funcionalidad existente.

Ejemplo:

```text
1.2.0 → 1.3.0
```

## PATCH

Se incrementará cuando se corrijan errores o se realicen modificaciones menores.

Ejemplo:

```text
1.3.0 → 1.3.1
```

---

# 10. Plantilla para Nuevas Versiones

Para registrar una nueva versión se utilizará la siguiente estructura:

```markdown
# [X.Y.Z] - YYYY-MM-DD

## Added

- Nueva funcionalidad.

## Changed

- Funcionalidad modificada.

## Fixed

- Error corregido.

## Security

- Cambio relacionado con seguridad.

## Documentation

- Documentación actualizada.
```

No es obligatorio utilizar todas las categorías en cada versión.

Solo se deben incluir las categorías que tengan cambios.

---

# 11. Estado Actual

Actualmente el proyecto se encuentra en una etapa de:

```text
Documentación
        ↓
Diseño
        ↓
Planificación de implementación
        ↓
Desarrollo
        ↓
Pruebas
        ↓
Versión estable
```

La versión `0.1.0` representa la etapa inicial del proyecto y no debe interpretarse como una versión final o productiva.

---

# 12. Próxima Actualización

El siguiente cambio importante deberá registrarse cuando se inicie o complete una funcionalidad concreta del sistema.

Ejemplo:

```text
## [0.2.0] - YYYY-MM-DD

### Added
- Sistema de autenticación.
- Inicio de sesión.
- Recuperación de contraseña.
- Gestión de sesiones.

### Security
- Control de autenticación.
- Protección de endpoints.
```

---

# 13. Nota

Este archivo debe mantenerse actualizado durante todo el ciclo de vida del proyecto.

Cada versión debe representar el estado real del sistema.

No se deben registrar como implementadas funcionalidades que únicamente estén:

- Planificadas.
- Diseñadas.
- Documentadas.
- En desarrollo.

La información del `CHANGELOG.md` debe reflejar únicamente cambios que realmente hayan sido realizados.