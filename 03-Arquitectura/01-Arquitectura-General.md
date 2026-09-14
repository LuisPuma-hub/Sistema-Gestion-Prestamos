# Arquitectura General

## 1. Objetivo

Definir la arquitectura general del Sistema de Gestión de Préstamos, estableciendo los componentes principales, sus responsabilidades y la forma en que se comunican entre sí.

El sistema estará orientado a la gestión de clientes, solicitudes de préstamos, préstamos, intereses, pagos, cobranzas, notificaciones y comunicación mediante WhatsApp.

---

## 2. Descripción general del sistema

El Sistema de Gestión de Préstamos será una solución compuesta principalmente por:

- Una aplicación móvil para el administrador/prestamista/cobrador.
- Una API Backend encargada de procesar las operaciones.
- Una base de datos relacional para almacenar la información.
- Un servicio de notificaciones push.
- Una integración con WhatsApp Business Platform para el envío de mensajes.

La aplicación móvil no tendrá acceso directo a la base de datos. Toda operación se realizará mediante la API Backend.

---

## 3. Arquitectura del sistema

El sistema utilizará una arquitectura cliente-servidor, donde la aplicación móvil actuará como cliente y el backend como servidor central.

La arquitectura general estará compuesta por las siguientes capas:

### 3.1 Aplicación móvil

La aplicación móvil será desarrollada utilizando .NET MAUI y C#.

Será utilizada inicialmente por el administrador, quien también cumplirá las funciones de:

- Prestamista.
- Cobrador.
- Administrador del sistema.

Desde la aplicación podrá realizar operaciones como:

- Iniciar sesión.
- Consultar el dashboard.
- Registrar clientes.
- Registrar información de avales.
- Consultar clientes.
- Registrar solicitudes de préstamos.
- Aprobar o rechazar solicitudes.
- Crear préstamos.
- Consultar préstamos.
- Registrar pagos.
- Consultar cobranzas.
- Consultar morosidad.
- Recibir notificaciones.
- Configurar horarios de notificación.
- Gestionar mensajes de WhatsApp.
- Consultar historial de operaciones.

La aplicación móvil no tendrá acceso directo a PostgreSQL.

Toda operación que modifique información deberá realizarse mediante la API Backend.

---

### 3.2 Backend

El backend será desarrollado utilizando ASP.NET Core Web API y C#.

Será el componente central del sistema y tendrá la responsabilidad de procesar las operaciones realizadas desde la aplicación móvil.

Entre sus principales responsabilidades estarán:

- Autenticar usuarios.
- Autorizar operaciones.
- Gestionar clientes.
- Gestionar avales.
- Gestionar solicitudes de préstamos.
- Gestionar préstamos.
- Calcular intereses.
- Generar periodos de interés.
- Registrar pagos.
- Aplicar pagos.
- Actualizar el capital pendiente.
- Determinar estados de préstamos.
- Determinar morosidad.
- Programar notificaciones.
- Enviar notificaciones mediante Firebase.
- Gestionar plantillas de WhatsApp.
- Enviar mensajes mediante WhatsApp.
- Registrar operaciones importantes.

El backend será la fuente de verdad del sistema.

---

### 3.3 Base de datos

La base de datos utilizará PostgreSQL.

Será responsable de almacenar de forma persistente la información del sistema.

Entre los principales datos almacenados estarán:

- Usuarios.
- Dispositivos.
- Configuración de notificaciones.
- Clientes.
- Avales.
- Solicitudes de préstamos.
- Préstamos.
- Periodos de interés.
- Pagos.
- Detalles de pagos.
- Plantillas de WhatsApp.
- Mensajes enviados.
- Notificaciones.

La aplicación móvil no realizará consultas directamente sobre la base de datos.

---

### 3.4 Firebase Cloud Messaging

Firebase Cloud Messaging (FCM) será utilizado para enviar notificaciones push al dispositivo móvil del administrador.

Las notificaciones podrán utilizarse para:

- Recordatorios de cobros próximos.
- Recordatorios de intereses pendientes.
- Avisos de cobros vencidos.
- Avisos de clientes morosos.
- Información sobre eventos importantes del sistema.

Las notificaciones deberán poder recibirse aunque la aplicación:

- Esté cerrada.
- Se encuentre en segundo plano.
- No esté abierta en pantalla.

El horario de las notificaciones será configurable por el administrador.

Por ejemplo:

- 08:00 a. m.
- 04:00 p. m.

Estos horarios no serán valores fijos. El administrador podrá modificarlos posteriormente.

---

### 3.5 WhatsApp Business Platform

WhatsApp será utilizado exclusivamente como canal de comunicación con los clientes.

El sistema no implementará una aplicación de mensajería ni un sistema de chat.

Su finalidad será enviar mensajes relacionados con la gestión de los préstamos.

Los principales tipos de mensajes serán:

- Recordatorios de pago.
- Avisos de interés próximo a vencer.
- Avisos de interés vencido.
- Confirmaciones de pago.
- Información relacionada con préstamos.
- Mensajes de marketing.

Para los mensajes automatizados se utilizarán plantillas aprobadas por la plataforma correspondiente cuando sea requerido.

---

## 4. Comunicación entre componentes

La aplicación móvil se comunicará con el backend mediante una API REST.

La comunicación utilizará:

- HTTPS.
- REST.
- JSON.

El flujo general será:

Aplicación móvil → API Backend → Base de datos

El backend también podrá comunicarse con servicios externos:

Backend → Firebase Cloud Messaging

Backend → WhatsApp Business Platform

La aplicación móvil no se comunicará directamente con PostgreSQL ni con los servicios externos cuando la operación requiera procesamiento de negocio.

---

## 5. Responsabilidad de cada componente

| Componente | Responsabilidad |
|---|---|
| Aplicación móvil | Interfaz, captura y presentación de información |
| Backend | Reglas de negocio, validaciones y procesamiento |
| PostgreSQL | Persistencia de información |
| Firebase FCM | Envío de notificaciones push |
| WhatsApp Business Platform | Comunicación con clientes |
| GitHub | Control de versiones |
| GitHub Copilot | Asistencia inline en el editor (autocompletado y chat en VS Code) |
| Muse Spark vía OpenCode | Agente de ingeniería (revisión, edición multi-archivo y verificación por terminal) |

---

## 6. Reglas financieras centralizadas

Las reglas financieras estarán implementadas en el backend.

La aplicación móvil no deberá calcular ni modificar directamente los valores financieros definitivos.

Entre las principales reglas que controlará el backend se encuentran:

- Cálculo del interés semanal.
- Generación de periodos de interés.
- Aplicación de pagos.
- Actualización del capital pendiente.
- Control de intereses vencidos.
- Control de morosidad.
- Cambio de estados del préstamo.

### 6.1 Regla de interés

El interés semanal será equivalente al 5 % del monto inicial del préstamo.

Ejemplo:

Monto inicial: S/ 100.00

Interés semanal: 5 %

Interés semanal:

S/ 100.00 × 5 % = S/ 5.00

El interés se mantendrá calculado sobre el monto inicial y no se generará sobre-interés sobre el capital pendiente.

Por ejemplo, si el cliente realiza un pago de S/ 50.00 sobre un préstamo inicial de S/ 100.00, el capital pendiente será S/ 50.00, pero el interés semanal continuará siendo S/ 5.00.

---

## 7. Aplicación de pagos

Cuando se registre un pago, el backend deberá aplicar el dinero siguiendo el siguiente orden:

1. Primero se cancelarán los intereses pendientes.
2. Si existe un excedente después de cancelar los intereses, este se aplicará al capital.
3. El capital pendiente se actualizará con el monto aplicado.

Ejemplo:

Préstamo inicial: S/ 100.00

Interés semanal: S/ 5.00

Pago realizado: S/ 10.00

Aplicación:

- S/ 5.00 → Interés.
- S/ 5.00 → Capital.

Nuevo capital pendiente:

S/ 95.00

Si el cliente únicamente paga S/ 5.00:

- S/ 5.00 → Interés.
- S/ 0.00 → Capital.

El capital permanecerá en:

S/ 100.00

---

## 8. Independencia entre capital e interés

El interés y el capital serán tratados como conceptos independientes.

Por lo tanto, un cliente puede:

- Tener capital pendiente e interés pagado.
- Tener capital pendiente e interés pendiente.
- Tener varios periodos de interés pendientes.
- Cancelar parcialmente el capital.
- Continuar pagando el interés calculado sobre el monto inicial.

No se generará interés sobre el interés pendiente ni sobre el capital reducido.

El interés continuará calculándose utilizando como base el monto inicial del préstamo.

---

## 9. Inicio y vencimiento del interés

Cada préstamo tendrá un día de cobro determinado a partir de la fecha de inicio.

Ejemplo:

Si el préstamo comienza un lunes, el siguiente periodo de interés vencerá el siguiente lunes al finalizar el día.

Por ejemplo:

- Inicio del préstamo: lunes 10 de agosto.
- Primer vencimiento de interés: lunes 17 de agosto.
- Segundo vencimiento: lunes 24 de agosto.
- Tercer vencimiento: lunes 31 de agosto.

El periodo de interés tendrá una fecha de inicio y una fecha de vencimiento.

---

## 10. Préstamos por cliente

La estructura de la base de datos permitirá que un cliente pueda tener múltiples préstamos a lo largo del tiempo.

La relación será:

Cliente → múltiples préstamos.

En la versión inicial del sistema se podrá establecer como regla que un cliente tenga solamente un préstamo activo simultáneamente.

Sin embargo, el modelo estará preparado para permitir posteriormente varios préstamos activos para un mismo cliente.

Esto permitirá ampliar el sistema sin tener que modificar la relación principal entre clientes y préstamos.

---

## 11. Morosidad

El sistema deberá controlar el cumplimiento de los pagos de intereses.

Un préstamo podrá pasar a un estado de morosidad cuando el cliente acumule más de dos pagos de interés fuera de plazo, de acuerdo con las reglas de negocio establecidas.

El sistema deberá conservar el historial de los periodos vencidos.

La condición de morosidad podrá ser modificada posteriormente por el administrador cuando corresponda.

También deberá existir la posibilidad de reactivar al cliente o préstamo cuando el administrador determine que corresponde.

---

## 12. Estados

El sistema manejará estados independientes para clientes y préstamos.

### Estados del cliente

Entre los posibles estados estarán:

- ACTIVO.
- EN OBSERVACIÓN.
- MOROSO.
- INACTIVO.

### Estados del préstamo

Entre los posibles estados estarán:

- PENDIENTE.
- ACTIVO.
- MOROSO.
- CANCELADO.
- ANULADO.

Los estados podrán ampliarse posteriormente según las necesidades del sistema.

---

## 13. Horarios de notificaciones

El administrador podrá configurar los horarios en los que desea recibir notificaciones.

Por ejemplo:

- 08:00 a. m.
- 04:00 p. m.

El sistema no limitará la configuración a estos dos horarios.

El administrador podrá definir los horarios que considere necesarios.

Las notificaciones deberán poder enviarse aunque la aplicación se encuentre cerrada o el usuario no esté utilizando activamente la aplicación.

La generación y programación de las notificaciones será responsabilidad del backend.

Firebase Cloud Messaging será utilizado como mecanismo de entrega al dispositivo móvil.

---

## 14. Seguridad

La comunicación entre la aplicación móvil y el backend deberá realizarse mediante HTTPS.

Además:

- Las contraseñas no se almacenarán en texto plano.
- Las credenciales de servicios externos no estarán dentro del código fuente.
- El backend validará todas las operaciones.
- Los datos financieros no podrán modificarse directamente desde el cliente móvil.
- Las operaciones financieras importantes deberán conservar información de auditoría.
- El acceso a los recursos estará protegido mediante autenticación y autorización.

---

## 15. Escalabilidad

La arquitectura estará preparada para futuras ampliaciones.

Entre ellas:

- Incorporación de otros usuarios.
- Incorporación de diferentes roles.
- Incorporación de varios cobradores.
- Incorporación de varios administradores.
- Múltiples préstamos activos por cliente.
- Nuevos canales de comunicación.
- Nuevos tipos de notificaciones.
- Panel administrativo web.
- Reportes avanzados.
- Nuevas integraciones externas.

---

## 16. Principio arquitectónico principal

El sistema seguirá el siguiente principio:

La aplicación móvil presenta y captura información.

El backend procesa y valida las reglas del negocio.

La base de datos conserva la información.

Los servicios externos proporcionan capacidades especializadas como notificaciones y comunicación mediante WhatsApp.

Por lo tanto, la distribución de responsabilidades será:

Aplicación móvil:
- Presentar.
- Capturar.
- Consultar.
- Solicitar operaciones.

Backend:
- Validar.
- Procesar.
- Calcular.
- Autorizar.
- Ejecutar reglas de negocio.

Base de datos:
- Almacenar.
- Relacionar.
- Mantener la integridad de los datos.

Servicios externos:
- Enviar notificaciones.
- Enviar mensajes de WhatsApp.

---

## 17. Resumen de arquitectura

La arquitectura general estará compuesta por:

.NET MAUI + C#
↓
ASP.NET Core Web API + C#
↓
Entity Framework Core
↓
PostgreSQL

El backend también tendrá integración con:

ASP.NET Core Web API
↓
Firebase Cloud Messaging

ASP.NET Core Web API
↓
WhatsApp Business Platform

Esta separación permitirá mantener una arquitectura organizada, escalable y mantenible, evitando que la lógica financiera dependa directamente de la aplicación móvil.