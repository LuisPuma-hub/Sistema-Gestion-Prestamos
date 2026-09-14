# 01. Pantalla de Login

## 1. Información general

| Campo | Descripción |
|---|---|
| Nombre | Pantalla de Inicio de Sesión |
| Archivo | `01-Pantalla-Login.md` |
| Módulo | Autenticación |
| Tipo | Pantalla principal |
| Prioridad | Alta |
| Acceso | Usuarios registrados |
| Plataforma | Aplicación móvil |
| Estado | Diseño |

---

## 2. Objetivo

La pantalla de Login permite que los usuarios autorizados ingresen al sistema mediante sus credenciales.

El objetivo principal es garantizar un acceso seguro al sistema de gestión de préstamos y dirigir al usuario hacia las funcionalidades correspondientes según su rol.

---

## 3. Usuarios

La pantalla estará disponible para los usuarios registrados en el sistema.

Los roles contemplados inicialmente son (enum cerrado, ver 09-Seguridad/02):

- Administrador.
- Cobrador.
- Prestamista (puede estar asociado a cobranza en v1).

El sistema determinará automáticamente los permisos y funcionalidades disponibles después de una autenticación exitosa.

---

## 4. Estructura de la pantalla

La pantalla estará compuesta por las siguientes secciones:

```text
┌─────────────────────────────────────┐
│                                     │
│              LOGO                   │
│                                     │
│       Sistema de Gestión            │
│             de Préstamos            │
│                                     │
│  ┌───────────────────────────────┐  │
│  │ Correo electrónico            │  │
│  └───────────────────────────────┘  │
│                                     │
│  ┌───────────────────────────────┐  │
│  │ Contraseña                 👁  │  │
│  └───────────────────────────────┘  │
│                                     │
│        ¿Olvidaste tu contraseña?    │
│                                     │
│  ┌───────────────────────────────┐  │
│  │          INICIAR SESIÓN       │  │
│  └───────────────────────────────┘  │
│                                     │
│       Mensaje de error, si aplica   │
│                                     │
└─────────────────────────────────────┘
```

---

## 5. Elementos de la interfaz

### 5.1 Logo

Se mostrará el logotipo de la aplicación en la parte superior de la pantalla.

**Características:**

- Imagen centrada.
- Tamaño adaptable a diferentes resoluciones.
- No debe ocupar demasiado espacio vertical.
- Debe mantener una relación adecuada entre el logotipo y el nombre del sistema.

---

### 5.2 Nombre del sistema

Debajo del logo se mostrará:

**Sistema de Gestión de Préstamos**

El texto permitirá identificar claramente la aplicación.

---

### 5.3 Campo de correo electrónico

Campo utilizado para ingresar el correo electrónico registrado del usuario.

**Propiedades:**

- Tipo: Email.
- Obligatorio: Sí.
- Placeholder: `Correo electrónico`.
- Validación de formato de correo.
- No debe aceptar valores vacíos.

**Ejemplo:**

```text
Correo electrónico
usuario@ejemplo.com
```

---

### 5.4 Campo de contraseña

Campo utilizado para ingresar la contraseña del usuario.

**Propiedades:**

- Tipo: Password.
- Obligatorio: Sí.
- Placeholder: `Contraseña`.
- Los caracteres deben ocultarse inicialmente.
- Debe incluir una opción para mostrar u ocultar la contraseña.

**Ejemplo:**

```text
Contraseña                         👁
••••••••••
```

---

### 5.5 Mostrar/Ocultar contraseña

El usuario podrá seleccionar el icono de visualización para cambiar entre:

- Contraseña oculta.
- Contraseña visible.

Esto facilita la validación visual de la contraseña introducida.

---

### 5.6 Recuperar contraseña

Se mostrará un enlace:

**¿Olvidaste tu contraseña?**

Al seleccionarlo, el sistema deberá dirigir al proceso de recuperación de contraseña.

El proceso podrá incluir:

1. Ingreso del correo electrónico.
2. Validación de existencia de la cuenta.
3. Envío de instrucciones de recuperación.
4. Cambio de contraseña.
5. Confirmación del cambio.

---

### 5.7 Botón Iniciar Sesión

Botón principal de la pantalla.

**Texto:**

`INICIAR SESIÓN`

Al seleccionarlo, el sistema deberá validar las credenciales ingresadas.

---

## 6. Estados de la pantalla

La pantalla deberá contemplar los siguientes estados:

### 6.1 Estado inicial

Los campos estarán vacíos.

```text
Correo electrónico
[                         ]

Contraseña
[                         ]

[      INICIAR SESIÓN      ]
```

---

### 6.2 Campos incompletos

Si el usuario intenta iniciar sesión sin completar los campos obligatorios, se mostrará un mensaje indicando los datos faltantes.

Ejemplo:

```text
⚠ Completa todos los campos obligatorios.
```

---

### 6.3 Correo inválido

Si el correo no tiene un formato válido:

```text
⚠ Ingresa un correo electrónico válido.
```

---

### 6.4 Credenciales incorrectas

Cuando el correo o contraseña no sean válidos:

```text
⚠ Correo electrónico o contraseña incorrectos.
```

Por seguridad, el sistema no deberá indicar cuál de los dos datos es incorrecto.

---

### 6.5 Cargando

Mientras el sistema procesa la autenticación, el botón deberá mostrar un indicador de carga.

```text
[       Iniciando sesión...       ]
```

Durante este estado se deberá evitar que el usuario envíe múltiples solicitudes.

---

### 6.6 Autenticación exitosa

Cuando las credenciales sean correctas:

1. Se valida la cuenta.
2. Se obtiene el rol del usuario.
3. Se generan o actualizan los datos de sesión.
4. Se registra el acceso si corresponde.
5. Se redirige al Dashboard.

Flujo:

```text
Login
  │
  ▼
Validar credenciales
  │
  ▼
¿Credenciales correctas?
  │
 ┌┴───────────────┐
 │                │
NO               SÍ
 │                │
 ▼                ▼
Error          Obtener rol
                  │
                  ▼
             Crear sesión
                  │
                  ▼
              Dashboard
```

---

## 7. Validaciones

| Campo | Validación |
|---|---|
| Correo | Obligatorio |
| Correo | Formato válido |
| Contraseña | Obligatoria |
| Contraseña | Validación contra credencial registrada |
| Botón | No enviar si existen campos inválidos |

Las validaciones deberán ejecutarse tanto en el cliente como en el servidor.

La validación del servidor será la responsable de determinar definitivamente si las credenciales son correctas.

---

## 8. Seguridad

La pantalla de Login deberá cumplir las siguientes consideraciones:

- No almacenar contraseñas en texto plano.
- Utilizar comunicación HTTPS.
- No mostrar la contraseña por defecto.
- No revelar si el correo existe cuando las credenciales son incorrectas.
- Implementar control de sesiones.
- Permitir cierre de sesión.
- Controlar intentos de autenticación.
- Registrar eventos de inicio de sesión cuando corresponda.
- Utilizar tokens o mecanismos seguros de autenticación.
- Evitar almacenar información sensible innecesaria en el dispositivo.

---

## 9. Flujo de navegación

### 9.1 Inicio de sesión exitoso

```text
Pantalla Login
      │
      ▼
Validación
      │
      ▼
Autenticación
      │
      ▼
Identificación del rol
      │
      ▼
Dashboard
```

---

### 9.2 Recuperación de contraseña

```text
Pantalla Login
      │
      ▼
¿Olvidaste tu contraseña?
      │
      ▼
Recuperar contraseña
      │
      ▼
Ingresar correo
      │
      ▼
Validar usuario
      │
      ▼
Proceso de recuperación
      │
      ▼
Nueva contraseña
      │
      ▼
Login
```

---

## 10. Comportamiento del botón Login

Al presionar **INICIAR SESIÓN**:

### Paso 1: Validar campos

El sistema verifica que:

- El correo no esté vacío.
- La contraseña no esté vacía.
- El correo tenga un formato válido.

### Paso 2: Enviar solicitud

Si los datos son válidos, se envía la solicitud de autenticación al backend.

### Paso 3: Procesar respuesta

El backend puede responder:

**Éxito**

```text
Autenticación correcta
```

o

**Error**

```text
Credenciales incorrectas
```

### Paso 4: Redireccionar

En caso de éxito, el usuario será dirigido al Dashboard.

---

## 11. Accesibilidad

La pantalla deberá considerar:

- Tamaño adecuado de los textos.
- Contraste suficiente.
- Campos claramente identificados.
- Botones con área táctil adecuada.
- Mensajes de error comprensibles.
- No depender exclusivamente del color para comunicar errores.
- Compatibilidad con diferentes tamaños de pantalla.
- Teclado apropiado para cada tipo de campo.

---

## 12. Diseño responsive

La pantalla deberá adaptarse a:

- Teléfonos pequeños.
- Teléfonos medianos.
- Teléfonos grandes.
- Orientación vertical.
- Diferentes densidades de pantalla.

Los elementos principales deberán mantenerse centrados y evitar desbordamientos.

---

## 13. Componentes funcionales

La implementación podrá dividirse en los siguientes componentes:

```text
LoginScreen
│
├── Logo
├── AppTitle
├── EmailInput
├── PasswordInput
├── ForgotPasswordButton
├── LoginButton
└── ErrorMessage
```

---

## 14. Datos enviados al backend

La aplicación podrá enviar una solicitud de autenticación con una estructura equivalente a:

```text
{
  "email": "usuario@ejemplo.com",
  "password": "********"
}
```

La contraseña deberá transmitirse únicamente mediante una conexión segura HTTPS.

---

## 15. Respuesta esperada del backend

Una autenticación exitosa podrá devolver información equivalente a:

```text
{
  "success": true,
  "message": "Autenticación exitosa",
  "token": "TOKEN",
  "user": {
    "id": 1,
    "name": "Nombre del usuario",
    "email": "usuario@ejemplo.com",
    "role": "admin"
  }
}
```

Los nombres definitivos de los campos dependerán del contrato establecido en la documentación de la API.

---

## 16. Reglas de negocio relacionadas

La pantalla deberá respetar las reglas definidas para autenticación y autorización.

### RN-LOGIN-01

Solo los usuarios registrados podrán acceder al sistema.

### RN-LOGIN-02

El usuario deberá proporcionar credenciales válidas.

### RN-LOGIN-03

Las credenciales deberán ser verificadas por el backend.

### RN-LOGIN-04

El usuario autenticado recibirá acceso según su rol.

### RN-LOGIN-05

Un usuario sin permisos suficientes no podrá acceder a funcionalidades restringidas.

### RN-LOGIN-06

La sesión deberá poder cerrarse desde el sistema.

### RN-LOGIN-07

Las credenciales no deberán almacenarse en texto plano.

---

## 17. Mensajes del sistema

| Situación | Mensaje |
|---|---|
| Campos vacíos | Completa todos los campos obligatorios. |
| Correo inválido | Ingresa un correo electrónico válido. |
| Credenciales incorrectas | Correo electrónico o contraseña incorrectos. |
| Error de conexión | No se pudo conectar con el servidor. |
| Servidor no disponible | El servicio no está disponible temporalmente. |
| Sesión iniciada | Inicio de sesión exitoso. |
| Recuperación | Se enviaron las instrucciones de recuperación. |

---

## 18. Criterios de aceptación

La pantalla será considerada terminada cuando:

- [ ] Se muestre correctamente el logo.
- [ ] Se muestre el nombre del sistema.
- [ ] Exista un campo para correo electrónico.
- [ ] Exista un campo para contraseña.
- [ ] Se pueda mostrar u ocultar la contraseña.
- [ ] Se valide el correo electrónico.
- [ ] Se validen campos obligatorios.
- [ ] Se pueda iniciar sesión.
- [ ] Se muestre un estado de carga.
- [ ] Se muestren mensajes de error.
- [ ] Se pueda acceder al proceso de recuperación de contraseña.
- [ ] Se redirija al Dashboard después de una autenticación exitosa.
- [ ] Se respete el rol del usuario.
- [ ] La pantalla sea responsive.
- [ ] La comunicación con el backend utilice HTTPS.
- [ ] No se almacene la contraseña en texto plano.

---

## 19. Relación con otras pantallas

| Pantalla | Relación |
|---|---|
| Recuperar contraseña | Permite recuperar el acceso |
| Dashboard | Destino después del Login |
| Perfil | Administración de información del usuario |
| Configuración | Administración de preferencias y parámetros autorizados |

---

## 20. Resultado esperado

La pantalla de Login debe proporcionar un acceso simple, seguro y claro al sistema.

El usuario deberá poder ingresar sus credenciales, recibir retroalimentación inmediata sobre posibles errores y acceder al Dashboard cuando la autenticación sea exitosa.

La pantalla constituye el punto de entrada principal a la aplicación móvil y establece el contexto de seguridad para el resto de los módulos.