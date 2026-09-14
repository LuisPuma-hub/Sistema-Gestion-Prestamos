# Arquitectura Mobile

## 1. Objetivo

Definir la arquitectura interna de la aplicación móvil del Sistema de Gestión de Préstamos.

La aplicación será utilizada principalmente por el administrador/prestamista/cobrador para gestionar clientes, préstamos, pagos y cobranzas.

---

## 2. Tecnología

La aplicación móvil utilizará:

- .NET MAUI.
- C#.
- MVVM.
- Visual Studio Code (Stable 64 bits, único IDE oficial) con extensiones obligatorias C#, C# Dev Kit y .NET MAUI, más .NET SDK LTS con workload MAUI y Android SDK.

La versión inicial estará orientada principalmente a Android.

---

## 3. Arquitectura MVVM

La aplicación utilizará el patrón MVVM.

MVVM significa:

Model
View
ViewModel

La comunicación será:

View
↓
ViewModel
↓
Service
↓
API Backend

---

## 4. View

Las Views representarán las pantallas de la aplicación.

Entre las principales estarán:

- Login.
- Dashboard.
- Clientes.
- Registro de cliente.
- Detalle de cliente.
- Aval.
- Solicitudes.
- Préstamos.
- Detalle del préstamo.
- Registrar pago.
- Cobranzas.
- Notificaciones.
- Configuración.

Las Views deberán encargarse principalmente de la presentación de información y la interacción con el usuario.

---

## 5. ViewModel

Los ViewModels manejarán el estado y comportamiento de cada pantalla.

Serán responsables de:

- Preparar información para la View.
- Ejecutar acciones.
- Realizar validaciones de interfaz.
- Solicitar datos al backend.
- Mostrar resultados.
- Controlar estados de carga.
- Manejar errores.

Los ViewModels no deberán contener reglas financieras definitivas.

---

## 6. Models

Los Models representarán la información utilizada por la aplicación.

Ejemplos:

- ClienteModel.
- AvalModel.
- SolicitudModel.
- PrestamoModel.
- PeriodoInteresModel.
- PagoModel.
- NotificacionModel.
- PlantillaWhatsAppModel.

Los modelos utilizados en la aplicación podrán corresponder a los modelos Request y Response de la API, pero no necesariamente serán idénticos a las entidades de la base de datos.

---

## 7. Services

Los Services serán responsables de la comunicación con el backend y otros componentes necesarios.

Ejemplos:

- AuthService.
- ClienteService.
- AvalService.
- SolicitudService.
- PrestamoService.
- PagoService.
- NotificacionService.
- WhatsAppService.

Estos servicios utilizarán la API Backend.

---

## 8. Comunicación con el backend

La aplicación realizará solicitudes HTTPS hacia la API.

Ejemplo:

Usuario
↓
View
↓
ViewModel
↓
ClienteService
↓
API Backend
↓
PostgreSQL

La aplicación no tendrá acceso directo a PostgreSQL.

---

## 9. Autenticación

La aplicación contará con una pantalla de inicio de sesión.

El proceso general será:

Usuario ingresa credenciales
↓
Aplicación
↓
API Backend
↓
Validación
↓
Token de autenticación
↓
Aplicación móvil

El mecanismo concreto de autenticación será definido en el documento de autenticación de la API.

---

## 10. Clientes

El módulo de clientes permitirá:

- Registrar clientes.
- Editar clientes.
- Consultar clientes.
- Buscar clientes.
- Consultar información del aval.
- Consultar observaciones.
- Consultar estado del cliente.

Los datos principales del cliente serán:

- Tipo de documento.
- Número de documento.
- Nombres.
- Apellidos.
- Número de celular.
- Recibo de luz.
- Recibo de agua.
- Observaciones.
- Estado.

---

## 11. Avales

La aplicación permitirá registrar dos tipos de aval:

### Cliente existente

Se seleccionará un cliente previamente registrado.

### Nueva persona

Se registrarán los datos de una persona nueva.

Los datos serán:

- Nombres.
- Apellidos.
- Teléfono.
- Dirección.

---

## 12. Préstamos

El módulo de préstamos permitirá:

- Registrar solicitudes.
- Aprobar solicitudes.
- Rechazar solicitudes.
- Consultar préstamos.
- Consultar capital pendiente.
- Consultar intereses.
- Consultar periodos.
- Consultar estado.
- Registrar pagos.

La aplicación mostrará la información calculada por el backend.

---

## 13. Pagos

La aplicación permitirá registrar pagos realizados por los clientes.

El usuario registrará:

- Monto.
- Fecha.
- Método de pago.
- Observaciones.

El backend determinará cómo se distribuye el pago.

El orden será:

1. Interés.
2. Capital.

La aplicación mostrará el resultado de la operación.

---

## 14. Notificaciones

La aplicación estará preparada para recibir notificaciones mediante Firebase Cloud Messaging.

Las notificaciones podrán aparecer cuando:

- Exista un cobro próximo.
- Exista un cobro vencido.
- Un cliente se encuentre moroso.
- Exista una operación importante.

Las notificaciones deberán funcionar aunque la aplicación esté cerrada.

---

## 15. Configuración

El administrador podrá configurar determinadas preferencias desde la aplicación.

Entre ellas:

- Horarios de notificación.
- Preferencias de comunicación.
- Configuración de mensajes.

La configuración definitiva dependerá de los módulos implementados.

---

## 16. Manejo de errores

La aplicación deberá mostrar mensajes adecuados cuando:

- No exista conexión.
- El servidor no esté disponible.
- Existan errores de validación.
- La sesión haya expirado.
- Una operación no esté autorizada.

No deberán mostrarse errores técnicos innecesarios al usuario final.

---

## 17. Seguridad

La aplicación deberá:

- Proteger las credenciales.
- Utilizar HTTPS.
- Gestionar de forma segura el token de autenticación.
- Evitar almacenar información financiera sensible innecesariamente.
- Controlar la sesión.
- Cerrar o renovar la sesión según corresponda.

---

## 18. Navegación principal

El flujo general será:

Login
↓
Dashboard
↓
Clientes
↓
Detalle del cliente

Desde el Dashboard también se podrá acceder a:

- Solicitudes.
- Préstamos.
- Cobranzas.
- Notificaciones.
- Configuración.

---

## 19. Principio arquitectónico

La aplicación móvil será principalmente responsable de:

- Presentar información.
- Capturar información.
- Realizar validaciones básicas.
- Consumir la API.
- Mostrar resultados.

La aplicación no será responsable de establecer las reglas financieras definitivas.

Estas reglas pertenecerán al backend.