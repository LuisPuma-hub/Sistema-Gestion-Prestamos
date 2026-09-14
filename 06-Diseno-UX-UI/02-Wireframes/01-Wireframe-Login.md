# Wireframe - Inicio de Sesión

## 1. Introducción

Este documento define el wireframe inicial de la pantalla de inicio de sesión del Sistema de Gestión de Préstamos.

La pantalla de inicio de sesión será el punto de acceso principal para los usuarios del sistema que no tengan una sesión activa.

El objetivo del diseño es proporcionar una interfaz simple, clara y rápida, permitiendo al usuario autenticarse mediante su correo electrónico y contraseña.

---

## 2. Objetivo de la Pantalla

La pantalla deberá permitir al usuario:

- Ingresar su correo electrónico.
- Ingresar su contraseña.
- Mostrar u ocultar la contraseña.
- Iniciar sesión.
- Visualizar mensajes de error cuando las credenciales sean incorrectas.
- Acceder a funcionalidades futuras de recuperación de contraseña.

---

## 3. Estructura General

La pantalla estará organizada verticalmente para facilitar su uso en dispositivos móviles.

El orden de los elementos será:

1. Logo o identidad visual del sistema.
2. Nombre del sistema.
3. Mensaje de bienvenida.
4. Campo de correo electrónico.
5. Campo de contraseña.
6. Opción para mostrar u ocultar contraseña.
7. Botón de inicio de sesión.
8. Mensajes de validación o error.
9. Opción futura de recuperación de contraseña.

---

## 4. Wireframe Principal

```text
┌─────────────────────────────────────┐
│                                     │
│                                     │
│               [ LOGO ]              │
│                                     │
│      SISTEMA DE PRÉSTAMOS           │
│                                     │
│       Inicia sesión para continuar  │
│                                     │
│                                     │
│  Correo electrónico                 │
│  ┌───────────────────────────────┐  │
│  │ usuario@correo.com            │  │
│  └───────────────────────────────┘  │
│                                     │
│  Contraseña                         │
│  ┌───────────────────────────────┐  │
│  │ •••••••••••••••••        👁   │  │
│  └───────────────────────────────┘  │
│                                     │
│       ¿Olvidaste tu contraseña?     │
│                                     │
│  ┌───────────────────────────────┐  │
│  │        INICIAR SESIÓN         │  │
│  └───────────────────────────────┘  │
│                                     │
│                                     │
└─────────────────────────────────────┘
```

---

## 5. Componentes de la Pantalla

### 5.1 Logo

En la parte superior se mostrará el logo o identidad visual del Sistema de Gestión de Préstamos.

El logo permitirá identificar visualmente la aplicación.

Inicialmente podrá utilizarse un elemento temporal hasta definir la identidad gráfica definitiva.

---

### 5.2 Nombre del Sistema

Debajo del logo se mostrará el nombre del sistema.

Ejemplo:

`Sistema de Gestión de Préstamos`

El nombre deberá ser fácilmente visible y mantener una jerarquía visual clara.

---

### 5.3 Mensaje de Bienvenida

Se mostrará un mensaje breve orientando al usuario.

Ejemplo:

`Inicia sesión para continuar`

Este mensaje deberá ayudar a identificar rápidamente el propósito de la pantalla.

---

### 5.4 Campo de Correo Electrónico

El usuario deberá ingresar su correo electrónico.

Ejemplo visual:

```text
┌─────────────────────────────────┐
│ ✉  usuario@correo.com          │
└─────────────────────────────────┘
```

Características:

- Campo obligatorio.
- Validación del formato de correo electrónico.
- Teclado adecuado para ingresar direcciones de correo.
- Mensaje de error cuando el formato sea incorrecto.

Ejemplo de error:

```text
Correo electrónico inválido.
```

---

### 5.5 Campo de Contraseña

El usuario deberá ingresar su contraseña.

Ejemplo:

```text
┌─────────────────────────────────┐
│ 🔒  •••••••••••••••••       👁 │
└─────────────────────────────────┘
```

Características:

- Campo obligatorio.
- El contenido estará oculto por defecto.
- El usuario podrá mostrar u ocultar la contraseña.
- Se utilizará un icono para controlar la visibilidad.

---

## 6. Mostrar u Ocultar Contraseña

El campo de contraseña contará con un botón o icono.

Flujo:

```text
Contraseña oculta
        │
        ▼
Usuario presiona 👁
        │
        ▼
Contraseña visible
        │
        ▼
Usuario presiona nuevamente
        │
        ▼
Contraseña oculta
```

Esta función ayudará al usuario a verificar que la contraseña fue escrita correctamente.

---

## 7. Recuperación de Contraseña

Como funcionalidad futura se podrá incluir:

`¿Olvidaste tu contraseña?`

Flujo futuro:

```text
Usuario selecciona
"¿Olvidaste tu contraseña?"
        │
        ▼
Ingresar correo electrónico
        │
        ▼
Solicitar recuperación
        │
        ▼
Sistema valida usuario
        │
        ▼
Enviar mecanismo de recuperación
        │
        ▼
Usuario establece nueva contraseña
```

Esta funcionalidad podrá implementarse posteriormente.

---

## 8. Botón de Inicio de Sesión

El botón principal será:

`INICIAR SESIÓN`

Al presionar el botón, la aplicación realizará el siguiente proceso:

```text
Usuario presiona
INICIAR SESIÓN
        │
        ▼
Validar campos
        │
        ├── Campos vacíos
        │       │
        │       ▼
        │   Mostrar errores
        │
        └── Campos válidos
                │
                ▼
        Enviar solicitud al backend
                │
                ▼
        POST /api/v1/auth/login
                │
                ▼
        Backend valida credenciales
                │
                ├── Incorrectas
                │       │
                │       ▼
                │   Mostrar mensaje
                │
                └── Correctas
                        │
                        ▼
                    Dashboard
```

---

## 9. Estado de Carga

Mientras se procesa la autenticación, el botón deberá indicar que la operación está en progreso.

Ejemplo:

```text
┌───────────────────────────────┐
│        INICIANDO SESIÓN...    │
└───────────────────────────────┘
```

Durante este proceso:

- El botón deberá estar temporalmente deshabilitado.
- Se evitarán múltiples solicitudes simultáneas.
- Se podrá mostrar un indicador de carga.

---

## 10. Mensajes de Error

La pantalla deberá mostrar mensajes claros cuando ocurra un problema.

### Campos vacíos

```text
El correo electrónico es obligatorio.
```

```text
La contraseña es obligatoria.
```

### Correo electrónico inválido

```text
Ingrese un correo electrónico válido.
```

### Credenciales incorrectas

```text
Correo electrónico o contraseña incorrectos.
```

### Usuario bloqueado

```text
Su cuenta se encuentra bloqueada.
```

### Error de conexión

```text
No se pudo conectar con el servidor.
Verifique su conexión e intente nuevamente.
```

Los mensajes no deberán revelar información sensible sobre el sistema.

---

## 11. Estados de la Pantalla

La pantalla podrá tener los siguientes estados:

### Estado inicial

Los campos están vacíos y el botón está disponible.

### Estado de edición

El usuario está ingresando sus credenciales.

### Estado de validación

La aplicación verifica que los campos sean correctos.

### Estado de carga

La aplicación espera la respuesta del backend.

### Estado de error

Se muestra un mensaje indicando el problema.

### Estado exitoso

Las credenciales son correctas y el usuario es dirigido al Dashboard.

---

## 12. Flujo de Navegación

```text
INICIO DE LA APLICACIÓN
        │
        ▼
Verificar sesión existente
        │
        ├── Sesión válida
        │       │
        │       ▼
        │   DASHBOARD
        │
        └── Sin sesión
                │
                ▼
         INICIO DE SESIÓN
                │
                ▼
        Ingresar credenciales
                │
                ▼
         Presionar "INICIAR SESIÓN"
                │
                ▼
        ¿Datos correctos?
             │
        ┌────┴────┐
        │         │
       NO        SÍ
        │         │
        ▼         ▼
   Mostrar      DASHBOARD
    error
```

---

## 13. Consideraciones de Experiencia de Usuario

La pantalla deberá cumplir las siguientes consideraciones:

1. Los campos deberán ser fáciles de identificar.
2. El botón principal deberá ser claramente visible.
3. Los mensajes de error deberán ser específicos y comprensibles.
4. El teclado no deberá ocultar los campos activos.
5. La pantalla deberá adaptarse a diferentes tamaños de dispositivos.
6. El usuario deberá poder mostrar u ocultar la contraseña.
7. Durante el proceso de inicio de sesión se evitarán múltiples solicitudes.
8. La interfaz deberá mantener una apariencia limpia y sencilla.
9. Se deberá proporcionar retroalimentación visual durante la carga.
10. El acceso deberá ser rápido y requerir la menor cantidad posible de pasos.

---

## 14. Consideraciones de Seguridad

La pantalla de inicio de sesión deberá cumplir las siguientes medidas:

1. La contraseña no deberá mostrarse por defecto.
2. Las credenciales deberán enviarse mediante una conexión segura.
3. La contraseña no deberá almacenarse en texto plano.
4. La aplicación no deberá registrar contraseñas en logs.
5. Los tokens recibidos después de la autenticación deberán almacenarse de forma segura.
6. Los mensajes de error no deberán revelar información innecesaria.
7. Se deberán controlar los intentos repetidos de inicio de sesión.

---

## 15. Resumen del Wireframe

La pantalla de inicio de sesión será la puerta de acceso al Sistema de Gestión de Préstamos.

Su estructura principal será:

```text
LOGO
 │
 ▼
NOMBRE DEL SISTEMA
 │
 ▼
MENSAJE DE BIENVENIDA
 │
 ▼
CORREO ELECTRÓNICO
 │
 ▼
CONTRASEÑA
 │
 ▼
¿OLVIDASTE TU CONTRASEÑA?
 │
 ▼
BOTÓN "INICIAR SESIÓN"
 │
 ▼
VALIDACIÓN
 │
 ├── Error ───► Mostrar mensaje
 │
 └── Correcto ─► Dashboard
```

El diseño definitivo podrá evolucionar posteriormente durante la implementación visual de la aplicación, manteniendo la estructura funcional definida en este documento.