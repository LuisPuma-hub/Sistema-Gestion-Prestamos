# Wireframe - Registro de Cliente

## 1. Introducción

Este documento define el wireframe para el proceso de registro de un nuevo cliente dentro del Sistema de Gestión de Préstamos.

El registro de clientes es una de las operaciones principales del sistema, ya que la información almacenada será utilizada posteriormente para gestionar préstamos, pagos, morosidad, notificaciones y comunicaciones mediante WhatsApp.

La pantalla deberá organizar la información de manera clara para evitar errores durante el registro.

---

## 2. Objetivo de la Pantalla

La pantalla deberá permitir registrar un nuevo cliente incluyendo:

- Tipo de documento.
- Número de documento.
- Nombres.
- Apellidos.
- Número de teléfono.
- Dirección.
- Referencia de dirección.
- Imagen de recibo de domicilio.
- Información del aval.
- Observaciones.
- Estado inicial del cliente.

---

## 3. Flujo General del Registro

El proceso de registro seguirá la siguiente secuencia:

```text
REGISTRAR CLIENTE
        │
        ▼
DATOS PERSONALES
        │
        ▼
DIRECCIÓN
        │
        ▼
DOCUMENTO DE DOMICILIO
        │
        ▼
INFORMACIÓN DEL AVAL
        │
        ▼
OBSERVACIONES
        │
        ▼
VALIDAR INFORMACIÓN
        │
        ▼
GUARDAR CLIENTE
        │
        ▼
CLIENTE REGISTRADO
```

---

## 4. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←       REGISTRAR CLIENTE           │
├─────────────────────────────────────┤
│                                     │
│ DATOS PERSONALES                    │
│                                     │
│ Tipo de documento                   │
│                                     │
│  (●) DNI        ( ) CE              │
│                                     │
│ Número de documento                 │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Nombres                             │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Apellidos                           │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Teléfono                            │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│                                     │
│              [ SIGUIENTE ]          │
└─────────────────────────────────────┘
```

La información podrá organizarse en varias secciones o pasos para evitar una pantalla demasiado extensa.

---

## 5. Sección de Datos Personales

La primera sección permitirá registrar la información básica del cliente.

### Campos

- Tipo de documento.
- Número de documento.
- Nombres.
- Apellidos.
- Teléfono.

---

### 5.1 Tipo de Documento

El sistema permitirá seleccionar el tipo de documento.

Opciones iniciales:

```text
(●) DNI

( ) CE
```

El tipo seleccionado determinará las validaciones aplicadas al número de documento.

---

### 5.2 Número de Documento

```text
Número de documento

┌─────────────────────────────────┐
│                                 │
└─────────────────────────────────┘
```

Características:

- Campo obligatorio.
- No deberá existir otro cliente con el mismo tipo y número de documento.
- Deberá validarse según el tipo seleccionado.

Ejemplo:

```text
DNI: 12345678
```

---

### 5.3 Nombres

```text
Nombres

┌─────────────────────────────────┐
│                                 │
└─────────────────────────────────┘
```

Características:

- Campo obligatorio.
- Permitirá registrar uno o más nombres.

Ejemplo:

```text
Carlos Alberto
```

---

### 5.4 Apellidos

```text
Apellidos

┌─────────────────────────────────┐
│                                 │
└─────────────────────────────────┘
```

Características:

- Campo obligatorio.
- Permitirá registrar los apellidos del cliente.

Ejemplo:

```text
Quispe Flores
```

---

### 5.5 Número de Teléfono

```text
Teléfono

┌─────────────────────────────────┐
│                                 │
└─────────────────────────────────┘
```

Características:

- Campo obligatorio.
- Deberá contener un número válido.
- Será utilizado para comunicaciones y WhatsApp.

Ejemplo:

```text
999999999
```

---

## 6. Sección de Dirección

Después de completar los datos personales, el usuario continuará con la información de domicilio.

```text
┌─────────────────────────────────────┐
│ ←       REGISTRAR CLIENTE           │
├─────────────────────────────────────┤
│                                     │
│ DIRECCIÓN                           │
│                                     │
│ Dirección                           │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Referencia                          │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│                                     │
│ [ ANTERIOR ]       [ SIGUIENTE ]    │
└─────────────────────────────────────┘
```

### Campos

- Dirección.
- Referencia.

La referencia permitirá registrar información adicional para facilitar la ubicación del cliente.

Ejemplo:

```text
Frente al parque principal.
```

---

## 7. Documento de Domicilio

El sistema permitirá registrar una imagen de un recibo de servicios.

Podrá utilizarse:

- Recibo de luz.
- Recibo de agua.

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←       REGISTRAR CLIENTE           │
├─────────────────────────────────────┤
│                                     │
│ DOCUMENTO DE DOMICILIO              │
│                                     │
│ Adjunte un recibo de luz o agua     │
│                                     │
│                                     │
│        ┌───────────────────┐        │
│        │                   │        │
│        │       📷          │        │
│        │                   │        │
│        │   TOMAR FOTO /    │        │
│        │   SELECCIONAR     │        │
│        │                   │        │
│        └───────────────────┘        │
│                                     │
│                                     │
│ [ ANTERIOR ]       [ SIGUIENTE ]    │
└─────────────────────────────────────┘
```

Una vez seleccionada la imagen, el sistema mostrará una vista previa.

```text
┌─────────────────────────────────┐
│                                 │
│       VISTA PREVIA DEL RECIBO   │
│                                 │
│                                 │
└─────────────────────────────────┘

[ CAMBIAR IMAGEN ]
```

La imagen deberá almacenarse de forma segura.

---

## 8. Información del Aval

El cliente podrá tener un aval asociado.

El usuario podrá seleccionar entre dos opciones:

```text
¿Cómo desea registrar el aval?

(●) Seleccionar cliente existente

( ) Registrar nuevo aval
```

---

## 9. Aval como Cliente Existente

Si el aval ya se encuentra registrado como cliente, se podrá buscar y seleccionar.

```text
┌─────────────────────────────────────┐
│ ←              AVAL                 │
├─────────────────────────────────────┤
│                                     │
│ 🔍 Buscar cliente                   │
│ ┌─────────────────────────────────┐ │
│ │ Nombre o documento              │ │
│ └─────────────────────────────────┘ │
│                                     │
│ RESULTADOS                          │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Carlos Pérez                    │ │
│ │ DNI: 12345678                   │ │
│ │                          [ + ]  │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ ANTERIOR ]       [ SIGUIENTE ]    │
└─────────────────────────────────────┘
```

Al seleccionar un cliente, se mostrará la información básica del aval seleccionado.

---

## 10. Registrar Nuevo Aval

Si el aval no existe como cliente, se podrá registrar una nueva persona.

```text
┌─────────────────────────────────────┐
│ ←         REGISTRAR AVAL            │
├─────────────────────────────────────┤
│                                     │
│ Nombres                             │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Apellidos                           │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Teléfono                            │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Dirección                           │
│ ┌─────────────────────────────────┐ │
│ │                                 │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ ANTERIOR ]       [ SIGUIENTE ]    │
└─────────────────────────────────────┘
```

Los datos iniciales del nuevo aval serán:

- Nombres.
- Apellidos.
- Teléfono.
- Dirección.

---

## 11. Sección de Observaciones

Esta sección permitirá registrar información adicional sobre el cliente.

```text
┌─────────────────────────────────────┐
│ ←       REGISTRAR CLIENTE           │
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
│ Estado inicial                      │
│                                     │
│ [ ACTIVO ▼ ]                        │
│                                     │
│ [ ANTERIOR ]       [ FINALIZAR ]    │
└─────────────────────────────────────┘
```

Los estados iniciales podrán ser:

- ACTIVO.
- OBSERVACIÓN.

El estado MOROSO será determinado posteriormente por las reglas relacionadas con los préstamos y pagos.

---

## 12. Validación de Datos

Antes de guardar el cliente, el sistema deberá validar toda la información.

Flujo:

```text
FINALIZAR
    │
    ▼
Validar datos
    │
    ├── Datos incorrectos
    │       │
    │       ▼
    │   Mostrar errores
    │
    └── Datos correctos
            │
            ▼
      Enviar información
            │
            ▼
      Backend procesa registro
            │
            ├── Error
            │       │
            │       ▼
            │   Mostrar mensaje
            │
            └── Correcto
                    │
                    ▼
             Cliente registrado
```

---

## 13. Mensajes de Validación

Ejemplos de mensajes:

```text
El número de documento es obligatorio.
```

```text
Ingrese los nombres del cliente.
```

```text
Ingrese los apellidos del cliente.
```

```text
Ingrese un número de teléfono válido.
```

```text
Ya existe un cliente con este documento.
```

Los mensajes deberán mostrarse cerca del campo correspondiente.

---

## 14. Confirmación del Registro

Antes de registrar definitivamente al cliente, se podrá mostrar una pantalla de resumen.

```text
┌─────────────────────────────────────┐
│        CONFIRMAR REGISTRO           │
├─────────────────────────────────────┤
│                                     │
│ Cliente: Carlos Quispe              │
│ Documento: DNI 12345678             │
│ Teléfono: 999999999                 │
│                                     │
│ Dirección:                          │
│ Calle Principal 123                 │
│                                     │
│ Aval:                               │
│ María Flores                        │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ [ EDITAR ]       [ CONFIRMAR ]      │
└─────────────────────────────────────┘
```

El usuario podrá regresar a editar la información antes de confirmar.

---

## 15. Registro Exitoso

Cuando el registro sea exitoso, se mostrará un mensaje.

```text
✓ Cliente registrado correctamente.
```

Después de registrar al cliente, el usuario podrá elegir:

```text
[ VER CLIENTE ]

[ REGISTRAR PRÉSTAMO ]

[ VOLVER A CLIENTES ]
```

Esto permitirá continuar rápidamente con el flujo de trabajo.

---

## 16. Flujo de Navegación

```text
LISTA DE CLIENTES
        │
        ▼
BOTÓN "+"
        │
        ▼
DATOS PERSONALES
        │
        ▼
DIRECCIÓN
        │
        ▼
DOCUMENTO DE DOMICILIO
        │
        ▼
AVAL
        │
        ├── Cliente existente
        │
        └── Nuevo aval
        │
        ▼
OBSERVACIONES
        │
        ▼
CONFIRMAR INFORMACIÓN
        │
        ▼
REGISTRAR CLIENTE
        │
        ▼
CLIENTE REGISTRADO
        │
        ├── Ver cliente
        │
        ├── Registrar préstamo
        │
        └── Volver a clientes
```

---

## 17. Estados de la Pantalla

El proceso de registro deberá considerar los siguientes estados:

### Estado inicial

Los campos están vacíos.

### Estado de edición

El usuario está ingresando información.

### Estado de validación

El sistema verifica los datos ingresados.

### Estado de carga

Se está enviando la información al servidor.

```text
Registrando cliente...
```

### Estado de error

Se muestra un mensaje indicando el problema.

### Estado exitoso

El cliente fue registrado correctamente.

---

## 18. Consideraciones de Experiencia de Usuario

El proceso deberá cumplir los siguientes principios:

1. Dividir el formulario en secciones claras.
2. Evitar mostrar demasiados campos simultáneamente.
3. Mantener visible el progreso del registro.
4. Validar los datos antes de avanzar.
5. Permitir regresar a pasos anteriores.
6. No perder la información ingresada al cambiar de sección.
7. Mostrar mensajes claros.
8. Permitir tomar o seleccionar la imagen del recibo.
9. Facilitar la búsqueda de un aval existente.
10. Permitir continuar rápidamente con el registro de un préstamo después de crear un cliente.

---

## 19. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│        REGISTRAR CLIENTE            │
├─────────────────────────────────────┤
│                                     │
│ 1. DATOS PERSONALES                 │
│    ├── Documento                    │
│    ├── Nombres                      │
│    ├── Apellidos                    │
│    └── Teléfono                     │
│                                     │
│ 2. DIRECCIÓN                        │
│    ├── Dirección                    │
│    └── Referencia                   │
│                                     │
│ 3. DOCUMENTO DE DOMICILIO           │
│    └── Imagen de recibo             │
│                                     │
│ 4. AVAL                             │
│    ├── Cliente existente            │
│    └── Nuevo aval                   │
│                                     │
│ 5. OBSERVACIONES                    │
│    └── Estado inicial               │
│                                     │
│         [ GUARDAR CLIENTE ]         │
└─────────────────────────────────────┘
```

---

## 20. Consideraciones Finales

El registro de clientes es un proceso fundamental dentro del Sistema de Gestión de Préstamos.

El diseño deberá facilitar el ingreso de información y reducir errores mediante validaciones claras.

Después de completar este proceso, el cliente podrá:

- Ser consultado desde la lista de clientes.
- Tener uno o varios préstamos asociados, según las reglas del sistema.
- Registrar pagos relacionados con sus préstamos.
- Recibir notificaciones.
- Recibir mensajes mediante WhatsApp.
- Ser identificado en los procesos de morosidad.

El siguiente wireframe será:

`05-Wireframe-Prestamos.md`

Este documento definirá la pantalla principal para consultar, buscar y gestionar los préstamos registrados en el sistema.