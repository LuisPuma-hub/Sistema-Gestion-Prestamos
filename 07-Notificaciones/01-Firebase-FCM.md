# Firebase Cloud Messaging (FCM)

## 1. Introducción

Firebase Cloud Messaging (FCM) será utilizado como servicio de mensajería para enviar notificaciones push desde el backend hacia la aplicación móvil desarrollada con .NET MAUI.

Las notificaciones permitirán informar al administrador y al cobrador sobre diferentes eventos relacionados con:

- Préstamos.
- Pagos.
- Vencimientos.
- Morosidad.
- Reactivación de préstamos.
- Operaciones importantes del sistema.

La generación de las notificaciones será responsabilidad del backend desarrollado con ASP.NET Core Web API.

La aplicación móvil .NET MAUI será responsable de recibir, procesar y mostrar las notificaciones en el dispositivo.

---

# 2. Objetivo

Definir la arquitectura y funcionamiento de Firebase Cloud Messaging dentro del sistema de gestión de préstamos.

Los objetivos principales son:

- Enviar notificaciones push a dispositivos móviles.
- Informar sobre pagos próximos.
- Informar sobre pagos vencidos.
- Informar sobre situaciones de morosidad.
- Informar sobre eventos importantes del sistema.
- Permitir recibir notificaciones aunque la aplicación no esté abierta.
- Mantener asociados los dispositivos con sus respectivos usuarios.
- Gestionar tokens FCM.
- Controlar errores de envío.
- Mantener trazabilidad de las notificaciones.

---

# 3. Arquitectura

La arquitectura de notificaciones estará basada en el siguiente flujo:

```text
┌──────────────────────────────┐
│       Aplicación .NET MAUI   │
│             C#               │
└──────────────┬───────────────┘
               │
               │ Registro de token
               ▼
┌──────────────────────────────┐
│    ASP.NET Core Web API      │
│             C#               │
└──────────────┬───────────────┘
               │
               │ Solicitud de envío
               ▼
┌──────────────────────────────┐
│ Firebase Cloud Messaging     │
│            FCM               │
└──────────────┬───────────────┘
               │
               │ Push Notification
               ▼
┌──────────────────────────────┐
│       Dispositivo móvil      │
│          .NET MAUI           │
└──────────────────────────────┘
```

El backend será responsable de determinar cuándo debe enviarse una notificación.

---

# 4. Componentes involucrados

Los componentes principales serán:

| Componente | Tecnología | Responsabilidad |
|---|---|---|
| Aplicación móvil | .NET MAUI | Recibir y mostrar notificaciones |
| Lenguaje móvil | C# | Desarrollo de la aplicación |
| Backend | ASP.NET Core Web API | Generar y enviar notificaciones |
| Base de datos | PostgreSQL | Almacenar usuarios, dispositivos y notificaciones |
| ORM | Entity Framework Core | Acceso a la base de datos |
| Servicio de notificaciones | Firebase Cloud Messaging | Entrega de notificaciones |
| Comunicación | HTTPS | Comunicación segura |

---

# 5. Configuración del proyecto Firebase

Se deberá crear un proyecto en Firebase para el sistema.

El proyecto Firebase deberá estar asociado con la aplicación móvil Android desarrollada mediante .NET MAUI.

La configuración deberá permitir:

- Identificar la aplicación.
- Generar y administrar tokens FCM.
- Recibir notificaciones push.
- Administrar la comunicación con Firebase.
- Configurar los servicios necesarios para Android.

---

# 6. Integración con .NET MAUI

La aplicación móvil será desarrollada utilizando:

```text
.NET MAUI
C#
```

La integración con Firebase deberá permitir que la aplicación:

1. Obtenga el token FCM del dispositivo.
2. Envíe el token al backend.
3. Reciba notificaciones.
4. Procese las notificaciones recibidas.
5. Muestre las notificaciones al usuario.
6. Actualice el token cuando sea necesario.
7. Permita navegar hacia la pantalla relacionada con la notificación.

---

# 7. Registro del dispositivo

Cuando un usuario inicie sesión en la aplicación, el sistema deberá asociar el dispositivo con el usuario.

Flujo:

```text
Usuario inicia sesión
        │
        ▼
.NET MAUI obtiene token FCM
        │
        ▼
Envía token al backend
        │
        ▼
ASP.NET Core valida usuario
        │
        ▼
Guarda dispositivo en PostgreSQL
```

El backend deberá mantener la relación:

```text
Usuario
   │
   └── Dispositivo
          │
          └── Token FCM
```

---

# 8. Token FCM

El token FCM identifica una instalación de la aplicación en un dispositivo.

El token deberá almacenarse en el backend.

Información conceptual:

```text
Device
├── Id
├── UsuarioId
├── TokenFCM
├── Plataforma
├── Activo
├── FechaRegistro
├── FechaActualizacion
└── UltimaActividad
```

El token no deberá considerarse permanente.

Firebase puede generar un nuevo token cuando cambien determinadas condiciones del dispositivo o de la aplicación.

---

# 9. Actualización del token

La aplicación deberá informar al backend cuando el token FCM cambie.

Flujo:

```text
Token anterior
      │
      ▼
Firebase genera nuevo token
      │
      ▼
.NET MAUI detecta cambio
      │
      ▼
Envía nuevo token
      │
      ▼
Backend actualiza dispositivo
```

El sistema deberá evitar mantener tokens obsoletos como activos.

---

# 10. Asociación usuario-dispositivo

Un usuario podrá utilizar más de un dispositivo.

Por ejemplo:

```text
Usuario
   │
   ├── Celular 1
   │      └── Token FCM
   │
   └── Celular 2
          └── Token FCM
```

El backend deberá poder almacenar múltiples dispositivos asociados al mismo usuario.

Esto permitirá enviar notificaciones a los dispositivos activos correspondientes.

---

# 11. Envío desde el backend

El envío de notificaciones deberá ser gestionado por el backend desarrollado con ASP.NET Core Web API.

El flujo será:

```text
Regla de negocio
      │
      ▼
ASP.NET Core
      │
      ▼
Generación de notificación
      │
      ▼
Obtención de token FCM
      │
      ▼
Firebase Cloud Messaging
      │
      ▼
Dispositivo
```

El cliente móvil no deberá encargarse de enviar notificaciones financieras a otros usuarios.

---

# 12. Backend como fuente de verdad

Las decisiones relacionadas con:

- Fechas de vencimiento.
- Intereses.
- Pagos.
- Saldos.
- Morosidad.
- Estados de préstamos.
- Generación de alertas.

serán realizadas por el backend.

La aplicación .NET MAUI solamente mostrará la información proporcionada por el backend.

Esto evita que una modificación del cliente móvil altere las reglas financieras del sistema.

---

# 13. Tipos de mensajes

El sistema podrá utilizar diferentes tipos de mensajes enviados mediante FCM.

## 13.1. Notificación visual

Contiene información que será mostrada al usuario.

Ejemplo:

```text
Título:
Recordatorio de pago

Mensaje:
El cliente Juan Pérez tiene un pago próximo.
```

---

## 13.2. Datos

El mensaje podrá incluir información que permita identificar la acción asociada.

Ejemplo conceptual:

```text
Tipo: PAYMENT_REMINDER
PrestamoId: 125
ClienteId: 48
```

---

## 13.3. Mensaje combinado

Se podrá utilizar información visual junto con datos internos.

Ejemplo:

```text
Título:
Pago vencido

Mensaje:
El cliente Juan Pérez tiene un pago pendiente.

Tipo:
PAYMENT_OVERDUE

PrestamoId:
125
```

---

# 14. Tipos de notificación del sistema

Se utilizarán los siguientes identificadores:

| Código | Descripción |
|---|---|
| PAYMENT_REMINDER | Recordatorio de pago |
| PAYMENT_OVERDUE | Pago vencido |
| DELINQUENCY | Morosidad |
| CRITICAL_DELINQUENCY | Morosidad crítica |
| PAYMENT_REGISTERED | Pago registrado |
| LOAN_REACTIVATED | Préstamo reactivado |

Estos identificadores permitirán que la aplicación determine qué acción realizar al recibir una notificación.

---

# 15. Notificación de pago próximo

Cuando el backend detecte que existe un pago próximo, podrá generar:

```text
Tipo:
PAYMENT_REMINDER
```

Ejemplo:

```text
Título:
Recordatorio de pago

Mensaje:
El cliente Juan Pérez tiene un pago de interés
programado para el 15/09/2026.
```

La fecha y el monto deberán provenir del backend.

---

# 16. Notificación de pago vencido

Cuando un pago supere su fecha de vencimiento y permanezca pendiente:

```text
Tipo:
PAYMENT_OVERDUE
```

Ejemplo:

```text
Título:
Pago vencido

Mensaje:
El cliente Juan Pérez tiene un pago pendiente.
```

---

# 17. Notificación de morosidad

Cuando el sistema detecte que un cliente tiene pagos de interés vencidos:

```text
Tipo:
DELINQUENCY
```

Ejemplo:

```text
Título:
Cliente en mora

Mensaje:
El cliente Juan Pérez registra pagos de interés vencidos.
```

---

# 18. Notificación de morosidad crítica

La condición definida para morosidad crítica es:

```text
Más de 2 pagos de interés vencidos
```

Cuando se cumpla esta condición:

```text
Tipo:
CRITICAL_DELINQUENCY
```

Ejemplo:

```text
Título:
Morosidad crítica

Mensaje:
El cliente Juan Pérez registra más de 2 pagos
de interés vencidos.
```

---

# 19. Notificación de pago registrado

Después de registrar correctamente un pago, el sistema podrá generar:

```text
Tipo:
PAYMENT_REGISTERED
```

Ejemplo:

```text
Título:
Pago registrado

Mensaje:
Se registró correctamente el pago del cliente Juan Pérez.
```

---

# 20. Notificación de reactivación

Cuando un préstamo sea reactivado:

```text
Tipo:
LOAN_REACTIVATED
```

Ejemplo:

```text
Título:
Préstamo reactivado

Mensaje:
El préstamo del cliente Juan Pérez fue reactivado.
```

La operación deberá quedar registrada en la auditoría.

---

# 21. Comportamiento de la aplicación

La aplicación .NET MAUI deberá considerar diferentes estados.

## Aplicación abierta

```text
FCM
 │
 ▼
.NET MAUI
 │
 ▼
Procesa mensaje
 │
 ▼
Muestra información
```

---

## Aplicación en segundo plano

```text
FCM
 │
 ▼
Sistema operativo
 │
 ▼
Notificación
```

La notificación deberá poder aparecer aunque el usuario no esté utilizando activamente la aplicación.

---

## Aplicación cerrada

La infraestructura de notificaciones deberá permitir recibir notificaciones push cuando la aplicación no esté abierta, respetando las capacidades y restricciones del sistema operativo.

---

# 22. Navegación desde una notificación

Las notificaciones podrán incluir información para determinar la pantalla de destino.

Ejemplo:

```text
Tipo:
PAYMENT_OVERDUE

PrestamoId:
125
```

Al seleccionar la notificación:

```text
Notificación
      │
      ▼
.NET MAUI
      │
      ▼
Identifica PAYMENT_OVERDUE
      │
      ▼
Obtiene PrestamoId
      │
      ▼
Abre detalle del préstamo
```

---

# 23. Base de datos

Las notificaciones y dispositivos deberán almacenarse en PostgreSQL.

Entity Framework Core será utilizado para acceder a los datos desde ASP.NET Core.

Entidades conceptuales:

```text
Usuario
   │
   └── Dispositivo
          │
          └── TokenFCM

Notificacion
   │
   ├── Usuario
   ├── Cliente
   └── Prestamo
```

---

# 24. Historial de notificaciones

El sistema deberá conservar un historial de las notificaciones generadas.

Información mínima:

```text
Id
UsuarioId
ClienteId
PrestamoId
Tipo
Titulo
Mensaje
FechaProgramada
FechaEnvio
Estado
Intentos
Error
FechaCreacion
FechaActualizacion
```

Esto permitirá consultar:

- Notificaciones enviadas.
- Notificaciones fallidas.
- Notificaciones pendientes.
- Cantidad de intentos.
- Fecha de envío.
- Error producido.

---

# 25. Estados

Estados normativos unificados (ver `04-Base-Datos/03-Diccionario-Datos.md §15`):

| Estado | Descripción |
|---|---|
| PENDIENTE | Creada y pendiente de envío |
| ENVIADO | Enviada correctamente |
| ENTREGADO | Entregada al dispositivo |
| LEIDO | Leída por el usuario |
| FALLIDO | No pudo enviarse |
| CANCELADO | Cancelada |

Quedan prohibidos como sinónimos `ENVIADA/LEIDA/ERROR/ENVIANDO/PROCESANDO` (`PROCESANDO` solo como estado transitorio interno del scheduler, nunca persistido como final).

---

# 26. Manejo de errores

El backend deberá manejar errores relacionados con:

- Firebase.
- Conectividad.
- Tokens inválidos.
- Tiempo de espera.
- Errores temporales.
- Configuración incorrecta.

Los errores deberán registrarse.

Ejemplo:

```text
Notificación:
PAYMENT_REMINDER

Estado:
FALLIDO

Error:
Token FCM inválido
```

---

# 27. Tokens inválidos

Cuando Firebase indique que un token ya no es válido, el backend deberá marcarlo como inactivo.

```text
Token inválido
      │
      ▼
Backend
      │
      ▼
Activo = false
```

No se deberán realizar reintentos indefinidos sobre un token inválido.

---

# 28. Reintentos

Ante errores temporales, el sistema podrá realizar reintentos.

Ejemplo:

```text
Intento 1
   │
   └── Error temporal
          │
          ▼
Intento 2
   │
   └── Error temporal
          │
          ▼
Intento 3
   │
   └── Fallo definitivo
```

El número máximo de reintentos deberá ser configurable.

---

# 29. Seguridad

Las credenciales y claves necesarias para la comunicación con Firebase deberán permanecer protegidas.

No deberán almacenarse:

- Claves privadas en la aplicación móvil.
- Credenciales administrativas.
- Secretos del backend.
- Credenciales de Firebase dentro del código fuente público.

La comunicación entre la aplicación y el backend deberá utilizar:

```text
HTTPS
```

---

# 30. Protección de información

Las notificaciones no deberán incluir información financiera excesivamente sensible cuando puedan ser visibles desde la pantalla bloqueada.

Ejemplo recomendado:

```text
Título:
Recordatorio de pago

Mensaje:
Existe un pago pendiente de revisión.
```

La información detallada deberá consultarse dentro de la aplicación después de la autenticación correspondiente.

---

# 31. Horarios programados

El sistema tendrá inicialmente dos horarios para las notificaciones programadas:

```text
08:00
16:00
```

Zona horaria:

```text
America/Lima
```

Estos horarios serán gestionados desde el backend.

La aplicación móvil no deberá depender de un temporizador local para determinar cuándo generar las notificaciones financieras.

---

# 32. Auditoría

Las operaciones importantes relacionadas con las notificaciones deberán poder ser auditadas.

Se deberá registrar:

- Fecha.
- Hora.
- Usuario.
- Tipo de notificación.
- Cliente relacionado.
- Préstamo relacionado.
- Estado.
- Resultado del envío.
- Error cuando corresponda.

---

# 33. Pruebas

Se deberán realizar pruebas para validar:

- Registro del token FCM.
- Actualización del token.
- Envío de notificaciones.
- Recepción en .NET MAUI.
- Aplicación abierta.
- Aplicación en segundo plano.
- Aplicación cerrada.
- Token inválido.
- Error de Firebase.
- Reintentos.
- Navegación desde la notificación.
- Notificaciones duplicadas.
- Asociación usuario-dispositivo.

---

# 34. Criterios de aceptación

La integración con FCM será considerada correctamente implementada cuando:

- [ ] La aplicación .NET MAUI pueda obtener un token FCM.
- [ ] El token pueda registrarse en el backend.
- [ ] El token quede asociado al usuario.
- [ ] El backend pueda enviar notificaciones mediante FCM.
- [ ] La aplicación pueda recibir las notificaciones.
- [ ] Se puedan identificar los diferentes tipos de notificación.
- [ ] Se pueda navegar hacia la pantalla correspondiente.
- [ ] Se registren las notificaciones en PostgreSQL.
- [ ] Se controlen tokens inválidos.
- [ ] Se registren errores.
- [ ] Se puedan realizar reintentos.
- [ ] Se eviten credenciales sensibles en la aplicación móvil.
- [ ] Se utilice HTTPS para la comunicación con el backend.
- [ ] Las notificaciones programadas respeten `America/Lima`.
- [ ] Los cálculos financieros permanezcan bajo responsabilidad del backend.

---

# 35. Tecnologías utilizadas

La implementación de este módulo utilizará:

```text
.NET MAUI
C#
ASP.NET Core Web API
Entity Framework Core
PostgreSQL
Firebase Cloud Messaging
HTTPS
Git
GitHub
```

---

# 36. Relación con otros módulos

Este documento se relaciona con:

```text
03-Arquitectura/
├── 01-Arquitectura-General.md
├── 02-Stack-Tecnologico.md
├── 03-Arquitectura-Backend.md
└── 04-Arquitectura-Mobile.md

05-API/
├── 01-Endpoints.md
├── 02-Autenticacion.md
└── 03-Modelos-Request-Response.md

07-Notificaciones/
└── 02-Programacion-Notificaciones.md

09-Seguridad/
```

---

# 37. Resumen

Firebase Cloud Messaging será el servicio encargado de entregar notificaciones push a la aplicación móvil .NET MAUI.

La arquitectura será:

```text
.NET MAUI
    │
    │ HTTPS
    ▼
ASP.NET Core Web API
    │
    ├── PostgreSQL
    │
    └── Firebase Cloud Messaging
              │
              ▼
          Dispositivo
```

El backend será responsable de las reglas y decisiones de negocio, mientras que FCM será responsable de la entrega de los mensajes al dispositivo.

La aplicación .NET MAUI será responsable de recibir y presentar las notificaciones al usuario.