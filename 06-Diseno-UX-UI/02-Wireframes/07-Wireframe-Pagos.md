# Wireframe - Pagos

## 1. Introducción

Este documento define el wireframe del módulo de Pagos del Sistema de Gestión de Préstamos.

Este módulo permitirá consultar los pagos registrados y registrar nuevos pagos asociados a los préstamos activos de los clientes.

El sistema deberá aplicar los pagos según las reglas de negocio establecidas:

1. Primero se deberá cubrir el interés pendiente.
2. Si el monto entregado supera el interés pendiente, el excedente se aplicará al capital.
3. El capital pendiente se reducirá únicamente cuando exista un monto disponible después de cubrir los intereses correspondientes.
4. El préstamo continuará activo mientras exista capital pendiente.

---

## 2. Objetivo de la Pantalla

La pantalla de Pagos deberá permitir:

- Visualizar pagos registrados.
- Buscar pagos por cliente o préstamo.
- Filtrar pagos por fecha.
- Filtrar pagos por estado.
- Consultar el detalle de un pago.
- Registrar un nuevo pago.
- Consultar el interés pendiente.
- Consultar el capital pendiente.
- Calcular automáticamente cómo se distribuirá un pago.
- Consultar el historial de pagos de un préstamo.

---

## 3. Regla de Aplicación de Pagos

La regla principal será:

```text
MONTO RECIBIDO
        │
        ▼
¿EXISTE INTERÉS PENDIENTE?
        │
        ├── SÍ
        │     │
        │     ▼
        │ PAGAR INTERÉS
        │     │
        │     ▼
        │ ¿SOBRA DINERO?
        │     │
        │     ├── SÍ
        │     │     │
        │     │     ▼
        │     │ APLICAR A CAPITAL
        │     │
        │     └── NO
        │
        └── NO
              │
              ▼
        APLICAR A CAPITAL
```

Ejemplo:

```text
Capital pendiente: S/ 1,000.00

Interés pendiente: S/ 50.00

Pago recibido: S/ 100.00
```

El sistema realizará:

```text
Pago al interés:

S/ 50.00

Monto restante:

S/ 50.00

Pago al capital:

S/ 50.00
```

Resultado:

```text
Capital pendiente anterior:
S/ 1,000.00

Capital pagado:
S/ 50.00

Nuevo capital pendiente:
S/ 950.00
```

---

## 4. Estructura General

La pantalla estará compuesta por:

1. Barra superior.
2. Barra de búsqueda.
3. Filtros.
4. Resumen de pagos.
5. Lista de pagos.
6. Acceso al detalle.
7. Botón para registrar un nuevo pago.
8. Navegación principal.

---

## 5. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←               PAGOS          🔔   │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 🔍 Buscar cliente o préstamo... │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ Todos ▼ ] [ Fecha ▼ ] [ Filtros ] │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ PAGOS REGISTRADOS                   │
│ Hoy: 8                              │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Carlos Quispe                   │ │
│ │ Préstamo #0001                  │ │
│ │ Pago: S/ 100.00                 │ │
│ │ 18/08/2026                >     │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ María Flores                    │ │
│ │ Préstamo #0002                  │ │
│ │ Pago: S/ 50.00                  │ │
│ │ 18/08/2026                >     │ │
│ └─────────────────────────────────┘ │
│                                     │
│                            ┌─────┐  │
│                            │  +  │  │
│                            └─────┘  │
├─────────────────────────────────────┤
│ 🏠 Inicio  💰 Préstamos  💳 Pagos  │
└─────────────────────────────────────┘
```

---

## 6. Barra de Búsqueda

La barra permitirá buscar pagos utilizando:

- Nombre del cliente.
- Apellidos.
- Número de documento.
- Número de préstamo.
- Código del pago.

Wireframe:

```text
┌─────────────────────────────────┐
│ 🔍 Buscar cliente o préstamo... │
└─────────────────────────────────┘
```

Flujo:

```text
Usuario realiza búsqueda
        │
        ▼
Sistema procesa información
        │
        ▼
Filtrar pagos relacionados
        │
        ├── Resultados encontrados
        │       │
        │       ▼
        │   Mostrar resultados
        │
        └── Sin resultados
                │
                ▼
        Mostrar mensaje
```

---

## 7. Filtros

Los pagos podrán filtrarse utilizando diferentes criterios.

```text
[ TODOS ▼ ] [ FECHA ▼ ] [ FILTROS ]
```

### Por periodo

- Hoy.
- Últimos 7 días.
- Últimos 30 días.
- Este mes.
- Periodo personalizado.

### Por cliente

Permitirá visualizar únicamente los pagos de un cliente.

### Por préstamo

Permitirá visualizar los pagos asociados a un préstamo específico.

### Por tipo de aplicación

Como información adicional, el sistema podrá permitir filtrar:

- Pagos aplicados únicamente a intereses.
- Pagos aplicados a capital.
- Pagos mixtos.

---

## 8. Resumen de Pagos

La pantalla podrá mostrar un resumen general.

Ejemplo:

```text
PAGOS DE HOY

Cantidad de pagos:
8

Monto recibido:
S/ 1,250.00
```

Como información adicional:

```text
Aplicado a intereses:
S/ 450.00

Aplicado a capital:
S/ 800.00
```

---

## 9. Lista de Pagos

Cada pago será mostrado mediante una tarjeta.

```text
┌─────────────────────────────────┐
│ Carlos Quispe                   │
│                                 │
│ Préstamo #0001                  │
│                                 │
│ Pago recibido: S/ 100.00        │
│                                 │
│ Interés: S/ 50.00               │
│ Capital: S/ 50.00               │
│                                 │
│ 18/08/2026                  >   │
└─────────────────────────────────┘
```

La información principal será:

- Cliente.
- Número del préstamo.
- Monto recibido.
- Monto aplicado a intereses.
- Monto aplicado al capital.
- Fecha del pago.

---

## 10. Registrar Nuevo Pago

La pantalla incluirá un botón para registrar un nuevo pago.

```text
┌─────┐
│  +  │
└─────┘
```

Flujo:

```text
LISTA DE PAGOS
        │
        ▼
BOTÓN "+"
        │
        ▼
REGISTRAR NUEVO PAGO
```

---

# 11. Wireframe - Registro de Pago

El proceso comenzará seleccionando el préstamo.

```text
┌─────────────────────────────────────┐
│ ←         REGISTRAR PAGO            │
├─────────────────────────────────────┤
│                                     │
│ SELECCIONAR PRÉSTAMO                │
│                                     │
│ 🔍 Buscar cliente o préstamo        │
│ ┌─────────────────────────────────┐ │
│ │ Nombre o código del préstamo    │ │
│ └─────────────────────────────────┘ │
│                                     │
│ RESULTADOS                          │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Carlos Quispe                   │ │
│ │ Préstamo #0001                  │ │
│ │ Pendiente: S/ 700.00            │ │
│ │ Estado: ACTIVO             [+]  │ │
│ └─────────────────────────────────┘ │
│                                     │
└─────────────────────────────────────┘
```

---

## 12. Selección del Préstamo

El usuario deberá seleccionar el préstamo correspondiente al pago.

El sistema mostrará:

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
S/ 50.00

Interés pendiente:
S/ 50.00
```

El usuario podrá confirmar que está registrando el pago en el préstamo correcto.

---

## 13. Ingreso del Monto

Después de seleccionar el préstamo:

```text
┌─────────────────────────────────────┐
│ ←         REGISTRAR PAGO            │
├─────────────────────────────────────┤
│                                     │
│ CLIENTE                             │
│ Carlos Quispe                       │
│                                     │
│ PRÉSTAMO #0001                      │
│                                     │
│ MONTO RECIBIDO                      │
│                                     │
│ S/ ┌──────────────────────────────┐ │
│    │                              │ │
│    └──────────────────────────────┘ │
│                                     │
│ [ ANTERIOR ]       [ CALCULAR ]     │
└─────────────────────────────────────┘
```

El monto deberá:

- Ser obligatorio.
- Ser mayor que cero.
- Ser un valor numérico válido.

Ejemplo:

```text
Monto recibido:

S/ 100.00
```

---

## 14. Cálculo Automático del Pago

Después de ingresar el monto, el sistema calculará automáticamente cómo distribuir el pago.

Ejemplo:

```text
┌─────────────────────────────────────┐
│          RESUMEN DEL PAGO           │
├─────────────────────────────────────┤
│                                     │
│ Monto recibido                      │
│ S/ 100.00                           │
│                                     │
│ Interés pendiente                   │
│ S/ 50.00                            │
│                                     │
│ Aplicado a interés                  │
│ S/ 50.00                            │
│                                     │
│ Aplicado a capital                  │
│ S/ 50.00                            │
│                                     │
│ Nuevo capital pendiente             │
│ S/ 650.00                           │
│                                     │
│ [ ANTERIOR ]       [ CONTINUAR ]    │
└─────────────────────────────────────┘
```

El usuario no deberá modificar manualmente la distribución calculada.

---

## 15. Pago Menor al Interés Pendiente

Ejemplo:

```text
Interés pendiente:
S/ 50.00

Pago recibido:
S/ 30.00
```

Resultado:

```text
Aplicado al interés:

S/ 30.00

Aplicado al capital:

S/ 0.00

Interés pendiente restante:

S/ 20.00
```

El capital no se modificará.

---

## 16. Pago Igual al Interés Pendiente

Ejemplo:

```text
Interés pendiente:
S/ 50.00

Pago recibido:
S/ 50.00
```

Resultado:

```text
Aplicado al interés:

S/ 50.00

Aplicado al capital:

S/ 0.00

Capital pendiente:

S/ 700.00
```

---

## 17. Pago Mayor al Interés Pendiente

Ejemplo:

```text
Interés pendiente:
S/ 50.00

Pago recibido:
S/ 150.00
```

Resultado:

```text
Aplicado al interés:

S/ 50.00

Monto restante:

S/ 100.00

Aplicado al capital:

S/ 100.00
```

Nuevo capital:

```text
Capital pendiente anterior:

S/ 700.00

Capital pagado:

S/ 100.00

Nuevo capital pendiente:

S/ 600.00
```

---

## 18. Fecha del Pago

El sistema deberá registrar la fecha en la que se realiza el pago.

```text
FECHA DEL PAGO

┌─────────────────────────────────┐
│ 📅 18/08/2026                   │
└─────────────────────────────────┘
```

Por defecto, el sistema podrá utilizar la fecha actual.

Dependiendo de los permisos del usuario, podrá permitirse modificar la fecha.

---

## 19. Método de Pago

El sistema podrá registrar el método utilizado.

Ejemplo:

```text
MÉTODO DE PAGO

[ EFECTIVO ▼ ]
```

Opciones iniciales:

- Efectivo.
- Transferencia.
- Yape.
- Plin.
- Otro.

El método podrá ampliarse posteriormente según las necesidades del negocio.

---

## 20. Observaciones

El usuario podrá registrar información adicional.

```text
OBSERVACIONES

┌─────────────────────────────────┐
│                                 │
│                                 │
│                                 │
└─────────────────────────────────┘
```

Ejemplos:

- Pago recibido por tercero.
- Pago parcial.
- Observación administrativa.

---

## 21. Confirmación del Pago

Antes de registrar el pago, se mostrará un resumen.

```text
┌─────────────────────────────────────┐
│           CONFIRMAR PAGO            │
├─────────────────────────────────────┤
│                                     │
│ Cliente                             │
│ Carlos Quispe                       │
│                                     │
│ Préstamo                            │
│ #0001                               │
│                                     │
│ Monto recibido                      │
│ S/ 100.00                           │
│                                     │
│ Aplicado a interés                  │
│ S/ 50.00                            │
│                                     │
│ Aplicado a capital                  │
│ S/ 50.00                            │
│                                     │
│ Nuevo capital pendiente             │
│ S/ 650.00                           │
│                                     │
│ Fecha                               │
│ 18/08/2026                          │
│                                     │
│ Método                              │
│ Efectivo                            │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ [ EDITAR ]       [ CONFIRMAR ]      │
└─────────────────────────────────────┘
```

---

## 22. Registro del Pago

Al presionar:

```text
[ CONFIRMAR ]
```

El sistema realizará:

```text
CONFIRMAR PAGO
        │
        ▼
Validar información
        │
        ├── Error
        │       │
        │       ▼
        │   Mostrar mensaje
        │
        └── Correcto
                │
                ▼
        Registrar pago
                │
                ▼
        Aplicar a interés
                │
                ▼
        Aplicar excedente a capital
                │
                ▼
        Actualizar capital pendiente
                │
                ▼
        Actualizar estado del préstamo
                │
                ▼
        Registrar historial
                │
                ▼
        Pago registrado
```

---

## 23. Finalización del Préstamo

Después de registrar un pago, el sistema deberá verificar:

```text
¿Capital pendiente = S/ 0.00?
        │
        ├── NO
        │    │
        │    ▼
        │ Préstamo continúa activo
        │
        └── SÍ
             │
             ▼
      Verificar obligaciones pendientes
             │
             ▼
       Finalizar préstamo
```

Cuando el préstamo cumpla RN-PRE-010 (`capital==0 AND intereses==0`):

```text
Estado:

CANCELADO
```

(`FINALIZADO` prohibido.)

---

## 24. Registro Exitoso

Cuando el pago se registre correctamente:

```text
✓ Pago registrado correctamente.
```

El usuario podrá seleccionar:

```text
[ VER PRÉSTAMO ]

[ REGISTRAR OTRO PAGO ]

[ VOLVER A PAGOS ]
```

---

## 25. Historial de Pagos

Desde el detalle del préstamo se podrá consultar el historial.

```text
HISTORIAL DE PAGOS

────────────────────────────

18/08/2026

Monto recibido:
S/ 100.00

Interés:
S/ 50.00

Capital:
S/ 50.00

────────────────────────────

11/08/2026

Monto recibido:
S/ 50.00

Interés:
S/ 50.00

Capital:
S/ 0.00
```

El historial deberá mantenerse incluso después de finalizar el préstamo.

---

## 26. Estados de Carga y Error

### Cargando información

```text
Cargando pagos...
```

### Registrando pago

```text
Registrando pago...
```

### Error

```text
No se pudo registrar el pago.

Verifique la información e intente nuevamente.

[ REINTENTAR ]
```

---

## 27. Mensajes de Validación

Ejemplos:

```text
Debe seleccionar un préstamo.
```

```text
Ingrese el monto recibido.
```

```text
El monto debe ser mayor que cero.
```

```text
No se puede registrar un pago en un préstamo finalizado.
```

```text
No se pudo procesar el pago. Intente nuevamente.
```

---

## 28. Flujo de Navegación

```text
DASHBOARD
    │
    ▼
PAGOS
    │
    ├── Buscar pagos
    │
    ├── Filtrar pagos
    │
    ├── Seleccionar pago
    │       │
    │       ▼
    │   Detalle del pago
    │
    └── Nuevo pago
            │
            ▼
      Seleccionar préstamo
            │
            ▼
       Ingresar monto
            │
            ▼
       Calcular distribución
            │
            ▼
       Registrar fecha
            │
            ▼
       Seleccionar método
            │
            ▼
       Agregar observación
            │
            ▼
       Confirmar pago
            │
            ▼
       Pago registrado
```

---

## 29. Consideraciones de Experiencia de Usuario

La pantalla deberá cumplir los siguientes principios:

1. Permitir localizar rápidamente un préstamo.
2. Mostrar claramente el capital pendiente.
3. Mostrar el interés pendiente antes de registrar el pago.
4. Calcular automáticamente la distribución del pago.
5. Evitar que el usuario realice cálculos manuales.
6. Mostrar claramente cuánto se aplicará al interés.
7. Mostrar claramente cuánto se aplicará al capital.
8. Mostrar el nuevo saldo antes de confirmar.
9. Evitar pagos duplicados.
10. Mantener un historial completo de todas las operaciones.

---

## 30. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│            REGISTRAR PAGO           │
├─────────────────────────────────────┤
│                                     │
│ 1. SELECCIONAR PRÉSTAMO             │
│                                     │
│ 2. VER ESTADO FINANCIERO            │
│    ├── Capital pendiente            │
│    └── Interés pendiente            │
│                                     │
│ 3. INGRESAR MONTO                   │
│    └── S/ __________                │
│                                     │
│ 4. CALCULAR DISTRIBUCIÓN            │
│    ├── Interés                      │
│    └── Capital                      │
│                                     │
│ 5. MÉTODO DE PAGO                   │
│                                     │
│ 6. OBSERVACIONES                    │
│                                     │
│       [ CONFIRMAR PAGO ]            │
└─────────────────────────────────────┘
```

---

## 31. Consideraciones Finales

El módulo de Pagos será responsable de registrar y controlar los pagos realizados por los clientes.

La funcionalidad principal deberá garantizar que cada pago sea procesado correctamente según las reglas financieras definidas.

El sistema deberá:

1. Registrar el monto recibido.
2. Identificar el interés pendiente.
3. Aplicar primero el pago al interés.
4. Aplicar el excedente al capital.
5. Actualizar el capital pendiente.
6. Registrar el movimiento en el historial.
7. Actualizar el estado del préstamo.
8. Detectar cuándo un préstamo puede finalizar.

El siguiente wireframe será:

`08-Wireframe-Morosidad.md`

Este documento definirá la pantalla para controlar préstamos atrasados, clientes morosos, pagos pendientes y el proceso de reactivación según las reglas de negocio.