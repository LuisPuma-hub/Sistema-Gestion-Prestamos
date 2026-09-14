# Wireframe - Notificaciones

## 1. Introducción

Este documento define el wireframe del módulo de Notificaciones del Sistema de Gestión de Préstamos.

El módulo permitirá gestionar, visualizar y consultar las notificaciones relacionadas con préstamos, pagos, vencimientos y situaciones de morosidad.

El sistema deberá generar recordatorios automáticos para facilitar el seguimiento de los pagos de los clientes.

Las notificaciones podrán ser enviadas a los dispositivos móviles mediante Firebase Cloud Messaging (FCM).

Según las reglas definidas para el sistema, las notificaciones podrán ejecutarse en horarios establecidos por el administrador, inicialmente a las:

- 8:00 a. m.
- 4:00 p. m.

La programación deberá funcionar incluso cuando la aplicación móvil se encuentre cerrada.

---

## 2. Objetivo de la Pantalla

La pantalla de Notificaciones deberá permitir:

- Visualizar las notificaciones recibidas.
- Identificar notificaciones pendientes de lectura.
- Consultar recordatorios de pago.
- Visualizar próximos vencimientos.
- Identificar pagos atrasados.
- Consultar notificaciones relacionadas con morosidad.
- Acceder al préstamo relacionado.
- Marcar notificaciones como leídas.
- Configurar preferencias de notificaciones.
- Consultar el historial de notificaciones.

---

## 3. Tipos de Notificaciones

El sistema podrá manejar inicialmente los siguientes tipos:

### Recordatorio de pago

Se enviará cuando un cliente tenga un pago próximo.

Ejemplo:

```text
Recordatorio de pago

El cliente Carlos Quispe tiene un pago programado próximamente.
```

---

### Pago próximo a vencer

Ejemplo:

```text
Pago próximo

El préstamo #0001 vence próximamente.
```

---

### Pago vencido

Ejemplo:

```text
Pago vencido

El cliente tiene un pago pendiente.
```

---

### Morosidad

Ejemplo:

```text
Cliente en morosidad

El préstamo #0001 presenta más de 2 pagos atrasados.
```

---

### Pago registrado

Ejemplo:

```text
Pago registrado correctamente.

Monto recibido: S/ 100.00
```

---

### Préstamo finalizado

Ejemplo:

```text
Préstamo finalizado.

El préstamo #0001 ha sido cancelado correctamente.
```

---

## 4. Flujo General de Notificaciones

El proceso general será:

```text
EVENTO DEL SISTEMA
        │
        ▼
IDENTIFICAR TIPO DE EVENTO
        │
        ├── Pago próximo
        │
        ├── Pago vencido
        │
        ├── Morosidad
        │
        ├── Pago registrado
        │
        └── Préstamo finalizado
        │
        ▼
GENERAR NOTIFICACIÓN
        │
        ▼
ENVIAR MEDIANTE FCM
        │
        ▼
DISPOSITIVO DEL USUARIO
        │
        ▼
USUARIO ABRE NOTIFICACIÓN
        │
        ▼
ACCEDER AL MÓDULO RELACIONADO
```

---

## 5. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←        NOTIFICACIONES        ⚙    │
├─────────────────────────────────────┤
│                                     │
│ NOTIFICACIONES                      │
│                                     │
│ [ Todas ] [ No leídas ]             │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ 🔴 Pago próximo                     │
│ Carlos Quispe                       │
│ Préstamo #0001 vence mañana.        │
│ Hace 10 minutos                >    │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ 🟠 Pago vencido                     │
│ María Flores                        │
│ Tiene un pago pendiente.            │
│ Hace 1 hora                    >    │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ ⚠ Cliente en morosidad              │
│ Carlos Quispe                       │
│ Tiene 3 pagos atrasados.            │
│ Hace 2 horas                   >    │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ ✓ Pago registrado                   │
│ Pago de S/ 100.00 registrado.       │
│ Ayer                          >     │
│                                     │
└─────────────────────────────────────┘
```

---

## 6. Lista de Notificaciones

Cada notificación mostrará información resumida.

```text
┌─────────────────────────────────┐
│ 🔔 TIPO DE NOTIFICACIÓN          │
│                                 │
│ Título                           │
│                                 │
│ Descripción de la notificación   │
│                                 │
│ Hace 10 minutos             >   │
└─────────────────────────────────┘
```

La información principal será:

- Tipo de notificación.
- Título.
- Mensaje.
- Fecha.
- Estado de lectura.
- Acceso al elemento relacionado.

---

## 7. Notificaciones No Leídas

Las notificaciones pendientes deberán diferenciarse visualmente.

Ejemplo:

```text
● Pago próximo

Carlos Quispe tiene un pago programado mañana.
```

El indicador:

```text
●
```

representará una notificación pendiente de lectura.

Cuando el usuario abra la notificación:

```text
NOTIFICACIÓN NO LEÍDA
        │
        ▼
USUARIO LA ABRE
        │
        ▼
MARCAR COMO LEÍDA
```

---

## 8. Filtros

El usuario podrá filtrar las notificaciones.

```text
[ TODAS ]

[ NO LEÍDAS ]

[ LEÍDAS ]
```

También podrán existir filtros por tipo:

```text
[ PAGOS ]

[ PRÉSTAMOS ]

[ MOROSIDAD ]

[ SISTEMA ]
```

---

## 9. Detalle de Notificación

Al seleccionar una notificación:

```text
LISTA DE NOTIFICACIONES
        │
        ▼
SELECCIONAR NOTIFICACIÓN
        │
        ▼
DETALLE
```

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←       DETALLE NOTIFICACIÓN        │
├─────────────────────────────────────┤
│                                     │
│ 🔔 PAGO PRÓXIMO                     │
│                                     │
│ Carlos Quispe                       │
│                                     │
│ Préstamo #0001                      │
│                                     │
│ El cliente tiene un pago programado │
│ para mañana.                        │
│                                     │
│ Fecha programada                    │
│ 25/08/2026                          │
│                                     │
│                                 │
│ [ VER PRÉSTAMO ]                    │
│                                     │
│ [ REGISTRAR PAGO ]                  │
└─────────────────────────────────────┘
```

---

## 10. Acceso al Préstamo

Desde una notificación relacionada con un préstamo:

```text
NOTIFICACIÓN
        │
        ▼
[ VER PRÉSTAMO ]
        │
        ▼
DETALLE DEL PRÉSTAMO
```

Esto permitirá al usuario consultar:

- Cliente.
- Capital pendiente.
- Intereses pendientes.
- Próxima fecha de pago.
- Historial de pagos.
- Estado del préstamo.

---

## 11. Recordatorios Automáticos

El sistema deberá generar recordatorios relacionados con los pagos.

Ejemplo:

```text
PRÉSTAMO ACTIVO
        │
        ▼
IDENTIFICAR PRÓXIMO PAGO
        │
        ▼
¿DEBE GENERARSE RECORDATORIO?
        │
        ├── NO
        │
        └── SÍ
             │
             ▼
      PROGRAMAR NOTIFICACIÓN
             │
             ▼
        HORA CONFIGURADA
             │
             ├── 08:00
             │
             └── 16:00
             │
             ▼
       ENVIAR NOTIFICACIÓN
```

---

## 12. Programación de Horarios

Inicialmente, el administrador podrá definir horarios para las notificaciones.

Wireframe:

```text
┌─────────────────────────────────────┐
│     PROGRAMACIÓN DE NOTIFICACIONES  │
├─────────────────────────────────────┤
│                                     │
│ HORARIO 1                           │
│                                     │
│ 🕗 08:00                             │
│                                     │
│ [ ACTIVADO ]                        │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ HORARIO 2                           │
│                                     │
│ 🕓 16:00                             │
│                                     │
│ [ ACTIVADO ]                        │
│                                     │
├─────────────────────────────────────┤
│                                     │
│         [ GUARDAR CAMBIOS ]         │
└─────────────────────────────────────┘
```

Los horarios podrán modificarse posteriormente según las necesidades del administrador.

---

## 13. Configuración de Notificaciones

Desde el botón:

```text
⚙
```

el usuario podrá acceder a la configuración.

```text
┌─────────────────────────────────────┐
│ ←     CONFIGURAR NOTIFICACIONES     │
├─────────────────────────────────────┤
│                                     │
│ Recordatorios de pago               │
│                            [ ON ]   │
│                                     │
│ Pagos vencidos                      │
│                            [ ON ]   │
│                                     │
│ Morosidad                           │
│                            [ ON ]   │
│                                     │
│ Pagos registrados                   │
│                            [ ON ]   │
│                                     │
│ Préstamos finalizados               │
│                            [ ON ]   │
│                                     │
├─────────────────────────────────────┤
│                                     │
│         [ GUARDAR CAMBIOS ]         │
└─────────────────────────────────────┘
```

---

## 14. Flujo de Configuración

```text
NOTIFICACIONES
        │
        ▼
CONFIGURACIÓN
        │
        ▼
MODIFICAR PREFERENCIAS
        │
        ├── Activar
        │
        └── Desactivar
        │
        ▼
GUARDAR CAMBIOS
        │
        ▼
ACTUALIZAR CONFIGURACIÓN
```

---

## 15. Historial de Notificaciones

El sistema deberá mantener un historial.

Ejemplo:

```text
HISTORIAL

────────────────────────────

25/08/2026

Pago próximo

Cliente:
Carlos Quispe

Estado:
LEÍDA

────────────────────────────

24/08/2026

Pago vencido

Cliente:
María Flores

Estado:
LEÍDA
```

El historial permitirá conocer las notificaciones generadas por el sistema.

---

## 16. Estado sin Notificaciones

Cuando no existan notificaciones:

```text
🔔

No tienes notificaciones.

Las nuevas notificaciones aparecerán aquí.
```

---

## 17. Estado de Carga

Mientras se obtiene la información:

```text
Cargando notificaciones...
```

La interfaz podrá utilizar:

- Indicador de carga.
- Skeleton loading.
- Lista temporal.

---

## 18. Estado de Error

Si ocurre un problema:

```text
No se pudieron cargar las notificaciones.

Verifique su conexión e intente nuevamente.

[ REINTENTAR ]
```

---

## 19. Permisos de Notificaciones

Cuando la aplicación se ejecute por primera vez o cuando sea necesario:

```text
┌─────────────────────────────────────┐
│                                     │
│         ACTIVAR NOTIFICACIONES      │
│                                     │
│ Recibe recordatorios sobre pagos,   │
│ préstamos y clientes morosos.       │
│                                     │
│ [ AHORA NO ]                        │
│                                     │
│ [ PERMITIR ]                        │
│                                     │
└─────────────────────────────────────┘
```

Si el usuario permite:

```text
PERMITIR
    │
    ▼
SOLICITAR PERMISO DEL SISTEMA
    │
    ├── ACEPTADO
    │      │
    │      ▼
    │   ACTIVAR NOTIFICACIONES
    │
    └── RECHAZADO
           │
           ▼
    MANTENER DESACTIVADAS
```

---

## 20. Notificación Relacionada con Morosidad

Ejemplo:

```text
┌─────────────────────────────────────┐
│ ⚠ CLIENTE EN MOROSIDAD              │
├─────────────────────────────────────┤
│                                     │
│ Carlos Quispe                       │
│                                     │
│ Préstamo #0001                      │
│                                     │
│ Pagos atrasados: 3                  │
│                                     │
│ Días de atraso: 10                  │
│                                     │
│                                     │
│ [ VER MOROSIDAD ]                   │
└─────────────────────────────────────┘
```

Flujo:

```text
NOTIFICACIÓN
        │
        ▼
[ VER MOROSIDAD ]
        │
        ▼
DETALLE DE MOROSIDAD
```

---

## 21. Notificación de Pago Registrado

Después de registrar correctamente un pago:

```text
✓ Pago registrado correctamente.

Cliente:
Carlos Quispe

Monto:
S/ 100.00

Aplicado a interés:
S/ 50.00

Aplicado a capital:
S/ 50.00
```

La notificación permitirá confirmar que la operación fue procesada correctamente.

---

## 22. Flujo de Navegación

```text
DASHBOARD
    │
    ▼
NOTIFICACIONES
    │
    ├── Todas
    │
    ├── No leídas
    │
    ├── Configuración
    │
    └── Seleccionar notificación
            │
            ▼
        Detalle
            │
            ├── Ver préstamo
            │
            ├── Registrar pago
            │
            └── Ver morosidad
```

---

## 23. Consideraciones de Experiencia de Usuario

La pantalla deberá:

1. Mostrar claramente las notificaciones más recientes.
2. Diferenciar las notificaciones no leídas.
3. Permitir acceder rápidamente al préstamo relacionado.
4. Permitir registrar pagos desde los recordatorios cuando corresponda.
5. Facilitar la configuración de preferencias.
6. Evitar mostrar información excesiva.
7. Mantener un historial de notificaciones.
8. Mostrar mensajes claros.
9. Informar correctamente cuando las notificaciones estén desactivadas.
10. Mantener consistencia visual con los demás módulos.

---

## 24. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│          NOTIFICACIONES        ⚙    │
├─────────────────────────────────────┤
│                                     │
│ [ TODAS ] [ NO LEÍDAS ]             │
│                                     │
├─────────────────────────────────────┤
│ 🔔 PAGO PRÓXIMO                     │
│ Carlos Quispe                       │
│ Préstamo #0001 vence mañana.        │
│ Hace 10 minutos                     │
├─────────────────────────────────────┤
│ ⚠ PAGO VENCIDO                      │
│ María Flores                        │
│ Tiene un pago pendiente.            │
│ Hace 1 hora                         │
├─────────────────────────────────────┤
│ ⚠ MOROSIDAD                         │
│ Carlos Quispe                       │
│ Tiene 3 pagos atrasados.            │
│ Hace 2 horas                        │
├─────────────────────────────────────┤
│ ✓ PAGO REGISTRADO                   │
│ S/ 100.00                           │
│ Ayer                                │
└─────────────────────────────────────┘
```

---

## 25. Consideraciones Finales

El módulo de Notificaciones será responsable de informar al administrador y usuarios autorizados sobre eventos importantes relacionados con los préstamos.

El sistema deberá permitir:

1. Generar recordatorios automáticos.
2. Informar sobre pagos próximos.
3. Informar sobre pagos vencidos.
4. Detectar situaciones de morosidad.
5. Mostrar pagos registrados.
6. Informar sobre préstamos finalizados.
7. Configurar preferencias de notificaciones.
8. Mantener un historial.
9. Permitir acceder directamente al elemento relacionado.
10. Ejecutar las notificaciones programadas en los horarios establecidos.

La integración técnica de este módulo será documentada posteriormente en:

```text
07-Notificaciones/
├── 01-Firebase-FCM.md
└── 02-Programacion-Notificaciones.md
```

El siguiente wireframe será:

`10-Wireframe-WhatsApp.md`

Este documento definirá la pantalla para gestionar la comunicación mediante WhatsApp, incluyendo el envío de recordatorios, mensajes personalizados, plantillas de Meta e historial de mensajería.