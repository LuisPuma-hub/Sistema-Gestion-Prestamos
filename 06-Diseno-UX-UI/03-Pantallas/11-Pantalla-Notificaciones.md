# Pantalla de Notificaciones

## 1. Información General

| Campo | Detalle |
|---|---|
| Nombre | Pantalla de Notificaciones |
| Archivo | `11-Pantalla-Notificaciones.md` |
| Módulo | Notificaciones |
| Tipo | Consulta y gestión de notificaciones |
| Acceso | Administrador y Cobrador |
| Prioridad | Alta |
| Plataforma | Aplicación móvil |
| Servicio | Firebase Cloud Messaging (FCM) |
| Zona horaria | `America/Lima` |
| Horarios iniciales | 08:00 y 16:00 |

---

# 2. Objetivo

La pantalla de Notificaciones permitirá al usuario consultar las notificaciones generadas por el sistema relacionadas con préstamos, pagos, vencimientos y morosidad.

El objetivo es proporcionar información oportuna sobre eventos importantes del sistema.

La pantalla deberá permitir:

- Consultar notificaciones.
- Identificar notificaciones no leídas.
- Marcar notificaciones como leídas.
- Consultar el detalle de una notificación.
- Acceder al préstamo relacionado.
- Acceder al cliente relacionado.
- Identificar pagos próximos.
- Identificar pagos vencidos.
- Identificar casos de morosidad.
- Actualizar la lista de notificaciones.
- Recibir notificaciones push mediante Firebase Cloud Messaging.

---

# 3. Acceso a la Pantalla

La pantalla estará disponible desde el menú principal de la aplicación.

Ejemplo:

```text
Dashboard
   └── 🔔 Notificaciones
```

También podrá accederse mediante el icono de campana ubicado en la barra superior.

```text
┌──────────────────────────────────────┐
│ Dashboard                         🔔 │
└──────────────────────────────────────┘
```

Si existen notificaciones no leídas, el icono podrá mostrar un indicador:

```text
🔔 5
```

---

# 4. Estructura General

La pantalla tendrá una estructura similar a:

```text
┌──────────────────────────────────────┐
│ ← Notificaciones                  ✓  │
├──────────────────────────────────────┤
│                                      │
│ [Todas] [No leídas]                  │
│                                      │
├──────────────────────────────────────┤
│                                      │
│ 🔴 Pago vencido                      │
│                                      │
│ El préstamo PR-000125 tiene un       │
│ interés semanal pendiente.           │
│                                      │
│ Hace 10 minutos                      │
│                                      │
├──────────────────────────────────────┤
│                                      │
│ 🟢 Pago registrado                   │
│                                      │
│ Se registró correctamente el pago    │
│ del préstamo PR-000130.              │
│                                      │
│ Hace 2 horas                         │
│                                      │
└──────────────────────────────────────┘
```

---

# 5. Encabezado

El encabezado deberá contener:

- Botón regresar.
- Título `Notificaciones`.
- Acción para marcar todas como leídas.
- Opcionalmente botón de actualización.

Ejemplo:

```text
← Notificaciones                  ✓
```

La acción `✓` permitirá marcar las notificaciones visibles como leídas.

---

# 6. Contador de Notificaciones

El sistema podrá mostrar el número de notificaciones no leídas.

Ejemplo:

```text
🔔 5
```

Si no existen notificaciones pendientes:

```text
🔔
```

El contador deberá actualizarse cuando:

- Se reciba una nueva notificación.
- Se marque una notificación como leída.
- Se marquen todas como leídas.
- Se sincronice nuevamente la información con el backend.

---

# 7. Filtros

La pantalla tendrá filtros básicos para facilitar la consulta.

## 7.1 Todas

Muestra todas las notificaciones disponibles.

```text
[Todas]
```

## 7.2 No leídas

Muestra únicamente las notificaciones pendientes de lectura.

```text
[No leídas]
```

Opcionalmente podrán incorporarse filtros adicionales por tipo.

---

# 8. Tipos de Notificaciones

El sistema deberá manejar diferentes categorías.

## 8.1 Próximo pago

Informa que se aproxima el vencimiento de un interés semanal.

Ejemplo:

```text
Próximo pago

El préstamo PR-000125 tiene un pago
programado próximamente.

Interés semanal: S/ 25.00
Vencimiento: 07/09/2026
```

---

## 8.2 Pago vencido

Informa que un interés semanal no fue pagado en la fecha establecida.

Ejemplo:

```text
Pago vencido

El préstamo PR-000125 tiene un
interés semanal vencido.

Monto pendiente: S/ 25.00
```

---

## 8.3 Morosidad

Informa que un préstamo presenta morosidad.

Ejemplo:

```text
Morosidad

El cliente Juan Pérez presenta
un pago de interés pendiente.

Préstamo: PR-000125
Intereses vencidos: 1
```

---

## 8.4 Morosidad crítica

Informa que un préstamo superó el límite definido.

Regla:

```text
Intereses vencidos > 2
```

Ejemplo:

```text
Morosidad crítica

El préstamo PR-000125 tiene
3 intereses semanales vencidos.

Se requiere seguimiento.
```

---

## 8.5 Pago registrado

Confirma que un pago fue registrado correctamente.

Ejemplo:

```text
Pago registrado

Se registró correctamente un pago
de S/ 100.00.

Préstamo: PR-000125
```

---

## 8.6 Pago aplicado

Puede informar la distribución de un pago.

Ejemplo:

```text
Pago aplicado

Pago: S/ 100.00

Intereses: S/ 75.00
Capital:   S/ 25.00
```

---

## 8.7 Reactivación

Informa que un préstamo fue reactivado.

Ejemplo:

```text
Préstamo reactivado

El préstamo PR-000125 fue
reactivado correctamente.
```

---

# 9. Tarjeta de Notificación

Cada notificación se mostrará mediante una tarjeta.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ 🔴 Pago vencido                 •   │
│                                      │
│ El préstamo PR-000125 tiene un       │
│ interés semanal pendiente.           │
│                                      │
│ S/ 25.00                             │
│ Vencimiento: 31/08/2026              │
│                                      │
│ Hace 2 horas                         │
└──────────────────────────────────────┘
```

El punto `•` representa una notificación no leída.

---

# 10. Estado de Lectura

Las notificaciones tendrán dos estados principales.

## 10.1 No leída

Una notificación no leída deberá distinguirse visualmente.

Puede utilizar:

- Fondo diferenciado.
- Indicador.
- Texto destacado.
- Icono.

Ejemplo:

```text
🔴 Pago vencido                         •
```

## 10.2 Leída

Una notificación leída tendrá una apariencia menos destacada.

Ejemplo:

```text
Pago registrado
Se registró correctamente el pago.
```

El diseño no deberá depender únicamente del color para distinguir ambos estados.

---

# 11. Detalle de Notificación

Al seleccionar una notificación se mostrará información completa.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ ← Detalle de Notificación            │
├──────────────────────────────────────┤
│                                      │
│ Pago vencido                         │
│                                      │
│ El préstamo PR-000125 tiene un       │
│ interés semanal pendiente.           │
│                                      │
│ Cliente: Juan Pérez                  │
│ Préstamo: PR-000125                  │
│ Monto: S/ 25.00                      │
│ Vencimiento: 31/08/2026              │
│                                      │
│ Fecha: 04/09/2026                    │
│ Hora: 08:00                          │
│                                      │
│ [Ver préstamo]                       │
│ [Ver cliente]                        │
└──────────────────────────────────────┘
```

---

# 12. Acciones de una Notificación

Dependiendo del tipo, una notificación podrá permitir:

- Marcar como leída.
- Ver préstamo.
- Ver cliente.
- Registrar pago.
- Ver morosidad.
- Contactar por WhatsApp.
- Ver detalle.

Ejemplo:

```text
Pago vencido

[Ver préstamo]
[Registrar pago]
[WhatsApp]
```

---

# 13. Marcar como Leída

Cuando el usuario abra una notificación no leída, el sistema podrá marcarla automáticamente como leída.

También podrá existir una acción manual.

Ejemplo:

```text
[Marcar como leída]
```

El cambio deberá sincronizarse con el backend.

---

# 14. Marcar Todas como Leídas

El encabezado podrá incluir:

```text
[Marcar todas como leídas]
```

Al seleccionar esta opción se deberá solicitar confirmación si existe un número elevado de notificaciones o si la operación implica una actualización masiva.

Ejemplo:

```text
¿Marcar todas las notificaciones como leídas?

[Cancelar]    [Confirmar]
```

---

# 15. Actualización de Notificaciones

La lista podrá actualizarse mediante:

- Botón de actualización.
- Pull to refresh.
- Sincronización automática.
- Recepción de una notificación push.

Ejemplo:

```text
┌──────────────────────────────┐
│ Desliza para actualizar      │
└──────────────────────────────┘
```

---

# 16. Notificaciones Push

La aplicación utilizará Firebase Cloud Messaging (FCM) para recibir notificaciones push.

Flujo:

```text
Backend
   ↓
Firebase Cloud Messaging
   ↓
Dispositivo móvil
   ↓
Aplicación
   ↓
Notificación
```

La aplicación deberá gestionar correctamente las notificaciones cuando:

- Está abierta.
- Está en segundo plano.
- Se encuentra bloqueada.
- El usuario toca la notificación.

---

# 17. Notificaciones con Aplicación Abierta

Cuando la aplicación se encuentre abierta, una nueva notificación podrá mostrarse mediante:

- Banner.
- Snackbar.
- Indicador en el icono.
- Actualización de la lista.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ 🔔 Nuevo pago vencido                │
│ El préstamo PR-000125 requiere       │
│ atención.                            │
└──────────────────────────────────────┘
```

---

# 18. Notificaciones con Aplicación Cerrada

Cuando la aplicación esté cerrada, las notificaciones push deberán ser procesadas por el sistema operativo mediante FCM.

Al seleccionar la notificación:

```text
Notificación
      ↓
Abrir aplicación
      ↓
Autenticación / sesión
      ↓
Pantalla relacionada
```

Si el usuario tiene una sesión válida, deberá dirigirse directamente al contenido relacionado cuando sea posible.

---

# 19. Horarios de Notificación

Las notificaciones programadas inicialmente estarán establecidas a:

```text
08:00
16:00
```

Zona horaria:

```text
America/Lima
```

Estos horarios deberán ser configurables según las reglas establecidas por el Administrador.

---

# 20. Programación de Notificaciones

El sistema deberá generar las notificaciones programadas de acuerdo con las condiciones del negocio.

Ejemplo:

```text
08:00
   ↓
Evaluar préstamos
   ↓
Identificar próximos vencimientos
   ↓
Identificar vencidos
   ↓
Identificar morosidad
   ↓
Generar notificaciones
```

La aplicación móvil no deberá depender de estar abierta para que el proceso de programación se ejecute.

La programación deberá gestionarse desde los servicios del backend y/o infraestructura destinada para tareas programadas.

---

# 21. Notificaciones de Próximo Vencimiento

El sistema deberá poder generar recordatorios para pagos próximos.

Ejemplo:

```text
Recordatorio de pago

El préstamo PR-000125 tiene un
pago próximo.

Interés semanal: S/ 25.00
Fecha de vencimiento: 07/09/2026
```

La fecha y condiciones exactas dependerán de las reglas configuradas.

---

# 22. Notificaciones de Morosidad

Cuando se detecte un pago vencido, el sistema podrá generar una notificación.

Ejemplo:

```text
Pago vencido

El cliente Juan Pérez tiene un
interés semanal pendiente.

Préstamo: PR-000125
Monto pendiente: S/ 25.00
```

Si la situación supera las 2 semanas vencidas:

```text
Morosidad crítica

El préstamo PR-000125 presenta
3 intereses semanales vencidos.

Requiere seguimiento.
```

---

# 23. Relación con Morosidad

La pantalla de Notificaciones deberá estar integrada con el módulo de Morosidad.

Flujo:

```text
Interés vencido
      ↓
Backend detecta morosidad
      ↓
Actualiza estado
      ↓
Genera notificación
      ↓
FCM
      ↓
Aplicación móvil
      ↓
Usuario
```

Al seleccionar la notificación podrá abrirse directamente:

```text
Detalle de Morosidad
```

---

# 24. Relación con Pagos

Después de registrar un pago, el sistema deberá actualizar la información relacionada.

Flujo:

```text
Registrar pago
      ↓
Backend valida pago
      ↓
Actualizar intereses
      ↓
Actualizar capital
      ↓
Actualizar morosidad
      ↓
Actualizar notificaciones
```

Por ejemplo, una notificación de pago vencido podrá dejar de aparecer como pendiente cuando la deuda correspondiente haya sido regularizada.

---

# 25. Relación con WhatsApp

Una notificación de morosidad podrá proporcionar acceso a WhatsApp.

Ejemplo:

```text
Morosidad crítica

[Ver morosidad]
[WhatsApp]
```

El usuario podrá revisar el caso antes de enviar un mensaje.

---

# 26. Historial

La aplicación podrá mantener un historial de notificaciones.

Cada registro deberá contener como mínimo:

- Identificador.
- Tipo.
- Título.
- Mensaje.
- Fecha.
- Hora.
- Estado de lectura.
- Cliente relacionado, cuando corresponda.
- Préstamo relacionado, cuando corresponda.

Ejemplo:

```text
04/09/2026  08:00
Pago vencido
PR-000125
No leída

03/09/2026  16:00
Morosidad
PR-000130
Leída
```

---

# 27. Persistencia del Estado de Lectura

El estado de lectura deberá persistirse para que no se pierda cuando:

- El usuario cierre la aplicación.
- El usuario cambie de dispositivo, cuando el sistema lo soporte.
- Se actualice la aplicación.
- Se cierre la sesión y vuelva a iniciarse.

La fuente oficial del estado deberá ser el backend.

---

# 28. Estado Sin Notificaciones

Si no existen notificaciones:

```text
┌──────────────────────────────────────┐
│                                      │
│               ✓                      │
│                                      │
│      No tienes notificaciones        │
│                                      │
│      Todo está actualizado.          │
│                                      │
└──────────────────────────────────────┘
```

---

# 29. Estado Sin Notificaciones No Leídas

Si el usuario selecciona:

```text
[No leídas]
```

y no existen registros pendientes:

```text
No tienes notificaciones pendientes.
```

---

# 30. Estado de Carga

Mientras se consulta la información:

```text
┌──────────────────────────────────────┐
│                                      │
│       Cargando notificaciones...     │
│                                      │
│                 ⟳                    │
│                                      │
└──────────────────────────────────────┘
```

Se recomienda utilizar skeleton loading.

---

# 31. Estado de Error

Si ocurre un problema de conexión:

```text
┌──────────────────────────────────────┐
│                                      │
│                ⚠                     │
│                                      │
│   No fue posible cargar las          │
│   notificaciones.                    │
│                                      │
│            [Reintentar]              │
│                                      │
└──────────────────────────────────────┘
```

---

# 32. Permisos de Notificaciones

La aplicación deberá solicitar al usuario los permisos necesarios para mostrar notificaciones push.

Flujo:

```text
Instalación
     ↓
Inicio de sesión
     ↓
Solicitar permiso
     ↓
Usuario acepta
     ↓
Registrar dispositivo
     ↓
Obtener token FCM
     ↓
Enviar token al backend
```

Si el usuario rechaza los permisos, la aplicación deberá continuar funcionando, aunque las notificaciones push podrán no estar disponibles.

---

# 33. Token del Dispositivo

La aplicación deberá registrar el token FCM asociado al dispositivo.

El backend deberá relacionarlo con:

```text
Usuario
   +
Dispositivo
   +
Token FCM
```

Cuando el token cambie, deberá actualizarse en el backend.

---

# 34. Seguridad

La pantalla deberá cumplir con:

- Autenticación obligatoria.
- Autorización basada en roles.
- Comunicación mediante HTTPS.
- Validación de permisos en backend.
- No incluir información financiera innecesaria en las notificaciones push.
- No incluir credenciales o información sensible en el contenido de las notificaciones.
- Validar la autenticidad de las acciones realizadas desde una notificación.

---

# 35. Privacidad

Las notificaciones deberán evitar mostrar información sensible en la pantalla bloqueada.

En lugar de:

```text
Juan Pérez debe S/ 1,500.00
del préstamo PR-000125.
```

se recomienda utilizar:

```text
Tienes una actualización relacionada
con un préstamo.
```

El detalle financiero podrá consultarse después de abrir la aplicación y validar la sesión.

---

# 36. Navegación desde una Notificación

Las notificaciones podrán utilizar navegación contextual.

Ejemplo:

```text
Notificación
     ↓
Pago vencido
     ↓
Abrir aplicación
     ↓
Detalle de préstamo
```

Otro ejemplo:

```text
Notificación
     ↓
Morosidad crítica
     ↓
Abrir aplicación
     ↓
Detalle de morosidad
```

La navegación deberá validar que el usuario tenga permisos para acceder al recurso.

---

# 37. Paginación

Cuando exista un número elevado de notificaciones, se utilizará paginación.

Ejemplo:

```text
Mostrando 1 - 20 de 100

[Cargar más]
```

Las notificaciones más recientes deberán mostrarse primero.

Orden recomendado:

```text
Fecha y hora descendente
```

Es decir:

```text
Más reciente
      ↓
Más antigua
```

---

# 38. Eliminación de Notificaciones

La eliminación definitiva de notificaciones deberá controlarse desde el backend.

La interfaz podrá permitir ocultar o archivar notificaciones si esta funcionalidad es definida posteriormente.

No se recomienda eliminar automáticamente registros que puedan ser necesarios para auditoría.

---

# 39. Auditoría

Las operaciones relevantes podrán quedar registradas.

Ejemplos:

```text
Notificación recibida
Notificación abierta
Notificación marcada como leída
Notificaciones marcadas como leídas
Acción ejecutada desde notificación
```

La auditoría podrá registrar:

- Usuario.
- Fecha.
- Hora.
- Tipo de acción.
- Notificación.
- Recurso relacionado.

---

# 40. Consideraciones Técnicas

La aplicación podrá recibir información similar a:

```text
notificationId
userId
type
title
message
read
createdAt
relatedClientId
relatedLoanId
action
```

Los nombres definitivos dependerán del contrato de la API.

La aplicación deberá utilizar la información proporcionada por el backend como fuente oficial.

---

# 41. Flujo Completo

El flujo general de notificaciones será:

```text
Evento del sistema
       ↓
Backend
       ↓
Evaluar reglas
       ↓
Generar notificación
       ↓
Guardar notificación
       ↓
Enviar mediante FCM
       ↓
Dispositivo móvil
       ↓
Usuario recibe notificación
       ↓
Usuario abre notificación
       ↓
Aplicación consulta backend
       ↓
Mostrar detalle
       ↓
Marcar como leída
       ↓
Acción relacionada
```

---

# 42. Ejemplo de Flujo de Morosidad

```text
Interés semanal vence
        ↓
Backend detecta vencimiento
        ↓
Préstamo pasa a VENCIDO
        ↓
Se genera notificación
        ↓
Usuario recibe alerta
        ↓
Usuario abre notificación
        ↓
Detalle de Morosidad
        ↓
Contactar cliente
        ↓
Registrar pago
        ↓
Actualizar préstamo
        ↓
Actualizar morosidad
```

Si existen más de 2 intereses vencidos:

```text
3 intereses vencidos
        ↓
Estado CRÍTICO
        ↓
Notificación de morosidad crítica
        ↓
Seguimiento prioritario
```

---

# 43. Accesibilidad

La pantalla deberá considerar:

- Texto legible.
- Contraste adecuado.
- Áreas táctiles suficientes.
- Etiquetas accesibles.
- Compatibilidad con lectores de pantalla.
- No depender exclusivamente del color.
- Indicadores textuales para los estados.

Ejemplo:

```text
🔴 CRÍTICO
```

en lugar de utilizar únicamente un color rojo.

---

# 44. Diseño Responsive

La pantalla deberá adaptarse a diferentes tamaños de dispositivos.

En dispositivos pequeños:

```text
┌─────────────────────┐
│ ← Notificaciones    │
├─────────────────────┤
│ [Todas] [No leídas] │
├─────────────────────┤
│ 🔴 Pago vencido     │
│ PR-000125           │
│ S/ 25.00 pendiente  │
│ Hace 10 min         │
├─────────────────────┤
│ 🟢 Pago registrado  │
│ PR-000130           │
│ Hace 2 horas        │
└─────────────────────┘
```

Los elementos deberán adaptarse sin provocar desplazamiento horizontal.

---

# 45. Criterios de Aceptación

La pantalla será considerada correctamente implementada cuando:

- [ ] El usuario pueda acceder a sus notificaciones.
- [ ] Se muestren las notificaciones más recientes primero.
- [ ] Se pueda diferenciar una notificación leída de una no leída.
- [ ] Se pueda filtrar entre todas y no leídas.
- [ ] Se pueda abrir el detalle de una notificación.
- [ ] Se pueda marcar una notificación como leída.
- [ ] Se puedan marcar todas como leídas.
- [ ] Se muestre el contador de notificaciones no leídas.
- [ ] El contador se actualice correctamente.
- [ ] Se puedan recibir notificaciones mediante FCM.
- [ ] Las notificaciones funcionen con la aplicación abierta.
- [ ] Las notificaciones funcionen con la aplicación en segundo plano.
- [ ] Las notificaciones puedan recibirse cuando la aplicación esté cerrada, según las capacidades del sistema operativo y permisos.
- [ ] Se puedan generar notificaciones de próximos pagos.
- [ ] Se puedan generar notificaciones de pagos vencidos.
- [ ] Se puedan generar notificaciones de morosidad.
- [ ] Se puedan generar notificaciones de morosidad crítica.
- [ ] Se puedan generar notificaciones de pagos registrados.
- [ ] Se respeten los horarios configurados.
- [ ] Los horarios utilicen la zona `America/Lima`.
- [ ] Se pueda navegar desde una notificación hacia el recurso relacionado.
- [ ] Se validen los permisos antes de acceder al recurso.
- [ ] No se exponga información financiera sensible innecesariamente.
- [ ] El backend sea la fuente oficial del estado de las notificaciones.
- [ ] Se controle el estado de carga.
- [ ] Se controle el estado vacío.
- [ ] Se controle el estado de error.
- [ ] La pantalla sea responsive.
- [ ] Se cumplan criterios básicos de accesibilidad.

---

# 46. Relación con Otros Módulos

La pantalla de Notificaciones se relaciona con:

```text
Notificaciones
    ├── Dashboard
    ├── Clientes
    ├── Préstamos
    ├── Pagos
    ├── Morosidad
    ├── WhatsApp
    └── Firebase FCM
```

Flujo principal:

```text
Evento financiero
      ↓
Backend
      ↓
Notificación
      ↓
FCM
      ↓
Aplicación móvil
      ↓
Usuario
```

---

# 47. Reglas de Negocio Relacionadas

La pantalla deberá respetar las reglas definidas en:

```text
02-Reglas-Negocio/
├── 02-Reglas-Prestamos.md
├── 03-Reglas-Pagos.md
├── 04-Reglas-Morosidad.md
├── 05-Reglas-Notificaciones.md
└── 06-Reglas-WhatsApp.md
```

Las reglas relacionadas con horarios, vencimientos, morosidad y generación de notificaciones deberán mantenerse centralizadas en el módulo de reglas de negocio.

---

# 48. Resultado Esperado

La pantalla deberá permitir al Administrador y al Cobrador mantenerse informados sobre los eventos relevantes del sistema.

El flujo principal será:

```text
Evento
   ↓
Notificación
   ↓
Usuario
   ↓
Consulta
   ↓
Acción
```

La integración con FCM permitirá que los usuarios puedan recibir avisos incluso cuando la aplicación no se encuentre abierta, siempre que los permisos y las condiciones del dispositivo lo permitan.

---

# 49. Siguiente Pantalla

La siguiente pantalla a documentar será:

```text
06-Diseno-UX-UI/03-Pantallas/12-Pantalla-WhatsApp.md
```

Esta pantalla documentará la gestión de mensajes, plantillas, historial y envío de comunicaciones mediante WhatsApp.