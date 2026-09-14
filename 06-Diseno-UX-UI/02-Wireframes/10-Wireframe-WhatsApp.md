# Wireframe - WhatsApp

## 1. Introducción

Este documento define el wireframe del módulo de WhatsApp del Sistema de Gestión de Préstamos.

Este módulo permitirá gestionar la comunicación con los clientes mediante WhatsApp, utilizando plantillas de mensajes previamente configuradas.

El sistema podrá utilizar WhatsApp para:

- Enviar recordatorios de pago.
- Informar sobre pagos próximos.
- Informar sobre pagos vencidos.
- Comunicar situaciones de morosidad.
- Enviar mensajes personalizados.
- Realizar campañas o comunicaciones informativas.
- Consultar el historial de mensajes enviados.

La integración técnica con la plataforma de Meta será documentada posteriormente en el módulo:

```text
08-WhatsApp/
├── 01-Integracion-Meta.md
├── 02-Plantillas.md
└── 03-Flujo-Mensajeria.md
```

---

## 2. Objetivo de la Pantalla

La pantalla de WhatsApp deberá permitir:

- Visualizar mensajes enviados.
- Buscar mensajes por cliente.
- Filtrar mensajes por fecha.
- Filtrar mensajes por estado.
- Enviar un nuevo mensaje.
- Seleccionar una plantilla.
- Personalizar variables del mensaje.
- Seleccionar uno o varios destinatarios cuando corresponda.
- Consultar el historial de mensajería.
- Acceder al cliente relacionado.
- Consultar el estado del envío.

---

## 3. Tipos de Mensajes

Inicialmente, el sistema podrá manejar los siguientes tipos de mensajes.

### Recordatorio de pago

Ejemplo:

```text
Hola {{nombre_cliente}}.

Le recordamos que tiene un pago pendiente.

Monto pendiente: S/ {{monto}}

Gracias.
```

---

### Pago próximo

```text
Hola {{nombre_cliente}}.

Le recordamos que su próximo pago está programado para:

{{fecha_pago}}

Monto: S/ {{monto}}
```

---

### Pago vencido

```text
Hola {{nombre_cliente}}.

Le informamos que tiene un pago pendiente.

Por favor, comuníquese con nosotros para regularizar su situación.
```

---

### Morosidad

```text
Hola {{nombre_cliente}}.

Su préstamo presenta pagos pendientes.

Por favor, comuníquese con nosotros para obtener mayor información.
```

---

### Mensaje personalizado

Permitirá enviar una comunicación utilizando una plantilla disponible.

---

## 4. Flujo General

El proceso de envío será:

```text
WHATSAPP
    │
    ▼
NUEVO MENSAJE
    │
    ▼
SELECCIONAR DESTINATARIO
    │
    ▼
SELECCIONAR PLANTILLA
    │
    ▼
CARGAR VARIABLES
    │
    ▼
REVISAR MENSAJE
    │
    ▼
CONFIRMAR ENVÍO
    │
    ▼
ENVIAR A WHATSAPP
    │
    ▼
REGISTRAR HISTORIAL
```

---

## 5. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←            WHATSAPP          ⚙    │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 🔍 Buscar cliente o mensaje...  │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ Todos ] [ Enviados ] [ Fallidos ] │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ HOY                                 │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 Carlos Quispe                │ │
│ │ Recordatorio de pago            │ │
│ │ ✓ Enviado                       │ │
│ │ 10:30 a. m.                >    │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 María Flores                 │ │
│ │ Pago próximo                    │ │
│ │ ✓ Enviado                       │ │
│ │ 09:15 a. m.                >    │ │
│ └─────────────────────────────────┘ │
│                                     │
│                            ┌─────┐  │
│                            │  +  │  │
│                            └─────┘  │
└─────────────────────────────────────┘
```

---

## 6. Barra de Búsqueda

La búsqueda permitirá localizar mensajes utilizando:

- Nombre del cliente.
- Apellidos.
- Número de documento.
- Número de teléfono.
- Tipo de mensaje.

Wireframe:

```text
┌─────────────────────────────────┐
│ 🔍 Buscar cliente o mensaje...  │
└─────────────────────────────────┘
```

---

## 7. Filtros

El usuario podrá filtrar los mensajes.

```text
[ TODOS ]

[ ENVIADOS ]

[ PENDIENTES ]

[ FALLIDOS ]
```

También podrán existir filtros por:

```text
[ FECHA ]

[ CLIENTE ]

[ TIPO DE MENSAJE ]
```

---

## 8. Lista de Mensajes

Cada registro mostrará:

```text
┌─────────────────────────────────┐
│ 👤 Nombre del cliente            │
│                                 │
│ Tipo de mensaje                  │
│                                 │
│ Estado: ENVIADO                  │
│                                 │
│ Fecha y hora                >    │
└─────────────────────────────────┘
```

Estados iniciales:

- Pendiente.
- Enviado.
- Entregado.
- Leído.
- Fallido.

La disponibilidad exacta de los estados dependerá de la información proporcionada por la integración.

---

## 9. Nuevo Mensaje

El usuario podrá iniciar un nuevo mensaje mediante:

```text
┌─────┐
│  +  │
└─────┘
```

Flujo:

```text
LISTA DE MENSAJES
        │
        ▼
BOTÓN "+"
        │
        ▼
NUEVO MENSAJE
```

---

## 10. Selección del Destinatario

El primer paso será seleccionar el cliente.

```text
┌─────────────────────────────────────┐
│ ←          NUEVO MENSAJE            │
├─────────────────────────────────────┤
│                                     │
│ SELECCIONAR CLIENTE                 │
│                                     │
│ 🔍 Buscar cliente                   │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Nombre o teléfono               │ │
│ └─────────────────────────────────┘ │
│                                     │
│ RESULTADOS                          │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 Carlos Quispe                │ │
│ │ 📱 999999999                    │ │
│ │                          [ + ]  │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 María Flores                 │ │
│ │ 📱 988888888                    │ │
│ │                          [ + ]  │ │
│ └─────────────────────────────────┘ │
└─────────────────────────────────────┘
```

---

## 11. Validación del Número de Teléfono

Antes de permitir el envío, el sistema deberá validar que el cliente tenga un número de teléfono registrado.

Flujo:

```text
SELECCIONAR CLIENTE
        │
        ▼
¿TIENE TELÉFONO?
        │
        ├── NO
        │    │
        │    ▼
        │ MOSTRAR MENSAJE
        │
        └── SÍ
             │
             ▼
      CONTINUAR PROCESO
```

Mensaje:

```text
El cliente seleccionado no tiene un número de teléfono registrado.
```

---

## 12. Selección de Plantilla

Después de seleccionar el cliente:

```text
┌─────────────────────────────────────┐
│ ←       SELECCIONAR PLANTILLA       │
├─────────────────────────────────────┤
│                                     │
│ TIPO DE MENSAJE                     │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Recordatorio de pago            │ │
│ │                         [ > ]   │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Pago próximo                    │ │
│ │                         [ > ]   │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Pago vencido                    │ │
│ │                         [ > ]   │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Mensaje personalizado           │ │
│ │                         [ > ]   │ │
│ └─────────────────────────────────┘ │
└─────────────────────────────────────┘
```

---

## 13. Vista Previa del Mensaje

El sistema mostrará una vista previa antes del envío.

```text
┌─────────────────────────────────────┐
│          VISTA PREVIA               │
├─────────────────────────────────────┤
│                                     │
│ PARA                                │
│ Carlos Quispe                       │
│ 999999999                           │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ Hola Carlos.                        │
│                                     │
│ Le recordamos que tiene un pago     │
│ programado.                         │
│                                     │
│ Monto: S/ 50.00                     │
│                                     │
│ Fecha: 25/08/2026                   │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ [ EDITAR ]          [ CONTINUAR ]   │
└─────────────────────────────────────┘
```

Las variables deberán ser reemplazadas automáticamente con la información correspondiente.

---

## 14. Variables de la Plantilla

Las plantillas podrán utilizar variables como:

```text
{{nombre_cliente}}

{{monto}}

{{fecha_pago}}

{{numero_prestamo}}

{{capital_pendiente}}
```

Ejemplo:

```text
PLANTILLA

Hola {{nombre_cliente}}.

Su próximo pago es el {{fecha_pago}}.

Monto: S/ {{monto}}
```

Resultado:

```text
Hola Carlos.

Su próximo pago es el 25/08/2026.

Monto: S/ 50.00
```

---

## 15. Confirmación del Envío

Antes de enviar:

```text
┌─────────────────────────────────────┐
│        CONFIRMAR ENVÍO              │
├─────────────────────────────────────┤
│                                     │
│ Cliente                             │
│ Carlos Quispe                       │
│                                     │
│ Teléfono                            │
│ 999999999                           │
│                                     │
│ Plantilla                           │
│ Recordatorio de pago                │
│                                     │
│ ¿Desea enviar este mensaje?         │
│                                     │
│ [ CANCELAR ]       [ ENVIAR ]       │
└─────────────────────────────────────┘
```

---

## 16. Proceso de Envío

Cuando el usuario confirme:

```text
[ ENVIAR ]
```

El sistema realizará:

```text
CONFIRMAR ENVÍO
        │
        ▼
VALIDAR INFORMACIÓN
        │
        ├── ERROR
        │     │
        │     ▼
        │ MOSTRAR MENSAJE
        │
        └── CORRECTO
              │
              ▼
      GENERAR MENSAJE
              │
              ▼
       ENVIAR A WHATSAPP
              │
              ▼
       RECIBIR RESPUESTA
              │
              ▼
      REGISTRAR HISTORIAL
              │
              ▼
       MOSTRAR RESULTADO
```

---

## 17. Mensaje Enviado Correctamente

Cuando el proceso sea exitoso:

```text
✓ Mensaje enviado correctamente.
```

El usuario podrá seleccionar:

```text
[ VER HISTORIAL ]

[ ENVIAR OTRO MENSAJE ]

[ VOLVER A WHATSAPP ]
```

---

## 18. Error en el Envío

Si ocurre un problema:

```text
No se pudo enviar el mensaje.

Verifique la información e intente nuevamente.

[ REINTENTAR ]
```

El error deberá quedar registrado para fines de seguimiento.

---

## 19. Detalle del Mensaje

Al seleccionar un mensaje del historial:

```text
┌─────────────────────────────────────┐
│ ←         DETALLE DEL MENSAJE       │
├─────────────────────────────────────┤
│                                     │
│ CLIENTE                             │
│ Carlos Quispe                       │
│                                     │
│ TELÉFONO                            │
│ 999999999                           │
│                                     │
│ TIPO                                │
│ Recordatorio de pago                │
│                                     │
│ ESTADO                              │
│ ENTREGADO                           │
│                                     │
│ FECHA                               │
│ 18/08/2026                          │
│                                     │
│ HORA                                │
│ 10:30 a. m.                         │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ MENSAJE                             │
│                                     │
│ Hola Carlos.                        │
│ Le recordamos que tiene un pago     │
│ pendiente.                          │
│                                     │
└─────────────────────────────────────┘
```

---

## 20. Historial de Mensajes por Cliente

Desde el detalle del cliente podrá visualizarse:

```text
HISTORIAL DE WHATSAPP

────────────────────────────

18/08/2026

Recordatorio de pago

Estado:
ENTREGADO

────────────────────────────

15/08/2026

Pago próximo

Estado:
LEÍDO

────────────────────────────

10/08/2026

Mensaje personalizado

Estado:
ENVIADO
```

---

## 21. Envío Automático de Recordatorios

El sistema podrá utilizar reglas automáticas.

```text
PRÉSTAMO ACTIVO
        │
        ▼
IDENTIFICAR PRÓXIMO PAGO
        │
        ▼
¿REQUIERE RECORDATORIO?
        │
        ├── NO
        │
        └── SÍ
             │
             ▼
       GENERAR MENSAJE
             │
             ▼
       SELECCIONAR PLANTILLA
             │
             ▼
       ENVIAR MENSAJE
             │
             ▼
      REGISTRAR RESULTADO
```

Los horarios y condiciones de envío podrán configurarse según las reglas del sistema.

---

## 22. Estado sin Mensajes

Cuando no existan registros:

```text
💬

No existen mensajes registrados.

Los mensajes enviados aparecerán aquí.
```

---

## 23. Estado de Carga

```text
Cargando mensajes...
```

---

## 24. Estado de Error

```text
No se pudieron cargar los mensajes.

Verifique su conexión e intente nuevamente.

[ REINTENTAR ]
```

---

## 25. Configuración

Desde el botón:

```text
⚙
```

se podrá acceder a configuraciones relacionadas con el módulo.

Ejemplo:

```text
┌─────────────────────────────────────┐
│ ←       CONFIGURACIÓN WHATSAPP      │
├─────────────────────────────────────┤
│                                     │
│ Envío automático                    │
│                            [ ON ]   │
│                                     │
│ Recordatorios de pago               │
│                            [ ON ]   │
│                                     │
│ Mensajes de morosidad               │
│                            [ ON ]   │
│                                     │
│ Registrar historial                 │
│                            [ ON ]   │
│                                     │
│       [ GUARDAR CAMBIOS ]           │
└─────────────────────────────────────┘
```

---

## 26. Flujo de Navegación

```text
DASHBOARD
    │
    ▼
WHATSAPP
    │
    ├── Buscar mensajes
    │
    ├── Filtrar mensajes
    │
    ├── Seleccionar mensaje
    │       │
    │       ▼
    │   Detalle del mensaje
    │
    └── Nuevo mensaje
            │
            ▼
      Seleccionar cliente
            │
            ▼
      Validar teléfono
            │
            ▼
      Seleccionar plantilla
            │
            ▼
      Generar variables
            │
            ▼
      Vista previa
            │
            ▼
      Confirmar envío
            │
            ▼
      Registrar historial
```

---

## 27. Consideraciones de Experiencia de Usuario

La pantalla deberá:

1. Permitir localizar rápidamente a un cliente.
2. Mostrar claramente el historial de mensajes.
3. Facilitar la selección de plantillas.
4. Generar automáticamente las variables del mensaje.
5. Mostrar una vista previa antes del envío.
6. Evitar envíos accidentales mediante una confirmación.
7. Mostrar claramente el estado de cada mensaje.
8. Registrar errores de envío.
9. Mantener coherencia visual con los demás módulos.
10. Facilitar el acceso al cliente o préstamo relacionado.

---

## 28. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│              WHATSAPP               │
├─────────────────────────────────────┤
│                                     │
│ 🔍 Buscar cliente o mensaje...      │
│                                     │
│ [ TODOS ] [ ENVIADOS ] [ FALLIDOS ] │
├─────────────────────────────────────┤
│                                     │
│ 👤 Carlos Quispe                    │
│ Recordatorio de pago                │
│ ✓ ENTREGADO                         │
│ Hoy 10:30 a. m.                     │
├─────────────────────────────────────┤
│                                     │
│ 👤 María Flores                     │
│ Pago próximo                        │
│ ✓ LEÍDO                             │
│ Hoy 09:15 a. m.                     │
├─────────────────────────────────────┤
│                                     │
│                            ┌─────┐  │
│                            │  +  │  │
│                            └─────┘  │
└─────────────────────────────────────┘
```

---

## 29. Consideraciones Finales

El módulo de WhatsApp permitirá centralizar la comunicación con los clientes dentro del Sistema de Gestión de Préstamos.

El sistema deberá permitir:

1. Enviar mensajes relacionados con préstamos.
2. Utilizar plantillas predefinidas.
3. Personalizar mensajes mediante variables.
4. Enviar recordatorios de pago.
5. Informar sobre pagos vencidos.
6. Comunicar situaciones de morosidad.
7. Consultar el historial de mensajes.
8. Visualizar el estado de los envíos.
9. Registrar errores.
10. Permitir automatizar determinados mensajes.

El siguiente wireframe será:

`11-Wireframe-Perfil.md`

Este documento definirá la pantalla del perfil del usuario, incluyendo información personal, datos de la cuenta, seguridad y opciones relacionadas con la sesión.