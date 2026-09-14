# Wireframe - Morosidad

## 1. Introducción

Este documento define el wireframe del módulo de Morosidad del Sistema de Gestión de Préstamos.

Este módulo permitirá identificar, controlar y gestionar los préstamos que presentan pagos atrasados.

La pantalla permitirá visualizar clientes con obligaciones pendientes, préstamos atrasados y la cantidad de pagos o intereses vencidos.

También permitirá realizar acciones relacionadas con la regularización y reactivación de préstamos, según las reglas de negocio establecidas.

---

## 2. Objetivo de la Pantalla

La pantalla de Morosidad deberá permitir:

- Visualizar préstamos con pagos atrasados.
- Identificar clientes morosos.
- Consultar la cantidad de pagos pendientes.
- Visualizar los días de atraso.
- Consultar el capital pendiente.
- Consultar los intereses pendientes.
- Acceder al detalle de la morosidad.
- Registrar acciones de seguimiento.
- Gestionar la reactivación de un cliente o préstamo cuando corresponda.
- Filtrar y buscar registros de morosidad.

---

## 3. Regla General de Morosidad

La morosidad estará relacionada con el incumplimiento de los pagos programados.

Flujo general:

```text
PRÉSTAMO ACTIVO
        │
        ▼
LLEGA FECHA DE PAGO
        │
        ▼
¿PAGO REALIZADO?
        │
        ├── SÍ
        │    │
        │    ▼
        │ PRÉSTAMO AL DÍA
        │
        └── NO
             │
             ▼
       PAGO ATRASADO
             │
             ▼
      ACTUALIZAR MOROSIDAD
             │
             ▼
       CONTABILIZAR ATRASOS
```

Cuando se cumplan las condiciones establecidas por las reglas de negocio, el préstamo o cliente podrá pasar a una situación de mayor riesgo.

---

## 4. Regla de Reactivación

Según las reglas iniciales del sistema:

- Si existen más de dos pagos de interés atrasados, el cliente o préstamo podrá requerir una acción administrativa.
- El sistema deberá permitir una opción de reactivación cuando corresponda.
- La reactivación deberá quedar registrada en el historial.
- La acción deberá ser realizada únicamente por usuarios autorizados.

Flujo:

```text
PRÉSTAMO CON ATRASOS
        │
        ▼
¿SUPERÓ EL LÍMITE DE ATRASOS?
        │
        ├── NO
        │    │
        │    ▼
        │ CONTINUAR SEGUIMIENTO
        │
        └── SÍ
             │
             ▼
       EVALUAR REACTIVACIÓN
             │
             ▼
        USUARIO AUTORIZADO
             │
             ▼
       REGISTRAR DECISIÓN
             │
             ├── REACTIVAR
             │
             └── MANTENER ESTADO
```

---

## 5. Estructura General

La pantalla estará compuesta por:

1. Barra superior.
2. Resumen de morosidad.
3. Barra de búsqueda.
4. Filtros.
5. Lista de préstamos atrasados.
6. Indicadores de prioridad.
7. Acceso al detalle.
8. Acciones de seguimiento.

---

## 6. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←            MOROSIDAD         🔔   │
├─────────────────────────────────────┤
│                                     │
│ RESUMEN                             │
│                                     │
│ Morosos: 12                         │
│ Préstamos atrasados: 15             │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 🔍 Buscar cliente o préstamo... │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ Todos ▼ ] [ Prioridad ▼ ]         │
│ [ Días atraso ▼ ] [ Filtros ]       │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Carlos Quispe                   │ │
│ │ Préstamo #0001                  │ │
│ │ Atrasos: 3                      │ │
│ │ Días: 10                        │ │
│ │ Pendiente: S/ 700.00            │ │
│ │ Estado: MOROSO             >    │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ María Flores                    │ │
│ │ Préstamo #0002                  │ │
│ │ Atrasos: 1                      │ │
│ │ Días: 4                         │ │
│ │ Pendiente: S/ 1,500.00          │ │
│ │ Estado: ATRASADO           >    │ │
│ └─────────────────────────────────┘ │
│                                     │
├─────────────────────────────────────┤
│ 🏠 Inicio  💰 Préstamos  ⚠ Morosidad│
└─────────────────────────────────────┘
```

---

## 7. Resumen de Morosidad

La parte superior mostrará indicadores generales.

Ejemplo:

```text
RESUMEN DE MOROSIDAD

Clientes morosos:
12

Préstamos atrasados:
15

Pagos vencidos:
28
```

También podrá mostrarse:

```text
Capital pendiente total:

S/ 15,800.00
```

E intereses pendientes:

```text
Intereses pendientes:

S/ 1,450.00
```

Estos indicadores permitirán conocer rápidamente la situación general de la cartera.

---

## 8. Barra de Búsqueda

La búsqueda permitirá localizar registros mediante:

- Nombre del cliente.
- Apellidos.
- Número de documento.
- Número de teléfono.
- Código del préstamo.

Wireframe:

```text
┌─────────────────────────────────┐
│ 🔍 Buscar cliente o préstamo... │
└─────────────────────────────────┘
```

---

## 9. Filtros

Los registros podrán filtrarse según diferentes criterios.

```text
[ TODOS ▼ ]

[ PRIORIDAD ▼ ]

[ DÍAS DE ATRASO ▼ ]

[ FILTROS ]
```

### Estado

- Todos.
- Atrasado.
- Moroso.
- En seguimiento.
- Reactivado.

### Cantidad de atrasos

- 1 atraso.
- 2 atrasos.
- Más de 2 atrasos.

### Días de atraso

- 1 a 7 días.
- 8 a 15 días.
- 16 a 30 días.
- Más de 30 días.

### Prioridad

- Baja.
- Media.
- Alta.
- Crítica.

---

## 10. Lista de Registros de Morosidad

Cada registro mostrará información resumida.

```text
┌─────────────────────────────────┐
│ Carlos Quispe                   │
│                                 │
│ Préstamo #0001                  │
│                                 │
│ Pagos atrasados: 3              │
│ Días de atraso: 10              │
│                                 │
│ Capital pendiente:              │
│ S/ 700.00                       │
│                                 │
│ Estado: MOROSO              >   │
└─────────────────────────────────┘
```

La información principal será:

- Cliente.
- Número del préstamo.
- Cantidad de atrasos.
- Días de atraso.
- Capital pendiente.
- Estado.

---

## 11. Prioridad de Atención

El sistema podrá clasificar los registros según su prioridad.

Ejemplo:

```text
PRIORIDAD BAJA

1 pago atrasado
```

```text
PRIORIDAD MEDIA

2 pagos atrasados
```

```text
PRIORIDAD ALTA

Más de 2 pagos atrasados
```

```text
PRIORIDAD CRÍTICA

Atraso prolongado
o situación definida por el administrador
```

La clasificación definitiva deberá respetar las reglas de negocio configuradas en el sistema.

---

## 12. Detalle de Morosidad

Al seleccionar un registro:

```text
LISTA DE MOROSIDAD
        │
        ▼
SELECCIONAR CLIENTE
        │
        ▼
DETALLE DE MOROSIDAD
```

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←        DETALLE DE MOROSIDAD       │
├─────────────────────────────────────┤
│                                     │
│ CLIENTE                             │
│ Carlos Quispe                       │
│ DNI: 12345678                       │
│ Teléfono: 999999999                 │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ PRÉSTAMO #0001                      │
│                                     │
│ Capital pendiente                   │
│ S/ 700.00                           │
│                                     │
│ Interés semanal                     │
│ S/ 50.00                            │
│                                     │
│ Interés pendiente                   │
│ S/ 150.00                           │
│                                     │
│ Pagos atrasados                     │
│ 3                                   │
│                                     │
│ Días de atraso                      │
│ 10                                  │
│                                     │
│ Estado                              │
│ MOROSO                              │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ [ REGISTRAR PAGO ]                  │
│                                     │
│ [ REGISTRAR SEGUIMIENTO ]           │
│                                     │
│ [ EVALUAR REACTIVACIÓN ]            │
└─────────────────────────────────────┘
```

---

## 13. Historial de Atrasos

El sistema deberá permitir consultar los atrasos registrados.

Ejemplo:

```text
HISTORIAL DE ATRASOS

────────────────────────────

Pago programado:

01/08/2026

Estado:

NO PAGADO

Días de atraso:

10

────────────────────────────

Pago programado:

08/08/2026

Estado:

NO PAGADO

Días de atraso:

3
```

Este historial permitirá conocer la evolución del préstamo.

---

## 14. Registro de Seguimiento

El usuario podrá registrar acciones realizadas para gestionar la morosidad.

Ejemplo:

```text
┌─────────────────────────────────────┐
│       REGISTRAR SEGUIMIENTO         │
├─────────────────────────────────────┤
│                                     │
│ Tipo de contacto                    │
│                                     │
│ [ LLAMADA ▼ ]                       │
│                                     │
│ Fecha                               │
│                                     │
│ 📅 18/08/2026                       │
│                                     │
│ Observaciones                       │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│          [ GUARDAR ]                │
└─────────────────────────────────────┘
```

Tipos de seguimiento iniciales:

- Llamada.
- WhatsApp.
- Visita.
- Mensaje.
- Otro.

---

## 15. Historial de Seguimiento

Cada acción deberá mantenerse registrada.

Ejemplo:

```text
HISTORIAL DE SEGUIMIENTO

────────────────────────────

18/08/2026

Tipo:
WhatsApp

Observación:
Cliente indicó que realizará el pago.

────────────────────────────

15/08/2026

Tipo:
Llamada

Observación:
No respondió.
```

---

## 16. Proceso de Reactivación

Cuando un préstamo cumpla las condiciones para ser evaluado:

```text
┌─────────────────────────────────────┐
│        EVALUAR REACTIVACIÓN         │
├─────────────────────────────────────┤
│                                     │
│ Cliente                             │
│ Carlos Quispe                       │
│                                     │
│ Préstamo                            │
│ #0001                               │
│                                     │
│ Pagos atrasados                     │
│ 3                                   │
│                                     │
│ Capital pendiente                   │
│ S/ 700.00                           │
│                                     │
│ Motivo de la decisión               │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ CANCELAR ]    [ REACTIVAR ]       │
└─────────────────────────────────────┘
```

Antes de ejecutar la acción, el sistema deberá verificar los permisos del usuario.

---

## 17. Confirmación de Reactivación

Antes de realizar la acción:

```text
¿Desea reactivar este préstamo?

El historial de morosidad se conservará.

[ CANCELAR ]

[ CONFIRMAR REACTIVACIÓN ]
```

La acción deberá registrarse en el historial.

---

## 18. Resultado de la Reactivación

Cuando la operación sea exitosa:

```text
✓ Préstamo reactivado correctamente.
```

El sistema deberá registrar:

- Fecha de reactivación.
- Usuario responsable.
- Motivo.
- Estado anterior.
- Nuevo estado.

---

## 19. Acceso al Registro de Pago

Desde el detalle de morosidad:

```text
[ REGISTRAR PAGO ]
```

Flujo:

```text
DETALLE DE MOROSIDAD
        │
        ▼
REGISTRAR PAGO
        │
        ▼
CALCULAR DISTRIBUCIÓN
        │
        ▼
CONFIRMAR
        │
        ▼
ACTUALIZAR MOROSIDAD
```

Después de registrar un pago, el sistema deberá recalcular automáticamente la situación del préstamo.

---

## 20. Actualización Automática de Morosidad

Cada vez que se produzca una operación relacionada con un préstamo:

```text
PAGO REGISTRADO
        │
        ▼
ACTUALIZAR INTERESES
        │
        ▼
ACTUALIZAR CAPITAL
        │
        ▼
REVISAR PAGOS ATRASADOS
        │
        ▼
ACTUALIZAR ESTADO
        │
        ├── AL DÍA
        │
        ├── ATRASADO
        │
        └── MOROSO
```

---

## 21. Estado sin Registros

Si no existen préstamos atrasados:

```text
✓ No existen registros de morosidad.

Todos los préstamos se encuentran al día.
```

---

## 22. Estado sin Resultados

Cuando una búsqueda no encuentre resultados:

```text
No se encontraron registros.

Intente realizar otra búsqueda.
```

---

## 23. Estado de Carga

Mientras se obtiene la información:

```text
Cargando registros de morosidad...
```

La interfaz podrá utilizar:

- Indicadores de carga.
- Skeleton loading.
- Tarjetas temporales.

---

## 24. Estado de Error

Si ocurre un problema:

```text
No se pudo cargar la información de morosidad.

Verifique su conexión e intente nuevamente.

[ REINTENTAR ]
```

---

## 25. Flujo de Navegación

```text
DASHBOARD
    │
    ▼
MOROSIDAD
    │
    ├── Buscar registro
    │
    ├── Aplicar filtros
    │
    ├── Seleccionar cliente
    │       │
    │       ▼
    │   Detalle de morosidad
    │       │
    │       ├── Registrar pago
    │       │
    │       ├── Ver historial
    │       │
    │       ├── Registrar seguimiento
    │       │
    │       └── Evaluar reactivación
    │
    └── Consultar indicadores
```

---

## 26. Consideraciones de Experiencia de Usuario

La pantalla deberá:

1. Permitir identificar rápidamente los préstamos con mayor riesgo.
2. Mostrar claramente la cantidad de atrasos.
3. Mostrar los días de atraso.
4. Permitir ordenar los registros por prioridad.
5. Facilitar el acceso al registro de pagos.
6. Mantener un historial de seguimiento.
7. Permitir registrar acciones realizadas.
8. Controlar las acciones de reactivación.
9. Mostrar información clara sobre capital e intereses pendientes.
10. Mantener coherencia visual con los demás módulos.

---

## 27. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│             MOROSIDAD               │
├─────────────────────────────────────┤
│                                     │
│ CLIENTES MOROSOS: 12                │
│ PRÉSTAMOS ATRASADOS: 15             │
│                                     │
├─────────────────────────────────────┤
│ 🔍 Buscar cliente o préstamo...     │
├─────────────────────────────────────┤
│ [ ESTADO ] [ PRIORIDAD ] [ FILTROS ]│
├─────────────────────────────────────┤
│                                     │
│ CARLOS QUISPE                  >    │
│ Préstamo #0001                     │
│ Atrasos: 3                          │
│ Días: 10                            │
│ Pendiente: S/ 700.00                │
│ Estado: MOROSO                      │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ MARÍA FLORES                   >    │
│ Préstamo #0002                     │
│ Atrasos: 1                          │
│ Días: 4                             │
│ Pendiente: S/ 1,500.00              │
│ Estado: ATRASADO                    │
│                                     │
├─────────────────────────────────────┤
│ 🏠 Inicio  💰 Préstamos  ⚠ Morosidad│
└─────────────────────────────────────┘
```

---

## 28. Consideraciones Finales

El módulo de Morosidad permitirá realizar un seguimiento organizado de los préstamos que presentan incumplimientos en sus pagos.

El sistema deberá permitir:

1. Identificar préstamos atrasados.
2. Calcular los días de atraso.
3. Contabilizar los pagos pendientes.
4. Clasificar los registros por prioridad.
5. Consultar capital e intereses pendientes.
6. Registrar acciones de seguimiento.
7. Registrar pagos directamente desde el módulo.
8. Evaluar y registrar procesos de reactivación.
9. Mantener un historial completo de morosidad.
10. Actualizar automáticamente el estado después de cada pago.

El siguiente wireframe será:

`09-Wireframe-Notificaciones.md`

Este documento definirá la pantalla para gestionar las notificaciones del sistema, incluyendo recordatorios de pago, próximas fechas de vencimiento y la programación de notificaciones automáticas.