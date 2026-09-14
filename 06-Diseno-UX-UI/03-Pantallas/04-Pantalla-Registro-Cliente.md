# 04. Pantalla de Registro de Cliente

## 1. Información general

| Campo | Descripción |
|---|---|
| Nombre | Registro de Cliente |
| Archivo | `04-Pantalla-Registro-Cliente.md` |
| Módulo | Gestión de Clientes |
| Tipo | Formulario |
| Prioridad | Alta |
| Acceso | Usuarios autorizados |
| Plataforma | Aplicación móvil |
| Estado | Diseño |

---

## 2. Objetivo

La pantalla de Registro de Cliente permite ingresar al sistema la información de una nueva persona que podrá solicitar y recibir préstamos.

El formulario deberá recopilar información de identificación, contacto, documentación y observaciones administrativas.

La información registrada deberá ser validada antes de almacenarse.

---

## 3. Acceso

La pantalla podrá abrirse desde:

```text
Dashboard
   │
   ▼
Clientes
   │
   ▼
+ Nuevo cliente
   │
   ▼
Registro de Cliente
```

También podrá accederse mediante acciones rápidas desde el Dashboard.

---

## 4. Estructura general

```text
┌─────────────────────────────────────┐
│ ←  Nuevo Cliente                    │
├─────────────────────────────────────┤
│                                     │
│ DATOS DE IDENTIFICACIÓN             │
│                                     │
│ Tipo de documento                   │
│ ○ DNI       ○ CE                    │
│                                     │
│ Número de documento                 │
│ ┌───────────────────────────────┐   │
│ │ 72456321                      │   │
│ └───────────────────────────────┘   │
│                                     │
│ Nombres                             │
│ ┌───────────────────────────────┐   │
│ │ Juan Carlos                   │   │
│ └───────────────────────────────┘   │
│                                     │
│ Apellidos                           │
│ ┌───────────────────────────────┐   │
│ │ Pérez López                   │   │
│ └───────────────────────────────┘   │
│                                     │
│ DATOS DE CONTACTO                   │
│                                     │
│ Teléfono                            │
│ ┌───────────────────────────────┐   │
│ │ 987654321                     │   │
│ └───────────────────────────────┘   │
│                                     │
│ DOCUMENTACIÓN                       │
│                                     │
│ Recibo de agua / electricidad      │
│ ┌───────────────────────────────┐   │
│ │       📷 Agregar imagen        │   │
│ └───────────────────────────────┘   │
│                                     │
│ OBSERVACIONES                       │
│ ┌───────────────────────────────┐   │
│ │                               │   │
│ └───────────────────────────────┘   │
│                                     │
│ [ Cancelar ] [ Guardar cliente ]    │
│                                     │
└─────────────────────────────────────┘
```

---

## 5. Sección de identificación

La primera sección del formulario corresponde a los datos personales del cliente.

```text
DATOS DE IDENTIFICACIÓN
```

Contendrá:

- Tipo de documento.
- Número de documento.
- Nombres.
- Apellidos.

---

## 6. Tipo de documento

El sistema permitirá seleccionar el tipo de documento.

Opciones:

```text
○ DNI
○ CE
```

Donde:

- DNI = Documento Nacional de Identidad.
- CE = Carné de Extranjería.

Solo deberá existir un tipo seleccionado al momento de guardar.

---

## 7. Número de documento

Campo utilizado para registrar el número del documento de identidad.

```text
Número de documento

[________________________]
```

### Validaciones

- Obligatorio.
- No debe estar vacío.
- Debe corresponder al tipo de documento seleccionado.
- No debe existir otro cliente con el mismo tipo y número de documento.
- Debe eliminar espacios innecesarios.
- Debe validarse también en el backend.

---

## 8. Nombres

Campo utilizado para registrar los nombres del cliente.

```text
Nombres

[________________________]
```

### Validaciones

- Obligatorio.
- No debe aceptar únicamente espacios.
- Debe permitir nombres compuestos.
- Se recomienda normalizar espacios.
- Debe conservarse correctamente la información ingresada.

---

## 9. Apellidos

Campo utilizado para registrar los apellidos.

```text
Apellidos

[________________________]
```

### Validaciones

- Obligatorio.
- No debe aceptar únicamente espacios.
- Debe permitir apellidos compuestos.
- Se recomienda normalizar espacios.

---

## 10. Sección de contacto

La segunda sección contiene la información necesaria para comunicarse con el cliente.

```text
DATOS DE CONTACTO
```

---

## 11. Número de teléfono

Campo utilizado para registrar el número telefónico del cliente.

```text
Teléfono

[________________________]
```

El número será importante para:

- Contacto directo.
- Recordatorios de pago.
- Notificaciones relacionadas con el préstamo.
- Integración con WhatsApp.
- Mensajes personalizados.

### Validaciones

- Obligatorio.
- Debe contener únicamente caracteres permitidos.
- Debe cumplir el formato establecido para números telefónicos.
- Debe validarse antes de utilizarlo para WhatsApp.

---

## 12. Sección de documentación

La aplicación permitirá registrar una imagen del recibo de agua o electricidad del cliente.

```text
DOCUMENTACIÓN
```

Esta documentación podrá utilizarse como información de respaldo del cliente.

---

## 13. Imagen del recibo

El usuario podrá agregar una imagen utilizando:

- Cámara.
- Galería del dispositivo.

Interfaz:

```text
┌───────────────────────────────┐
│                               │
│       📷 Agregar imagen       │
│                               │
└───────────────────────────────┘
```

Después de seleccionar la imagen:

```text
┌───────────────────────────────┐
│                               │
│       Vista previa            │
│                               │
│      [ Imagen recibo ]        │
│                               │
│  [Cambiar]     [Eliminar]     │
└───────────────────────────────┘
```

---

## 14. Validaciones de imagen

El sistema deberá verificar:

- Que el archivo sea una imagen válida.
- Que el tamaño no supere el límite establecido.
- Que el formato sea compatible.
- Que la imagen pueda ser cargada correctamente.
- Que el usuario pueda reemplazarla antes de guardar.

Formatos recomendados:

- JPG.
- JPEG.
- PNG.

---

## 15. Observaciones

Se incluirá un campo para registrar comentarios administrativos relacionados con el cliente.

```text
Observaciones

┌─────────────────────────────────┐
│                                 │
│                                 │
│                                 │
└─────────────────────────────────┘
```

Las observaciones podrán utilizarse para:

- Registrar información adicional.
- Registrar advertencias administrativas.
- Registrar situaciones particulares.
- Dejar constancia de información relevante.

No deberán utilizarse para almacenar contraseñas, tokens u otra información sensible que no corresponda.

---

## 16. Estado inicial del cliente

Al registrarse un nuevo cliente, el sistema deberá asignar un estado inicial.

Estado recomendado:

```text
Activo
```

El estado deberá ser gestionado por las reglas del negocio.

Los estados contemplados son:

- Activo.
- Observación.
- Moroso.

El usuario no debería establecer manualmente el estado `Moroso` durante el registro. Este estado deberá derivarse de la situación de pagos y las reglas de morosidad.

---

## 17. Botón Guardar cliente

El formulario deberá incluir:

```text
[ Guardar cliente ]
```

Al seleccionarlo se deberá:

1. Validar los campos.
2. Mostrar los errores correspondientes.
3. Preparar la información.
4. Enviar los datos al backend.
5. Crear el registro.
6. Confirmar la operación.
7. Regresar al listado o mostrar el detalle del nuevo cliente.

---

## 18. Botón Cancelar

El botón:

```text
[ Cancelar ]
```

permitirá abandonar el formulario.

Si existen datos ingresados, se recomienda solicitar confirmación:

```text
¿Deseas salir?

Los datos ingresados no se guardarán.

[Continuar editando] [Salir]
```

---

## 19. Estado de validación

Cuando existan errores, deberán mostrarse junto al campo correspondiente.

Ejemplo:

```text
Número de documento

[                      ]

⚠ El número de documento es obligatorio.
```

Otro ejemplo:

```text
Teléfono

[ 98765 ]

⚠ Ingresa un número de teléfono válido.
```

---

## 20. Validación de cliente duplicado

Antes de registrar al cliente, el sistema deberá verificar si ya existe una persona con el mismo documento.

Ejemplo:

```text
⚠ Ya existe un cliente registrado
   con este número de documento.
```

El registro no deberá crearse hasta solucionar la duplicidad.

---

## 21. Estado de guardado

Mientras el sistema procesa el registro:

```text
[ Guardando cliente... ]
```

Durante este proceso se deberá impedir el envío repetido del formulario.

---

## 22. Registro exitoso

Cuando el cliente sea creado correctamente:

```text
✓ Cliente registrado correctamente.
```

Después de la confirmación, el sistema podrá:

- Regresar a Clientes.
- Abrir el detalle del cliente.
- Permitir registrar un préstamo.

Flujo recomendado:

```text
Registro de Cliente
        │
        ▼
Cliente creado
        │
        ▼
Detalle del cliente
```

---

## 23. Error de registro

Si ocurre un problema:

```text
No se pudo registrar el cliente.

Verifica la información e inténtalo nuevamente.
```

Los datos introducidos deberían conservarse siempre que sea técnicamente posible.

---

## 24. Flujo completo

```text
                 ┌──────────────────────┐
                 │   Clientes           │
                 └──────────┬───────────┘
                            │
                            ▼
                 ┌──────────────────────┐
                 │ + Nuevo cliente      │
                 └──────────┬───────────┘
                            │
                            ▼
                 ┌──────────────────────┐
                 │ Registro de cliente  │
                 └──────────┬───────────┘
                            │
                            ▼
                    Completar datos
                            │
                            ▼
                     Validar datos
                            │
                       ¿Correctos?
                      /           \
                    NO             SÍ
                    │               │
                    ▼               ▼
              Mostrar errores   Enviar backend
                                    │
                                    ▼
                             Crear cliente
                                    │
                               ¿Correcto?
                              /          \
                            NO            SÍ
                            │              │
                            ▼              ▼
                         Error        Confirmación
                                          │
                                          ▼
                                  Detalle del cliente
```

---

## 25. Datos enviados al backend

La aplicación podrá enviar una estructura equivalente a:

```text
{
  "documentType": "DNI",
  "documentNumber": "72456321",
  "firstNames": "Juan Carlos",
  "lastNames": "Pérez López",
  "phone": "987654321",
  "receiptImage": "archivo",
  "observations": "Cliente nuevo"
}
```

La estructura definitiva deberá coincidir con el contrato de la API.

---

## 26. Datos de respuesta

Una respuesta exitosa podrá tener una estructura equivalente a:

```text
{
  "success": true,
  "message": "Cliente registrado correctamente",
  "client": {
    "id": 125,
    "documentType": "DNI",
    "documentNumber": "72456321",
    "firstNames": "Juan Carlos",
    "lastNames": "Pérez López",
    "phone": "987654321",
    "status": "active"
  }
}
```

Los nombres definitivos de los campos dependerán de la API.

---

## 27. Reglas de negocio relacionadas

### RN-CLIENTE-01

Todo cliente deberá tener un tipo de documento.

### RN-CLIENTE-02

Todo cliente deberá tener un número de documento válido.

### RN-CLIENTE-03

No podrá existir más de un cliente con el mismo tipo y número de documento.

### RN-CLIENTE-04

El nombre y apellido del cliente serán obligatorios.

### RN-CLIENTE-05

El número telefónico será obligatorio para facilitar la comunicación con el cliente.

### RN-CLIENTE-06

El recibo de agua o electricidad deberá almacenarse como documentación asociada al cliente cuando sea requerido por el proceso de registro.

### RN-CLIENTE-07

Un cliente nuevo tendrá inicialmente estado activo, salvo que exista una regla específica que determine lo contrario.

### RN-CLIENTE-08

El estado de morosidad no será asignado arbitrariamente durante el registro.

### RN-CLIENTE-09

Un cliente podrá tener múltiples préstamos.

---

## 28. Seguridad y privacidad

La información del cliente constituye información personal y deberá manejarse adecuadamente.

Se deberá:

- Utilizar HTTPS.
- Controlar el acceso mediante autenticación.
- Aplicar autorización por rol.
- Proteger las imágenes almacenadas.
- Evitar exponer documentos innecesariamente.
- Validar los datos también en el backend.
- No almacenar información sensible sin justificación.
- Registrar operaciones importantes cuando corresponda.

---

## 29. Accesibilidad

El formulario deberá:

- Utilizar etiquetas claras.
- Mantener tamaños de texto legibles.
- Proporcionar mensajes de error comprensibles.
- Permitir navegación lógica entre campos.
- Utilizar teclados apropiados.
- Mantener botones con tamaño táctil adecuado.
- No depender exclusivamente del color para comunicar errores.

---

## 30. Diseño responsive

El formulario deberá adaptarse a diferentes tamaños de pantalla.

En dispositivos pequeños:

```text
┌───────────────────────┐
│ ← Nuevo Cliente       │
├───────────────────────┤
│ Tipo documento        │
│ ○ DNI   ○ CE          │
│                       │
│ Número documento      │
│ [___________________] │
│                       │
│ Nombres               │
│ [___________________] │
│                       │
│ Apellidos             │
│ [___________________] │
│                       │
│ Teléfono              │
│ [___________________] │
│                       │
│ Recibo                 │
│ [ 📷 Agregar imagen ] │
│                       │
│ Observaciones         │
│ [___________________] │
│ [___________________] │
│                       │
│ [ Guardar cliente ]   │
└───────────────────────┘
```

---

## 31. Componentes funcionales

La pantalla podrá estructurarse mediante:

```text
ClientRegistrationScreen
│
├── RegistrationHeader
│
├── DocumentTypeSelector
│
├── DocumentNumberInput
│
├── FirstNamesInput
│
├── LastNamesInput
│
├── PhoneInput
│
├── ReceiptImagePicker
│   ├── CameraOption
│   ├── GalleryOption
│   └── ImagePreview
│
├── ObservationsInput
│
├── CancelButton
│
└── SaveClientButton
```

---

## 32. Criterios de aceptación

La pantalla será considerada terminada cuando:

- [ ] Se pueda seleccionar DNI.
- [ ] Se pueda seleccionar CE.
- [ ] Se pueda ingresar el número de documento.
- [ ] Se valide el número de documento.
- [ ] Se pueda ingresar nombres.
- [ ] Se pueda ingresar apellidos.
- [ ] Se pueda ingresar el número de teléfono.
- [ ] Se pueda agregar una imagen del recibo.
- [ ] Se pueda utilizar la cámara.
- [ ] Se pueda seleccionar una imagen desde la galería.
- [ ] Se pueda visualizar una vista previa.
- [ ] Se pueda reemplazar la imagen.
- [ ] Se pueda eliminar la imagen antes de guardar.
- [ ] Se puedan ingresar observaciones.
- [ ] Se validen todos los campos obligatorios.
- [ ] Se detecten clientes duplicados.
- [ ] Se muestre el estado de carga.
- [ ] Se impida el envío duplicado.
- [ ] Se muestre confirmación después del registro.
- [ ] Se gestione correctamente el error.
- [ ] Se pueda cancelar el registro.
- [ ] Se respete el sistema de permisos.
- [ ] Se protejan los datos personales.
- [ ] La pantalla sea responsive.

---

## 33. Resultado esperado

La pantalla de Registro de Cliente debe permitir crear un registro completo, válido y consistente dentro del sistema.

Una vez finalizado el registro, el cliente deberá estar disponible en el módulo de Clientes y podrá posteriormente asociarse a uno o varios préstamos.

La información registrada deberá servir como base para las operaciones posteriores de préstamos, pagos, morosidad, notificaciones y comunicación mediante WhatsApp.