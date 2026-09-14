# Flujo de Navegación

## 1. Introducción

Este documento define la estructura de navegación de la aplicación móvil del Sistema de Gestión de Préstamos.

El objetivo es establecer cómo los usuarios se desplazarán entre las diferentes pantallas del sistema, considerando los módulos principales, las funciones disponibles y los permisos según el rol del usuario.

La navegación deberá ser simple, clara y orientada a facilitar las operaciones diarias de gestión de clientes, préstamos y pagos.

---

## 2. Usuarios del Sistema

Inicialmente, el sistema contará con los siguientes roles:

- Administrador.
- Prestamista.
- Cobrador.

Dependiendo de la configuración del sistema, un mismo usuario podrá tener más de un rol.

El administrador tendrá acceso a las funciones de gestión y configuración.

El prestamista podrá gestionar clientes y préstamos según los permisos asignados.

El cobrador podrá consultar clientes, préstamos y registrar pagos según los permisos asignados.

---

## 3. Flujo General de Navegación

El flujo principal de la aplicación será:

```text
Inicio de la aplicación
        │
        ▼
Verificación de sesión
        │
        ├── Sesión válida ───────────────► Dashboard
        │
        └── Sin sesión ──────────────────► Inicio de sesión
                                              │
                                              ▼
                                       Validación de usuario
                                              │
                                              ├── Error ───► Mostrar mensaje
                                              │
                                              └── Correcto
                                                   │
                                                   ▼
                                                Dashboard
```

La aplicación verificará automáticamente si existe una sesión válida.

Si el usuario ya tiene una sesión activa, será dirigido directamente al Dashboard.

Si no existe una sesión válida, deberá iniciar sesión.

---

## 4. Pantalla de Inicio de Sesión

La pantalla de inicio de sesión será el punto de acceso para usuarios no autenticados.

El flujo será:

```text
Inicio de sesión
      │
      ├── Ingresar correo electrónico
      │
      ├── Ingresar contraseña
      │
      ▼
Botón "Iniciar sesión"
      │
      ▼
Validación
      │
      ├── Datos incorrectos ──► Mostrar error
      │
      └── Datos correctos ────► Dashboard
```

Funciones principales:

- Ingresar correo electrónico.
- Ingresar contraseña.
- Mostrar u ocultar contraseña.
- Iniciar sesión.
- Mostrar mensajes de error.
- Mantener la sesión cuando corresponda.

Como funcionalidad futura, podrá incorporarse:

- Recuperación de contraseña.
- Cambio de contraseña.
- Cierre de sesiones en otros dispositivos.

---

## 5. Dashboard

Después de iniciar sesión correctamente, el usuario será dirigido al Dashboard.

El Dashboard mostrará información resumida del sistema.

Ejemplos de información:

- Total de clientes.
- Préstamos activos.
- Préstamos pendientes.
- Préstamos vencidos.
- Capital prestado.
- Capital recuperado.
- Pagos recientes.
- Clientes con retrasos.

El flujo será:

```text
Dashboard
│
├── Clientes
│
├── Préstamos
│
├── Pagos
│
├── Morosidad
│
├── Notificaciones
│
└── Perfil / Configuración
```

Las opciones visibles dependerán de los permisos del usuario autenticado.

---

## 6. Flujo del Módulo de Clientes

El módulo de clientes permitirá consultar y gestionar la información de los clientes.

```text
Dashboard
    │
    ▼
Clientes
    │
    ├── Lista de clientes
    │       │
    │       ├── Buscar cliente
    │       │
    │       ├── Filtrar clientes
    │       │
    │       └── Seleccionar cliente
    │               │
    │               ▼
    │         Detalle del cliente
    │               │
    │               ├── Editar información
    │               │
    │               ├── Ver préstamos
    │               │
    │               └── Ver historial
    │
    └── Registrar cliente
            │
            ├── Datos personales
            │
            ├── Dirección
            │
            ├── Documento
            │
            ├── Información del aval
            │
            ├── Subir recibo
            │
            ▼
        Guardar cliente
```

---

## 7. Flujo de Registro de Cliente

El registro de un cliente podrá realizarse mediante una secuencia organizada.

```text
Registrar cliente
        │
        ▼
Datos personales
        │
        ├── Tipo de documento
        ├── Número de documento
        ├── Nombres
        ├── Apellidos
        └── Teléfono
        │
        ▼
Dirección
        │
        ├── Dirección
        └── Referencia
        │
        ▼
Aval
        │
        ├── Seleccionar cliente existente
        │
        └── Registrar nuevo aval
                │
                ├── Nombres
                ├── Apellidos
                ├── Teléfono
                └── Dirección
        │
        ▼
Documento de domicilio
        │
        └── Subir imagen de recibo
        │
        ▼
Observaciones
        │
        ▼
Guardar cliente
```

El sistema deberá validar los campos obligatorios antes de permitir el registro.

---

## 8. Flujo del Módulo de Préstamos

El módulo permitirá registrar y consultar préstamos.

```text
Dashboard
    │
    ▼
Préstamos
    │
    ├── Lista de préstamos
    │       │
    │       ├── Filtrar por estado
    │       ├── Buscar cliente
    │       └── Seleccionar préstamo
    │               │
    │               ▼
    │         Detalle del préstamo
    │               │
    │               ├── Ver resumen financiero
    │               ├── Ver pagos
    │               ├── Ver cuotas
    │               └── Registrar pago
    │
    └── Nuevo préstamo
            │
            ▼
        Seleccionar cliente
            │
            ▼
        Ingresar monto
            │
            ▼
        Configurar condiciones
            │
            ▼
        Revisar información
            │
            ▼
        Registrar préstamo
            │
            ▼
        Pendiente / Activo
```

---

## 9. Flujo de Registro de Préstamo

El registro de un préstamo seguirá el siguiente proceso:

```text
Nuevo préstamo
      │
      ▼
Seleccionar cliente
      │
      ├── Cliente existente
      │
      └── Buscar cliente
              │
              ▼
        Seleccionar cliente
      │
      ▼
Ingresar monto del préstamo
      │
      ▼
Definir interés semanal
      │
      ▼
Definir fecha de desembolso
      │
      ▼
Agregar observaciones
      │
      ▼
Revisar resumen
      │
      ▼
Registrar solicitud
      │
      ▼
Préstamo pendiente
      │
      ▼
Aprobación
      │
      ▼
Préstamo activo
```

Un cliente podrá tener múltiples préstamos, siempre que las reglas de negocio lo permitan.

---

## 10. Flujo del Módulo de Pagos

El módulo de pagos permitirá registrar los pagos realizados por los clientes.

```text
Dashboard
    │
    ▼
Pagos
    │
    ├── Historial de pagos
    │
    └── Registrar pago
            │
            ▼
        Buscar préstamo
            │
            ▼
        Ver resumen financiero
            │
            ├── Capital pendiente
            ├── Interés pendiente
            └── Total pendiente
            │
            ▼
        Ingresar monto
            │
            ▼
        Seleccionar método de pago
            │
            ▼
        Confirmar pago
            │
            ▼
        Backend procesa pago
            │
            ├── Aplicar interés
            │
            └── Aplicar capital restante
            │
            ▼
        Actualizar préstamo
```

El usuario no realizará manualmente la distribución entre interés y capital.

El backend aplicará automáticamente las reglas financieras.

---

## 11. Flujo del Módulo de Morosidad

Este módulo permitirá identificar préstamos con pagos atrasados.

```text
Dashboard
    │
    ▼
Morosidad
    │
    ├── Lista de préstamos con retraso
    │
    ├── Buscar cliente
    │
    ├── Filtrar por días de atraso
    │
    └── Seleccionar préstamo
            │
            ▼
        Detalle de morosidad
            │
            ├── Días de atraso
            ├── Pagos pendientes
            ├── Historial
            │
            └── Acciones permitidas
```

Según las reglas del negocio, cuando un cliente acumule más de dos pagos de interés atrasados, podrá requerir un proceso de reactivación.

---

## 12. Flujo de Notificaciones

El sistema podrá generar notificaciones relacionadas con pagos y préstamos.

```text
Evento programado
        │
        ▼
Backend verifica préstamos
        │
        ├── Pago próximo
        │
        ├── Pago pendiente
        │
        └── Pago atrasado
        │
        ▼
Generar notificación
        │
        ▼
Firebase Cloud Messaging
        │
        ▼
Dispositivo móvil
```

Las notificaciones podrán ejecutarse en los horarios configurados por el administrador.

Inicialmente se consideran horarios como:

- 08:00.
- 16:00.

La configuración definitiva deberá poder modificarse desde el sistema.

---

## 13. Flujo de Mensajería por WhatsApp

La mensajería podrá utilizar plantillas previamente configuradas.

```text
Evento
    │
    ├── Pago próximo
    ├── Pago pendiente
    ├── Pago atrasado
    └── Mensaje manual
    │
    ▼
Seleccionar plantilla
    │
    ▼
Obtener datos del cliente
    │
    ▼
Personalizar variables
    │
    ▼
Enviar solicitud al Backend
    │
    ▼
API de WhatsApp
    │
    ▼
Cliente recibe mensaje
```

Los mensajes enviados deberán quedar registrados en el historial del sistema.

---

## 14. Flujo de Perfil

El usuario podrá acceder a su información personal.

```text
Dashboard
    │
    ▼
Perfil
    │
    ├── Ver información personal
    │
    ├── Editar información
    │
    ├── Cambiar contraseña
    │
    └── Cerrar sesión
```

---

## 15. Flujo de Configuración

Las opciones de configuración estarán disponibles principalmente para usuarios autorizados.

```text
Dashboard
    │
    ▼
Configuración
    │
    ├── Usuarios
    │
    ├── Roles y permisos
    │
    ├── Notificaciones
    │
    ├── WhatsApp
    │
    └── Configuración general
```

---

## 16. Navegación Principal Propuesta

La aplicación podrá utilizar una navegación principal mediante una barra inferior o un menú lateral, dependiendo del diseño final.

Una propuesta inicial es:

```text
┌──────────────────────────────────────┐
│              DASHBOARD               │
├──────────────────────────────────────┤
│                                      │
│        Información principal         │
│                                      │
├──────────────────────────────────────┤
│ Inicio │ Clientes │ Préstamos │ Más │
└──────────────────────────────────────┘
```

Dentro de la sección **Más** podrían encontrarse:

- Pagos.
- Morosidad.
- Notificaciones.
- WhatsApp.
- Perfil.
- Configuración.

La estructura definitiva podrá modificarse durante la etapa de wireframes.

---

## 17. Principios de Navegación

La navegación deberá cumplir los siguientes principios:

1. Mantener un acceso rápido a las funciones principales.
2. Evitar demasiados niveles de navegación.
3. Mostrar únicamente las opciones permitidas para cada usuario.
4. Permitir regresar fácilmente a la pantalla anterior.
5. Mantener una estructura consistente entre módulos.
6. Mostrar mensajes claros cuando una operación sea exitosa o presente errores.
7. Confirmar operaciones sensibles.
8. Facilitar el acceso rápido al registro de clientes, préstamos y pagos.
9. Priorizar las operaciones frecuentes.
10. Mantener una experiencia sencilla para usuarios con diferentes niveles de conocimiento tecnológico.

---

## 18. Mapa General de Navegación

```text
APLICACIÓN
│
├── Inicio de sesión
│
└── Dashboard
    │
    ├── Clientes
    │   ├── Lista
    │   ├── Buscar
    │   ├── Registrar
    │   └── Detalle
    │       ├── Editar
    │       └── Préstamos
    │
    ├── Préstamos
    │   ├── Lista
    │   ├── Nuevo préstamo
    │   └── Detalle
    │       ├── Resumen
    │       ├── Cuotas
    │       └── Pagos
    │
    ├── Pagos
    │   ├── Historial
    │   └── Registrar pago
    │
    ├── Morosidad
    │   ├── Lista
    │   └── Detalle
    │
    ├── Notificaciones
    │
    ├── WhatsApp
    │   ├── Historial
    │   └── Programar mensaje
    │
    ├── Perfil
    │
    └── Configuración
        ├── Usuarios
        ├── Roles
        ├── Notificaciones
        └── WhatsApp
```

---

## 19. Consideraciones Finales

El flujo de navegación definido en este documento representa la estructura funcional inicial de la aplicación móvil.

Durante el desarrollo de los wireframes y pantallas podrán realizarse ajustes para mejorar:

- La experiencia de usuario.
- La facilidad de navegación.
- La cantidad de pasos necesarios.
- La visibilidad de las funciones principales.
- La adaptación a dispositivos móviles.

El diseño deberá priorizar especialmente las operaciones más frecuentes del sistema:

1. Buscar cliente.
2. Registrar cliente.
3. Registrar préstamo.
4. Consultar préstamo.
5. Registrar pago.
6. Revisar clientes con pagos atrasados.