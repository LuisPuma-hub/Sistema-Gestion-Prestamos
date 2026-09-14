# Wireframe - Perfil

## 1. Introducción

Este documento define el wireframe de la pantalla de Perfil del usuario dentro del Sistema de Gestión de Préstamos.

Esta pantalla permitirá al usuario autorizado consultar y gestionar la información relacionada con su cuenta, incluyendo sus datos personales, información de acceso, seguridad y sesión.

El módulo estará disponible para los usuarios registrados en el sistema, como administradores y otros usuarios que puedan incorporarse posteriormente.

---

## 2. Objetivo de la Pantalla

La pantalla de Perfil deberá permitir:

- Visualizar la información del usuario.
- Consultar los datos de la cuenta.
- Editar información personal.
- Consultar el rol asignado.
- Cambiar la contraseña.
- Gestionar opciones de seguridad.
- Cerrar sesión.
- Acceder a la configuración relacionada con la cuenta.

---

## 3. Información del Perfil

La pantalla mostrará inicialmente:

- Foto de perfil.
- Nombre completo.
- Correo electrónico.
- Número de teléfono.
- Rol del usuario.
- Estado de la cuenta.

Ejemplo:

```text
Nombre:
Luis Pérez

Correo:
luis@email.com

Teléfono:
999999999

Rol:
Administrador

Estado:
Activo
```

---

## 4. Wireframe Principal

```text
┌─────────────────────────────────────┐
│ ←              PERFIL               │
├─────────────────────────────────────┤
│                                     │
│              ┌───────┐              │
│              │  👤   │              │
│              └───────┘              │
│                                     │
│           Luis Pérez                 │
│        Administrador                 │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ INFORMACIÓN PERSONAL                │
│                                     │
│ 👤 Nombre completo             >    │
│ 📧 Correo electrónico          >    │
│ 📱 Teléfono                    >    │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ CUENTA Y SEGURIDAD                  │
│                                     │
│ 🔒 Cambiar contraseña          >    │
│ 🛡 Seguridad                   >    │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ ⚙ Configuración                >    │
│                                     │
│ 🚪 Cerrar sesión                    │
│                                     │
└─────────────────────────────────────┘
```

---

## 5. Encabezado del Perfil

La parte superior mostrará la información principal del usuario.

```text
          ┌─────────┐
          │   👤    │
          └─────────┘

          Luis Pérez

        Administrador
```

El usuario podrá seleccionar la imagen de perfil para modificarla, si esta funcionalidad se encuentra habilitada.

---

## 6. Edición de Información Personal

Al seleccionar:

```text
INFORMACIÓN PERSONAL
```

se mostrará:

```text
┌─────────────────────────────────────┐
│ ←       INFORMACIÓN PERSONAL        │
├─────────────────────────────────────┤
│                                     │
│ Foto de perfil                      │
│                                     │
│              ┌───────┐              │
│              │  👤   │              │
│              └───────┘              │
│                                     │
│ Nombre completo                     │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Luis Pérez                      │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Correo electrónico                  │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ luis@email.com                  │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Teléfono                            │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 999999999                       │ │
│ └─────────────────────────────────┘ │
│                                     │
│          [ GUARDAR CAMBIOS ]        │
└─────────────────────────────────────┘
```

---

## 7. Validación de Datos

Antes de guardar los cambios, el sistema deberá validar:

- Nombre obligatorio.
- Formato válido del correo electrónico.
- Número de teléfono válido.
- Campos requeridos.

Flujo:

```text
EDITAR INFORMACIÓN
        │
        ▼
GUARDAR CAMBIOS
        │
        ▼
VALIDAR DATOS
        │
        ├── ERROR
        │     │
        │     ▼
        │ MOSTRAR VALIDACIÓN
        │
        └── CORRECTO
              │
              ▼
       ACTUALIZAR PERFIL
              │
              ▼
       MOSTRAR CONFIRMACIÓN
```

---

## 8. Cambio de Contraseña

El usuario podrá modificar su contraseña desde:

```text
🔒 Cambiar contraseña
```

Wireframe:

```text
┌─────────────────────────────────────┐
│ ←        CAMBIAR CONTRASEÑA         │
├─────────────────────────────────────┤
│                                     │
│ Contraseña actual                   │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ •••••••••                       │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Nueva contraseña                    │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ •••••••••                       │ │
│ └─────────────────────────────────┘ │
│                                     │
│ Confirmar contraseña                │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ •••••••••                       │ │
│ └─────────────────────────────────┘ │
│                                     │
│       [ CAMBIAR CONTRASEÑA ]        │
└─────────────────────────────────────┘
```

---

## 9. Validación de Contraseña

El sistema deberá verificar:

```text
¿CONTRASEÑA ACTUAL CORRECTA?
        │
        ├── NO
        │     │
        │     ▼
        │ MOSTRAR ERROR
        │
        └── SÍ
              │
              ▼
¿NUEVA CONTRASEÑA VÁLIDA?
              │
              ├── NO
              │     │
              │     ▼
              │ MOSTRAR REQUISITOS
              │
              └── SÍ
                    │
                    ▼
          ¿CONTRASEÑAS COINCIDEN?
                    │
                    ├── NO
                    │     │
                    │     ▼
                    │ MOSTRAR ERROR
                    │
                    └── SÍ
                          │
                          ▼
                   ACTUALIZAR CONTRASEÑA
```

---

## 10. Requisitos de la Contraseña

La nueva contraseña deberá cumplir las reglas definidas por el sistema.

Ejemplo:

```text
REQUISITOS

✓ Mínimo 8 caracteres

✓ Al menos una letra

✓ Al menos un número
```

Las reglas definitivas estarán documentadas en:

```text
09-Seguridad/
└── 01-Autenticacion.md
```

---

## 11. Cambio Exitoso

Cuando la contraseña sea actualizada:

```text
✓ Contraseña actualizada correctamente.
```

Dependiendo de la política de seguridad, el sistema podrá:

```text
CAMBIAR CONTRASEÑA
        │
        ▼
INVALIDAR SESIONES ANTERIORES
        │
        ▼
MANTENER SESIÓN ACTUAL
```

o solicitar un nuevo inicio de sesión.

---

## 12. Módulo de Seguridad

La pantalla de seguridad podrá mostrar información relacionada con la cuenta.

```text
┌─────────────────────────────────────┐
│ ←            SEGURIDAD              │
├─────────────────────────────────────┤
│                                     │
│ Estado de la cuenta                 │
│                                     │
│ ✓ Activa                            │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ Último acceso                       │
│                                     │
│ 18/08/2026                          │
│ 10:30 a. m.                         │
│                                     │
├─────────────────────────────────────┤
│                                     │
│ Contraseña                          │
│                                     │
│ Última actualización                │
│ 10/08/2026                          │
│                                     │
│ [ CAMBIAR CONTRASEÑA ]              │
└─────────────────────────────────────┘
```

---

## 13. Información del Rol

El perfil podrá mostrar el rol del usuario.

Ejemplo:

```text
ROL

Administrador
```

Dependiendo de la configuración del sistema, los roles podrán incluir:

- Administrador.
- Prestamista.
- Cobrador.
- Usuario autorizado.

Inicialmente, el sistema podrá manejar un administrador como usuario principal y permitir la incorporación de otros usuarios en futuras versiones.

---

## 14. Acceso a Configuración

Desde el perfil:

```text
⚙ Configuración
```

el usuario podrá acceder a opciones generales de la aplicación.

Flujo:

```text
PERFIL
    │
    ▼
CONFIGURACIÓN
    │
    ├── Preferencias
    │
    ├── Notificaciones
    │
    ├── Configuración general
    │
    └── Seguridad
```

Las configuraciones generales serán definidas en:

```text
12-Wireframe-Configuracion.md
```

---

## 15. Cerrar Sesión

El usuario podrá cerrar sesión desde:

```text
🚪 Cerrar sesión
```

Antes de finalizar:

```text
┌─────────────────────────────────────┐
│                                     │
│         ¿CERRAR SESIÓN?             │
│                                     │
│ ¿Está seguro de que desea cerrar    │
│ sesión?                             │
│                                     │
│ [ CANCELAR ]                        │
│                                     │
│ [ CERRAR SESIÓN ]                   │
│                                     │
└─────────────────────────────────────┘
```

---

## 16. Flujo de Cierre de Sesión

```text
PERFIL
    │
    ▼
CERRAR SESIÓN
    │
    ▼
CONFIRMAR ACCIÓN
    │
    ├── CANCELAR
    │      │
    │      ▼
    │ VOLVER AL PERFIL
    │
    └── CONFIRMAR
           │
           ▼
     ELIMINAR SESIÓN LOCAL
           │
           ▼
     INVALIDAR TOKEN
           │
           ▼
       PANTALLA LOGIN
```

---

## 17. Estado de Carga

Mientras se obtiene la información:

```text
Cargando perfil...
```

La interfaz podrá utilizar:

- Indicador de carga.
- Skeleton loading.
- Datos temporales.

---

## 18. Estado de Error

Si ocurre un problema:

```text
No se pudo cargar la información del perfil.

Verifique su conexión e intente nuevamente.

[ REINTENTAR ]
```

---

## 19. Mensaje de Actualización Exitosa

Después de actualizar la información:

```text
✓ Información actualizada correctamente.
```

El sistema deberá reflejar los cambios inmediatamente.

---

## 20. Flujo de Navegación

```text
DASHBOARD
    │
    ▼
PERFIL
    │
    ├── Información personal
    │       │
    │       ▼
    │   Editar información
    │       │
    │       ▼
    │   Guardar cambios
    │
    ├── Cambiar contraseña
    │
    ├── Seguridad
    │
    ├── Configuración
    │
    └── Cerrar sesión
            │
            ▼
        Confirmar
            │
            ▼
          LOGIN
```

---

## 21. Consideraciones de Experiencia de Usuario

La pantalla deberá:

1. Mostrar claramente la información del usuario.
2. Facilitar la edición de datos personales.
3. Mantener protegida la información sensible.
4. Solicitar la contraseña actual antes de cambiarla.
5. Validar correctamente los datos ingresados.
6. Mostrar mensajes claros de éxito o error.
7. Solicitar confirmación antes de cerrar sesión.
8. Mantener una estructura simple y fácil de navegar.
9. Mantener coherencia visual con los demás módulos.
10. Permitir acceder rápidamente a las opciones importantes de la cuenta.

---

## 22. Wireframe Simplificado

```text
┌─────────────────────────────────────┐
│               PERFIL                │
├─────────────────────────────────────┤
│                                     │
│               👤                    │
│                                     │
│            Luis Pérez               │
│          Administrador              │
│                                     │
├─────────────────────────────────────┤
│ 👤 Información personal        >    │
├─────────────────────────────────────┤
│ 🔒 Cambiar contraseña          >    │
├─────────────────────────────────────┤
│ 🛡 Seguridad                   >    │
├─────────────────────────────────────┤
│ ⚙ Configuración                >    │
├─────────────────────────────────────┤
│ 🚪 Cerrar sesión                    │
└─────────────────────────────────────┘
```

---

## 23. Consideraciones Finales

El módulo de Perfil permitirá a los usuarios gestionar la información relacionada con su cuenta dentro del Sistema de Gestión de Préstamos.

El sistema deberá permitir:

1. Consultar información personal.
2. Editar los datos permitidos.
3. Visualizar el rol asignado.
4. Consultar información de seguridad.
5. Cambiar la contraseña.
6. Validar correctamente la información ingresada.
7. Acceder a la configuración general.
8. Cerrar sesión de forma segura.
9. Mostrar mensajes claros sobre las operaciones realizadas.
10. Mantener la información de la cuenta protegida.

El siguiente y último wireframe será:

`12-Wireframe-Configuracion.md`

Este documento definirá la pantalla de configuración general del sistema, incluyendo opciones relacionadas con préstamos, pagos, notificaciones, WhatsApp y preferencias generales.