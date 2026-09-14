# Wireframe - Dashboard

## 1. Introducción

Este documento define el wireframe de la pantalla principal o Dashboard del Sistema de Gestión de Préstamos.

El Dashboard será la pantalla principal que verá el usuario después de iniciar sesión correctamente.

Su objetivo es mostrar un resumen general de la situación del negocio y proporcionar acceso rápido a las funciones más utilizadas del sistema.

La información mostrada podrá variar según el rol y los permisos del usuario autenticado.

---

## 2. Objetivo de la Pantalla

El Dashboard deberá permitir al usuario visualizar rápidamente información importante relacionada con:

- Clientes registrados.
- Préstamos activos.
- Préstamos pendientes.
- Préstamos vencidos.
- Capital prestado.
- Capital recuperado.
- Intereses generados.
- Pagos recientes.
- Clientes con retrasos.
- Accesos rápidos a las funciones principales.

El diseño deberá priorizar la información que permita al usuario conocer rápidamente el estado general de los préstamos.

---

## 3. Estructura General

La pantalla estará organizada en las siguientes secciones:

1. Barra superior.
2. Mensaje de bienvenida.
3. Resumen principal.
4. Indicadores financieros.
5. Accesos rápidos.
6. Actividad reciente.
7. Navegación principal.

---

## 4. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ☰                    🔔      👤    │
├─────────────────────────────────────┤
│                                     │
│ Hola, Juan                          │
│ Bienvenido al Sistema de Préstamos  │
│                                     │
├─────────────────────────────────────┤
│                                     │
│        RESUMEN GENERAL              │
│                                     │
│ ┌───────────────┐ ┌───────────────┐ │
│ │ CLIENTES      │ │ PRÉSTAMOS     │ │
│ │     120       │ │      45       │ │
│ └───────────────┘ └───────────────┘ │
│                                     │
│ ┌───────────────┐ ┌───────────────┐ │
│ │ PENDIENTES    │ │ VENCIDOS      │ │
│ │      5        │ │       8       │ │
│ └───────────────┘ └───────────────┘ │
│                                     │
├─────────────────────────────────────┤
│                                     │
│       RESUMEN FINANCIERO            │
│                                     │
│ Capital prestado                    │
│ S/ 50,000.00                        │
│                                     │
│ Capital recuperado                  │
│ S/ 30,000.00                        │
│                                     │
│ Intereses generados                 │
│ S/ 5,000.00                         │
│                                     │
├─────────────────────────────────────┤
│                                     │
│          ACCESOS RÁPIDOS            │
│                                     │
│   👤 Cliente      💰 Préstamo       │
│                                     │
│   💵 Pago         ⚠ Morosidad       │
│                                     │
├─────────────────────────────────────┤
│                                     │
│         ACTIVIDAD RECIENTE          │
│                                     │
│ Carlos Quispe                       │
│ Pago registrado - S/ 50.00          │
│                                     │
│ María Flores                        │
│ Nuevo préstamo registrado           │
│                                     │
├─────────────────────────────────────┤
│  🏠 Inicio  👥 Clientes  💰 Más     │
└─────────────────────────────────────┘
```

---

## 5. Barra Superior

La parte superior de la pantalla contendrá los principales controles globales.

```text
┌─────────────────────────────────────┐
│ ☰                    🔔      👤    │
└─────────────────────────────────────┘
```

Los elementos serán:

- Menú.
- Notificaciones.
- Perfil del usuario.

### 5.1 Menú

El botón de menú permitirá acceder a opciones adicionales de la aplicación.

Dependiendo del diseño final, podrá abrir:

- Un menú lateral.
- Una lista de opciones.
- Configuración.
- Módulos adicionales.

---

### 5.2 Notificaciones

El icono de notificaciones permitirá consultar:

- Recordatorios de pagos.
- Pagos próximos.
- Pagos vencidos.
- Notificaciones del sistema.
- Alertas relacionadas con préstamos.

Cuando existan notificaciones pendientes, podrá mostrarse un indicador visual.

---

### 5.3 Perfil

El icono de perfil permitirá acceder a:

- Información del usuario.
- Configuración personal.
- Cambio de contraseña.
- Cerrar sesión.

---

## 6. Mensaje de Bienvenida

Debajo de la barra superior se mostrará un mensaje personalizado.

Ejemplo:

```text
Hola, Juan

Bienvenido al Sistema de Gestión de Préstamos
```

El sistema podrá mostrar el nombre del usuario autenticado.

El objetivo es proporcionar una experiencia más personalizada.

---

## 7. Resumen General

Esta sección mostrará indicadores importantes del sistema.

Ejemplo:

```text
┌───────────────┐ ┌───────────────┐
│ CLIENTES      │ │ PRÉSTAMOS     │
│     120       │ │      45       │
└───────────────┘ └───────────────┘

┌───────────────┐ ┌───────────────┐
│ PENDIENTES    │ │ VENCIDOS      │
│      5        │ │       8       │
└───────────────┘ └───────────────┘
```

Los indicadores podrán ser seleccionables para acceder directamente al módulo correspondiente.

Por ejemplo:

```text
Usuario selecciona "CLIENTES"
        │
        ▼
Lista de clientes
```

---

## 8. Indicadores Principales

Inicialmente, el Dashboard mostrará los siguientes indicadores:

### 8.1 Total de Clientes

Cantidad total de clientes registrados en el sistema.

Ejemplo:

```text
CLIENTES

120
```

Al seleccionar este indicador, el usuario será dirigido al listado de clientes.

---

### 8.2 Préstamos Activos

Cantidad de préstamos que actualmente se encuentran activos.

Ejemplo:

```text
PRÉSTAMOS ACTIVOS

45
```

Al seleccionar este indicador, se mostrará la lista de préstamos activos.

---

### 8.3 Préstamos Pendientes

Cantidad de préstamos registrados que todavía requieren una acción o aprobación.

Ejemplo:

```text
PENDIENTES

5
```

---

### 8.4 Préstamos Vencidos

Cantidad de préstamos con pagos pendientes o vencidos según las reglas de negocio.

Ejemplo:

```text
VENCIDOS

8
```

Al seleccionar este indicador, el usuario será dirigido al módulo de morosidad o préstamos vencidos.

---

## 9. Resumen Financiero

Esta sección mostrará información económica importante.

```text
RESUMEN FINANCIERO

Capital prestado
S/ 50,000.00

Capital recuperado
S/ 30,000.00

Intereses generados
S/ 5,000.00
```

Los datos deberán calcularse a partir de la información registrada en el sistema.

El backend será responsable de proporcionar los valores consolidados.

---

## 10. Accesos Rápidos

Los accesos rápidos permitirán realizar las operaciones más frecuentes.

Propuesta inicial:

```text
┌─────────────────────────────────────┐
│          ACCESOS RÁPIDOS            │
│                                     │
│    👤              💰               │
│ Nuevo Cliente    Nuevo Préstamo      │
│                                     │
│    💵              ⚠                │
│ Registrar Pago    Ver Morosidad      │
└─────────────────────────────────────┘
```

Las acciones serán:

### Nuevo Cliente

Dirige al formulario de registro de cliente.

```text
Dashboard
    │
    ▼
Nuevo Cliente
```

---

### Nuevo Préstamo

Dirige al proceso de registro de un nuevo préstamo.

```text
Dashboard
    │
    ▼
Nuevo Préstamo
```

---

### Registrar Pago

Permite buscar un préstamo y registrar un nuevo pago.

```text
Dashboard
    │
    ▼
Registrar Pago
```

---

### Ver Morosidad

Permite visualizar préstamos con pagos atrasados.

```text
Dashboard
    │
    ▼
Morosidad
```

---

## 11. Actividad Reciente

Esta sección mostrará las últimas operaciones realizadas en el sistema.

Ejemplo:

```text
ACTIVIDAD RECIENTE

─────────────────────────────────

Carlos Quispe

Pago registrado
S/ 50.00

Hoy - 10:30 AM

─────────────────────────────────

María Flores

Nuevo préstamo registrado

Hoy - 09:15 AM

─────────────────────────────────

José Pérez

Cliente registrado

Ayer - 04:20 PM
```

Inicialmente podrán mostrarse las últimas operaciones.

El usuario podrá seleccionar una actividad para consultar más detalles.

---

## 12. Navegación Inferior

La aplicación podrá utilizar una barra de navegación inferior para las funciones principales.

Propuesta:

```text
┌─────────────────────────────────────┐
│                                     │
│   🏠          👥          ☰         │
│ Inicio      Clientes       Más       │
│                                     │
└─────────────────────────────────────┘
```

La navegación principal podría incluir:

- Inicio.
- Clientes.
- Préstamos.
- Más.

Dentro de la sección **Más** podrían encontrarse:

- Pagos.
- Morosidad.
- Notificaciones.
- WhatsApp.
- Perfil.
- Configuración.

La estructura definitiva podrá ajustarse durante el desarrollo visual.

---

## 13. Estados del Dashboard

El Dashboard deberá considerar diferentes estados.

### Estado de carga

Mientras se obtiene la información:

```text
Cargando información...
```

Podrán mostrarse indicadores visuales de carga.

---

### Estado sin información

Cuando todavía no existan registros:

```text
Aún no existen clientes registrados.
```

```text
Aún no existen préstamos registrados.
```

El sistema podrá mostrar acciones rápidas para crear el primer registro.

---

### Estado con información

Se mostrarán los indicadores, accesos rápidos y actividad reciente.

---

### Estado de error

Si ocurre un problema al obtener la información:

```text
No se pudo cargar la información.

[ REINTENTAR ]
```

---

## 14. Flujo de Navegación del Dashboard

```text
DASHBOARD
│
├── Indicadores
│   │
│   ├── Clientes
│   │       └── Lista de clientes
│   │
│   ├── Préstamos activos
│   │       └── Lista de préstamos
│   │
│   ├── Pendientes
│   │       └── Préstamos pendientes
│   │
│   └── Vencidos
│           └── Morosidad
│
├── Accesos rápidos
│   │
│   ├── Nuevo cliente
│   │
│   ├── Nuevo préstamo
│   │
│   ├── Registrar pago
│   │
│   └── Morosidad
│
├── Actividad reciente
│   │
│   └── Detalle de operación
│
├── Notificaciones
│
└── Perfil
```

---

## 15. Información Según el Rol

La información visible podrá variar según el rol del usuario.

### Administrador

Podrá visualizar:

- Información general.
- Clientes.
- Préstamos.
- Pagos.
- Morosidad.
- Usuarios.
- Configuración.
- Notificaciones.
- WhatsApp.

### Prestamista

Podrá visualizar las funciones relacionadas con:

- Clientes.
- Préstamos.
- Consultas.
- Pagos según permisos.

### Cobrador

Podrá visualizar principalmente:

- Clientes asignados o disponibles.
- Préstamos.
- Pagos.
- Morosidad según permisos.

El sistema deberá ocultar o deshabilitar las funciones no autorizadas.

---

## 16. Consideraciones de Experiencia de Usuario

El Dashboard deberá cumplir los siguientes principios:

1. Mostrar primero la información más importante.
2. Evitar una cantidad excesiva de información.
3. Permitir acceder rápidamente a las funciones más utilizadas.
4. Mantener indicadores fáciles de interpretar.
5. Utilizar textos claros.
6. Mostrar retroalimentación durante la carga.
7. Adaptarse a diferentes tamaños de pantalla.
8. Facilitar el acceso al registro de clientes.
9. Facilitar el acceso al registro de préstamos.
10. Facilitar el acceso al registro de pagos.

---

## 17. Prioridad de la Información

La información del Dashboard deberá organizarse según la siguiente prioridad:

### Prioridad alta

- Pagos vencidos.
- Clientes con retrasos.
- Préstamos pendientes.
- Notificaciones importantes.

### Prioridad media

- Préstamos activos.
- Capital prestado.
- Capital recuperado.
- Pagos recientes.

### Prioridad baja

- Información histórica.
- Estadísticas detalladas.
- Configuraciones adicionales.

---

## 18. Wireframe Simplificado

La estructura principal de la pantalla será:

```text
┌─────────────────────────────────────┐
│ MENU       NOTIFICACIONES    PERFIL │
├─────────────────────────────────────┤
│                                     │
│           BIENVENIDA                │
│                                     │
├─────────────────────────────────────┤
│                                     │
│         RESUMEN GENERAL             │
│                                     │
│   CLIENTES       PRÉSTAMOS          │
│                                     │
│   PENDIENTES     VENCIDOS           │
│                                     │
├─────────────────────────────────────┤
│                                     │
│        RESUMEN FINANCIERO           │
│                                     │
├─────────────────────────────────────┤
│                                     │
│         ACCESOS RÁPIDOS             │
│                                     │
│ NUEVO CLIENTE    NUEVO PRÉSTAMO     │
│                                     │
│ REGISTRAR PAGO   VER MOROSIDAD      │
│                                     │
├─────────────────────────────────────┤
│                                     │
│         ACTIVIDAD RECIENTE          │
│                                     │
├─────────────────────────────────────┤
│                                     │
│   INICIO       CLIENTES       MÁS   │
└─────────────────────────────────────┘
```

---

## 19. Consideraciones Finales

El Dashboard será el centro principal de navegación del Sistema de Gestión de Préstamos.

Su diseño deberá permitir que el usuario identifique rápidamente:

- La situación general del negocio.
- Los préstamos activos.
- Los préstamos pendientes.
- Los pagos vencidos.
- Las operaciones recientes.
- Las acciones que requieren atención.

Los wireframes posteriores deberán mantener coherencia visual y funcional con la estructura definida en este Dashboard.

El diseño visual definitivo, incluyendo colores, tipografías, iconos y estilos, será definido posteriormente durante la etapa de diseño de pantallas.