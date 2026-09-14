# Integraciones

## 1. Objetivo

Definir los servicios externos que serán utilizados por el Sistema de Gestión de Préstamos y establecer la función de cada integración.

---

## 2. Integraciones principales

El sistema tendrá inicialmente las siguientes integraciones:

- Firebase Cloud Messaging.
- WhatsApp Business Platform.
- PostgreSQL.
- GitHub.

Las integraciones estarán controladas principalmente desde el backend.

---

## 3. Firebase Cloud Messaging

### 3.1 Objetivo

Firebase Cloud Messaging será utilizado para enviar notificaciones push al dispositivo móvil del administrador.

### 3.2 Tipos de notificaciones

Las notificaciones podrán utilizarse para:

- Cobro próximo.
- Interés próximo a vencer.
- Interés vencido.
- Cliente moroso.
- Pago registrado.
- Eventos importantes.

### 3.3 Funcionamiento

El dispositivo móvil obtendrá un token FCM.

El token será asociado al usuario correspondiente.

El backend almacenará la información necesaria para identificar el dispositivo.

Cuando corresponda enviar una notificación:

Backend
↓
Firebase Cloud Messaging
↓
Dispositivo móvil

### 3.4 Horarios

El administrador podrá configurar los horarios de notificación.

Ejemplos:

- 08:00 a. m.
- 04:00 p. m.

Los horarios podrán modificarse posteriormente.

### 3.5 Aplicación cerrada

Las notificaciones deberán poder recibirse aunque la aplicación:

- Esté cerrada.
- Esté en segundo plano.
- No esté actualmente abierta.

La entrega dependerá del funcionamiento del sistema operativo y de Firebase Cloud Messaging.

---

## 4. WhatsApp Business Platform

### 4.1 Objetivo

WhatsApp será utilizado como canal de comunicación entre el sistema y los clientes.

No se implementará una aplicación de mensajería.

No se almacenará una conversación como si fuera un sistema de chat.

El objetivo será realizar envíos controlados desde el sistema.

---

### 4.2 Tipos de mensajes

El sistema podrá utilizar WhatsApp para:

- Recordatorios de pago.
- Recordatorios de intereses.
- Avisos de vencimiento.
- Confirmaciones de pago.
- Información sobre préstamos.
- Mensajes de marketing.

---

### 4.3 Plantillas

Los mensajes automatizados podrán utilizar plantillas.

Las plantillas podrán contener variables.

Ejemplo conceptual:

Hola {{nombre}}.

Le recordamos que su pago de S/ {{monto}} correspondiente al préstamo {{prestamo}} tiene vencimiento el {{fecha}}.

La administración y aprobación de las plantillas dependerá de las políticas de WhatsApp Business Platform.

---

### 4.4 Flujo de envío

El flujo será:

Backend
↓
Seleccionar cliente
↓
Seleccionar plantilla
↓
Completar variables
↓
Enviar a WhatsApp Business Platform
↓
WhatsApp
↓
Cliente

El backend deberá registrar el resultado del envío.

---

### 4.5 Historial

El sistema deberá conservar información básica del mensaje enviado, como:

- Cliente.
- Número de destino.
- Plantilla utilizada.
- Fecha de envío.
- Estado.
- Identificador externo cuando esté disponible.

---

## 5. PostgreSQL

PostgreSQL será el sistema de almacenamiento principal.

El backend utilizará Entity Framework Core para comunicarse con PostgreSQL.

Flujo:

Backend
↓
Entity Framework Core
↓
PostgreSQL

La aplicación móvil no tendrá acceso directo a la base de datos.

---

## 6. Almacenamiento de documentos

Los clientes podrán registrar imágenes de:

- Recibo de luz.
- Recibo de agua.

Estos archivos deberán almacenarse mediante un sistema de almacenamiento de archivos.

La base de datos almacenará la referencia correspondiente al archivo.

Por ejemplo:

Cliente
↓
Referencia del recibo de luz
↓
Archivo almacenado

Cliente
↓
Referencia del recibo de agua
↓
Archivo almacenado

La tecnología específica para el almacenamiento de archivos será definida durante la implementación.

---

## 7. GitHub

GitHub será utilizado como plataforma de control y alojamiento del código fuente.

Permitirá:

- Repositorios.
- Ramas.
- Pull Requests.
- Issues.
- Control de versiones.
- Historial de cambios.
- Integración con GitHub Copilot (inline).
- Integración con Muse Spark vía OpenCode (agente).

---

## 8. Seguridad de las integraciones

Las credenciales utilizadas para servicios externos no deberán almacenarse directamente en el código fuente.

Se deberán utilizar mecanismos seguros para almacenar:

- Claves.
- Tokens.
- Secretos.
- Credenciales.
- Cadenas de conexión.

La configuración dependerá del entorno:

- Desarrollo.
- Pruebas.
- Producción.

---

## 9. Manejo de errores

Cada integración deberá contemplar posibles errores.

### Firebase

Posibles errores:

- Token inválido.
- Dispositivo no disponible.
- Servicio temporalmente no disponible.

### WhatsApp

Posibles errores:

- Plantilla no aprobada.
- Número inválido.
- Error de autenticación.
- Límite de envío.
- Servicio no disponible.

### PostgreSQL

Posibles errores:

- Error de conexión.
- Tiempo de espera.
- Restricción de integridad.
- Error de transacción.

El backend deberá registrar los errores importantes y proporcionar respuestas controladas a la aplicación móvil.

---

## 10. Arquitectura de integraciones

La arquitectura general será:

Aplicación móvil
↓
ASP.NET Core Web API
↓
├── PostgreSQL
├── Firebase Cloud Messaging
├── WhatsApp Business Platform
└── Almacenamiento de archivos

La aplicación móvil no se comunicará directamente con estos servicios cuando la operación dependa de reglas del negocio.

---

## 11. Principio de integración

Las integraciones externas deberán permanecer desacopladas de la lógica principal del sistema.

Por ejemplo:

El módulo de préstamos no deberá depender directamente de Firebase.

El backend será quien determine cuándo corresponde enviar una notificación y utilizará el servicio correspondiente para realizar el envío.

Esto permitirá reemplazar o ampliar servicios externos en el futuro sin modificar significativamente la lógica principal del sistema.