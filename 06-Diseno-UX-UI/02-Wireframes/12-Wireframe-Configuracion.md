# Wireframe - Configuración

## 1. Introducción

Este documento define el wireframe de la pantalla de Configuración del Sistema de Gestión de Préstamos.

El módulo permitirá al administrador gestionar las principales preferencias y parámetros de funcionamiento de la aplicación.

Desde esta pantalla se podrá acceder a configuraciones relacionadas con:

- Sistema.
- Préstamos.
- Pagos.
- Morosidad.
- Notificaciones.
- WhatsApp.
- Seguridad.
- Cuenta del usuario.

Las opciones disponibles dependerán del rol y permisos del usuario autenticado.

---

## 2. Objetivo de la Pantalla

La pantalla de Configuración deberá permitir:

- Consultar la configuración actual del sistema.
- Modificar parámetros permitidos.
- Configurar opciones de préstamos.
- Configurar opciones de pagos.
- Configurar notificaciones.
- Configurar integración de WhatsApp.
- Gestionar preferencias generales.
- Acceder a opciones de seguridad.
- Guardar cambios.
- Restaurar configuraciones cuando corresponda.

---

## 3. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←           CONFIGURACIÓN           │
├─────────────────────────────────────┤
│                                     │
│ GENERAL                             │
│                                     │
│ ⚙ Configuración general        >   │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ PRÉSTAMOS                           │
│                                     │
│ 💰 Configuración de préstamos  >   │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ PAGOS                               │
│                                     │
│ 💵 Configuración de pagos       >  │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ NOTIFICACIONES                      │
│                                     │
│ 🔔 Notificaciones               >   │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ WHATSAPP                            │
│                                     │
│ 💬 Configuración WhatsApp       >  │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ SEGURIDAD                           │
│                                     │
│ 🛡 Seguridad                    >   │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ CUENTA                              │
│                                     │
│ 👤 Perfil                       >   │
│                                     │
└─────────────────────────────────────┘
```

---

## 4. Configuración General

La sección de configuración general permitirá administrar parámetros básicos de la aplicación.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←      CONFIGURACIÓN GENERAL        │
├─────────────────────────────────────┤
│                                     │
│ Nombre del sistema                  │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Sistema Gestión Préstamos       │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Moneda                              │
│                                     │
│ [ Soles (S/) ▼ ]                    │
│                                     │
│ Zona horaria                        │
│                                     │
│ [ America/Lima ▼ ]                  │
│                                     │
│ Formato de fecha                    │
│                                     │
│ [ DD/MM/YYYY ▼ ]                    │
│                                     │
│          [ GUARDAR CAMBIOS ]        │
└─────────────────────────────────────┘
```

---

## 5. Configuración de Préstamos

Esta sección permitirá configurar parámetros relacionados con los préstamos.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←     CONFIGURACIÓN PRÉSTAMOS       │
├─────────────────────────────────────┤
│                                     │
│ Interés semanal                     │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 5                               │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Tipo de interés                     │
│                                     │
│ [ Porcentaje sobre capital inicial ]│
│                                     │
│ Frecuencia de pago                 │
│                                     │
│ [ Semanal ▼ ]                       │
│                                     │
│ Permitir múltiples préstamos        │
│                            [ ON ]   │
│                                     │
│          [ GUARDAR CAMBIOS ]        │
└─────────────────────────────────────┘
```

---

## 6. Interés del Préstamo

El sistema utilizará inicialmente una tasa de interés semanal del:

```text
5 %
```

El interés será calculado sobre el capital inicial del préstamo.

Ejemplo:

```text
Capital inicial:

S/ 1,000.00

Interés semanal:

5 %

Interés semanal:

S/ 50.00
```

El interés no será compuesto.

Es decir, el interés semanal no se calculará sobre el saldo acumulado de intereses, sino sobre el capital inicial definido para el préstamo.

---

## 7. Frecuencia de Pago

Inicialmente:

```text
[ SEMANAL ]
```

La frecuencia permitirá determinar cuándo debe realizarse el pago de interés.

Ejemplo:

```text
Inicio del préstamo:
Lunes 01/09/2026

Próximo pago:
Lunes 08/09/2026
```

---

## 8. Múltiples Préstamos

El sistema permitirá manejar múltiples préstamos por cliente.

Configuración:

```text
Permitir múltiples préstamos

[ ON ]
```

Si está activado:

```text
CLIENTE
   │
   ├── Préstamo #0001
   │
   ├── Préstamo #0002
   │
   └── Préstamo #0003
```

Cada préstamo deberá mantenerse como una operación independiente.

---

## 9. Configuración de Pagos

Esta sección permitirá configurar parámetros relacionados con el registro y distribución de pagos.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←       CONFIGURACIÓN DE PAGOS      │
├─────────────────────────────────────┤
│                                     │
│ Aplicación de pagos                 │
│                                     │
│ [ Interés primero ▼ ]               │
│                                     │
│ Permitir pagos parciales            │
│                            [ ON ]   │
│                                     │
│ Registrar comprobante               │
│                            [ ON ]   │
│                                     │
│ Confirmación antes de registrar     │
│                            [ ON ]   │
│                                     │
│          [ GUARDAR CAMBIOS ]        │
└─────────────────────────────────────┘
```

---

## 10. Distribución del Pago

Según las reglas del sistema, cuando un cliente realiza un pago:

```text
PAGO RECIBIDO
      │
      ▼
PAGAR INTERÉS PENDIENTE
      │
      ▼
¿SOBRA DINERO?
      │
      ├── NO
      │
      └── SÍ
           │
           ▼
      REDUCIR CAPITAL
```

Ejemplo:

```text
Pago recibido:

S/ 100.00

Interés pendiente:

S/ 50.00

Capital:

S/ 50.00
```

---

## 11. Configuración de Morosidad

La sección permitirá administrar parámetros relacionados con los atrasos.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←      CONFIGURACIÓN MOROSIDAD      │
├─────────────────────────────────────┤
│                                     │
│ Límite de atrasos                   │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 2                               │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Activar seguimiento automático      │
│                            [ ON ]   │
│                                     │
│ Permitir reactivación               │
│                            [ ON ]   │
│                                     │
│          [ GUARDAR CAMBIOS ]        │
└─────────────────────────────────────┘
```

---

## 12. Límite de Atrasos

Inicialmente se utilizará como referencia:

```text
Más de 2 pagos de interés atrasados
```

Cuando se supere este límite:

```text
PRÉSTAMO ATRASADO
       │
       ▼
CONTABILIZAR ATRASOS
       │
       ▼
¿SUPERÓ 2 ATRASOS?
       │
       ├── NO
       │
       └── SÍ
            │
            ▼
       MARCAR MOROSIDAD
            │
            ▼
       EVALUAR REACTIVACIÓN
```

---

## 13. Configuración de Notificaciones

Esta sección permitirá administrar las notificaciones automáticas.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←    CONFIGURACIÓN NOTIFICACIONES   │
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
├─────────────────────────────────────┤
│                                     │
│ HORARIOS                            │
│                                     │
│ Horario 1                           │
│ 🕗 08:00                             │
│                            [ ON ]   │
│                                     │
│ Horario 2                           │
│ 🕓 16:00                             │
│                            [ ON ]   │
│                                     │
│          [ GUARDAR CAMBIOS ]        │
└─────────────────────────────────────┘
```

---

## 14. Horarios de Notificaciones

Los horarios iniciales establecidos son:

```text
08:00 a. m.

04:00 p. m.
```

Estos horarios podrán ser modificados posteriormente por el usuario autorizado.

El sistema deberá ejecutar las notificaciones programadas independientemente de que la aplicación móvil se encuentre abierta o cerrada.

---

## 15. Configuración de WhatsApp

Esta sección permitirá administrar las preferencias relacionadas con WhatsApp.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←       CONFIGURACIÓN WHATSAPP      │
├─────────────────────────────────────┤
│                                     │
│ Integración                         │
│                            [ ON ]   │
│                                     │
│ Recordatorios automáticos            │
│                            [ ON ]   │
│                                     │
│ Mensajes de morosidad               │
│                            [ ON ]   │
│                                     │
│ Mensajes personalizados             │
│                            [ ON ]   │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ PLANTILLAS                          │
│                                     │
│ Gestionar plantillas            >   │
│                                     │
│ Historial de mensajes           >   │
│                                     │
│          [ GUARDAR CAMBIOS ]        │
└─────────────────────────────────────┘
```

---

## 16. Gestión de Plantillas

Desde:

```text
Gestionar plantillas
```

el usuario autorizado podrá consultar las plantillas disponibles.

Ejemplo:

```text
┌─────────────────────────────────────┐
│ ←          PLANTILLAS               │
├─────────────────────────────────────┤
│                                     │
│ Recordatorio de pago            >   │
│                                     │
│ Pago próximo                    >   │
│                                     │
│ Pago vencido                    >   │
│                                     │
│ Morosidad                       >   │
│                                     │
│ Mensaje personalizado            >  │
│                                     │
└─────────────────────────────────────┘
```

La administración técnica de las plantillas estará relacionada con la integración de Meta.

---

## 17. Configuración de Seguridad

Esta sección permitirá acceder a opciones relacionadas con la seguridad.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←           SEGURIDAD               │
├─────────────────────────────────────┤
│                                     │
│ Cambiar contraseña              >   │
│                                     │
│ Sesiones activas                >   │
│                                     │
│ Cerrar otras sesiones            >  │
│                                     │
│ Protección de cuenta            >   │
│                                     │
└─────────────────────────────────────┘
```

Las reglas de seguridad serán definidas en:

```text
09-Seguridad/
├── 01-Autenticacion.md
├── 02-Autorizacion.md
└── 03-Proteccion-Datos.md
```

---

## 18. Sesiones Activas

El sistema podrá mostrar las sesiones activas del usuario.

Ejemplo:

```text
SESIONES ACTIVAS

────────────────────────────

Dispositivo:
Android

Último acceso:
Hoy, 08:30 p. m.

Estado:
ACTUAL

────────────────────────────

Dispositivo:
Otro dispositivo

Último acceso:
Ayer, 06:20 p. m.

Estado:
ACTIVA
```

El usuario autorizado podrá cerrar sesiones cuando corresponda.

---

## 19. Restaurar Configuración

Cuando corresponda, podrá existir una opción para restaurar determinados parámetros.

```text
[ RESTAURAR VALORES PREDETERMINADOS ]
```

Antes de ejecutar:

```text
¿Desea restaurar los valores predeterminados?

Los cambios actuales de esta sección serán reemplazados.

[ CANCELAR ]

[ RESTAURAR ]
```

Esta opción deberá estar restringida a usuarios autorizados.

---

## 20. Guardar Cambios

Después de modificar una configuración:

```text
[ GUARDAR CAMBIOS ]
```

El sistema deberá validar la información antes de almacenarla.

Flujo:

```text
MODIFICAR CONFIGURACIÓN
        │
        ▼
GUARDAR CAMBIOS
        │
        ▼
VALIDAR INFORMACIÓN
        │
        ├── ERROR
        │     │
        │     ▼
        │ MOSTRAR VALIDACIÓN
        │
        └── CORRECTO
              │
              ▼
       GUARDAR CONFIGURACIÓN
              │
              ▼
       MOSTRAR CONFIRMACIÓN
```

---

## 21. Confirmación de Cambios

Después de guardar correctamente:

```text
✓ Configuración actualizada correctamente.
```

Los nuevos valores deberán aplicarse según el tipo de configuración modificada.

---

## 22. Error al Guardar

Si ocurre un error:

```text
No se pudo actualizar la configuración.

Verifique la información e intente nuevamente.

[ REINTENTAR ]
```

---

## 23. Control de Permisos

No todos los usuarios podrán modificar todas las configuraciones.

Flujo:

```text
USUARIO
   │
   ▼
ACCEDER A CONFIGURACIÓN
   │
   ▼
VERIFICAR ROL
   │
   ├── AUTORIZADO
   │       │
   │       ▼
   │   PERMITIR CAMBIOS
   │
   └── NO AUTORIZADO
           │
           ▼
      SOLO LECTURA
```

Las reglas detalladas de autorización se documentarán en:

```text
09-Seguridad/02-Autorizacion.md
```

---

## 24. Estado de Carga

Mientras se obtiene la configuración:

```text
Cargando configuración...
```

---

## 25. Estado de Error

Si no es posible obtener la configuración:

```text
No se pudo cargar la configuración.

Verifique su conexión e intente nuevamente.

[ REINTENTAR ]
```

---

## 26. Flujo General de Navegación

```text
DASHBOARD
    │
    ▼
CONFIGURACIÓN
    │
    ├── Configuración general
    │
    ├── Configuración de préstamos
    │
    ├── Configuración de pagos
    │
    ├── Configuración de morosidad
    │
    ├── Configuración de notificaciones
    │
    ├── Configuración WhatsApp
    │
    ├── Seguridad
    │
    └── Perfil
```

---

## 27. Flujo de Modificación

```text
CONFIGURACIÓN
      │
      ▼
SELECCIONAR SECCIÓN
      │
      ▼
VISUALIZAR VALORES
      │
      ▼
MODIFICAR
      │
      ▼
VALIDAR PERMISOS
      │
      ▼
VALIDAR DATOS
      │
      ▼
GUARDAR
      │
      ▼
CONFIRMACIÓN
```

---

## 28. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│            CONFIGURACIÓN            │
├─────────────────────────────────────┤
│                                     │
│ ⚙ Configuración general        >   │
│                                     │
│ 💰 Préstamos                    >   │
│                                     │
│ 💵 Pagos                        >   │
│                                     │
│ ⚠ Morosidad                     >   │
│                                     │
│ 🔔 Notificaciones               >   │
│                                     │
│ 💬 WhatsApp                     >   │
│                                     │
│ 🛡 Seguridad                    >   │
│                                     │
│ 👤 Perfil                       >   │
│                                     │
└─────────────────────────────────────┘
```

---

## 29. Consideraciones de Experiencia de Usuario

La pantalla deberá:

1. Organizar las configuraciones por categorías.
2. Evitar mostrar opciones innecesarias al usuario.
3. Diferenciar configuraciones modificables y de solo lectura.
4. Validar los cambios antes de guardarlos.
5. Mostrar confirmaciones claras.
6. Solicitar confirmación para acciones críticas.
7. Respetar los permisos del usuario.
8. Mantener una navegación sencilla.
9. Mantener coherencia visual con el resto de la aplicación.
10. Evitar modificaciones accidentales de parámetros financieros.

---

## 30. Consideraciones sobre Parámetros Financieros

Los parámetros financieros son especialmente importantes porque afectan directamente el cálculo de los préstamos.

Por ello, cualquier modificación relacionada con:

- Tasa de interés.
- Frecuencia de pago.
- Distribución de pagos.
- Límite de morosidad.
- Condiciones de préstamo.

deberá estar protegida mediante permisos adecuados.

Además, los cambios deberán quedar registrados para mantener trazabilidad.

---

## 31. Historial de Cambios de Configuración

El sistema podrá registrar modificaciones importantes.

Ejemplo:

```text
HISTORIAL DE CONFIGURACIÓN

────────────────────────────

18/08/2026 - 10:30

Usuario:
Administrador

Configuración:
Interés semanal

Valor anterior:
5 %

Nuevo valor:
6 %

────────────────────────────

18/08/2026 - 11:00

Usuario:
Administrador

Configuración:
Límite de morosidad

Valor anterior:
2

Nuevo valor:
3
```

Este historial permitirá conocer:

- Quién realizó el cambio.
- Qué configuración modificó.
- Valor anterior.
- Nuevo valor.
- Fecha.
- Hora.

---

## 32. Consideraciones Finales

El módulo de Configuración permitirá centralizar los parámetros principales del Sistema de Gestión de Préstamos.

El sistema deberá permitir:

1. Gestionar la configuración general.
2. Configurar parámetros de préstamos.
3. Configurar parámetros de pagos.
4. Configurar reglas relacionadas con morosidad.
5. Configurar notificaciones.
6. Gestionar preferencias de WhatsApp.
7. Acceder a opciones de seguridad.
8. Gestionar información del perfil.
9. Controlar los permisos de modificación.
10. Registrar cambios importantes de configuración.

Los parámetros financieros deberán contar con controles de autorización y trazabilidad debido a su impacto sobre los cálculos del sistema.

---

## 33. Estructura Final de Wireframes

Con este documento se completa la documentación de los wireframes principales:

```text
02-Wireframes/
│
├── 01-Wireframe-Login.md
├── 02-Wireframe-Dashboard.md
├── 03-Wireframe-Clientes.md
├── 04-Wireframe-Registro-Cliente.md
├── 05-Wireframe-Prestamos.md
├── 06-Wireframe-Registro-Prestamo.md
├── 07-Wireframe-Pagos.md
├── 08-Wireframe-Morosidad.md
├── 09-Wireframe-Notificaciones.md
├── 10-Wireframe-WhatsApp.md
├── 11-Wireframe-Perfil.md
└── 12-Wireframe-Configuracion.md
```

Todos estos wireframes servirán como referencia para la posterior implementación de las interfaces de la aplicación móvil.

El siguiente paso será desarrollar la documentación correspondiente a:

```text
06-Diseno-UX-UI/03-Pantallas/
```

donde se podrán definir con mayor detalle las pantallas finales, componentes, estados, navegación y comportamiento de la interfaz.