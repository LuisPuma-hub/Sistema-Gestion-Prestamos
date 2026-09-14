# 08 - Flujo de Mensajería de WhatsApp

## 1. Objetivo

Este documento define el flujo de mensajería utilizado por el **Sistema de Gestión de Préstamos** para generar, validar, enviar y registrar comunicaciones mediante **WhatsApp Business Platform de Meta**.

El objetivo es establecer un proceso controlado que permita:

- Detectar eventos relacionados con clientes y préstamos.
- Determinar cuándo debe enviarse un mensaje.
- Seleccionar la plantilla correspondiente.
- Obtener la información necesaria.
- Validar las variables.
- Evitar mensajes duplicados.
- Enviar el mensaje mediante Meta.
- Registrar el resultado.
- Procesar estados y errores.
- Mantener trazabilidad de las comunicaciones.

---

# 2. Alcance

El flujo contempla las comunicaciones automáticas y manuales relacionadas con:

- Recordatorios de pago.
- Pagos vencidos.
- Morosidad.
- Morosidad crítica.
- Confirmación de pagos.
- Reactivación.
- Comunicaciones administrativas.

El flujo será gestionado principalmente por el **backend ASP.NET Core Web API**.

La aplicación móvil desarrollada con **.NET MAUI** permitirá al administrador y al cobrador consultar y gestionar las comunicaciones de acuerdo con sus permisos.

---

# 3. Arquitectura general del flujo

El flujo general será:

```text
                 SISTEMA
                    │
                    ▼
             Evento de negocio
                    │
                    ▼
          Evaluación de reglas
                    │
                    ▼
          ¿Debe enviar mensaje?
               ┌────┴────┐
              No         Sí
              │           │
              ▼           ▼
           Finalizar   Seleccionar
                        plantilla
                           │
                           ▼
                    Obtener variables
                           │
                           ▼
                    Validar información
                           │
                           ▼
                   Verificar duplicado
                           │
                           ▼
                   Enviar solicitud
                           │
                           ▼
                WhatsApp Business API
                           │
                           ▼
                     Procesar respuesta
                           │
                           ▼
                  Registrar comunicación
                           │
                           ▼
                   Actualizar estado
```

---

# 4. Componentes involucrados

Los principales componentes del flujo son:

| Componente | Responsabilidad |
|---|---|
| .NET MAUI | Interfaz móvil |
| ASP.NET Core Web API | Lógica y coordinación |
| PostgreSQL | Persistencia de información |
| Entity Framework Core | Acceso a datos |
| Sistema de préstamos | Generación de eventos |
| Programador de tareas | Ejecución de procesos automáticos |
| WhatsApp Business Platform | Envío de mensajes |
| Meta | Procesamiento de mensajes |
| Sistema de auditoría | Registro de operaciones |

---

# 5. Responsabilidad del backend

El backend será el componente principal encargado de controlar el flujo.

Sus responsabilidades serán:

1. Detectar o recibir eventos.
2. Consultar la información del cliente.
3. Consultar el préstamo relacionado.
4. Evaluar las reglas de negocio.
5. Determinar el tipo de comunicación.
6. Seleccionar la plantilla.
7. Construir las variables.
8. Validar los datos.
9. Verificar duplicados.
10. Crear la solicitud de envío.
11. Enviar el mensaje a Meta.
12. Procesar la respuesta.
13. Registrar el mensaje.
14. Procesar posteriormente los estados recibidos.
15. Registrar errores.
16. Aplicar reintentos cuando corresponda.

---

# 6. Eventos que pueden generar mensajes

Los principales eventos serán:

| Evento | Descripción |
|---|---|
| PAGO_PROXIMO | Se aproxima la fecha del pago |
| PAGO_VENCIDO | Existe un pago pendiente |
| MOROSIDAD_DETECTADA | Se detecta morosidad |
| MOROSIDAD_CRITICA | Se supera el umbral establecido |
| PAGO_REGISTRADO | Se registra un pago |
| REACTIVACION | Cliente elegible para reactivación |
| COMUNICACION_ADMINISTRATIVA | Comunicación generada por administración |

---

# 7. Evento de pago próximo

Cuando el sistema detecte que se aproxima la fecha correspondiente al pago semanal, se generará el evento:

```text
PAGO_PROXIMO
```

El backend deberá:

1. Identificar el préstamo.
2. Identificar al cliente.
3. Obtener la fecha correspondiente.
4. Obtener el monto de referencia.
5. Seleccionar la plantilla de recordatorio.
6. Validar la información.
7. Verificar que no se haya enviado previamente el recordatorio.
8. Solicitar el envío.

Plantilla:

```text
wh_pago_recordatorio_v1
```

---

# 8. Evento de pago vencido

Cuando un pago supere la fecha esperada sin haber sido regularizado, se generará:

```text
PAGO_VENCIDO
```

Flujo:

```text
Fecha esperada
      ↓
¿Pago registrado?
   ┌──┴──┐
  Sí     No
  ↓       ↓
Finalizar  Generar evento
              ↓
        Seleccionar plantilla
              ↓
          Enviar mensaje
```

Plantilla:

```text
wh_pago_vencido_v1
```

---

# 9. Evento de morosidad

El sistema deberá controlar los pagos vencidos y determinar cuándo corresponde generar una comunicación de morosidad.

Evento:

```text
MOROSIDAD_DETECTADA
```

La evaluación deberá realizarse utilizando la información financiera almacenada y las reglas de negocio.

El mensaje podrá contener:

- Nombre del cliente.
- Número de semanas vencidas.
- Interés pendiente.
- Información para regularización.

Plantilla:

```text
wh_morosidad_aviso_v1
```

---

# 10. Evento de morosidad crítica

La regla establecida para el sistema considera morosidad crítica cuando existen:

> Más de 2 pagos de interés semanales vencidos.

Cuando esta condición se cumpla se generará:

```text
MOROSIDAD_CRITICA
```

Flujo:

```text
Evaluar pagos vencidos
        ↓
¿Más de 2 vencidos?
      ┌─┴─┐
     No   Sí
     ↓     ↓
   Finalizar
           ↓
    Generar evento
           ↓
    Seleccionar plantilla
           ↓
       Validar datos
           ↓
        Enviar
```

Plantilla:

```text
wh_morosidad_critica_v1
```

---

# 11. Evento de pago registrado

Cuando el administrador o cobrador registre correctamente un pago, el sistema podrá generar:

```text
PAGO_REGISTRADO
```

El mensaje permitirá informar al cliente que su pago fue registrado.

Plantilla:

```text
wh_pago_registrado_v1
```

La información financiera utilizada deberá provenir del backend.

---

# 12. Evento de reactivación

Cuando las reglas de negocio determinen que un cliente puede ser considerado para una reactivación, podrá generarse:

```text
REACTIVACION
```

El envío deberá respetar:

- Estado del cliente.
- Estado de los préstamos.
- Reglas de reactivación.
- Permisos del usuario.
- Configuración del sistema.

Plantilla:

```text
wh_cliente_reactivacion_v1
```

---

# 13. Flujo de selección de plantilla

El backend deberá relacionar cada evento con una plantilla.

```text
Evento
  ↓
Identificar tipo
  ↓
Consultar configuración
  ↓
Obtener plantilla
  ↓
¿Está habilitada?
 ┌────┴────┐
No         Sí
↓           ↓
Registrar   Continuar
error
```

Relación:

| Evento | Plantilla |
|---|---|
| PAGO_PROXIMO | `wh_pago_recordatorio_v1` |
| PAGO_VENCIDO | `wh_pago_vencido_v1` |
| MOROSIDAD_DETECTADA | `wh_morosidad_aviso_v1` |
| MOROSIDAD_CRITICA | `wh_morosidad_critica_v1` |
| PAGO_REGISTRADO | `wh_pago_registrado_v1` |
| REACTIVACION | `wh_cliente_reactivacion_v1` |
| COMUNICACION_ADMINISTRATIVA | `wh_administrativo_v1` |

---

# 14. Obtención de datos

Una vez seleccionada la plantilla, el backend deberá obtener la información necesaria.

Ejemplo:

```text
Cliente
├── Nombre
├── Teléfono
└── Estado

Préstamo
├── Número
├── Capital inicial
├── Interés
├── Saldo
└── Estado

Pago
├── Monto
├── Fecha
└── Estado
```

Los datos deberán obtenerse desde PostgreSQL mediante Entity Framework Core.

---

# 15. Construcción de variables

El backend deberá transformar la información obtenida en las variables necesarias para la plantilla.

Ejemplo:

```text
Plantilla:

Hola {{nombre_cliente}}.

Te recordamos que tienes un pago programado para el {{fecha_pago}}.

Monto de referencia: {{monto_pago}}.
```

Datos:

```text
nombre_cliente = Juan
fecha_pago = 10/09/2026
monto_pago = S/ 5.00
```

Resultado:

```text
Hola Juan.

Te recordamos que tienes un pago programado para el 10/09/2026.

Monto de referencia: S/ 5.00.
```

---

# 16. Validación de datos

Antes de enviar el mensaje se deberán validar:

### Cliente

- Existe.
- Tiene teléfono registrado.
- El teléfono tiene formato válido.
- El cliente puede recibir la comunicación.

### Préstamo

Cuando corresponda:

- Existe.
- Pertenece al cliente.
- Está correctamente identificado.

### Información financiera

- El monto es válido.
- El saldo es válido.
- El interés corresponde con la información del préstamo.
- La fecha es válida.

### Plantilla

- Existe.
- Está habilitada.
- Corresponde al evento.
- Tiene una versión válida.

---

# 17. Verificación de duplicados

Antes del envío, el sistema deberá verificar si el mismo evento ya fue procesado.

Ejemplo:

```text
Evento:
PAGO_PROXIMO

Cliente:
CL-001

Préstamo:
PRE-001

Fecha:
10/09/2026
```

Si ya existe un mensaje enviado para ese evento, el sistema deberá evitar generar otro mensaje idéntico.

---

# 18. Identificador de comunicación

Cada comunicación deberá disponer de un identificador interno.

Ejemplo:

```text
MSG-20260910-000001
```

Este identificador permitirá relacionar:

```text
Evento
  ↓
Mensaje
  ↓
Solicitud
  ↓
Respuesta de Meta
  ↓
Estado
```

---

# 19. Creación del mensaje

Una vez superadas las validaciones, el backend deberá crear un registro de comunicación.

Información conceptual:

```text
Mensaje
├── Id
├── ClienteId
├── PrestamoId
├── Evento
├── Plantilla
├── Version
├── FechaCreacion
└── Estado
```

Estado inicial:

```text
PENDIENTE
```

---

# 20. Envío a WhatsApp

El backend enviará la solicitud a la integración configurada con WhatsApp Business Platform.

Flujo:

```text
ASP.NET Core Web API
        ↓
WhatsAppService
        ↓
Construcción de solicitud
        ↓
WhatsApp Business Platform
        ↓
Respuesta
```

El componente `WhatsAppService` será responsable de encapsular la comunicación con Meta.

---

# 21. Resultado inicial del envío

La respuesta recibida deberá ser procesada por el backend.

Resultado conceptual:

```text
Solicitud
   ↓
Respuesta de Meta
   ↓
¿Solicitud aceptada?
 ┌─────┴─────┐
Sí           No
↓             ↓
Registrar     Registrar
identificador error
externo
```

La aceptación de la solicitud no debe interpretarse automáticamente como que el cliente ya leyó el mensaje.

---

# 22. Estados del mensaje

El sistema podrá manejar estados internos como:

| Estado | Descripción |
|---|---|
| PENDIENTE | Mensaje preparado para envío |
| ENVIANDO | Solicitud en proceso |
| ENVIADO | Solicitud aceptada por la plataforma |
| ENTREGADO | Mensaje entregado según el estado recibido |
| LEIDO | Mensaje leído según el estado recibido |
| ERROR | Ocurrió un error |
| CANCELADO | Envío cancelado antes de completarse |

La correspondencia exacta entre los estados externos y los estados internos deberá definirse durante la implementación.

---

# 23. Webhooks

La integración podrá utilizar webhooks para recibir eventos provenientes de Meta.

Los webhooks permitirán procesar información relacionada con:

- Estados de mensajes.
- Entrega.
- Lectura.
- Errores.
- Eventos relacionados con la cuenta configurada.

Flujo:

```text
WhatsApp / Meta
      ↓
Webhook
      ↓
ASP.NET Core Web API
      ↓
Validar evento
      ↓
Identificar mensaje
      ↓
Actualizar estado
      ↓
Registrar auditoría
```

---

# 24. Identificación del mensaje externo

Cuando Meta proporcione un identificador del mensaje, el sistema deberá almacenarlo.

Ejemplo:

```text
Id interno:
MSG-20260910-000001

Id externo:
ID proporcionado por Meta
```

Esto permitirá relacionar el registro interno con el mensaje gestionado por la plataforma.

---

# 25. Procesamiento de estados

Cuando se reciba un evento de estado:

```text
Webhook
   ↓
Obtener identificador externo
   ↓
Buscar mensaje
   ↓
¿Existe?
 ┌───┴───┐
Sí       No
↓         ↓
Actualizar Registrar
estado    evento no identificado
```

El sistema deberá mantener la trazabilidad de los cambios de estado.

---

# 26. Manejo de errores

Los errores podrán producirse por diferentes motivos:

- Número inválido.
- Plantilla no disponible.
- Error de autenticación.
- Credenciales incorrectas.
- Problemas de conexión.
- Restricciones de la plataforma.
- Datos incompletos.
- Error interno del backend.

Cada error deberá registrarse.

Ejemplo:

```text
Mensaje
   ↓
Error
   ↓
Registrar error
   ↓
¿Es temporal?
 ┌────┴────┐
Sí         No
↓           ↓
Reintentar  Finalizar
```

---

# 27. Reintentos

Los errores temporales podrán generar reintentos.

El sistema deberá:

1. Registrar el intento.
2. Determinar si el error es reintentable.
3. Aplicar un límite de intentos.
4. Esperar el intervalo configurado.
5. Reintentar.
6. Registrar el resultado.

No deberán realizarse reintentos infinitos.

---

# 28. Control de idempotencia

La idempotencia será importante para evitar mensajes duplicados.

Ejemplo:

```text
Evento PAGO_REGISTRADO
        ↓
Crear identificador de evento
        ↓
¿Ya fue procesado?
   ┌────┴────┐
  Sí         No
  ↓           ↓
No enviar   Procesar
              ↓
            Enviar
```

El backend deberá utilizar mecanismos que permitan detectar solicitudes ya procesadas.

---

# 29. Programación de mensajes

Los mensajes automáticos relacionados con pagos y morosidad podrán generarse mediante procesos programados.

Los horarios iniciales definidos para las notificaciones del sistema son:

```text
08:00
16:00
```

El programador deberá:

1. Ejecutarse en los horarios configurados.
2. Consultar los eventos pendientes.
3. Evaluar las reglas.
4. Generar las comunicaciones correspondientes.
5. Evitar duplicados.
6. Registrar resultados.

---

# 30. Flujo del programador

```text
Programador
    ↓
08:00 / 16:00
    ↓
Consultar préstamos
    ↓
Evaluar pagos
    ↓
Evaluar morosidad
    ↓
Generar eventos
    ↓
Procesar comunicaciones
    ↓
Enviar mensajes
    ↓
Registrar resultados
```

El procesamiento deberá realizarse en el backend y no depender de que la aplicación móvil esté abierta.

---

# 31. Mensajes manuales

Además de los mensajes automáticos, determinadas comunicaciones podrán iniciarse manualmente desde la aplicación.

Ejemplo:

```text
Administrador/Cobrador
        ↓
Seleccionar cliente
        ↓
Seleccionar comunicación
        ↓
Confirmar acción
        ↓
Backend
        ↓
Validar permisos
        ↓
Validar datos
        ↓
Enviar
```

La disponibilidad de esta funcionalidad dependerá de los permisos asignados al usuario.

---

# 32. Validación de permisos

Antes de permitir una comunicación manual, el backend deberá verificar:

- Usuario autenticado.
- Rol del usuario.
- Permiso correspondiente.
- Cliente válido.
- Comunicación permitida.
- Plantilla habilitada.

El backend deberá realizar la validación incluso si la aplicación móvil ya la realizó.

---

# 33. Registro de auditoría

Las comunicaciones deberán generar información de auditoría.

Se deberá registrar cuando corresponda:

- Usuario que inició la acción.
- Evento.
- Cliente.
- Préstamo.
- Plantilla.
- Fecha y hora.
- Resultado.
- Error.
- Identificador del mensaje.

Para procesos automáticos deberá identificarse que el origen fue un proceso automático.

---

# 34. Historial de comunicaciones

La aplicación deberá permitir consultar el historial de comunicaciones según los permisos correspondientes.

Información:

| Campo | Descripción |
|---|---|
| Fecha | Fecha y hora |
| Cliente | Destinatario |
| Préstamo | Préstamo asociado |
| Evento | Motivo |
| Plantilla | Plantilla utilizada |
| Estado | Estado actual |
| Resultado | Resultado del proceso |

---

# 35. Relación con los pagos

El flujo de WhatsApp no deberá modificar directamente los valores financieros.

Por ejemplo, cuando se registra un pago:

```text
Registrar pago
      ↓
Backend procesa pago
      ↓
Aplicar primero al interés
      ↓
Excedente al capital
      ↓
Actualizar saldo
      ↓
Confirmar operación
      ↓
Generar evento PAGO_REGISTRADO
      ↓
Enviar confirmación WhatsApp
```

El mensaje deberá utilizar los valores resultantes de la operación financiera.

---

# 36. Relación con la morosidad

La morosidad deberá determinarse mediante las reglas financieras del sistema.

Flujo:

```text
Consultar pagos
      ↓
Identificar vencidos
      ↓
Contabilizar semanas
      ↓
Evaluar condición
      ↓
¿Morosidad?
 ┌────┴────┐
No         Sí
↓           ↓
Finalizar   Generar evento
                ↓
         Seleccionar plantilla
                ↓
              Enviar
```

La aplicación móvil no deberá determinar por sí sola si un cliente es moroso.

---

# 37. Seguridad de las credenciales

Las credenciales utilizadas para la integración con Meta deberán mantenerse exclusivamente en el backend.

No deberán almacenarse en:

- Código de la aplicación móvil.
- Archivos públicos.
- Repositorios Git.
- Variables visibles en la interfaz.
- Documentación pública.

Se recomienda utilizar mecanismos seguros de configuración de secretos para los diferentes entornos.

---

# 38. Separación de ambientes

Se deberán considerar al menos:

```text
Desarrollo
   ↓
Pruebas
   ↓
Producción
```

Cada ambiente deberá utilizar su propia configuración correspondiente.

No se deberán utilizar credenciales de producción durante las pruebas de desarrollo.

---

# 39. Flujo completo automático

El flujo completo será:

```text
┌─────────────────────────────┐
│     Evento del sistema      │
└──────────────┬──────────────┘
               ↓
┌─────────────────────────────┐
│ Evaluar reglas de negocio   │
└──────────────┬──────────────┘
               ↓
        ¿Enviar mensaje?
          ┌────┴────┐
         No         Sí
         ↓           ↓
      Finalizar   Obtener datos
                     ↓
              Seleccionar plantilla
                     ↓
               Validar variables
                     ↓
             Verificar duplicados
                     ↓
              Crear comunicación
                     ↓
               Enviar a Meta
                     ↓
              Procesar respuesta
                     ↓
              Registrar resultado
                     ↓
              Recibir estados
                     ↓
              Actualizar historial
```

---

# 40. Flujo completo manual

Para comunicaciones iniciadas por un usuario:

```text
Administrador/Cobrador
          ↓
Seleccionar cliente
          ↓
Seleccionar comunicación
          ↓
Confirmar
          ↓
ASP.NET Core Web API
          ↓
Validar autenticación
          ↓
Validar autorización
          ↓
Validar cliente
          ↓
Validar plantilla
          ↓
Construir variables
          ↓
Verificar duplicados
          ↓
Enviar a Meta
          ↓
Registrar resultado
```

---

# 41. Arquitectura lógica

La implementación deberá mantener separación de responsabilidades.

Una estructura conceptual del backend será:

```text
API
 │
 ├── Controllers
 │
 ├── Services
 │    ├── LoanService
 │    ├── PaymentService
 │    ├── MorosidadService
 │    ├── NotificationService
 │    └── WhatsAppService
 │
 ├── Repositories / Data Access
 │
 ├── Entities
 │
 ├── DTOs
 │
 └── Infrastructure
      ├── PostgreSQL
      ├── Firebase
      └── WhatsApp Meta
```

---

# 42. WhatsAppService

El componente `WhatsAppService` tendrá como responsabilidad encapsular la comunicación con WhatsApp Business Platform.

Responsabilidades:

- Preparar solicitudes.
- Utilizar la plantilla correspondiente.
- Enviar mensajes.
- Procesar respuestas.
- Identificar errores.
- Retornar información al servicio que lo invocó.

No deberá contener directamente las reglas financieras del sistema.

---

# 43. Separación de responsabilidades

Las responsabilidades deberán mantenerse separadas:

| Componente | Responsabilidad |
|---|---|
| LoanService | Gestión de préstamos |
| PaymentService | Gestión de pagos |
| MorosidadService | Evaluación de morosidad |
| NotificationService | Gestión de notificaciones |
| WhatsAppService | Integración con WhatsApp |
| AuditService | Auditoría |
| Scheduler | Procesos programados |

Esto facilita el mantenimiento y las pruebas del sistema.

---

# 44. Integración con Firebase

WhatsApp y Firebase FCM tendrán funciones diferentes.

Ejemplo:

```text
Pago próximo
     │
     ├───────────────┐
     ↓               ↓
   FCM             WhatsApp
     ↓               ↓
Usuario interno    Cliente
```

FCM podrá utilizarse para notificar al administrador o cobrador dentro de la aplicación.

WhatsApp podrá utilizarse para comunicarse con el cliente.

---

# 45. Ejemplo: pago próximo

Supongamos:

```text
Cliente: Juan
Préstamo: PRE-001
Capital inicial: S/ 100.00
Interés semanal: 5 %
```

El interés semanal correspondiente al capital inicial sería:

```text
S/ 100.00 × 5 % = S/ 5.00
```

Cuando corresponda enviar el recordatorio:

```text
Evento
PAGO_PROXIMO
      ↓
Plantilla
wh_pago_recordatorio_v1
      ↓
Variables
Juan
fecha correspondiente
S/ 5.00
      ↓
WhatsApp
```

El valor financiero deberá ser calculado o validado por el backend.

---

# 46. Ejemplo: pago vencido

Si el cliente no realiza el pago correspondiente:

```text
Fecha esperada
      ↓
No existe pago registrado
      ↓
PAGO_VENCIDO
      ↓
wh_pago_vencido_v1
      ↓
Enviar comunicación
      ↓
Registrar resultado
```

Si posteriormente el cliente realiza el pago:

```text
Pago registrado
      ↓
Actualizar préstamo
      ↓
PAGO_REGISTRADO
      ↓
wh_pago_registrado_v1
```

---

# 47. Ejemplo: morosidad crítica

Si el sistema detecta más de 2 pagos de interés semanales vencidos:

```text
Pagos vencidos
      ↓
Evaluación
      ↓
Más de 2
      ↓
MOROSIDAD_CRITICA
      ↓
wh_morosidad_critica_v1
      ↓
Validación
      ↓
Envío
      ↓
Registro
```

La comunicación deberá utilizar información actualizada del cliente y del préstamo.

---

# 48. Trazabilidad

Cada mensaje deberá poder rastrearse desde su origen hasta su resultado.

La trazabilidad será:

```text
Evento de negocio
      ↓
ID de evento
      ↓
ID de comunicación
      ↓
Plantilla
      ↓
Solicitud
      ↓
ID externo
      ↓
Estado
      ↓
Resultado final
```

Esto permitirá investigar problemas y mantener un historial confiable.

---

# 49. Consideraciones de rendimiento

El procesamiento de mensajes deberá evitar afectar negativamente las operaciones principales del sistema.

Se recomienda:

- Procesar comunicaciones de forma desacoplada cuando sea conveniente.
- Evitar bloquear operaciones financieras esperando respuestas externas.
- Controlar reintentos.
- Registrar tiempos de procesamiento.
- Evitar consultas innecesarias.
- Procesar lotes de mensajes cuando corresponda.

---

# 50. Disponibilidad

Si WhatsApp no está disponible temporalmente, las operaciones principales del sistema no deberán quedar inutilizadas.

Por ejemplo:

```text
Registrar pago
      ↓
Pago procesado correctamente
      ↓
Generar comunicación
      ↓
WhatsApp no disponible
      ↓
Registrar pendiente/error
      ↓
Reintentar posteriormente
```

La indisponibilidad temporal de WhatsApp no deberá revertir una operación financiera que ya fue confirmada correctamente.

---

# 51. Consistencia financiera

El sistema deberá priorizar la consistencia de las operaciones financieras.

Las siguientes operaciones deberán procesarse en el backend:

- Cálculo de intereses.
- Registro de pagos.
- Aplicación del pago al interés.
- Aplicación del excedente al capital.
- Actualización del saldo.
- Determinación de morosidad.

WhatsApp deberá utilizar los resultados confirmados de estas operaciones.

---

# 52. Criterios de aceptación

El flujo de mensajería será considerado correctamente implementado cuando:

- [ ] Los eventos puedan generar comunicaciones.
- [ ] Cada evento tenga una plantilla asociada.
- [ ] El backend valide los datos antes del envío.
- [ ] El sistema evite mensajes duplicados.
- [ ] Los mensajes sean enviados mediante la integración configurada con Meta.
- [ ] Los identificadores externos sean registrados.
- [ ] Los estados recibidos sean procesados.
- [ ] Los errores sean registrados.
- [ ] Existan mecanismos de reintento.
- [ ] Se mantenga la trazabilidad.
- [ ] Las comunicaciones automáticas funcionen aunque la aplicación móvil esté cerrada.
- [ ] Los horarios programados sean respetados.
- [ ] Las comunicaciones manuales respeten los permisos.
- [ ] Las operaciones financieras permanezcan centralizadas en el backend.
- [ ] Las credenciales de Meta permanezcan protegidas.
- [ ] Los mensajes puedan consultarse en el historial.

---

# 53. Dependencias

Este flujo depende de:

- ASP.NET Core Web API.
- .NET MAUI.
- PostgreSQL.
- Entity Framework Core.
- WhatsApp Business Platform de Meta.
- Configuración de Meta Business.
- Plantillas de WhatsApp.
- Sistema de préstamos.
- Sistema de pagos.
- Sistema de morosidad.
- Programador de tareas.
- Sistema de autenticación.
- Sistema de autorización.
- Sistema de auditoría.

---

# 54. Resumen

El flujo de mensajería de WhatsApp permitirá automatizar las comunicaciones entre el sistema y los clientes.

El proceso comenzará con un evento de negocio, como:

- Pago próximo.
- Pago vencido.
- Morosidad.
- Morosidad crítica.
- Pago registrado.
- Reactivación.

El backend ASP.NET Core Web API evaluará las reglas de negocio, seleccionará la plantilla, obtendrá las variables, validará la información y enviará la comunicación mediante WhatsApp Business Platform de Meta.

Posteriormente, el sistema registrará el resultado y procesará los estados recibidos para mantener un historial completo.

La arquitectura garantiza que la lógica financiera permanezca centralizada en el backend y que WhatsApp funcione como un canal de comunicación desacoplado de las operaciones principales del sistema.