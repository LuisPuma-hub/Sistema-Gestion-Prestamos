# Integración con Meta WhatsApp Business Platform

## 1. Introducción

El sistema de gestión de préstamos utilizará la plataforma oficial de WhatsApp Business de Meta para permitir el envío de mensajes relacionados con clientes, préstamos, pagos y morosidad.

La integración será realizada desde el backend desarrollado con ASP.NET Core Web API utilizando C#.

La aplicación móvil desarrollada con .NET MAUI no se comunicará directamente con la API de WhatsApp. Todas las operaciones relacionadas con WhatsApp serán gestionadas por el backend.

La arquitectura será:

```text
┌──────────────────────────┐
│       .NET MAUI          │
│       Aplicación móvil   │
└────────────┬─────────────┘
             │
             │ HTTPS / REST
             ▼
┌──────────────────────────┐
│    ASP.NET Core Web API  │
│           C#             │
└──────┬───────────┬───────┘
       │           │
       │           ▼
       │    ┌──────────────┐
       │    │  PostgreSQL  │
       │    └──────────────┘
       │
       ▼
┌──────────────────────────┐
│ Meta WhatsApp Business   │
│        Platform          │
└────────────┬─────────────┘
             │
             ▼
        WhatsApp
```

---

# 2. Objetivo

Definir la arquitectura, configuración y funcionamiento de la integración entre el sistema de gestión de préstamos y WhatsApp Business Platform de Meta.

Los objetivos principales son:

- Enviar mensajes de WhatsApp a clientes.
- Enviar recordatorios de pago.
- Informar sobre pagos pendientes.
- Informar sobre morosidad.
- Permitir mensajes personalizados mediante plantillas aprobadas.
- Registrar el historial de mensajes enviados.
- Consultar el estado de los mensajes.
- Recibir eventos mediante webhooks.
- Mantener las credenciales protegidas.
- Centralizar la comunicación desde el backend.

---

# 3. Alcance

La integración contempla:

- Configuración de Meta Business.
- Configuración de WhatsApp Business.
- Configuración del número telefónico.
- Configuración de la aplicación de Meta.
- Obtención de credenciales necesarias.
- Integración con el backend ASP.NET Core.
- Envío de mensajes.
- Uso de plantillas.
- Variables dinámicas.
- Recepción de webhooks.
- Registro de mensajes.
- Control de errores.
- Reintentos.
- Auditoría.
- Seguridad.

No se contempla que la aplicación móvil realice directamente solicitudes hacia la API de Meta.

---

# 4. Arquitectura de integración

El backend será el intermediario entre el sistema y Meta.

```text
Usuario
   │
   ▼
.NET MAUI
   │
   │ Solicitud
   ▼
ASP.NET Core Web API
   │
   ├── Validación
   ├── Reglas de negocio
   ├── PostgreSQL
   └── Servicio WhatsApp
          │
          ▼
   Meta WhatsApp Business
          │
          ▼
       WhatsApp
```

La comunicación deberá realizarse mediante HTTPS.

---

# 5. Tecnologías utilizadas

| Componente | Tecnología |
|---|---|
| Aplicación móvil | .NET MAUI |
| Lenguaje | C# |
| Backend | ASP.NET Core Web API |
| Base de datos | PostgreSQL |
| ORM | Entity Framework Core |
| API de WhatsApp | WhatsApp Business Platform de Meta |
| Comunicación | REST / JSON |
| Seguridad | HTTPS |
| Control de versiones | Git / GitHub |

---

# 6. Principio de comunicación

La aplicación móvil no deberá contener las credenciales necesarias para comunicarse con Meta.

El flujo correcto será:

```text
.NET MAUI
    │
    │ HTTPS
    ▼
ASP.NET Core
    │
    │ Credenciales protegidas
    ▼
Meta WhatsApp API
    │
    ▼
WhatsApp
```

Esto permite proteger los secretos y centralizar las reglas de negocio.

---

# 7. Componentes de Meta

La integración deberá contemplar los componentes necesarios de la plataforma empresarial de Meta.

Conceptualmente se utilizarán:

```text
Meta Business
      │
      ├── Aplicación de Meta
      │
      ├── WhatsApp Business Account
      │
      ├── Número telefónico
      │
      └── Credenciales/API
```

La configuración definitiva dependerá de la cuenta y del entorno utilizado.

---

# 8. WhatsApp Business Account

El sistema utilizará una cuenta empresarial de WhatsApp para gestionar la comunicación con los clientes.

La cuenta deberá estar configurada para permitir el envío de mensajes desde la plataforma de Meta.

Se deberá mantener correctamente asociada:

```text
Meta Business
      │
      ▼
WhatsApp Business Account
      │
      ▼
Número de WhatsApp
```

---

# 9. Número de teléfono

El sistema deberá utilizar un número telefónico configurado para WhatsApp Business.

El número deberá estar correctamente asociado a la cuenta empresarial.

La configuración deberá contemplar:

- Número telefónico.
- Identificador del número.
- Cuenta empresarial.
- Configuración de mensajería.
- Estado de la cuenta.

---

# 10. Credenciales

Las credenciales necesarias para comunicarse con Meta deberán mantenerse exclusivamente en el backend.

No deberán almacenarse directamente en:

```text
.NET MAUI
```

ni dentro del repositorio público.

Las credenciales deberán gestionarse mediante variables de entorno o un mecanismo seguro de almacenamiento de secretos.

Ejemplo conceptual:

```text
WHATSAPP_ACCESS_TOKEN
WHATSAPP_PHONE_NUMBER_ID
WHATSAPP_BUSINESS_ACCOUNT_ID
```

Los nombres anteriores son referencias conceptuales de configuración y podrán ajustarse a la implementación final.

---

# 11. Protección de credenciales

Las credenciales deberán:

- Mantenerse fuera del código fuente.
- No incluirse en Git.
- No almacenarse dentro de la aplicación móvil.
- No mostrarse en logs.
- No enviarse al cliente.
- Utilizarse únicamente desde el backend.

Ejemplo:

```text
.NET MAUI
    │
    └── NO posee Access Token

ASP.NET Core
    │
    └── Posee acceso seguro al secreto
```

---

# 12. Servicio de WhatsApp

Dentro del backend se recomienda separar la integración en un servicio específico.

Arquitectura conceptual:

```text
ASP.NET Core
      │
      ▼
WhatsAppService
      │
      ├── Enviar mensaje
      ├── Enviar plantilla
      ├── Procesar respuesta
      └── Registrar resultado
```

Esto permitirá mantener separada la lógica de WhatsApp de las demás reglas del sistema.

---

# 13. Flujo de envío

El flujo general será:

```text
Inicio
   │
   ▼
Evento del sistema
   │
   ▼
Determinar destinatario
   │
   ▼
Validar número
   │
   ▼
Determinar tipo de mensaje
   │
   ▼
Seleccionar plantilla
   │
   ▼
Reemplazar variables
   │
   ▼
ASP.NET Core
   │
   ▼
Meta WhatsApp API
   │
   ▼
WhatsApp
   │
   ▼
Cliente
```

---

# 14. Envío desde el backend

El backend deberá realizar las solicitudes hacia la plataforma de Meta.

La aplicación móvil solamente podrá solicitar al backend una determinada acción cuando corresponda.

Ejemplo:

```text
Administrador
      │
      ▼
.NET MAUI
      │
      │ Solicitar envío
      ▼
ASP.NET Core
      │
      ├── Validar permisos
      ├── Validar cliente
      ├── Validar número
      └── Preparar mensaje
              │
              ▼
          Meta API
```

---

# 15. Tipos de mensajes

El sistema podrá manejar diferentes tipos de comunicación.

Entre ellos:

- Recordatorios de pago.
- Avisos de pago vencido.
- Mensajes relacionados con morosidad.
- Confirmaciones.
- Mensajes administrativos.
- Mensajes personalizados permitidos por la plataforma.

Los mensajes que requieran plantillas deberán utilizar plantillas configuradas y aprobadas en Meta.

---

# 16. Recordatorio de pago

Uno de los principales usos será enviar recordatorios de pago.

Ejemplo conceptual:

```text
Hola Juan.

Te recordamos que tienes un pago de interés
semanal pendiente.

Fecha de vencimiento:
15/09/2026

Gracias.
```

Los datos deberán obtenerse del backend.

---

# 17. Aviso de pago vencido

Cuando exista un pago vencido, el sistema podrá enviar un mensaje.

Ejemplo:

```text
Hola Juan.

Tenemos registrado un pago pendiente
que ha superado su fecha de vencimiento.

Por favor, comunícate con el encargado
para regularizar tu situación.
```

El contenido definitivo deberá ajustarse a la plantilla aprobada correspondiente.

---

# 18. Mensajes de morosidad

Cuando un cliente presente morosidad, el sistema podrá enviar un mensaje correspondiente.

Ejemplo:

```text
Hola Juan.

Registramos pagos de interés pendientes.
Por favor, comunícate con nosotros
para revisar tu situación.
```

Los mensajes deberán respetar las políticas y requisitos aplicables de WhatsApp Business.

---

# 19. Morosidad crítica

La regla de negocio establecida para el sistema es:

```text
Más de 2 pagos de interés vencidos
```

Cuando se cumpla esta condición, el backend podrá generar una comunicación específica.

Ejemplo conceptual:

```text
Cliente:
Juan Pérez

Pagos vencidos:
3

Estado:
Morosidad crítica
```

La decisión de enviar el mensaje será realizada por el backend.

---

# 20. Variables dinámicas

Las plantillas podrán utilizar variables dinámicas.

Ejemplo:

```text
{nombre_cliente}
{fecha_vencimiento}
{monto_interes}
{saldo_pendiente}
{dias_mora}
```

El backend será responsable de reemplazar estas variables antes del envío.

Ejemplo:

```text
Plantilla:

Hola {nombre_cliente}.
Tu pago vence el {fecha_vencimiento}.

Datos reales:

Hola Juan Pérez.
Tu pago vence el 15/09/2026.
```

---

# 21. Validación de información

Antes de enviar un mensaje, el backend deberá verificar:

- Cliente existente.
- Cliente activo cuando corresponda.
- Número telefónico válido.
- Préstamo relacionado.
- Información financiera disponible.
- Plantilla válida.
- Variables completas.
- Permisos del usuario cuando el envío sea manual.

Si alguna validación falla, el mensaje no deberá enviarse.

---

# 22. Registro de mensajes

Cada mensaje deberá registrarse en PostgreSQL.

Información conceptual:

```text
WhatsAppMessage
├── Id
├── ClienteId
├── PrestamoId
├── UsuarioId
├── Tipo
├── Plantilla
├── NumeroDestino
├── Mensaje
├── MetaMessageId
├── Estado
├── FechaEnvio
├── Error
├── Intentos
├── CreatedAt
└── UpdatedAt
```

Esto permitirá mantener un historial de comunicación.

---

# 23. Estados de mensajes

Estados normativos unificados (ver `04-Base-Datos/03 §15`):

| Estado | Descripción |
|---|---|
| PENDIENTE | Mensaje pendiente de envío |
| ENVIADO | Solicitud enviada correctamente |
| ENTREGADO | Mensaje entregado |
| LEIDO | Mensaje leído |
| FALLIDO | Error en el envío |
| CANCELADO | Envío cancelado |

Quedan prohibidos `ENVIADA/LEIDA/ERROR/ENVIANDO/PROCESANDO` como sinónimos.

Los estados disponibles dependerán de la información proporcionada por Meta.

---

# 24. Identificador del mensaje

Cada mensaje enviado por Meta podrá tener un identificador único.

El backend deberá almacenarlo.

Ejemplo conceptual:

```text
MetaMessageId:
wamid.xxxxxxxxx
```

Este identificador permitirá relacionar las respuestas y eventos recibidos desde Meta con el mensaje original.

---

# 25. Webhooks

Los webhooks permitirán que Meta envíe información al backend.

El flujo será:

```text
Meta
  │
  │ Evento
  ▼
ASP.NET Core Web API
  │
  ▼
Webhook
  │
  ▼
Procesamiento
  │
  ▼
PostgreSQL
```

Los webhooks podrán utilizarse para recibir información relacionada con:

- Estado de mensajes.
- Entrega.
- Lectura.
- Errores.
- Eventos relacionados con la cuenta.

---

# 26. Endpoint de webhook

El backend deberá disponer de un endpoint específico para recibir eventos de Meta.

Ejemplo conceptual:

```text
POST /api/whatsapp/webhook
```

El endpoint deberá:

1. Recibir la solicitud.
2. Validar el origen cuando corresponda.
3. Procesar el evento.
4. Identificar el mensaje.
5. Actualizar su estado.
6. Registrar el evento.
7. Responder correctamente.

---

# 27. Verificación del webhook

La configuración del webhook deberá contemplar el mecanismo de verificación requerido por Meta.

El backend deberá comprobar que la solicitud de configuración corresponda al proceso esperado.

La información de verificación deberá mantenerse protegida.

---

# 28. Procesamiento de estados

Ejemplo:

```text
Mensaje enviado
      │
      ▼
Meta
      │
      ▼
Estado: entregado
      │
      ▼
Webhook
      │
      ▼
ASP.NET Core
      │
      ▼
PostgreSQL
      │
      ▼
Estado actualizado
```

Esto permitirá mantener actualizado el historial de mensajes.

---

# 29. Manejo de errores

El backend deberá manejar errores provenientes de Meta.

Ejemplos:

- Número inválido.
- Plantilla no disponible.
- Parámetros incorrectos.
- Credenciales inválidas.
- Restricciones de envío.
- Problemas temporales.
- Error de conexión.

El error deberá registrarse sin exponer credenciales sensibles.

---

# 30. Reintentos

Ante errores temporales, el sistema podrá realizar reintentos.

Ejemplo:

```text
Intento 1
   │
   └── Error temporal
          │
          ▼
Intento 2
   │
   └── Error temporal
          │
          ▼
Intento 3
   │
   └── Error definitivo
```

Los errores permanentes no deberán provocar reintentos indefinidos.

---

# 31. Prevención de duplicados

El sistema deberá evitar enviar varias veces el mismo mensaje por una misma causa.

Ejemplo:

```text
Recordatorio de pago
        │
        ▼
¿Ya fue enviado?
        │
   ┌────┴────┐
   │         │
  Sí         No
   │         │
   ▼         ▼
No enviar   Enviar
```

Se podrá utilizar una clave única basada en:

```text
ClienteId
PrestamoId
Tipo
Fecha
```

---

# 32. Programación automática

Los mensajes automáticos podrán integrarse con el módulo de programación de notificaciones.

Ejemplo:

```text
Scheduler
    │
    ▼
Detecta pago próximo
    │
    ▼
Genera evento
    │
    ▼
WhatsAppService
    │
    ▼
Meta WhatsApp API
```

Los horarios iniciales del sistema son:

```text
08:00
16:00
```

Zona horaria:

```text
America/Lima
```

---

# 33. Diferencia entre FCM y WhatsApp

Firebase Cloud Messaging y WhatsApp tendrán funciones diferentes.

| Servicio | Función |
|---|---|
| FCM | Notificaciones push dentro del ecosistema de la aplicación |
| WhatsApp | Mensajes enviados al número de WhatsApp del cliente |

Ejemplo:

```text
FCM
 │
 └── 📱 Notificación en la aplicación

WhatsApp
 │
 └── 💬 Mensaje al número del cliente
```

Ambos podrán utilizarse de forma complementaria.

---

# 34. Seguridad

La integración deberá cumplir las siguientes medidas:

- Utilizar HTTPS.
- Proteger tokens de acceso.
- No almacenar credenciales en .NET MAUI.
- No incluir secretos en Git.
- Validar permisos.
- Validar datos antes del envío.
- Registrar operaciones.
- Proteger endpoints.
- Evitar exponer información sensible en logs.

---

# 35. Control de acceso

No todos los usuarios deberán poder enviar mensajes manualmente.

El backend deberá verificar los permisos del usuario.

Ejemplo:

```text
Administrador
     │
     └── Puede enviar mensajes

Cobrador
     │
     └── Según permisos configurados
```

Las reglas definitivas de autorización se establecerán en el módulo de seguridad.

---

# 36. Auditoría

Las acciones de WhatsApp deberán ser auditables.

Se deberá registrar:

- Usuario que inició el envío.
- Cliente.
- Préstamo.
- Tipo de mensaje.
- Plantilla.
- Fecha.
- Hora.
- Resultado.
- Identificador de Meta.
- Error cuando corresponda.

---

# 37. Protección de datos

Los datos de los clientes deberán manejarse de acuerdo con las políticas de seguridad y protección de datos definidas para el sistema.

No se deberá enviar información innecesaria.

Los mensajes deberán contener únicamente la información requerida para cumplir su objetivo.

La información financiera detallada deberá consultarse dentro de la aplicación cuando sea necesario.

---

# 38. Logs

Los logs del backend podrán registrar:

```text
Solicitud iniciada
Cliente identificado
Plantilla seleccionada
Solicitud enviada
Respuesta recibida
Estado actualizado
```

No deberán registrarse:

```text
Access Token
Secretos
Credenciales
Información sensible innecesaria
```

---

# 39. Configuración por ambientes

La integración deberá permitir diferentes configuraciones para:

```text
Desarrollo
Pruebas
Producción
```

Ejemplo:

```text
Desarrollo
   │
   └── Credenciales de desarrollo

Pruebas
   │
   └── Credenciales de pruebas

Producción
   │
   └── Credenciales de producción
```

Las credenciales de cada ambiente deberán mantenerse separadas.

---

# 40. Pruebas

Se deberán realizar pruebas para validar:

- Configuración de la cuenta.
- Configuración del número.
- Autenticación.
- Envío de mensajes.
- Uso de plantillas.
- Variables dinámicas.
- Números inválidos.
- Mensajes fallidos.
- Webhooks.
- Estados de mensajes.
- Reintentos.
- Duplicados.
- Control de permisos.
- Auditoría.

---

# 41. Casos de prueba

## Caso 1: Envío correcto

**Dado:**

- Cliente válido.
- Número válido.
- Plantilla válida.
- Credenciales correctas.

**Resultado esperado:**

```text
Mensaje enviado correctamente.
```

---

## Caso 2: Número inválido

**Dado:**

- Número incorrecto.

**Resultado esperado:**

```text
El sistema registra el error
y no realiza reintentos indefinidos.
```

---

## Caso 3: Plantilla inválida

**Dado:**

- Plantilla no disponible.

**Resultado esperado:**

```text
Mensaje rechazado.
Error registrado.
```

---

## Caso 4: Error temporal

**Dado:**

- Meta presenta un error temporal.

**Resultado esperado:**

```text
Se realiza un reintento.
```

---

## Caso 5: Webhook

**Dado:**

- Meta informa que un mensaje fue entregado.

**Resultado esperado:**

```text
El backend actualiza el mensaje
a estado ENTREGADO.
```

---

## Caso 6: Mensaje duplicado

**Dado:**

- Ya existe un mensaje equivalente.

**Resultado esperado:**

```text
El sistema evita enviar un duplicado.
```

---

# 42. Criterios de aceptación

La integración será considerada correctamente implementada cuando:

- [ ] El backend pueda comunicarse con Meta WhatsApp Business Platform.
- [ ] Las credenciales permanezcan protegidas.
- [ ] La aplicación .NET MAUI no almacene credenciales de Meta.
- [ ] Se puedan enviar mensajes mediante el backend.
- [ ] Se puedan utilizar plantillas.
- [ ] Se puedan reemplazar variables dinámicas.
- [ ] Se registren los mensajes en PostgreSQL.
- [ ] Se almacene el identificador del mensaje.
- [ ] Se puedan procesar webhooks.
- [ ] Se actualicen los estados de los mensajes.
- [ ] Se controlen errores.
- [ ] Se realicen reintentos ante errores temporales.
- [ ] Se eviten mensajes duplicados.
- [ ] Se controlen los permisos.
- [ ] Se mantenga auditoría.
- [ ] Se utilice HTTPS.
- [ ] Las credenciales estén separadas por ambiente.

---

# 43. Dependencias

Este módulo depende de:

```text
03-Arquitectura/
├── 01-Arquitectura-General.md
├── 02-Stack-Tecnologico.md
├── 03-Arquitectura-Backend.md
└── 05-Integraciones.md

04-Base-Datos/

05-API/

07-Notificaciones/

09-Seguridad/
```

También se relacionará con:

```text
08-WhatsApp/
├── 02-Plantillas.md
└── 03-Flujo-Mensajeria.md
```

---

# 44. Consideraciones futuras

En futuras versiones se podrá implementar:

- Bandeja de mensajes.
- Historial avanzado de conversaciones.
- Estadísticas de envío.
- Estadísticas de entrega.
- Gestión avanzada de plantillas.
- Programación individual de mensajes.
- Diferentes campañas de comunicación.
- Respuestas automatizadas.
- Automatización de consultas frecuentes.
- Integración con otros canales de comunicación.

---

# 45. Resumen

La integración con WhatsApp será centralizada en el backend ASP.NET Core.

La arquitectura será:

```text
                .NET MAUI
                    │
                    │ HTTPS
                    ▼
          ASP.NET Core Web API
                    │
          ┌─────────┴─────────┐
          │                   │
          ▼                   ▼
      PostgreSQL       WhatsAppService
                              │
                              ▼
                   Meta WhatsApp Business
                              │
                              ▼
                          WhatsApp
```

El backend será responsable de:

- Validar solicitudes.
- Aplicar reglas de negocio.
- Obtener información del cliente.
- Preparar mensajes.
- Gestionar plantillas.
- Comunicarse con Meta.
- Registrar resultados.
- Procesar webhooks.
- Controlar errores.
- Mantener auditoría.

La aplicación .NET MAUI no tendrá acceso directo a las credenciales de Meta.

La integración permitirá complementar las notificaciones push de Firebase Cloud Messaging con mensajes enviados directamente al WhatsApp del cliente.