# Programación de Notificaciones

## 1. Introducción

El sistema de gestión de préstamos requiere un mecanismo automático para evaluar los préstamos, pagos y situaciones de morosidad y generar las notificaciones correspondientes.

La programación de las notificaciones será gestionada desde el backend desarrollado con:

```text
C#
ASP.NET Core Web API
```

El backend ejecutará tareas programadas para evaluar la información almacenada en PostgreSQL y determinar qué notificaciones deben generarse.

Las notificaciones serán enviadas mediante Firebase Cloud Messaging (FCM) hacia la aplicación móvil desarrollada con .NET MAUI.

---

# 2. Objetivo

Definir el funcionamiento de la programación automática de notificaciones del sistema.

Los objetivos son:

- Detectar pagos próximos.
- Detectar pagos vencidos.
- Detectar situaciones de morosidad.
- Detectar morosidad crítica.
- Generar notificaciones automáticamente.
- Enviar notificaciones mediante FCM.
- Evitar notificaciones duplicadas.
- Registrar el historial de notificaciones.
- Controlar errores y reintentos.
- Mantener trazabilidad.
- Respetar la zona horaria de Perú.

---

# 3. Alcance

Este documento contempla:

- Scheduler del backend.
- Tareas programadas.
- Evaluación de préstamos.
- Evaluación de pagos.
- Detección de vencimientos.
- Detección de morosidad.
- Generación de notificaciones.
- Envío mediante FCM.
- Control de duplicados.
- Reintentos.
- Registro de resultados.
- Auditoría.

La configuración específica de Firebase se encuentra en:

```text
07-Notificaciones/01-Firebase-FCM.md
```

---

# 4. Arquitectura

El proceso tendrá la siguiente arquitectura:

```text
┌───────────────────────────┐
│ Scheduler ASP.NET Core    │
│          C#               │
└─────────────┬─────────────┘
              │
              ▼
┌───────────────────────────┐
│       Backend             │
│    ASP.NET Core Web API   │
└─────────────┬─────────────┘
              │
              ▼
┌───────────────────────────┐
│        PostgreSQL         │
│ Préstamos / Pagos / Mora  │
└─────────────┬─────────────┘
              │
              ▼
┌───────────────────────────┐
│ Generación de            │
│ notificaciones            │
└─────────────┬─────────────┘
              │
              ▼
┌───────────────────────────┐
│ Firebase Cloud Messaging  │
│           FCM             │
└─────────────┬─────────────┘
              │
              ▼
┌───────────────────────────┐
│       .NET MAUI           │
│        Aplicación móvil   │
└───────────────────────────┘
```

---

# 5. Tecnologías

La programación utilizará:

| Componente | Tecnología |
|---|---|
| Backend | ASP.NET Core Web API |
| Lenguaje | C# |
| ORM | Entity Framework Core |
| Base de datos | PostgreSQL |
| Notificaciones | Firebase Cloud Messaging |
| Aplicación móvil | .NET MAUI |
| Comunicación | HTTPS |

---

# 6. Zona horaria

Todas las tareas relacionadas con fechas y horarios deberán utilizar:

```text
America/Lima
```

Esto corresponde a la zona horaria de Perú.

La zona horaria deberá utilizarse para:

- Fechas de vencimiento.
- Programación de tareas.
- Fechas de pago.
- Morosidad.
- Registro de eventos.
- Historial de notificaciones.

---

# 7. Horarios de ejecución

Los horarios iniciales serán (job de notificaciones; el job de morosidad corre separado a las `00:05`):

```text
08:00
16:00
```

Estos horarios serán administrados por el backend.

Ejemplo:

```text
08:00
 │
 └── Evaluación automática

16:00
 │
 └── Evaluación automática
```

Los horarios podrán modificarse posteriormente desde la configuración del sistema por un usuario autorizado.

---

# 8. Tipos de notificaciones programadas

Las principales notificaciones serán:

| Tipo | Código |
|---|---|
| Recordatorio de pago | PAYMENT_REMINDER |
| Pago vencido | PAYMENT_OVERDUE |
| Morosidad | DELINQUENCY |
| Morosidad crítica | CRITICAL_DELINQUENCY |

---

# 9. Notificaciones por evento

No todas las notificaciones dependerán del scheduler.

Algunas serán generadas inmediatamente cuando ocurra un evento.

Ejemplos:

```text
Pago registrado
       │
       ▼
PAYMENT_REGISTERED
```

```text
Préstamo reactivado
       │
       ▼
LOAN_REACTIVATED
```

Estas notificaciones podrán enviarse inmediatamente mediante FCM.

---

# 10. Scheduler

El scheduler será el componente encargado de ejecutar automáticamente las tareas.

Su responsabilidad será:

1. Ejecutar en el horario configurado.
2. Consultar préstamos activos.
3. Evaluar pagos.
4. Detectar vencimientos.
5. Detectar morosidad.
6. Generar notificaciones.
7. Enviar notificaciones.
8. Registrar resultados.

---

# 11. Flujo general

```text
Inicio
   │
   ▼
Scheduler ejecuta tarea
   │
   ▼
Consultar préstamos activos
   │
   ▼
Evaluar pagos
   │
   ├───────────────┐
   │               │
   ▼               ▼
Pago próximo    Pago vencido
   │               │
   ▼               ▼
Recordatorio     Alerta
   │               │
   └───────┬───────┘
           │
           ▼
    Evaluar morosidad
           │
           ▼
¿Más de 2 pagos vencidos?
           │
      ┌────┴────┐
      │         │
     Sí         No
      │         │
      ▼         ▼
Morosidad     Continuar
crítica
      │
      └────┬────┘
           │
           ▼
Generar notificaciones
           │
           ▼
Enviar mediante FCM
           │
           ▼
Registrar resultado
           │
           ▼
Fin
```

---

# 12. Evaluación de préstamos

El backend deberá consultar los préstamos que se encuentren activos.

Para cada préstamo se deberá evaluar:

```text
Capital inicial
Fecha de inicio
Estado
Próximo vencimiento
Interés semanal
Pagos realizados
Saldo pendiente
```

La información será obtenida desde PostgreSQL mediante Entity Framework Core.

---

# 13. Regla de interés

La regla definida para el proyecto es:

```text
Interés semanal = Capital inicial × 5%
```

Ejemplo:

```text
Capital inicial:
S/ 1,000

Interés:
5%

Interés semanal:
S/ 50
```

El interés se calcula sobre el capital inicial y no sobre el saldo restante.

---

# 14. Interés no compuesto

El interés será no compuesto.

Esto significa que los intereses pendientes no se sumarán al capital para generar nuevos intereses.

Ejemplo:

```text
Capital inicial:
S/ 1,000

Interés semanal:
S/ 50

Semana 1:
S/ 50

Semana 2:
S/ 50

Semana 3:
S/ 50
```

El sistema no deberá calcular:

```text
S/ 1,000 + interés acumulado
```

como nuevo capital para el cálculo del interés.

---

# 15. Frecuencia semanal

La frecuencia establecida es semanal.

Si un préstamo comienza el lunes:

```text
Inicio:
Lunes 01/09/2026

Primer vencimiento:
Lunes 08/09/2026
```

El siguiente vencimiento se calculará agregando siete días.

---

# 16. Recordatorio de pago

Cuando el sistema detecte un próximo vencimiento, podrá generar:

```text
PAYMENT_REMINDER
```

Ejemplo:

```text
Título:
Recordatorio de pago

Mensaje:
El cliente Juan Pérez tiene un pago próximo.
```

La información mostrada deberá proceder del backend.

---

# 17. Pago vencido

Un pago será considerado vencido cuando:

```text
Fecha actual > Fecha de vencimiento
```

y exista un monto pendiente.

El sistema deberá verificar el estado real del pago antes de generar la notificación.

---

# 18. Notificación de pago vencido

Cuando exista un pago vencido:

```text
PAYMENT_OVERDUE
```

Ejemplo:

```text
Título:
Pago vencido

Mensaje:
El cliente Juan Pérez tiene un pago pendiente.
```

La notificación deberá permitir identificar el préstamo relacionado.

---

# 19. Detección de morosidad

El backend deberá contar los pagos de interés vencidos.

Ejemplo:

```text
Cliente:
Juan Pérez

Pagos vencidos:
2

Estado:
Mora
```

---

# 20. Morosidad crítica

La regla establecida es:

```text
Pagos de interés vencidos > 2
```

Por lo tanto:

```text
0 pagos → Al día

1 pago → Mora

2 pagos → Mora

3 o más pagos → Morosidad crítica
```

Cuando se alcance la condición crítica:

```text
CRITICAL_DELINQUENCY
```

---

# 21. Reactivación

El sistema contempla la posibilidad de reactivar un préstamo después de una situación de morosidad.

La reactivación deberá ser realizada por un usuario autorizado.

La operación deberá registrar:

```text
Usuario
Fecha
Hora
Cliente
Préstamo
Estado anterior
Nuevo estado
Observación
```

Después de la reactivación podrá generarse:

```text
LOAN_REACTIVATED
```

---

# 22. Aplicación de pagos

La lógica de pagos establece que el pago se aplica primero al interés.

Posteriormente, si existe un excedente, se aplica al capital.

Ejemplo:

```text
Interés pendiente:
S/ 50

Pago realizado:
S/ 100
```

Aplicación:

```text
S/ 50 → Interés
S/ 50 → Capital
```

El scheduler deberá utilizar la información real registrada en el sistema.

---

# 23. Backend como fuente de verdad

El backend será responsable de:

- Calcular intereses.
- Determinar vencimientos.
- Determinar saldos.
- Determinar morosidad.
- Validar pagos.
- Determinar estados.
- Generar notificaciones.

La aplicación .NET MAUI no deberá realizar estas decisiones de manera independiente.

---

# 24. Prevención de duplicados

El sistema deberá evitar que la misma notificación sea enviada varias veces para el mismo evento.

Ejemplo:

```text
08:00
 │
 └── PAYMENT_REMINDER enviado

16:00
 │
 └── Verifica notificación existente
        │
        └── No vuelve a enviar
```

Para controlar los duplicados se deberá utilizar un identificador único de la notificación.

---

# 25. Identificador de notificación

Se podrá utilizar una clave conceptual como:

```text
ClienteId
+
PrestamoId
+
Tipo
+
FechaVencimiento
+
FechaProgramada
```

Ejemplo:

```text
48-125-PAYMENT_REMINDER-15/09/2026-08:00
```

La base de datos deberá garantizar que no se creen duplicados equivalentes.

---

# 26. Idempotencia

Las tareas programadas deberán ser idempotentes.

Esto significa que una ejecución repetida no deberá generar múltiples notificaciones iguales.

Ejemplo:

```text
Ejecución 1
   │
   └── Genera notificación

Ejecución 2
   │
   └── Detecta notificación existente
           │
           └── No duplica
```

---

# 27. Concurrencia

El backend deberá controlar la ejecución simultánea de tareas.

Esto es importante si en el futuro existen varios servidores ejecutando la aplicación.

Se podrán utilizar mecanismos como:

- Locks.
- Transacciones.
- Restricciones únicas.
- Control de tareas.
- Identificadores de ejecución.

La implementación concreta dependerá de la arquitectura final del backend.

---

# 28. Cola de procesamiento

Si aumenta el volumen de notificaciones, podrá incorporarse una cola de procesamiento.

Arquitectura futura:

```text
Scheduler
    │
    ▼
Generación de tareas
    │
    ▼
Cola
    │
    ▼
Procesador
    │
    ▼
Firebase FCM
```

Esto permitirá separar la generación de notificaciones del proceso de envío.

---

# 29. Reintentos

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

# 30. Tokens inválidos

Cuando FCM informe que un token ya no es válido:

```text
Token inválido
      │
      ▼
Backend
      │
      ▼
Marcar dispositivo como inactivo
```

No se deberán realizar reintentos indefinidos.

---

# 31. Registro de notificaciones

Cada notificación deberá registrarse en PostgreSQL.

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

Entity Framework Core será utilizado para realizar las operaciones de persistencia.

---

# 32. Estados

Estados normativos (ver `04-Base-Datos/03 §15`): `PENDIENTE | ENVIADO | ENTREGADO | LEIDO | FALLIDO | CANCELADO`. Prohibidos `ENVIADA/FALLIDA/CANCELADA/PROCESANDO` como persistidos.

---

# 33. Registro de ejecución del scheduler

Se recomienda mantener información sobre cada ejecución.

Ejemplo:

```text
Fecha:
15/09/2026

Hora:
08:00

Préstamos evaluados:
125

Notificaciones generadas:
34

Notificaciones enviadas:
32

Notificaciones fallidas:
2
```

Esto permitirá monitorear el funcionamiento del sistema.

---

# 34. Auditoría

Las operaciones relacionadas con notificaciones deberán ser trazables.

Se deberá registrar:

- Fecha.
- Hora.
- Tarea ejecutada.
- Usuario cuando corresponda.
- Cliente.
- Préstamo.
- Tipo de notificación.
- Estado.
- Resultado.
- Error.

---

# 35. Configuración de horarios

Los horarios podrán almacenarse en PostgreSQL.

Ejemplo:

```text
HorarioNotificacion1:
08:00

HorarioNotificacion2:
16:00

ZonaHoraria:
America/Lima
```

Los valores deberán ser modificables únicamente por usuarios autorizados.

---

# 36. Configuración de parámetros

Podrán configurarse:

| Parámetro | Valor inicial |
|---|---|
| Hora 1 | 08:00 |
| Hora 2 | 16:00 |
| Zona horaria | America/Lima |
| Interés semanal | 5% |
| Frecuencia | Semanal |
| Umbral de morosidad crítica | Más de 2 pagos |
| Reintentos máximos | Configurable |

Los parámetros financieros deberán contar con control de autorización y auditoría.

---

# 37. Seguridad

El scheduler deberá ejecutarse en el backend.

La aplicación .NET MAUI no deberá contener:

- Credenciales del backend.
- Claves privadas.
- Secretos de Firebase.
- Credenciales administrativas.

Las comunicaciones deberán realizarse mediante:

```text
HTTPS
```

---

# 38. Manejo de errores

Un error en una notificación no deberá detener todo el proceso.

Ejemplo:

```text
Préstamo 1 → OK
Préstamo 2 → OK
Préstamo 3 → ERROR
Préstamo 4 → OK
Préstamo 5 → OK
```

El préstamo 3 deberá registrarse como error y los demás deberán continuar procesándose.

---

# 39. Monitoreo

El sistema deberá permitir controlar:

- Última ejecución del scheduler.
- Ejecuciones exitosas.
- Ejecuciones fallidas.
- Notificaciones generadas.
- Notificaciones enviadas.
- Notificaciones fallidas.
- Cantidad de reintentos.
- Tokens inválidos.
- Tiempo de procesamiento.

---

# 40. Pruebas

## Caso 1: Recordatorio

**Dado:**

- Préstamo activo.
- Pago próximo.
- No existe notificación equivalente.

**Resultado esperado:**

```text
Se genera PAYMENT_REMINDER.
```

---

## Caso 2: Pago vencido

**Dado:**

- Pago pendiente.
- Fecha de vencimiento superada.

**Resultado esperado:**

```text
Se genera PAYMENT_OVERDUE.
```

---

## Caso 3: Morosidad

**Dado:**

- Cliente con pagos de interés vencidos.

**Resultado esperado:**

```text
Se genera DELINQUENCY.
```

---

## Caso 4: Morosidad crítica

**Dado:**

- Cliente con 3 o más pagos de interés vencidos.

**Resultado esperado:**

```text
Se genera CRITICAL_DELINQUENCY.
```

---

## Caso 5: Duplicados

**Dado:**

- La misma tarea se ejecuta dos veces.

**Resultado esperado:**

```text
No se generan notificaciones duplicadas.
```

---

## Caso 6: Token inválido

**Dado:**

- FCM devuelve token inválido.

**Resultado esperado:**

```text
El dispositivo queda marcado como inactivo.
```

---

## Caso 7: Error temporal

**Dado:**

- FCM presenta un error temporal.

**Resultado esperado:**

```text
Se realiza un reintento.
```

---

## Caso 8: Cambio de horario

**Dado:**

- Un usuario autorizado cambia el horario.

**Resultado esperado:**

```text
Las próximas ejecuciones utilizan el nuevo horario.
```

---

# 41. Criterios de aceptación

La programación de notificaciones será considerada correctamente implementada cuando:

- [ ] El scheduler se ejecute automáticamente.
- [ ] Se utilice `America/Lima`.
- [ ] Se ejecuten inicialmente las tareas a las 08:00 y 16:00.
- [ ] Se consulten los préstamos activos.
- [ ] Se detecten pagos próximos.
- [ ] Se detecten pagos vencidos.
- [ ] Se detecte morosidad.
- [ ] Se detecte morosidad crítica con más de 2 pagos vencidos.
- [ ] Se generen notificaciones mediante FCM.
- [ ] Se eviten notificaciones duplicadas.
- [ ] Se registren las notificaciones en PostgreSQL.
- [ ] Se controlen errores.
- [ ] Se realicen reintentos cuando corresponda.
- [ ] Se controlen tokens inválidos.
- [ ] Se registre la ejecución del scheduler.
- [ ] Se mantenga auditoría.
- [ ] Los horarios sean configurables.
- [ ] El backend sea responsable de las reglas financieras.
- [ ] La aplicación .NET MAUI no sea responsable de calcular la morosidad.

---

# 42. Dependencias

Este módulo depende de:

```text
02-Reglas-Negocio/
├── 02-Reglas-Prestamos.md
├── 03-Reglas-Pagos.md
├── 04-Reglas-Morosidad.md
└── 05-Reglas-Notificaciones.md

03-Arquitectura/
├── 01-Arquitectura-General.md
├── 02-Stack-Tecnologico.md
└── 03-Arquitectura-Backend.md

04-Base-Datos/

05-API/

07-Notificaciones/
└── 01-Firebase-FCM.md
```

---

# 43. Consideraciones futuras

En futuras versiones se podrá incorporar:

- Cola de mensajes.
- Procesamiento distribuido.
- Diferentes horarios por tipo de notificación.
- Resúmenes diarios.
- Resúmenes semanales.
- Priorización de notificaciones.
- Panel de monitoreo.
- Estadísticas de entrega.
- Preferencias de usuario.
- Escalamiento de alertas.
- Integración con WhatsApp.

---

# 44. Resumen

El sistema utilizará un scheduler en el backend ASP.NET Core para evaluar automáticamente los préstamos y pagos.

La arquitectura será:

```text
             Scheduler
                 │
                 ▼
       ASP.NET Core Web API
                 │
                 ▼
            PostgreSQL
                 │
                 ▼
       Reglas de negocio
                 │
                 ▼
       Generación de alerta
                 │
                 ▼
             FCM
                 │
                 ▼
            .NET MAUI
                 │
                 ▼
        Dispositivo móvil
```

El backend será responsable de determinar cuándo corresponde una notificación.

Firebase Cloud Messaging será responsable de entregar la notificación.

La aplicación .NET MAUI será responsable de recibirla y mostrarla al usuario.

Las tareas programadas se ejecutarán inicialmente a las:

```text
08:00
16:00
```

utilizando la zona horaria:

```text
America/Lima
```

Las reglas financieras continuarán siendo responsabilidad exclusiva del backend.