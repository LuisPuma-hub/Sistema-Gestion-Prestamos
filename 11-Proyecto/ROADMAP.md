# Roadmap - Sistema de Gestión de Préstamos

Este documento define la planificación evolutiva del **Sistema de Gestión de Préstamos**, estableciendo las funcionalidades y mejoras previstas para las diferentes etapas del proyecto.

El roadmap puede modificarse conforme avance el desarrollo, siempre que los cambios sean documentados y mantengan coherencia con los requisitos y reglas de negocio establecidos.

---

# 1. Objetivo del Roadmap

El objetivo de este documento es establecer una guía para:

- Organizar el desarrollo del sistema.
- Priorizar funcionalidades.
- Dividir el proyecto en etapas.
- Identificar dependencias.
- Planificar versiones.
- Controlar el avance del proyecto.
- Identificar funcionalidades futuras.

El roadmap representa una **planificación**, por lo que las funcionalidades indicadas como pendientes no deben considerarse implementadas.

---

# 2. Estado General

El proyecto se encuentra en una etapa inicial de documentación y planificación.

```text
Documentación
      ↓
Diseño
      ↓
Configuración técnica
      ↓
Desarrollo
      ↓
Integración
      ↓
Pruebas
      ↓
Versión estable
```

Estado actual:

```text
Documentación      ██████████  Completa
Diseño UX/UI       ██████████  Documentado
Arquitectura       ██████████  Documentada
Seguridad          ██████████  Documentada
Pruebas            ██████████  Planificadas
Desarrollo         ░░░░░░░░░░  Pendiente / En inicio
Integración        ░░░░░░░░░░  Pendiente
Producción         ░░░░░░░░░░  Pendiente
```

---

# 3. Principios de Priorización

Las funcionalidades se priorizarán considerando:

1. Dependencias técnicas.
2. Importancia para el negocio.
3. Impacto financiero.
4. Seguridad.
5. Integridad de datos.
6. Experiencia del usuario.
7. Facilidad de integración.
8. Necesidad para las pruebas.

Las funcionalidades financieras tendrán prioridad sobre funcionalidades complementarias debido a su impacto directo en la información económica del sistema.

---

# 4. Fase 1 - Preparación del Proyecto

**Versión objetivo:** `0.1.0`

**Estado:** Documentación y planificación.

## Objetivos

Establecer las bases del proyecto antes de comenzar la implementación.

## Actividades

- Definir requisitos.
- Definir alcance.
- Definir reglas de negocio.
- Definir arquitectura.
- Definir stack tecnológico.
- Definir estructura de base de datos.
- Definir API.
- Definir navegación.
- Definir wireframes.
- Definir pantallas.
- Definir seguridad.
- Definir estrategia de pruebas.
- Crear documentación general.

## Tecnologías definidas

- C#.
- .NET MAUI.
- ASP.NET Core Web API.
- Entity Framework Core.
- PostgreSQL.
- Firebase Cloud Messaging.
- WhatsApp Business Platform de Meta.
- REST.
- JSON.
- HTTPS.
- Git.
- GitHub.
- GitHub Copilot (inline en VS Code).
- Muse Spark vía OpenCode (agente de ingeniería para revisión, edición multi-archivo y verificación).
- Visual Studio Code (Stable 64 bits, único IDE oficial, con extensiones C#, C# Dev Kit y .NET MAUI + .NET SDK LTS con workload MAUI + Android SDK). No se usará Visual Studio 2026.

## Resultado esperado

Contar con una base documental y técnica suficiente para iniciar el desarrollo.

---

# 5. Fase 2 - Configuración Técnica

**Versión objetivo:** `0.2.0`

**Estado:** Pendiente.

## Objetivos

Preparar el entorno de desarrollo y la estructura inicial del software.

## Actividades

### Backend

- Crear proyecto ASP.NET Core Web API.
- Configurar C#.
- Configurar Entity Framework Core.
- Configurar PostgreSQL.
- Configurar estructura de capas.
- Configurar manejo de errores.
- Configurar logging.
- Configurar configuración por ambientes.

### Mobile

- Crear proyecto .NET MAUI.
- Configurar navegación.
- Configurar estructura de la aplicación.
- Configurar servicios.
- Configurar consumo de API.
- Preparar manejo de sesión.

### Base de datos

- Crear base de datos PostgreSQL.
- Configurar conexión.
- Crear entidades iniciales.
- Configurar relaciones.
- Crear migraciones.
- Ejecutar migraciones iniciales.

### Control de versiones

- Configurar Git.
- Crear repositorio GitHub.
- Definir ramas.
- Definir convenciones de commits.
- Configurar `.gitignore`.

## Resultado esperado

Contar con una estructura técnica funcional sobre la cual comenzar el desarrollo de los módulos.

---

# 6. Fase 3 - Autenticación y Usuarios

**Versión objetivo:** `0.3.0`

**Estado:** Pendiente.

## Objetivos

Implementar el acceso seguro al sistema y la gestión básica de usuarios.

## Funcionalidades

- Inicio de sesión.
- Validación de credenciales.
- Gestión de sesiones.
- Cierre de sesión.
- Recuperación de contraseña.
- Cambio de contraseña.
- Consulta de sesiones activas.
- Cierre de otras sesiones.
- Control de cuentas.
- Roles.
- Permisos.

## Roles iniciales

```text
ADMIN
COBRADOR
```

## Seguridad

Se implementará:

- Hash seguro de contraseñas.
- HTTPS.
- Autenticación de API.
- Autorización por roles y permisos.
- Protección de endpoints.
- Validación en backend.
- Control de sesiones.
- Protección contra accesos no autorizados.

## Resultado esperado

Los usuarios autorizados podrán ingresar al sistema y acceder únicamente a las funcionalidades correspondientes a sus permisos.

---

# 7. Fase 4 - Gestión de Clientes

**Versión objetivo:** `0.4.0`

**Estado:** Pendiente.

## Objetivos

Implementar el módulo de administración de clientes.

## Funcionalidades

- Listar clientes.
- Buscar clientes.
- Registrar clientes.
- Editar clientes.
- Consultar detalle.
- Registrar documento.
- Registrar nombres.
- Registrar apellidos.
- Registrar teléfono.
- Registrar dirección cuando corresponda.
- Registrar imagen del recibo de agua o electricidad.
- Registrar observaciones.
- Gestionar estado del cliente.

## Estados previstos

```text
ACTIVO
OBSERVACIÓN
MOROSO
```

## Avales

Se implementará la posibilidad de:

- Registrar un nuevo aval.
- Seleccionar un cliente existente como aval.
- Consultar información del aval.

## Resultado esperado

Contar con un módulo completo para administrar la información de los clientes.

---

# 8. Fase 5 - Gestión de Préstamos

**Versión objetivo:** `0.5.0`

**Estado:** Pendiente.

## Objetivos

Implementar el módulo central de administración de préstamos.

## Funcionalidades

- Registrar solicitud de préstamo.
- Aprobar préstamo.
- Consultar préstamos.
- Consultar detalle.
- Gestionar estado.
- Asociar cliente.
- Asociar aval.
- Registrar capital inicial.
- Registrar fecha de inicio.
- Calcular interés.
- Consultar capital pendiente.
- Consultar intereses pendientes.
- Mantener historial.

## Regla financiera

La configuración inicial establece:

```text
Interés semanal = 5 %
Base de cálculo = capital inicial
Interés compuesto = No
Frecuencia = semanal
```

## Múltiples préstamos

Un cliente podrá tener múltiples préstamos.

Cada préstamo tendrá información financiera independiente.

## Resultado esperado

Contar con un módulo funcional para registrar y controlar préstamos.

---

# 9. Fase 6 - Gestión de Pagos

**Versión objetivo:** `0.6.0`

**Estado:** Pendiente.

## Objetivos

Implementar el registro y control de pagos.

## Funcionalidades

- Registrar pago.
- Consultar pagos.
- Consultar historial.
- Registrar pagos parciales.
- Calcular importe pendiente.
- Aplicar pago al interés.
- Aplicar excedente al capital.
- Actualizar saldo.
- Registrar fecha.
- Registrar usuario responsable.
- Mantener trazabilidad.

## Orden de aplicación

```text
Pago
 ↓
Interés pendiente
 ↓
Capital pendiente
```

## Ejemplo

```text
Interés pendiente: S/ 5.00
Capital pendiente:  S/ 100.00
Pago:               S/ 8.00

Resultado:

Interés pagado:     S/ 5.00
Capital pagado:     S/ 3.00
Capital restante:   S/ 97.00
```

## Resultado esperado

Contar con un sistema confiable para registrar y controlar los pagos.

---

# 10. Fase 7 - Gestión de Morosidad

**Versión objetivo:** `0.7.0`

**Estado:** Pendiente.

## Objetivos

Implementar el seguimiento automático de obligaciones vencidas.

## Funcionalidades

- Detectar intereses vencidos.
- Registrar vencimientos.
- Contabilizar pagos atrasados.
- Identificar clientes morosos.
- Mostrar cantidad de intereses vencidos.
- Actualizar estado del cliente.
- Generar alertas.
- Permitir reactivación cuando corresponda.

## Regla crítica

```text
Más de 2 intereses vencidos
        ↓
Situación crítica de morosidad
```

## Resultado esperado

Contar con información actualizada sobre los clientes que presentan incumplimientos.

---

# 11. Fase 8 - Notificaciones FCM

**Versión objetivo:** `0.8.0`

**Estado:** Pendiente.

## Objetivos

Implementar notificaciones móviles mediante Firebase Cloud Messaging.

## Tipos de notificación

- Próximo pago.
- Pago vencido.
- Morosidad.
- Eventos importantes del préstamo.
- Comunicaciones administrativas.

## Horarios iniciales

```text
08:00
16:00
```

## Consideraciones

Las notificaciones programadas deben depender del backend o de un mecanismo de programación del servidor y no exclusivamente de que la aplicación móvil se encuentre abierta.

## Resultado esperado

Los usuarios recibirán información relevante sobre las operaciones de cobranza y préstamos.

---

# 12. Fase 9 - Integración con WhatsApp

**Versión objetivo:** `0.9.0`

**Estado:** Pendiente.

## Objetivos

Integrar WhatsApp Business Platform de Meta para automatizar comunicaciones con clientes.

## Funcionalidades

- Configurar integración con Meta.
- Configurar cuenta empresarial.
- Configurar número empresarial.
- Configurar plantillas.
- Enviar mensajes.
- Utilizar variables dinámicas.
- Enviar recordatorios.
- Enviar mensajes de morosidad.
- Enviar confirmaciones.
- Registrar historial.
- Recibir estados mediante webhooks.
- Controlar errores.
- Implementar reintentos controlados.
- Evitar mensajes duplicados.

## Tipos de mensajes

```text
PAGO_PRÓXIMO
PAGO_VENCIDO
MOROSIDAD_DETECTADA
MOROSIDAD_CRÍTICA
PAGO_REGISTRADO
REACTIVACIÓN
COMUNICACIÓN_ADMINISTRATIVA
```

## Resultado esperado

Contar con comunicación automatizada y trazable mediante WhatsApp.

---

# 13. Fase 10 - Integración General

**Versión objetivo:** `0.10.0`

**Estado:** Pendiente.

## Objetivos

Integrar todos los módulos desarrollados.

## Integraciones

```text
Mobile
   ↓
API
   ↓
Base de Datos
   ↓
Reglas de Negocio

API
 ├── FCM
 └── WhatsApp
```

## Actividades

- Integrar autenticación.
- Integrar clientes.
- Integrar préstamos.
- Integrar pagos.
- Integrar morosidad.
- Integrar notificaciones.
- Integrar WhatsApp.
- Validar permisos.
- Validar navegación.
- Validar sincronización.
- Validar errores.
- Validar estados.

## Resultado esperado

Contar con un flujo completo desde el registro del cliente hasta la gestión del préstamo, pagos y comunicaciones.

---

# 14. Fase 11 - Pruebas Integrales

**Versión objetivo:** `0.11.0`

**Estado:** Pendiente.

## Objetivos

Validar el funcionamiento completo del sistema.

## Pruebas

### Funcionales

- Login.
- Clientes.
- Avales.
- Préstamos.
- Pagos.
- Morosidad.
- Notificaciones.
- WhatsApp.

### Financieras

- Cálculo del 5 %.
- Cálculo sobre capital inicial.
- Interés no compuesto.
- Frecuencia semanal.
- Pagos parciales.
- Aplicación de interés antes del capital.
- Múltiples préstamos.
- Actualización de saldos.
- Morosidad.

### Seguridad

- Autenticación.
- Autorización.
- Roles.
- Permisos.
- Sesiones.
- Protección de endpoints.
- Protección de información sensible.

### Integración

- API ↔ Mobile.
- API ↔ PostgreSQL.
- API ↔ FCM.
- API ↔ WhatsApp.

## Resultado esperado

Identificar y corregir errores antes de la primera versión estable.

---

# 15. Fase 12 - Optimización

**Versión objetivo:** `0.12.0`

**Estado:** Futuro.

## Objetivos

Mejorar rendimiento, estabilidad y experiencia de usuario.

## Actividades

- Optimizar consultas de PostgreSQL.
- Optimizar endpoints.
- Mejorar tiempos de respuesta.
- Reducir consultas innecesarias.
- Mejorar manejo de errores.
- Mejorar experiencia móvil.
- Optimizar consumo de recursos.
- Revisar logs.
- Revisar seguridad.
- Mejorar trazabilidad.

---

# 16. Fase 13 - Primera Versión Estable

**Versión objetivo:** `1.0.0`

**Estado:** Futuro.

## Criterios

La versión `1.0.0` deberá alcanzarse cuando:

- Los módulos principales estén implementados.
- Las reglas financieras estén correctamente implementadas.
- Las pruebas críticas hayan sido aprobadas.
- La autenticación funcione correctamente.
- La autorización esté implementada.
- La base de datos sea estable.
- La API funcione correctamente.
- La aplicación móvil pueda comunicarse con el backend.
- FCM esté integrado.
- WhatsApp esté integrado cuando corresponda.
- No existan errores críticos conocidos.
- La documentación esté actualizada.

## Resultado

Primera versión estable del Sistema de Gestión de Préstamos.

---

# 17. Funcionalidades Futuras

Después de la versión `1.0.0` podrán evaluarse funcionalidades adicionales.

## Reportes

- Reporte de préstamos.
- Reporte de pagos.
- Reporte de morosidad.
- Reporte de ingresos por intereses.
- Reporte por período.
- Exportación de información.

## Dashboard avanzado

- Indicadores financieros.
- Préstamos activos.
- Capital colocado.
- Capital pendiente.
- Pagos del período.
- Clientes morosos.
- Intereses generados.

## Auditoría avanzada

- Historial de cambios.
- Usuario responsable.
- Fecha y hora.
- Registro de modificaciones financieras.
- Seguimiento de operaciones críticas.

## Mejoras de comunicación

- Más plantillas de WhatsApp.
- Automatización avanzada.
- Segmentación de clientes.
- Historial de comunicaciones.
- Programación de mensajes.

---

# 18. Priorización

Las funcionalidades se clasifican de la siguiente manera:

| Prioridad | Descripción |
|---|---|
| Crítica | Necesaria para el funcionamiento principal |
| Alta | Importante para completar el sistema |
| Media | Mejora significativa |
| Baja | Funcionalidad complementaria |
| Futuro | Funcionalidad posterior a la primera versión |

## Prioridad crítica

- Autenticación.
- Clientes.
- Préstamos.
- Pagos.
- Cálculos financieros.
- Morosidad.
- Base de datos.
- API.
- Seguridad.

## Prioridad alta

- FCM.
- WhatsApp.
- Historial.
- Auditoría.
- Mejoras de UX/UI.

## Prioridad media

- Reportes.
- Dashboard avanzado.
- Exportaciones.

## Prioridad futura

- Automatizaciones adicionales.
- Nuevos canales de comunicación.
- Funcionalidades analíticas avanzadas.

---

# 19. Dependencias Principales

El desarrollo debe respetar las siguientes dependencias:

```text
Base de Datos
      ↓
Backend / API
      ↓
Autenticación
      ↓
Clientes
      ↓
Préstamos
      ↓
Pagos
      ↓
Morosidad
      ↓
Notificaciones / WhatsApp
      ↓
Integración
      ↓
Pruebas
      ↓
Versión estable
```

La aplicación móvil depende de los endpoints disponibles en el backend.

Las funcionalidades de notificaciones y WhatsApp dependen de sus respectivas configuraciones externas.

---

# 20. Criterios de Cambio del Roadmap

El roadmap puede modificarse cuando:

- Aparezcan nuevos requisitos.
- Cambien las reglas de negocio.
- Se detecten dependencias técnicas.
- Una integración externa cambie.
- Se descubran problemas durante las pruebas.
- Se priorice una funcionalidad diferente.
- Se requiera mejorar la seguridad.
- Se determine que una funcionalidad debe dividirse en varias etapas.

Todo cambio importante deberá actualizar la documentación correspondiente.

---

# 21. Relación con el Changelog

El roadmap representa:

```text
LO QUE SE PLANEA HACER
```

El changelog representa:

```text
LO QUE YA CAMBIÓ
```

Por ejemplo:

```text
ROADMAP

Fase 5
Gestión de préstamos
Pendiente


Después de implementarlo:


ROADMAP

Fase 5
Gestión de préstamos
Completada
```

Y en `CHANGELOG.md`:

```text
## [0.5.0]

### Added

- Registro de préstamos.
- Consulta de préstamos.
- Cálculo de interés semanal.
```

---

# 22. Resumen de Versiones

| Versión | Etapa | Estado |
|---|---|---|
| 0.1.0 | Documentación y planificación | Actual |
| 0.2.0 | Configuración técnica | Pendiente |
| 0.3.0 | Autenticación y usuarios | Pendiente |
| 0.4.0 | Gestión de clientes | Pendiente |
| 0.5.0 | Gestión de préstamos | Pendiente |
| 0.6.0 | Gestión de pagos | Pendiente |
| 0.7.0 | Gestión de morosidad | Pendiente |
| 0.8.0 | Notificaciones FCM | Pendiente |
| 0.9.0 | Integración WhatsApp | Pendiente |
| 0.10.0 | Integración general | Pendiente |
| 0.11.0 | Pruebas integrales | Pendiente |
| 0.12.0 | Optimización | Futuro |
| 1.0.0 | Primera versión estable | Futuro |

---

# 23. Criterio de Finalización del Proyecto

El proyecto podrá considerarse listo para una primera versión estable cuando se cumplan como mínimo las siguientes condiciones:

```text
[ ] Requisitos implementados
[ ] Reglas de negocio implementadas
[ ] Base de datos funcional
[ ] API funcional
[ ] Aplicación móvil funcional
[ ] Autenticación funcional
[ ] Autorización funcional
[ ] Gestión de clientes funcional
[ ] Gestión de préstamos funcional
[ ] Gestión de pagos funcional
[ ] Gestión de morosidad funcional
[ ] Notificaciones funcionales
[ ] Integración WhatsApp funcional
[ ] Pruebas críticas aprobadas
[ ] Seguridad validada
[ ] Documentación actualizada
[ ] Sin errores críticos conocidos
```

---

# 24. Mantenimiento del Roadmap

Este documento debe revisarse periódicamente durante el desarrollo.

Cuando una funcionalidad cambie de estado, se deberá actualizar:

- Estado de la fase.
- Versión objetivo.
- Funcionalidades.
- Dependencias.
- Documentación relacionada.

Los cambios importantes también deberán registrarse en:

[CHANGELOG.md](./CHANGELOG.md)

---

# 25. Estado Final del Roadmap

El roadmap proporciona una guía de desarrollo, pero no constituye un cronograma rígido.

Las fechas definitivas y la duración de cada fase dependerán de:

- Complejidad de implementación.
- Disponibilidad de recursos.
- Resultados de las pruebas.
- Integraciones externas.
- Cambios en requisitos.
- Evolución del proyecto.

La prioridad principal será entregar una solución **funcional, segura, mantenible y financieramente consistente** antes de incorporar funcionalidades secundarias.