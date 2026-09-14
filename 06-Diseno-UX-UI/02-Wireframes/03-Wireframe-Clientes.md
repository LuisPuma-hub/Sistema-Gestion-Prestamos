# Wireframe - Clientes

## 1. Introducción

Este documento define el wireframe de la pantalla principal del módulo de Clientes del Sistema de Gestión de Préstamos.

Esta pantalla permitirá consultar, buscar, filtrar y acceder a la información detallada de los clientes registrados en el sistema.

También proporcionará un acceso rápido para registrar un nuevo cliente.

---

## 2. Objetivo de la Pantalla

La pantalla de Clientes deberá permitir:

- Visualizar la lista de clientes registrados.
- Buscar clientes por nombre, apellido o número de documento.
- Filtrar clientes según su estado.
- Consultar información básica de cada cliente.
- Acceder al detalle de un cliente.
- Registrar un nuevo cliente.
- Identificar rápidamente clientes activos, en observación o con morosidad.

---

## 3. Estructura General

La pantalla estará compuesta por las siguientes secciones:

1. Barra superior.
2. Título de la pantalla.
3. Barra de búsqueda.
4. Filtros.
5. Resumen de clientes.
6. Lista de clientes.
7. Botón para registrar un nuevo cliente.
8. Navegación principal.

---

## 4. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←              CLIENTES        🔔   │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 🔍 Buscar cliente...            │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ Todos ▼ ]  [ Estado ▼ ]  [ Filtro ]│
│                                     │
├─────────────────────────────────────┤
│                                     │
│ CLIENTES REGISTRADOS                │
│ Total: 120                          │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 Carlos Quispe                │ │
│ │ DNI: 12345678                   │ │
│ │ 📞 999999999                    │ │
│ │ Estado: ACTIVO                  │ │
│ │                           >     │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 María Flores                 │ │
│ │ DNI: 87654321                   │ │
│ │ 📞 988888888                    │ │
│ │ Estado: OBSERVACIÓN             │ │
│ │                           >     │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 José Pérez                   │ │
│ │ CE: 001234567                   │ │
│ │ 📞 977777777                    │ │
│ │ Estado: MOROSO                  │ │
│ │                           >     │ │
│ └─────────────────────────────────┘ │
│                                     │
│                                     │
│                            ┌─────┐  │
│                            │  +  │  │
│                            └─────┘  │
├─────────────────────────────────────┤
│ 🏠 Inicio    👥 Clientes      ☰ Más │
└─────────────────────────────────────┘
```

---

## 5. Barra Superior

La parte superior de la pantalla contendrá los principales elementos de navegación.

```text
┌─────────────────────────────────────┐
│ ←              CLIENTES        🔔   │
└─────────────────────────────────────┘
```

Elementos:

- Botón para regresar.
- Título de la pantalla.
- Acceso a notificaciones.

El botón de regreso podrá retornar al Dashboard o a la pantalla anterior, dependiendo del flujo de navegación.

---

## 6. Barra de Búsqueda

La barra de búsqueda permitirá localizar rápidamente un cliente.

Ejemplo:

```text
┌─────────────────────────────────┐
│ 🔍 Buscar cliente...            │
└─────────────────────────────────┘
```

La búsqueda podrá realizarse utilizando:

- Nombres.
- Apellidos.
- Número de documento.
- Número de teléfono.

Ejemplo de flujo:

```text
Usuario escribe información
        │
        ▼
Sistema procesa búsqueda
        │
        ▼
Filtrar resultados
        │
        ├── Cliente encontrado
        │       │
        │       ▼
        │   Mostrar resultado
        │
        └── Sin resultados
                │
                ▼
        Mostrar mensaje
```

Mensaje:

```text
No se encontraron clientes.
```

---

## 7. Filtros

La pantalla permitirá filtrar los clientes registrados.

Propuesta inicial:

```text
[ Todos ▼ ]  [ Estado ▼ ]  [ Filtro ]
```

Los filtros podrán incluir:

### Por estado

- Todos.
- Activo.
- Observación.
- Moroso.
- Inactivo.

### Por tipo de documento

- DNI.
- CE.

### Por condición de préstamos

- Con préstamos activos.
- Sin préstamos activos.
- Con pagos atrasados.

Los filtros podrán utilizarse de forma individual o combinada.

---

## 8. Resumen de Clientes

Antes de la lista se podrá mostrar un resumen simple.

Ejemplo:

```text
CLIENTES REGISTRADOS

Total: 120
```

Como mejora futura, se podrá incluir:

```text
Activos: 100
Observación: 12
Morosos: 8
```

Esto permitirá conocer rápidamente la distribución de los clientes.

---

## 9. Lista de Clientes

Cada cliente será mostrado mediante una tarjeta o elemento seleccionable.

Ejemplo:

```text
┌─────────────────────────────────┐
│ 👤 Carlos Quispe                │
│                                 │
│ DNI: 12345678                   │
│ 📞 999999999                    │
│                                 │
│ Estado: ACTIVO                  │
│                             >   │
└─────────────────────────────────┘
```

La información mínima mostrada será:

- Nombre completo.
- Tipo y número de documento.
- Número de teléfono.
- Estado del cliente.

La información deberá ser suficiente para identificar rápidamente al cliente.

---

## 10. Estados del Cliente

El sistema podrá mostrar visualmente el estado del cliente.

Estados principales:

### ACTIVO

Cliente que puede realizar operaciones dentro del sistema.

```text
Estado: ACTIVO
```

### OBSERVACIÓN

Cliente que requiere una revisión o presenta alguna observación registrada.

```text
Estado: OBSERVACIÓN
```

### MOROSO

Cliente con pagos atrasados según las reglas de negocio.

```text
Estado: MOROSO
```

### INACTIVO

Cliente que no podrá realizar nuevas operaciones mientras mantenga este estado.

```text
Estado: INACTIVO
```

El diseño visual definitivo podrá utilizar etiquetas o indicadores para diferenciar los estados.

---

## 11. Acceso al Detalle del Cliente

Al seleccionar un cliente, el usuario será dirigido a su pantalla de detalle.

Flujo:

```text
LISTA DE CLIENTES
        │
        ▼
Seleccionar cliente
        │
        ▼
DETALLE DEL CLIENTE
        │
        ├── Información personal
        │
        ├── Información de contacto
        │
        ├── Dirección
        │
        ├── Aval
        │
        ├── Documento de domicilio
        │
        ├── Observaciones
        │
        └── Préstamos
```

Desde el detalle se podrán realizar acciones según los permisos del usuario.

---

## 12. Acciones Disponibles en un Cliente

Desde la pantalla de detalle se podrán realizar las siguientes acciones:

- Editar información.
- Consultar préstamos.
- Registrar un nuevo préstamo.
- Consultar historial de pagos.
- Consultar aval.
- Consultar observaciones.
- Cambiar estado del cliente, según permisos.

La disponibilidad de estas acciones dependerá del rol del usuario.

---

## 13. Botón Registrar Nuevo Cliente

La pantalla incluirá un botón flotante o botón principal.

Ejemplo:

```text
┌─────┐
│  +  │
└─────┘
```

Al seleccionarlo:

```text
LISTA DE CLIENTES
        │
        ▼
Botón "+"
        │
        ▼
REGISTRAR NUEVO CLIENTE
```

El usuario será dirigido al formulario de registro.

---

## 14. Flujo de Navegación

```text
DASHBOARD
    │
    ▼
CLIENTES
    │
    ├── Buscar cliente
    │       │
    │       ▼
    │   Resultados
    │
    ├── Filtrar clientes
    │       │
    │       ▼
    │   Lista filtrada
    │
    ├── Seleccionar cliente
    │       │
    │       ▼
    │   Detalle del cliente
    │       │
    │       ├── Editar
    │       ├── Ver préstamos
    │       ├── Registrar préstamo
    │       └── Ver historial
    │
    └── Nuevo cliente
            │
            ▼
      Registro de cliente
```

---

## 15. Estado de Carga

Mientras se obtiene la información de los clientes, se deberá mostrar un estado de carga.

Ejemplo:

```text
Cargando clientes...
```

La interfaz podrá utilizar:

- Indicador de carga.
- Tarjetas temporales.
- Skeleton loading.

Durante la carga se deberá evitar mostrar información incompleta.

---

## 16. Estado sin Clientes

Si todavía no existen clientes registrados, se mostrará un mensaje.

Ejemplo:

```text
No hay clientes registrados.

Comienza registrando tu primer cliente.

[ REGISTRAR CLIENTE ]
```

Esto permitirá al usuario acceder rápidamente al formulario de registro.

---

## 17. Estado sin Resultados

Cuando una búsqueda no encuentre coincidencias:

```text
No se encontraron clientes.

Intenta realizar otra búsqueda.
```

También podrá mostrarse una acción:

```text
[ LIMPIAR BÚSQUEDA ]
```

---

## 18. Estado de Error

Si ocurre un problema al obtener la información:

```text
No se pudo cargar la lista de clientes.

Verifique su conexión e intente nuevamente.

[ REINTENTAR ]
```

Los errores deberán ser claros y no mostrar información técnica innecesaria.

---

## 19. Paginación y Carga de Resultados

Cuando la cantidad de clientes sea elevada, la aplicación deberá evitar cargar todos los registros simultáneamente.

Se utilizará paginación o carga progresiva.

Ejemplo:

```text
CLIENTES

Cliente 1
Cliente 2
Cliente 3
Cliente 4
Cliente 5

Cargando más clientes...
```

El backend deberá proporcionar la información paginada.

---

## 20. Consideraciones de Experiencia de Usuario

La pantalla deberá cumplir los siguientes principios:

1. Permitir encontrar un cliente rápidamente.
2. Mantener visible la opción para registrar clientes.
3. Mostrar únicamente la información necesaria en la lista.
4. Facilitar el acceso al detalle del cliente.
5. Permitir filtrar fácilmente la información.
6. Mostrar claramente el estado de cada cliente.
7. Adaptarse a diferentes tamaños de pantalla.
8. Evitar listas demasiado cargadas visualmente.
9. Mostrar estados de carga y error.
10. Mantener coherencia con el Dashboard y las demás pantallas.

---

## 21. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│ ←            CLIENTES          🔔   │
├─────────────────────────────────────┤
│ 🔍 Buscar cliente...                │
├─────────────────────────────────────┤
│ [ TODOS ] [ ESTADO ] [ FILTROS ]    │
├─────────────────────────────────────┤
│ CLIENTES REGISTRADOS: 120           │
├─────────────────────────────────────┤
│                                     │
│ 👤 CARLOS QUISPE               >    │
│ DNI: 12345678                       │
│ Estado: ACTIVO                      │
│                                     │
├─────────────────────────────────────┤
│ 👤 MARÍA FLORES                >    │
│ DNI: 87654321                       │
│ Estado: OBSERVACIÓN                 │
│                                     │
├─────────────────────────────────────┤
│ 👤 JOSÉ PÉREZ                  >    │
│ CE: 001234567                       │
│ Estado: MOROSO                      │
│                                     │
│                               [+]   │
├─────────────────────────────────────┤
│ 🏠 Inicio    👥 Clientes      ☰ Más │
└─────────────────────────────────────┘
```

---

## 22. Consideraciones Finales

La pantalla de Clientes será uno de los módulos principales del Sistema de Gestión de Préstamos.

Su diseño deberá permitir realizar rápidamente las operaciones más frecuentes:

1. Buscar un cliente.
2. Consultar su información.
3. Identificar su estado.
4. Revisar sus préstamos.
5. Registrar un nuevo cliente.
6. Acceder a su historial.

El siguiente wireframe desarrollará el proceso completo para registrar un nuevo cliente, incluyendo:

- Datos personales.
- Documento de identidad.
- Información de contacto.
- Dirección.
- Aval.
- Documento de domicilio.
- Observaciones.
- Validación y guardado del registro.