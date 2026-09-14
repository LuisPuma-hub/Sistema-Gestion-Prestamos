# Wireframe - Registro de Préstamo

## 1. Introducción

Este documento define el wireframe para el proceso de registro de un nuevo préstamo dentro del Sistema de Gestión de Préstamos.

El registro de préstamos permitirá asociar un préstamo a un cliente previamente registrado, definir el monto del capital y establecer las condiciones necesarias para iniciar el seguimiento de pagos e intereses.

Un cliente podrá tener uno o varios préstamos registrados, según las reglas de negocio definidas para el sistema.

El sistema deberá calcular automáticamente la información relacionada con los intereses y pagos semanales de acuerdo con las condiciones establecidas.

---

## 2. Objetivo de la Pantalla

La pantalla deberá permitir:

- Seleccionar un cliente registrado.
- Consultar información básica del cliente.
- Registrar el monto del préstamo.
- Definir la fecha de inicio del préstamo.
- Calcular automáticamente el interés semanal.
- Visualizar el resumen financiero.
- Registrar observaciones.
- Validar la información.
- Confirmar el registro del préstamo.

---

## 3. Regla Principal del Préstamo

El sistema utilizará inicialmente la siguiente regla de negocio:

- El préstamo tendrá un capital inicial.
- El interés será del 5 % semanal sobre el capital inicial.
- El interés no será compuesto.
- El interés semanal se mantendrá calculado sobre el monto inicial del préstamo.
- El cliente realizará pagos semanales.
- Los pagos se aplicarán primero al interés pendiente.
- El monto restante de un pago podrá aplicarse al capital pendiente.
- El préstamo continuará activo mientras exista capital pendiente.

Ejemplo:

```text
Capital inicial: S/ 1,000.00

Interés semanal:
S/ 1,000.00 × 5 %

Interés semanal:
S/ 50.00
```

Por lo tanto:

```text
Capital inicial: S/ 1,000.00
Interés semanal: S/ 50.00
```

---

## 4. Flujo General del Registro

El proceso de registro seguirá la siguiente secuencia:

```text
REGISTRAR PRÉSTAMO
        │
        ▼
SELECCIONAR CLIENTE
        │
        ▼
INGRESAR MONTO DEL PRÉSTAMO
        │
        ▼
DEFINIR FECHA DE INICIO
        │
        ▼
CALCULAR INTERÉS SEMANAL
        │
        ▼
REGISTRAR OBSERVACIONES
        │
        ▼
REVISAR INFORMACIÓN
        │
        ▼
CONFIRMAR PRÉSTAMO
        │
        ▼
PRÉSTAMO REGISTRADO
```

---

## 5. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←       REGISTRAR PRÉSTAMO          │
├─────────────────────────────────────┤
│                                     │
│ CLIENTE                             │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 🔍 Buscar cliente...            │ │
│ └─────────────────────────────────┘ │
│                                     │
│ CLIENTE SELECCIONADO                │
│                                     │
│ Carlos Quispe                       │
│ DNI: 12345678                       │
│ Teléfono: 999999999                 │
│                                     │
│                                     │
│              [ SIGUIENTE ]          │
└─────────────────────────────────────┘
```

---

## 6. Selección del Cliente

El primer paso será seleccionar el cliente al cual se asociará el préstamo.

El usuario podrá buscar mediante:

- Nombres.
- Apellidos.
- Número de documento.
- Número de teléfono.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←       REGISTRAR PRÉSTAMO          │
├─────────────────────────────────────┤
│                                     │
│ SELECCIONAR CLIENTE                 │
│                                     │
│ 🔍 Buscar cliente                   │
│ ┌─────────────────────────────────┐ │
│ │ Nombre o documento              │ │
│ └─────────────────────────────────┘ │
│                                     │
│ RESULTADOS                          │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 Carlos Quispe                │ │
│ │ DNI: 12345678                   │ │
│ │ Estado: ACTIVO                  │ │
│ │                          [ + ]  │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 👤 María Flores                 │ │
│ │ DNI: 87654321                   │ │
│ │ Estado: ACTIVO                  │ │
│ │                          [ + ]  │ │
│ └─────────────────────────────────┘ │
│                                     │
└─────────────────────────────────────┘
```

Una vez seleccionado el cliente, se mostrará un resumen de su información.

---

## 7. Validación del Cliente

Antes de continuar, el sistema deberá validar que el cliente pueda recibir un nuevo préstamo.

Se deberán considerar las reglas de negocio relacionadas con:

- Estado del cliente.
- Restricciones administrativas.
- Observaciones registradas.
- Situación de morosidad.
- Permisos del usuario que registra el préstamo.

Flujo:

```text
SELECCIONAR CLIENTE
        │
        ▼
Validar estado
        │
        ├── Cliente no permitido
        │       │
        │       ▼
        │   Mostrar mensaje
        │
        └── Cliente válido
                │
                ▼
         Continuar registro
```

Ejemplo de mensaje:

```text
Este cliente no puede registrar un nuevo préstamo en su estado actual.
```

---

## 8. Monto del Préstamo

El usuario deberá ingresar el capital inicial.

```text
┌─────────────────────────────────────┐
│ ←       REGISTRAR PRÉSTAMO          │
├─────────────────────────────────────┤
│                                     │
│ DATOS DEL PRÉSTAMO                  │
│                                     │
│ Monto del préstamo                  │
│                                     │
│ S/ ┌──────────────────────────────┐ │
│    │                              │ │
│    └──────────────────────────────┘ │
│                                     │
│                                     │
│ [ ANTERIOR ]       [ SIGUIENTE ]    │
└─────────────────────────────────────┘
```

Características:

- Campo obligatorio.
- Deberá aceptar únicamente valores numéricos válidos.
- El monto deberá ser mayor que cero.
- El sistema deberá validar las restricciones definidas por el negocio.

Ejemplo:

```text
Monto del préstamo:

S/ 1,000.00
```

---

## 9. Fecha de Inicio

El usuario deberá definir la fecha de inicio del préstamo.

```text
┌─────────────────────────────────────┐
│                                     │
│ FECHA DE INICIO                     │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 📅 18/08/2026                   │ │
│ └─────────────────────────────────┘ │
│                                     │
│                                     │
│ [ ANTERIOR ]       [ SIGUIENTE ]    │
└─────────────────────────────────────┘
```

La fecha de inicio será utilizada para determinar el primer pago semanal.

Ejemplo:

```text
Fecha de inicio:

Lunes 18/08/2026
```

El siguiente pago será programado una semana después.

```text
Inicio:
18/08/2026

        │
        ▼

Primer pago:
25/08/2026
```

---

## 10. Cálculo del Interés Semanal

El sistema calculará automáticamente el interés semanal.

La fórmula será:

```text
Interés semanal = Capital inicial × 5 %
```

Ejemplo:

```text
Capital inicial:
S/ 1,000.00

Interés:
5 %

────────────────────────

INTERÉS SEMANAL

S/ 50.00
```

El usuario no deberá modificar manualmente el valor del interés si la tasa está definida como una regla fija del sistema.

---

## 11. Resumen Financiero

Antes de continuar, se mostrará un resumen del préstamo.

```text
┌─────────────────────────────────────┐
│         RESUMEN DEL PRÉSTAMO        │
├─────────────────────────────────────┤
│                                     │
│ Cliente                             │
│ Carlos Quispe                       │
│                                     │
│ Capital inicial                     │
│ S/ 1,000.00                         │
│                                     │
│ Tasa de interés semanal             │
│ 5 %                                 │
│                                     │
│ Interés semanal                     │
│ S/ 50.00                            │
│                                     │
│ Fecha de inicio                     │
│ 18/08/2026                          │
│                                     │
│ Primer pago                         │
│ 25/08/2026                          │
│                                     │
│ [ ANTERIOR ]       [ SIGUIENTE ]    │
└─────────────────────────────────────┘
```

---

## 12. Observaciones

El usuario podrá registrar información adicional relacionada con el préstamo.

```text
┌─────────────────────────────────────┐
│ ←       REGISTRAR PRÉSTAMO          │
├─────────────────────────────────────┤
│                                     │
│ OBSERVACIONES                       │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ │                                 │ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│                                     │
│ [ ANTERIOR ]       [ SIGUIENTE ]    │
└─────────────────────────────────────┘
```

Las observaciones podrán contener información administrativa relevante.

---

## 13. Revisión de la Información

Antes de confirmar el préstamo, el sistema mostrará un resumen completo.

```text
┌─────────────────────────────────────┐
│        CONFIRMAR PRÉSTAMO           │
├─────────────────────────────────────┤
│                                     │
│ CLIENTE                             │
│ Carlos Quispe                       │
│ DNI: 12345678                       │
│                                     │
│ DATOS DEL PRÉSTAMO                  │
│                                     │
│ Capital inicial                     │
│ S/ 1,000.00                         │
│                                     │
│ Interés semanal                     │
│ 5 %                                 │
│                                     │
│ Monto semanal de interés            │
│ S/ 50.00                            │
│                                     │
│ Fecha de inicio                     │
│ 18/08/2026                          │
│                                     │
│ Primer pago                         │
│ 25/08/2026                          │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ [ EDITAR ]       [ CONFIRMAR ]      │
└─────────────────────────────────────┘
```

El usuario podrá regresar a modificar la información antes de confirmar.

---

## 14. Confirmación del Préstamo

Cuando el usuario presione:

```text
[ CONFIRMAR ]
```

El sistema realizará el siguiente proceso:

```text
CONFIRMAR
    │
    ▼
Validar información
    │
    ├── Datos incorrectos
    │       │
    │       ▼
    │   Mostrar errores
    │
    └── Datos correctos
            │
            ▼
       Registrar préstamo
            │
            ▼
      Crear registro inicial
            │
            ▼
      Calcular primer pago
            │
            ▼
      Programar recordatorio
            │
            ▼
      Préstamo registrado
```

---

## 15. Creación del Calendario de Pagos

Después de registrar el préstamo, el sistema deberá generar la información necesaria para controlar los pagos.

La programación inicial será semanal.

Ejemplo:

```text
PRÉSTAMO INICIADO

18/08/2026
        │
        ▼
SEMANA 1

25/08/2026
Interés: S/ 50.00
        │
        ▼
SEMANA 2

01/09/2026
Interés: S/ 50.00
        │
        ▼
SEMANA 3

08/09/2026
Interés: S/ 50.00
```

El ciclo continuará mientras exista capital pendiente.

---

## 16. Registro Exitoso

Cuando el préstamo sea registrado correctamente:

```text
✓ Préstamo registrado correctamente.
```

El usuario podrá seleccionar:

```text
[ VER PRÉSTAMO ]

[ REGISTRAR PAGO ]

[ VOLVER A PRÉSTAMOS ]
```

---

## 17. Mensajes de Validación

Ejemplos:

```text
Debe seleccionar un cliente.
```

```text
Ingrese el monto del préstamo.
```

```text
El monto debe ser mayor que cero.
```

```text
Seleccione una fecha válida.
```

```text
El cliente no puede recibir un préstamo en su estado actual.
```

Los mensajes deberán ser claros y mostrarse cerca del campo correspondiente cuando sea posible.

---

## 18. Estados del Proceso

El registro del préstamo deberá considerar los siguientes estados:

### Estado inicial

Todavía no se ha seleccionado un cliente.

### Estado de edición

El usuario está ingresando los datos del préstamo.

### Estado de cálculo

El sistema calcula automáticamente el interés semanal.

### Estado de validación

El sistema verifica los datos ingresados.

### Estado de carga

Se está registrando la información.

```text
Registrando préstamo...
```

### Estado de error

Se muestra un mensaje indicando el problema.

### Estado exitoso

El préstamo fue registrado correctamente.

---

## 19. Flujo de Navegación

```text
LISTA DE PRÉSTAMOS
        │
        ▼
BOTÓN "+"
        │
        ▼
SELECCIONAR CLIENTE
        │
        ▼
VALIDAR CLIENTE
        │
        ▼
INGRESAR MONTO
        │
        ▼
SELECCIONAR FECHA DE INICIO
        │
        ▼
CALCULAR INTERÉS SEMANAL
        │
        ▼
REGISTRAR OBSERVACIONES
        │
        ▼
REVISAR INFORMACIÓN
        │
        ▼
CONFIRMAR PRÉSTAMO
        │
        ▼
PRÉSTAMO REGISTRADO
        │
        ├── Ver préstamo
        │
        ├── Registrar pago
        │
        └── Volver a préstamos
```

---

## 20. Consideraciones de Experiencia de Usuario

El proceso deberá cumplir los siguientes principios:

1. Permitir buscar y seleccionar rápidamente un cliente.
2. Mostrar claramente la información del cliente seleccionado.
3. Evitar cálculos manuales innecesarios.
4. Calcular automáticamente el interés semanal.
5. Mostrar el resumen financiero antes de confirmar.
6. Permitir regresar a pasos anteriores.
7. Mantener la información ingresada al retroceder.
8. Mostrar mensajes de validación claros.
9. Evitar registrar el mismo préstamo varias veces por error.
10. Mostrar una confirmación clara después del registro.

---

## 21. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│       REGISTRAR PRÉSTAMO            │
├─────────────────────────────────────┤
│                                     │
│ 1. SELECCIONAR CLIENTE              │
│    └── Buscar y seleccionar         │
│                                     │
│ 2. MONTO DEL PRÉSTAMO               │
│    └── S/ __________                │
│                                     │
│ 3. FECHA DE INICIO                  │
│    └── 📅 __/__/____                │
│                                     │
│ 4. INTERÉS SEMANAL                  │
│    ├── Tasa: 5 %                    │
│    └── Calculado automáticamente    │
│                                     │
│ 5. OBSERVACIONES                    │
│                                     │
│ 6. CONFIRMAR INFORMACIÓN            │
│                                     │
│       [ REGISTRAR PRÉSTAMO ]        │
└─────────────────────────────────────┘
```

---

## 22. Consideraciones Finales

El registro de préstamos será uno de los procesos centrales del Sistema de Gestión de Préstamos.

El diseño deberá garantizar que la información sea registrada correctamente y que los cálculos financieros se realicen automáticamente según las reglas de negocio.

Después del registro, el préstamo podrá:

- Ser consultado desde la lista de préstamos.
- Tener múltiples pagos asociados.
- Generar intereses semanales sobre el capital inicial.
- Generar recordatorios de pago.
- Ser identificado como atrasado cuando corresponda.
- Formar parte del módulo de morosidad.
- Finalizar cuando el capital pendiente llegue a cero y se cumplan las condiciones establecidas.

El siguiente wireframe será:

`07-Wireframe-Pagos.md`

Este documento definirá la pantalla para consultar y registrar pagos, incluyendo la selección del préstamo, el cálculo de intereses pendientes, la aplicación del pago primero al interés y posteriormente al capital.