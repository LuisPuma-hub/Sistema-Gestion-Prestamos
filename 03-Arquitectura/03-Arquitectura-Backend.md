# Arquitectura Backend

## 1. Objetivo

Definir la estructura interna del backend del Sistema de Gestión de Préstamos, estableciendo las responsabilidades de cada capa y evitando que la lógica del negocio quede mezclada con la presentación o el acceso a datos.

---

## 2. Tecnología

El backend utilizará:

- ASP.NET Core Web API.
- C#.
- Entity Framework Core.
- PostgreSQL.

---

## 3. Arquitectura por capas

El backend utilizará una arquitectura organizada por responsabilidades.

La estructura general será:

API
↓
Application
↓
Domain
↓
Infrastructure

Cada capa tendrá una responsabilidad específica.

---

## 4. Capa API

La capa API será el punto de entrada de las solicitudes realizadas por la aplicación móvil.

Será responsable de:

- Recibir solicitudes HTTP.
- Validar parámetros básicos.
- Gestionar autenticación.
- Gestionar autorización.
- Ejecutar los casos de uso.
- Devolver respuestas HTTP.
- Manejar errores de comunicación.

Los Controllers estarán ubicados principalmente en esta capa.

Ejemplos de recursos (prefijo normativo `/api/v1`, ver `05-API/01-Endpoints.md`):

- /api/v1/auth
- /api/v1/clientes
- /api/v1/avales
- /api/v1/solicitudes
- /api/v1/prestamos
- /api/v1/pagos
- /api/v1/notificaciones
- /api/v1/whatsapp

Los Controllers no deberán contener reglas financieras complejas.

---

## 5. Capa Application

La capa Application contendrá los casos de uso del sistema.

Ejemplos:

- Registrar cliente.
- Actualizar cliente.
- Registrar aval.
- Crear solicitud.
- Aprobar solicitud.
- Rechazar solicitud.
- Crear préstamo.
- Registrar pago.
- Aplicar pago.
- Consultar cobranza.
- Consultar morosidad.
- Reactivar préstamo.
- Programar notificación.
- Enviar mensaje de WhatsApp.

Esta capa coordinará las operaciones necesarias para ejecutar cada caso de uso.

---

## 6. Capa Domain

La capa Domain contendrá las reglas fundamentales del negocio.

Entre las principales entidades estarán:

- Cliente.
- Aval.
- Solicitud.
- Préstamo.
- Periodo de interés.
- Pago.

También podrá contener:

- Enumeraciones.
- Objetos de valor.
- Validaciones.
- Reglas financieras.

---

## 7. Reglas financieras

Las reglas financieras deberán estar centralizadas en el backend.

### Interés

El interés semanal será:

Monto inicial × 5 %

Ejemplo:

Monto inicial: S/ 100.00

Interés semanal: S/ 5.00

El interés se mantendrá basado en el monto inicial.

No se generará interés sobre:

- Intereses pendientes.
- Capital reducido.

---

## 8. Aplicación de pagos

El caso de uso de registro de pago deberá aplicar el pago siguiendo este orden normativo (RN-PAG-003):

1. Mora pendiente.
2. Intereses pendientes FIFO por fecha_vencimiento asc.
3. Capital.

Ejemplo:

Capital inicial: S/ 100.00

Interés pendiente: S/ 5.00

Pago: S/ 15.00

Resultado:

- S/ 5.00 → Interés.
- S/ 10.00 → Capital.

Nuevo capital:

S/ 90.00.

---

## 9. Periodos de interés

Cada préstamo tendrá periodos de interés.

Cada periodo deberá registrar:

- Fecha de inicio.
- Fecha de vencimiento.
- Número de periodo.
- Interés generado.
- Interés pagado.
- Interés pendiente.
- Estado.

Si un préstamo comienza un lunes, el periodo correspondiente podrá vencer el siguiente lunes al finalizar el día.

---

## 10. Morosidad

El backend será responsable de determinar si un préstamo cumple las condiciones para ser considerado moroso.

La condición inicial será:

Más de dos pagos de interés realizados fuera de plazo.

El sistema deberá registrar el historial de los periodos vencidos.

El administrador podrá posteriormente reactivar un cliente o préstamo cuando corresponda.

---

## 11. Capa Infrastructure

Esta capa contendrá las implementaciones técnicas necesarias para comunicarse con sistemas externos y la base de datos.

Incluirá:

- Entity Framework Core.
- DbContext.
- Configuración de entidades.
- Repositorios cuando sean necesarios.
- Servicios de Firebase.
- Servicios de WhatsApp.
- Almacenamiento de archivos.
- Implementaciones de servicios externos.

---

## 12. Acceso a datos

Entity Framework Core será utilizado para comunicarse con PostgreSQL.

El flujo será:

Application
↓
Infrastructure
↓
Entity Framework Core
↓
PostgreSQL

La aplicación móvil nunca accederá directamente a PostgreSQL.

---

## 13. Integración con Firebase

El backend será responsable de comunicarse con Firebase Cloud Messaging.

El flujo será:

Backend
↓
Firebase Cloud Messaging
↓
Dispositivo móvil

El backend determinará cuándo debe generarse una notificación.

---

## 14. Integración con WhatsApp

El backend será responsable de comunicarse con WhatsApp Business Platform.

El flujo será:

Backend
↓
WhatsApp Business Platform
↓
Cliente

El backend determinará:

- Cliente destinatario.
- Plantilla.
- Variables.
- Momento de envío.
- Resultado del envío.

---

## 15. Seguridad

El backend deberá implementar:

- Autenticación.
- Autorización.
- HTTPS.
- Validación de entradas.
- Protección de credenciales.
- Control de acceso.
- Manejo seguro de tokens.
- Auditoría de operaciones importantes.

Las credenciales de servicios externos no deberán almacenarse directamente en el código fuente.

---

## 16. Respuestas de la API

La API deberá utilizar respuestas consistentes.

Las respuestas deberán permitir identificar:

- Éxito.
- Error de validación.
- No autorizado.
- Prohibido.
- Recurso no encontrado.
- Error interno.

Los modelos de respuesta serán definidos posteriormente en la documentación de API.

---

## 17. Principio de responsabilidad

Cada capa deberá cumplir una función específica.

API:

Recibir y responder solicitudes.

Application:

Ejecutar casos de uso.

Domain:

Aplicar reglas del negocio.

Infrastructure:

Implementar acceso a datos y servicios externos.

Esta separación permitirá facilitar:

- Mantenimiento.
- Pruebas.
- Escalabilidad.
- Reutilización.
- Modificaciones futuras.

---

## 18. Estructura conceptual

La estructura conceptual del backend será:

Backend

├── API
├── Application
├── Domain
└── Infrastructure

La estructura física de carpetas y proyectos se definirá durante la implementación.