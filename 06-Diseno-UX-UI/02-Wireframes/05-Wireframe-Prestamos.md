# Wireframe - Préstamos

## 1. Introducción

Este documento define el wireframe de la pantalla principal del módulo de Préstamos del Sistema de Gestión de Préstamos.

Esta pantalla permitirá visualizar, buscar, filtrar y acceder al detalle de los préstamos registrados en el sistema.

Un cliente podrá tener uno o varios préstamos asociados, según las reglas de negocio definidas para el sistema.

El módulo permitirá identificar rápidamente el estado de cada préstamo, su capital pendiente, los pagos realizados y posibles atrasos.

---

## 2. Objetivo de la Pantalla

La pantalla de Préstamos deberá permitir:

- Visualizar todos los préstamos registrados.
- Buscar préstamos por cliente o número de préstamo.
- Filtrar préstamos según su estado.
- Consultar información resumida de cada préstamo.
- Identificar préstamos con pagos atrasados.
- Acceder al detalle de un préstamo.
- Registrar un nuevo préstamo.
- Consultar el capital pendiente.
- Consultar los intereses y pagos relacionados.

---

## 3. Estructura General

La pantalla estará compuesta por las siguientes secciones:

1. Barra superior.
2. Título de la pantalla.
3. Barra de búsqueda.
4. Filtros.
5. Resumen de préstamos.
6. Lista de préstamos.
7. Botón para registrar un nuevo préstamo.
8. Navegación principal.

---

## 4. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←             PRÉSTAMOS        🔔   │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 🔍 Buscar préstamo o cliente... │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ Todos ▼ ] [ Estado ▼ ] [ Filtros ]│
│                                     │
├─────────────────────────────────────┤
│                                     │
│ PRÉSTAMOS REGISTRADOS               │
│ Total: 45                           │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Carlos Quispe                   │ │
│ │ Préstamo #0001                  │ │
│ │ Capital: S/ 1,000.00            │ │
│ │ Pendiente: S/ 700.00            │ │
│ │ Estado: ACTIVO              >   │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ María Flores                    │ │
│ │ Préstamo #0002                  │ │
│ │ Capital: S/ 2,000.00            │ │
│ │ Pendiente: S/ 1,500.00          │ │
│ │ Estado: ATRASADO            >   │ │
│ └─────────────────────────────────┘ │
│                                     │
│                            ┌─────┐  │
│                            │  +  │  │
│                            └─────┘  │
├─────────────────────────────────────┤
│ 🏠 Inicio  👥 Clientes  💰 Préstamos│
└─────────────────────────────────────┘
```

---

## 5. Barra Superior

La parte superior de la pantalla permitirá acceder a las principales opciones de navegación.

```text
┌─────────────────────────────────────┐
│ ←             PRÉSTAMOS        🔔   │
└─────────────────────────────────────┘
```

Elementos:

- Botón para regresar.
- Título de la pantalla.
- Acceso a notificaciones.

El botón de regreso permitirá retornar al Dashboard o a la pantalla anterior.

---

## 6. Barra de Búsqueda

La barra de búsqueda permitirá localizar rápidamente un préstamo.

```text
┌─────────────────────────────────┐
│ 🔍 Buscar préstamo o cliente... │
└─────────────────────────────────┘
```

La búsqueda podrá realizarse mediante:

- Nombre del cliente.
- Apellidos del cliente.
- Número de documento.
- Número de préstamo.
- Número de teléfono.

Ejemplo de flujo:

```text
Usuario ingresa información
        │
        ▼
Sistema procesa búsqueda
        │
        ▼
Filtrar préstamos
        │
        ├── Resultados encontrados
        │       │
        │       ▼
        │   Mostrar préstamos
        │
        └── Sin resultados
                │
                ▼
        Mostrar mensaje
```

---

## 7. Filtros

El usuario podrá filtrar los préstamos según diferentes criterios.

```text
[ TODOS ▼ ] [ ESTADO ▼ ] [ FILTROS ]
```

Los filtros podrán incluir:

### Por estado

- Todos.
- Pendiente.
- Activo.
- Atrasado.
- Finalizado.
- Cancelado.

### Por cliente

Permitirá seleccionar los préstamos de un cliente específico.

### Por fecha

Permitirá filtrar préstamos según:

- Fecha de registro.
- Fecha de aprobación.
- Periodo determinado.

### Por situación de pago

- Al día.
- Con pago próximo.
- Con pagos atrasados.

---

## 8. Resumen de Préstamos

Antes de mostrar la lista, se podrá presentar un resumen general.

```text
PRÉSTAMOS REGISTRADOS

Total: 45
```

Como información adicional, podrá mostrarse:

```text
Activos: 30
Atrasados: 8
Finalizados: 7
```

El objetivo será permitir al usuario conocer rápidamente la situación general de los préstamos.

---

## 9. Lista de Préstamos

Cada préstamo será mostrado mediante una tarjeta o elemento seleccionable.

Ejemplo:

```text
┌─────────────────────────────────┐
│ Carlos Quispe                   │
│                                 │
│ Préstamo #0001                  │
│ Capital inicial: S/ 1,000.00    │
│ Capital pendiente: S/ 700.00    │
│                                 │
│ Estado: ACTIVO              >   │
└─────────────────────────────────┘
```

La información mínima mostrada será:

- Nombre del cliente.
- Número o código del préstamo.
- Capital inicial.
- Capital pendiente.
- Estado actual.

---

## 10. Estados del Préstamo

Los préstamos podrán tener diferentes estados.

### PENDIENTE

El préstamo ha sido registrado o solicitado, pero todavía requiere una acción antes de quedar activo.

```text
Estado: PENDIENTE
```

---

### ACTIVO

El préstamo se encuentra vigente y tiene capital pendiente por pagar.

```text
Estado: ACTIVO
```

El cliente deberá continuar realizando los pagos correspondientes.

---

### ATRASADO

El préstamo presenta uno o más pagos pendientes fuera de la fecha establecida.

```text
Estado: ATRASADO
```

Este estado estará relacionado con las reglas de morosidad.

---

### CANCELADO (saldado)

El cliente ha pagado completamente capital (`==0`) e intereses (`==0`) según RN-PRE-010; transición automática por trigger, terminal.

```text
Estado: CANCELADO
```

`FINALIZADO` prohibido (usar `CANCELADO`).

---

### ANULADO

El préstamo fue anulado por error de registro, solo antes del primer pago, con motivo + actor (RN-PRE-011).

```text
Estado: ANULADO
```

La cancelación deberá registrar el motivo y mantener el historial correspondiente.

---

## 11. Acceso al Detalle del Préstamo

Al seleccionar un préstamo, el usuario será dirigido a la pantalla de detalle.

```text
LISTA DE PRÉSTAMOS
        │
        ▼
Seleccionar préstamo
        │
        ▼
DETALLE DEL PRÉSTAMO
        │
        ├── Información del cliente
        │
        ├── Datos del préstamo
        │
        ├── Capital inicial
        │
        ├── Capital pendiente
        │
        ├── Intereses
        │
        ├── Próximo pago
        │
        ├── Historial de pagos
        │
        └── Estado
```

---

## 12. Información Principal del Préstamo

En el detalle se podrá mostrar información como:

```text
CLIENTE

Carlos Quispe

DNI: 12345678

────────────────────────────

PRÉSTAMO #0001

Capital inicial:
S/ 1,000.00

Capital pendiente:
S/ 700.00

Interés semanal:
5%

Próximo pago:
18/08/2026

Estado:
ACTIVO
```

La información exacta dependerá de las reglas de negocio definidas para el préstamo.

---

## 13. Resumen Financiero del Préstamo

El detalle deberá permitir visualizar el estado financiero.

```text
RESUMEN DEL PRÉSTAMO

Capital inicial:
S/ 1,000.00

Capital pagado:
S/ 300.00

Capital pendiente:
S/ 700.00

Interés semanal:
S/ 50.00

Intereses pagados:
S/ 200.00
```

El interés semanal será calculado de acuerdo con la regla establecida para el préstamo.

---

## 14. Acciones Disponibles

Desde el detalle del préstamo podrán estar disponibles las siguientes acciones:

- Registrar pago.
- Consultar historial de pagos.
- Ver información del cliente.
- Consultar aval.
- Consultar morosidad.
- Editar información permitida.
- Finalizar préstamo, cuando corresponda.
- Cancelar préstamo, según permisos.

La disponibilidad de cada acción dependerá del rol del usuario.

---

## 15. Registrar Nuevo Préstamo

La pantalla incluirá un botón para registrar un nuevo préstamo.

```text
┌─────┐
│  +  │
└─────┘
```

Flujo:

```text
LISTA DE PRÉSTAMOS
        │
        ▼
Botón "+"
        │
        ▼
REGISTRAR NUEVO PRÉSTAMO
```

El usuario será dirigido al formulario correspondiente.

---

## 16. Estado de Carga

Mientras se obtiene la información:

```text
Cargando préstamos...
```

Podrán utilizarse:

- Indicador de carga.
- Skeleton loading.
- Tarjetas temporales.

---

## 17. Estado sin Préstamos

Si todavía no existen préstamos registrados:

```text
No hay préstamos registrados.

Comienza registrando un nuevo préstamo.

[ REGISTRAR PRÉSTAMO ]
```

---

## 18. Estado sin Resultados

Cuando la búsqueda no encuentre préstamos:

```text
No se encontraron préstamos.

Intenta realizar otra búsqueda.
```

También podrá mostrarse:

```text
[ LIMPIAR BÚSQUEDA ]
```

---

## 19. Estado de Error

Si ocurre un problema al cargar la información:

```text
No se pudo cargar la información.

Verifique su conexión e intente nuevamente.

[ REINTENTAR ]
```

Los mensajes deberán ser comprensibles y no mostrar detalles técnicos innecesarios.

---

## 20. Paginación y Carga Progresiva

Cuando exista una gran cantidad de préstamos, la aplicación deberá utilizar paginación o carga progresiva.

Ejemplo:

```text
PRÉSTAMOS

Préstamo #0001
Préstamo #0002
Préstamo #0003
Préstamo #0004

Cargando más préstamos...
```

El backend deberá proporcionar los resultados de manera paginada.

---

## 21. Flujo de Navegación

```text
DASHBOARD
    │
    ▼
PRÉSTAMOS
    │
    ├── Buscar préstamo
    │       │
    │       ▼
    │   Resultados
    │
    ├── Filtrar préstamos
    │       │
    │       ▼
    │   Lista filtrada
    │
    ├── Seleccionar préstamo
    │       │
    │       ▼
    │   Detalle del préstamo
    │       │
    │       ├── Registrar pago
    │       ├── Ver historial
    │       ├── Ver cliente
    │       └── Consultar morosidad
    │
    └── Nuevo préstamo
            │
            ▼
      Registro de préstamo
```

---

## 22. Consideraciones de Experiencia de Usuario

La pantalla deberá cumplir los siguientes principios:

1. Permitir localizar un préstamo rápidamente.
2. Mostrar claramente el estado del préstamo.
3. Permitir identificar el capital pendiente.
4. Facilitar el acceso al registro de pagos.
5. Mostrar alertas sobre préstamos atrasados.
6. Mantener visible la opción para registrar un nuevo préstamo.
7. Evitar mostrar información excesiva en la lista.
8. Permitir filtrar préstamos fácilmente.
9. Adaptarse a diferentes tamaños de pantalla.
10. Mantener coherencia visual con los demás módulos.

---

## 23. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│ ←            PRÉSTAMOS         🔔   │
├─────────────────────────────────────┤
│ 🔍 Buscar préstamo o cliente...     │
├─────────────────────────────────────┤
│ [ TODOS ] [ ESTADO ] [ FILTROS ]    │
├─────────────────────────────────────┤
│ PRÉSTAMOS REGISTRADOS: 45           │
├─────────────────────────────────────┤
│                                     │
│ CARLOS QUISPE                  >    │
│ Préstamo #0001                     │
│ Capital: S/ 1,000.00                │
│ Pendiente: S/ 700.00                │
│ Estado: ACTIVO                      │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ MARÍA FLORES                   >    │
│ Préstamo #0002                     │
│ Capital: S/ 2,000.00                │
│ Pendiente: S/ 1,500.00              │
│ Estado: ATRASADO                    │
│                                     │
│                               [+]   │
├─────────────────────────────────────┤
│ 🏠 Inicio  👥 Clientes  💰 Préstamos│
└─────────────────────────────────────┘
```

---

## 24. Consideraciones Finales

El módulo de Préstamos será uno de los componentes centrales del Sistema de Gestión de Préstamos.

La pantalla permitirá al usuario:

1. Consultar todos los préstamos registrados.
2. Buscar préstamos específicos.
3. Filtrar préstamos por estado.
4. Identificar préstamos activos y atrasados.
5. Consultar el capital pendiente.
6. Acceder al detalle de cada préstamo.
7. Registrar un nuevo préstamo.
8. Acceder rápidamente al registro de pagos.

El siguiente wireframe será:

`06-Wireframe-Registro-Prestamo.md`

Este documento definirá el proceso completo para registrar un nuevo préstamo, incluyendo la selección del cliente, monto del préstamo, condiciones, cálculo del interés semanal, validación y confirmación del registro.