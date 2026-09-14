# Pantalla de WhatsApp

## 1. Información General

| Campo | Detalle |
|---|---|
| Nombre | Pantalla de WhatsApp |
| Archivo | `12-Pantalla-WhatsApp.md` |
| Módulo | WhatsApp |
| Tipo | Gestión y envío de mensajes |
| Acceso | Administrador y Cobrador |
| Prioridad | Alta |
| Plataforma | Aplicación móvil |
| Integración | Meta WhatsApp Business Platform |
| Moneda | Soles (S/) |
| Zona horaria | `America/Lima` |
| Formato de fecha | `DD/MM/YYYY` |

---

# 2. Objetivo

La pantalla de WhatsApp permitirá gestionar y enviar mensajes personalizados a los clientes del sistema mediante la integración con WhatsApp Business.

El módulo estará orientado principalmente a:

- Recordatorios de pago.
- Avisos de pagos vencidos.
- Comunicaciones relacionadas con morosidad.
- Confirmaciones de pago.
- Mensajes personalizados.
- Seguimiento de comunicaciones.
- Consulta del historial de mensajes enviados.
- Uso de plantillas autorizadas.

La aplicación deberá utilizar la integración oficial correspondiente y respetar las políticas y restricciones establecidas por Meta.

---

# 3. Acceso a la Pantalla

La pantalla podrá ser accesible desde el menú principal:

```text
Dashboard
   └── WhatsApp
```

También podrá abrirse desde:

```text
Clientes
   └── Cliente
        └── WhatsApp
```

Desde préstamos:

```text
Préstamos
   └── Detalle del préstamo
          └── WhatsApp
```

Desde morosidad:

```text
Morosidad
   └── Cliente / Préstamo
          └── WhatsApp
```

---

# 4. Estructura General

La pantalla estará organizada en diferentes secciones.

```text
┌──────────────────────────────────────┐
│ ← WhatsApp                           │
├──────────────────────────────────────┤
│                                      │
│ [Nuevo mensaje]                      │
│                                      │
├──────────────────────────────────────┤
│ Mensajes recientes                  │
│                                      │
│ Juan Pérez                           │
│ Recordatorio de pago                │
│ Enviado                              │
│ 04/09/2026 08:00                    │
│                                      │
├──────────────────────────────────────┤
│ María López                          │
│ Pago vencido                         │
│ Entregado                            │
│ 04/09/2026 08:15                    │
└──────────────────────────────────────┘
```

---

# 5. Encabezado

El encabezado deberá mostrar:

- Botón regresar.
- Título `WhatsApp`.
- Acción para crear un mensaje.
- Opcionalmente botón de actualización.

Ejemplo:

```text
← WhatsApp                         +
```

El botón `+` permitirá iniciar el envío de un nuevo mensaje.

---

# 6. Secciones Principales

La pantalla podrá organizarse mediante pestañas.

```text
[Mensajes] [Plantillas] [Historial]
```

## Mensajes

Permite consultar y enviar mensajes.

## Plantillas

Permite consultar las plantillas disponibles.

## Historial

Permite consultar las comunicaciones realizadas.

---

# 7. Nuevo Mensaje

Al seleccionar:

```text
[Nuevo mensaje]
```

se abrirá el formulario de envío.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ ← Nuevo mensaje                      │
├──────────────────────────────────────┤
│ Cliente                              │
│ [Seleccionar cliente]                │
│                                      │
│ Teléfono                             │
│ +51 999 999 999                      │
│                                      │
│ Tipo de mensaje                      │
│ [Seleccionar plantilla]              │
│                                      │
│ Mensaje                              │
│ ┌──────────────────────────────────┐ │
│ │ Hola {{nombre}}...               │ │
│ └──────────────────────────────────┘ │
│                                      │
│ [Vista previa]                       │
│                                      │
│ [Cancelar]       [Enviar]            │
└──────────────────────────────────────┘
```

---

# 8. Selección del Cliente

El usuario podrá seleccionar un cliente registrado en el sistema.

El selector deberá permitir buscar por:

- Nombre.
- Apellidos.
- Número de documento.
- Número de teléfono.

Ejemplo:

```text
🔍 Buscar cliente...
```

Resultado:

```text
Juan Pérez
DNI: 12345678
+51 999 999 999
```

Una vez seleccionado el cliente se mostrará su información básica.

---

# 9. Validación del Número de Teléfono

Antes de enviar un mensaje, el sistema deberá verificar que el cliente tenga un número válido.

Se deberá validar:

- Existencia del número.
- Formato correcto.
- Código de país cuando corresponda.
- Disponibilidad del número para WhatsApp según la integración.

Si no existe teléfono:

```text
Este cliente no tiene un número de teléfono
registrado.
```

El botón de envío deberá permanecer deshabilitado.

---

# 10. Plantillas de WhatsApp

Los mensajes automáticos deberán utilizar plantillas previamente configuradas y aprobadas para los casos en que la plataforma lo requiera.

Ejemplos:

```text
Recordatorio de pago
Pago vencido
Morosidad
Confirmación de pago
```

La aplicación deberá consultar las plantillas disponibles desde el backend.

---

# 11. Selección de Plantilla

El usuario podrá seleccionar una plantilla.

Ejemplo:

```text
Seleccionar plantilla

○ Recordatorio de pago
○ Pago vencido
○ Morosidad
○ Confirmación de pago
○ Mensaje personalizado
```

Al seleccionar una plantilla, se cargarán sus variables.

---

# 12. Variables de Plantilla

Las plantillas podrán utilizar variables dinámicas.

Ejemplo:

```text
Hola {{nombre}},

te recordamos que tienes un pago pendiente
correspondiente al préstamo {{prestamo}}.

Monto pendiente: {{monto}}
Fecha de vencimiento: {{fecha}}
```

Variables posibles:

```text
{{nombre}}
{{prestamo}}
{{monto}}
{{fecha}}
{{interes}}
{{capital}}
{{semanas_morosidad}}
```

El backend deberá proporcionar los valores correspondientes.

---

# 13. Mensaje Personalizado

Cuando las reglas y la integración lo permitan, el usuario podrá enviar un mensaje personalizado.

Ejemplo:

```text
Mensaje:

Hola Juan, te recordamos que puedes
comunicarte con nosotros para coordinar
tu próximo pago.
```

El sistema deberá aplicar las restricciones correspondientes de la plataforma de WhatsApp Business.

---

# 14. Vista Previa

Antes de enviar el mensaje se deberá mostrar una vista previa.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ Vista previa                         │
├──────────────────────────────────────┤
│                                      │
│ Hola Juan Pérez.                     │
│                                      │
│ Te recordamos que tienes un pago     │
│ pendiente de S/ 25.00 correspondiente│
│ al préstamo PR-000125.               │
│                                      │
└──────────────────────────────────────┘

[Cancelar]              [Enviar]
```

La vista previa deberá mostrar el mensaje con las variables reemplazadas.

---

# 15. Confirmación de Envío

Antes de realizar el envío se deberá solicitar confirmación.

Ejemplo:

```text
¿Deseas enviar este mensaje?

Cliente:
Juan Pérez

Número:
+51 999 999 999

Plantilla:
Recordatorio de pago

[Cancelar]      [Confirmar envío]
```

La confirmación permitirá evitar envíos accidentales.

---

# 16. Envío del Mensaje

El flujo de envío será:

```text
Seleccionar cliente
        ↓
Seleccionar plantilla
        ↓
Completar variables
        ↓
Vista previa
        ↓
Confirmar
        ↓
Backend
        ↓
Meta WhatsApp Business
        ↓
Resultado
```

La aplicación móvil no deberá comunicarse directamente con las credenciales privadas de Meta.

---

# 17. Arquitectura del Envío

La comunicación deberá seguir una arquitectura similar a:

```text
Aplicación móvil
       ↓
API Backend
       ↓
Servicio WhatsApp
       ↓
Meta WhatsApp Business Platform
       ↓
WhatsApp
       ↓
Cliente
```

Las credenciales y tokens de acceso deberán permanecer protegidos en el backend.

---

# 18. Estados del Mensaje

Cada mensaje podrá presentar diferentes estados.

## 18.1 Pendiente

```text
PENDIENTE
```

El mensaje ha sido solicitado pero todavía no se ha confirmado su envío.

## 18.2 Enviado

```text
ENVIADO
```

El mensaje fue aceptado para envío.

## 18.3 Entregado

```text
ENTREGADO
```

La plataforma confirmó la entrega cuando dicha información esté disponible.

## 18.4 Leído

```text
LEÍDO
```

La plataforma confirmó la lectura cuando dicha información esté disponible.

## 18.5 Error

```text
ERROR
```

El envío no pudo completarse.

---

# 19. Indicadores de Estado

Ejemplo:

```text
Juan Pérez
Recordatorio de pago

✓ Enviado
04/09/2026 08:00
```

Otro ejemplo:

```text
María López
Pago vencido

✓✓ Entregado
04/09/2026 08:15
```

El diseño deberá incluir también texto explícito y no depender únicamente de iconos o colores.

---

# 20. Historial de Mensajes

La pantalla deberá permitir consultar el historial de mensajes.

Cada registro podrá mostrar:

- Cliente.
- Teléfono.
- Tipo de mensaje.
- Plantilla.
- Fecha.
- Hora.
- Estado.
- Usuario que inició el envío.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ Juan Pérez                           │
│ Recordatorio de pago                 │
│ 04/09/2026 08:00                     │
│ Estado: Entregado                    │
└──────────────────────────────────────┘
```

---

# 21. Búsqueda de Mensajes

El historial deberá permitir buscar por:

- Nombre del cliente.
- Documento.
- Teléfono.
- Tipo de mensaje.
- Código de préstamo.

Ejemplo:

```text
🔍 Buscar mensaje...
```

---

# 22. Filtros del Historial

Se podrán utilizar filtros como:

```text
[Todos]
[Enviados]
[Entregados]
[Leídos]
[Error]
```

También podrán existir filtros por:

- Fecha.
- Cliente.
- Préstamo.
- Tipo de mensaje.
- Usuario.

---

# 23. Detalle del Mensaje

Al seleccionar un mensaje se mostrará su información completa.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ ← Detalle del mensaje                │
├──────────────────────────────────────┤
│ Cliente: Juan Pérez                  │
│ Teléfono: +51 999 999 999            │
│                                      │
│ Plantilla: Recordatorio de pago      │
│                                      │
│ Mensaje:                             │
│ Hola Juan Pérez...                   │
│                                      │
│ Enviado: 04/09/2026 08:00             │
│ Estado: Entregado                    │
│                                      │
│ Préstamo: PR-000125                  │
└──────────────────────────────────────┘
```

---

# 24. Mensajes Relacionados con Pagos

El sistema podrá utilizar WhatsApp para enviar recordatorios de pago.

Ejemplo:

```text
Recordatorio de pago

Cliente: Juan Pérez
Préstamo: PR-000125
Interés semanal: S/ 25.00
Vencimiento: 07/09/2026
```

El contenido definitivo dependerá de la plantilla configurada.

---

# 25. Mensajes de Morosidad

Cuando exista morosidad, el sistema podrá enviar un mensaje al cliente.

Ejemplo:

```text
Aviso de pago pendiente

Hola Juan Pérez.

Te informamos que el préstamo PR-000125
presenta un pago pendiente.

Por favor, comunícate con nosotros para
coordinar la regularización.
```

Cuando exista morosidad crítica, el sistema podrá utilizar una plantilla específica.

---

# 26. Mensajes de Confirmación de Pago

Después de registrar correctamente un pago, el sistema podrá enviar una confirmación.

Ejemplo:

```text
Confirmación de pago

Hola Juan Pérez.

Se registró correctamente tu pago
correspondiente al préstamo PR-000125.

Monto recibido: S/ 100.00

Gracias.
```

La generación y envío deberá respetar las reglas configuradas.

---

# 27. Mensajes Automáticos

Algunas comunicaciones podrán generarse automáticamente.

Ejemplo:

```text
Próximo vencimiento
        ↓
Backend detecta condición
        ↓
Selecciona plantilla
        ↓
Obtiene datos del cliente
        ↓
Genera mensaje
        ↓
WhatsApp
```

La aplicación móvil no deberá ejecutar por sí misma procesos críticos de mensajería programada.

---

# 28. Horarios de Mensajería

Cuando existan mensajes programados, se deberán respetar los horarios configurados.

Horarios iniciales:

```text
08:00
16:00
```

Zona horaria:

```text
America/Lima
```

Estos horarios podrán modificarse según la configuración del sistema.

---

# 29. Prevención de Envíos Duplicados

El sistema deberá evitar el envío accidental de mensajes duplicados.

Antes de enviar se deberá verificar:

- Cliente.
- Préstamo.
- Tipo de mensaje.
- Plantilla.
- Fecha.
- Mensajes enviados recientemente.
- Estado del mensaje anterior.

Ejemplo:

```text
Ya existe un mensaje de recordatorio
enviado para este préstamo recientemente.

¿Deseas continuar?

[Cancelar]    [Enviar]
```

Las validaciones definitivas deberán realizarse en el backend.

---

# 30. Control de Envíos Automáticos

El sistema deberá permitir controlar los mensajes automáticos para evitar comunicaciones excesivas.

Se podrán establecer reglas como:

```text
Un recordatorio por periodo
Un mensaje de morosidad por periodo
```

Las reglas definitivas deberán establecerse en:

```text
02-Reglas-Negocio/06-Reglas-WhatsApp.md
```

---

# 31. Error de Envío

Si el envío falla, deberá mostrarse un mensaje claro.

Ejemplo:

```text
No fue posible enviar el mensaje.

Motivo:
El servicio de WhatsApp no está disponible.

[Reintentar]
```

El mensaje deberá quedar registrado con estado:

```text
ERROR
```

cuando corresponda.

---

# 32. Reintento

Los mensajes que fallen podrán permitir un nuevo intento cuando las condiciones lo permitan.

Ejemplo:

```text
Estado: ERROR

[Reintentar]
```

Antes del reintento se deberá verificar nuevamente:

- Cliente.
- Número.
- Plantilla.
- Autorización.
- Estado de la integración.

---

# 33. Estado de la Integración

La pantalla podrá mostrar el estado de la conexión con el servicio de WhatsApp.

Ejemplo:

```text
WhatsApp
● Conectado
```

O:

```text
WhatsApp
● Servicio no disponible
```

El estado deberá provenir del backend.

No se deberán mostrar credenciales ni tokens.

---

# 34. Configuración de WhatsApp

Las configuraciones administrativas relacionadas con la integración podrán incluir:

- Estado de integración.
- Número empresarial.
- Plantillas.
- Mensajes automáticos.
- Horarios.
- Reglas de envío.

Estas configuraciones deberán gestionarse según los permisos del usuario.

---

# 35. Permisos por Rol

## 35.1 Administrador

El Administrador podrá:

- Consultar mensajes.
- Enviar mensajes.
- Consultar historial.
- Gestionar plantillas cuando corresponda.
- Configurar mensajes automáticos.
- Consultar estados.
- Consultar errores.
- Gestionar la integración si tiene los permisos correspondientes.

## 35.2 Cobrador

El Cobrador podrá:

- Consultar clientes.
- Enviar mensajes autorizados.
- Utilizar plantillas disponibles.
- Consultar historial relacionado con sus acciones.
- Contactar clientes desde préstamos y morosidad.

Las acciones de configuración deberán restringirse al personal autorizado.

---

# 36. Seguridad

La integración deberá cumplir con medidas de seguridad.

Se deberá:

- Utilizar HTTPS.
- Mantener tokens y credenciales exclusivamente en el backend.
- No almacenar tokens privados en la aplicación móvil.
- Validar permisos.
- Registrar operaciones importantes.
- Proteger los datos del cliente.
- Validar los mensajes en backend.
- Evitar exposición de información financiera innecesaria.

---

# 37. Protección de Credenciales

Las credenciales de Meta WhatsApp Business nunca deberán almacenarse directamente en el código de la aplicación móvil.

No se deberá utilizar:

```text
ACCESS_TOKEN = "token-real"
```

dentro de Flutter.

Las credenciales deberán gestionarse en el backend mediante mecanismos seguros.

---

# 38. Auditoría

Las acciones relacionadas con WhatsApp deberán registrarse cuando corresponda.

Ejemplos:

```text
Mensaje creado
Mensaje enviado
Mensaje entregado
Mensaje leído
Mensaje fallido
Reintento
Plantilla seleccionada
Configuración modificada
```

La auditoría podrá incluir:

- Usuario.
- Fecha.
- Hora.
- Cliente.
- Préstamo.
- Tipo de mensaje.
- Resultado.
- Identificador del mensaje externo cuando corresponda.

---

# 39. Privacidad

Los mensajes deberán contener únicamente la información necesaria.

No se deberá incluir:

- Contraseñas.
- Tokens.
- Información interna del sistema.
- Datos sensibles innecesarios.
- Información de otros clientes.

Ejemplo recomendado:

```text
Hola Juan.

Te recordamos que tienes un pago pendiente
de tu préstamo.

Comunícate con nosotros para coordinar.
```

---

# 40. Estado de Carga

Mientras se procesa un envío:

```text
Enviando mensaje...

⟳
```

El botón de envío deberá deshabilitarse temporalmente.

```text
[Enviando...]
```

Esto evita múltiples solicitudes accidentales.

---

# 41. Estado Sin Historial

Si no existen mensajes:

```text
┌──────────────────────────────────────┐
│                                      │
│               💬                     │
│                                      │
│      No existen mensajes enviados    │
│                                      │
│          [Nuevo mensaje]             │
│                                      │
└──────────────────────────────────────┘
```

---

# 42. Estado de Error

Si no es posible cargar el historial:

```text
┌──────────────────────────────────────┐
│                                      │
│               ⚠                      │
│                                      │
│   No fue posible cargar el historial.│
│                                      │
│            [Reintentar]              │
│                                      │
└──────────────────────────────────────┘
```

---

# 43. Paginación

El historial deberá utilizar paginación cuando exista una cantidad elevada de registros.

Ejemplo:

```text
Mostrando 1 - 20 de 150

[Cargar más]
```

Los mensajes más recientes deberán mostrarse primero.

---

# 44. Navegación

El flujo principal será:

```text
Dashboard
    ↓
WhatsApp
    ↓
Nuevo mensaje
    ↓
Seleccionar cliente
    ↓
Seleccionar plantilla
    ↓
Completar variables
    ↓
Vista previa
    ↓
Confirmar
    ↓
Enviar
    ↓
Resultado
```

También existirá navegación contextual:

```text
Cliente
   ↓
WhatsApp

Préstamo
   ↓
WhatsApp

Morosidad
   ↓
WhatsApp
```

---

# 45. Flujo de Mensaje de Morosidad

Ejemplo completo:

```text
Interés vencido
      ↓
Backend detecta morosidad
      ↓
Genera evento
      ↓
Selecciona plantilla
      ↓
Obtiene datos del cliente
      ↓
Genera mensaje
      ↓
Envía a WhatsApp
      ↓
Guarda historial
      ↓
Actualiza estado
```

---

# 46. Flujo de Mensaje Manual

```text
Usuario
   ↓
WhatsApp
   ↓
Nuevo mensaje
   ↓
Seleccionar cliente
   ↓
Seleccionar plantilla
   ↓
Vista previa
   ↓
Confirmar
   ↓
Backend
   ↓
Meta WhatsApp Business
   ↓
Resultado
```

---

# 47. Integración con Otros Módulos

El módulo de WhatsApp estará relacionado con:

```text
WhatsApp
    ├── Clientes
    ├── Préstamos
    ├── Pagos
    ├── Morosidad
    ├── Notificaciones
    └── Auditoría
```

Ejemplo:

```text
Morosidad
    ↓
Contactar cliente
    ↓
WhatsApp
    ↓
Registrar mensaje
    ↓
Actualizar seguimiento
```

---

# 48. Consideraciones Técnicas

La aplicación podrá recibir información similar a:

```text
messageId
clientId
loanId
phone
templateId
messageType
messageContent
status
createdAt
sentAt
deliveredAt
readAt
errorMessage
```

Los nombres definitivos dependerán del contrato de la API.

El backend será responsable de:

- Validar el usuario.
- Validar permisos.
- Validar el cliente.
- Validar el número.
- Validar la plantilla.
- Construir el mensaje.
- Comunicarse con Meta.
- Registrar el resultado.
- Procesar estados y eventos.
- Mantener el historial.

---

# 49. Webhooks

Cuando la integración lo permita, el backend podrá utilizar webhooks para recibir actualizaciones del estado de los mensajes.

Flujo:

```text
Meta WhatsApp
      ↓
Webhook
      ↓
Backend
      ↓
Actualizar mensaje
      ↓
Aplicación móvil
```

Ejemplo:

```text
ENVIADO
   ↓
ENTREGADO
   ↓
LEÍDO
```

Si ocurre un error:

```text
ENVIADO
   ↓
ERROR
```

---

# 50. Accesibilidad

La pantalla deberá considerar:

- Texto legible.
- Contraste adecuado.
- Botones táctiles suficientemente grandes.
- Etiquetas claras.
- Mensajes de error comprensibles.
- Compatibilidad con lectores de pantalla.
- Indicadores textuales de estado.
- No depender únicamente del color.

Ejemplo:

```text
✓ ENTREGADO
⚠ ERROR
```

---

# 51. Diseño Responsive

La pantalla deberá adaptarse a diferentes tamaños de dispositivos.

Ejemplo para pantalla pequeña:

```text
┌─────────────────────┐
│ ← WhatsApp       +  │
├─────────────────────┤
│ [Mensajes]          │
│ [Plantillas]        │
│ [Historial]         │
├─────────────────────┤
│ Juan Pérez          │
│ Recordatorio        │
│ ✓ Entregado         │
│ 04/09/2026 08:00    │
├─────────────────────┤
│ María López         │
│ Pago vencido        │
│ ✓ Enviado           │
│ 04/09/2026 08:15    │
└─────────────────────┘
```

Los elementos deberán ajustarse al ancho disponible sin generar desplazamiento horizontal.

---

# 52. Criterios de Aceptación

La pantalla será considerada correctamente implementada cuando:

- [ ] El usuario pueda acceder al módulo WhatsApp.
- [ ] Se pueda seleccionar un cliente.
- [ ] Se pueda visualizar su número de teléfono.
- [ ] Se valide el número antes del envío.
- [ ] Se puedan consultar las plantillas disponibles.
- [ ] Se pueda seleccionar una plantilla.
- [ ] Se puedan completar las variables.
- [ ] Se muestre una vista previa.
- [ ] Se solicite confirmación antes de enviar.
- [ ] Se pueda enviar un mensaje autorizado.
- [ ] Los mensajes sean procesados mediante el backend.
- [ ] Las credenciales de Meta no estén almacenadas en la aplicación móvil.
- [ ] Se registre el historial de mensajes.
- [ ] Se muestre el estado del mensaje.
- [ ] Se puedan consultar mensajes enviados.
- [ ] Se puedan filtrar mensajes.
- [ ] Se pueda buscar por cliente.
- [ ] Se pueda buscar por préstamo.
- [ ] Se registren los errores de envío.
- [ ] Se pueda realizar un reintento cuando corresponda.
- [ ] Se puedan enviar recordatorios de pago.
- [ ] Se puedan enviar mensajes relacionados con morosidad.
- [ ] Se puedan enviar confirmaciones de pago cuando corresponda.
- [ ] Se eviten envíos duplicados.
- [ ] Se respeten los permisos por rol.
- [ ] Se registren las operaciones importantes en auditoría.
- [ ] Se proteja la información del cliente.
- [ ] Se controle el estado de carga.
- [ ] Se controle el estado vacío.
- [ ] Se controle el estado de error.
- [ ] La pantalla sea responsive.
- [ ] Se cumplan criterios básicos de accesibilidad.

---

# 53. Reglas de Negocio Relacionadas

La pantalla deberá respetar las reglas definidas en:

```text
02-Reglas-Negocio/
├── 01-Reglas-Clientes.md
├── 02-Reglas-Prestamos.md
├── 03-Reglas-Pagos.md
├── 04-Reglas-Morosidad.md
├── 05-Reglas-Notificaciones.md
└── 06-Reglas-WhatsApp.md
```

Las reglas relacionadas con plantillas, mensajes automáticos, horarios, restricciones y envío deberán mantenerse centralizadas en `06-Reglas-WhatsApp.md`.

---

# 54. Resultado Esperado

La pantalla deberá proporcionar una herramienta centralizada para la comunicación entre el negocio y sus clientes mediante WhatsApp.

El flujo principal será:

```text
Cliente
   ↓
Préstamo
   ↓
Evento
   ↓
WhatsApp
   ↓
Plantilla
   ↓
Mensaje
   ↓
Envío
   ↓
Estado
   ↓
Historial
```

La integración deberá mantener separadas las responsabilidades:

```text
Aplicación móvil
      ↓
Interfaz y solicitud
      ↓
Backend
      ↓
Reglas y seguridad
      ↓
Meta WhatsApp Business
      ↓
Envío del mensaje
```

De esta manera se evita exponer credenciales privadas y se centraliza el control de los mensajes.

---

# 55. Fin del Módulo de Pantallas

Con esta pantalla se completa la documentación inicial de las pantallas principales definidas para:

```text
06-Diseno-UX-UI/03-Pantallas/
```

Archivos completados:

```text
01-Pantalla-Login.md
02-Pantalla-Dashboard.md
03-Pantalla-Clientes.md
04-Pantalla-Registro-Cliente.md
05-Pantalla-Detalle-Cliente.md
06-Pantalla-Prestamos.md
07-Pantalla-Registro-Prestamo.md
08-Pantalla-Detalle-Prestamo.md
09-Pantalla-Pagos.md
10-Pantalla-Morosidad.md
11-Pantalla-Notificaciones.md
12-Pantalla-WhatsApp.md
```

El siguiente paso de documentación será continuar con el siguiente módulo de la estructura del proyecto.