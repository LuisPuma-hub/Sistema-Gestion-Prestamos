# 08 - Plantillas de WhatsApp

## 1. Objetivo

Este documento define las plantillas de mensajes de WhatsApp que serán utilizadas por el **Sistema de Gestión de Préstamos** para enviar comunicaciones automáticas y personalizadas a los clientes.

Las plantillas estarán orientadas principalmente a:

- Recordatorios de pagos.
- Avisos de pagos vencidos.
- Notificaciones de morosidad.
- Confirmaciones de pagos.
- Reactivación de clientes.
- Comunicaciones administrativas relacionadas con los préstamos.

El sistema utilizará la **WhatsApp Business Platform de Meta** para el envío de mensajes y el backend será responsable de determinar cuándo corresponde utilizar cada plantilla.

---

## 2. Alcance

Las plantillas contempladas en este documento serán utilizadas por:

- Administrador.
- Cobrador.
- Procesos automáticos del sistema.

Las plantillas deberán integrarse con:

- ASP.NET Core Web API.
- .NET MAUI.
- PostgreSQL.
- Entity Framework Core.
- WhatsApp Business Platform de Meta.
- Sistema de programación de notificaciones.

---

## 3. Principios para las plantillas

Las plantillas deberán cumplir los siguientes principios:

1. Ser claras y fáciles de entender.
2. Utilizar lenguaje profesional.
3. Evitar mensajes innecesariamente extensos.
4. Personalizar el contenido utilizando variables.
5. Evitar incluir información sensible innecesaria.
6. Mantener consistencia entre los diferentes mensajes.
7. Permitir identificar claramente al negocio.
8. Utilizar únicamente plantillas previamente configuradas y aprobadas para el envío mediante WhatsApp.
9. Registrar el uso de cada plantilla.
10. Evitar envíos duplicados.
11. Mantener trazabilidad de los mensajes enviados.

---

# 4. Categorías de mensajes

Las plantillas se organizarán de acuerdo con el objetivo de la comunicación.

| Código | Categoría | Descripción |
|---|---|---|
| WH-PAGO-01 | Recordatorio de pago | Recuerda al cliente que tiene un pago próximo |
| WH-PAGO-02 | Pago vencido | Informa que existe un pago pendiente fuera de la fecha esperada |
| WH-MOR-01 | Morosidad | Informa que el cliente presenta morosidad |
| WH-MOR-02 | Morosidad crítica | Comunicación para casos de morosidad que requieren atención |
| WH-PAGO-03 | Pago registrado | Confirma que un pago fue registrado |
| WH-CLI-01 | Reactivación | Comunicación relacionada con la reactivación del cliente |
| WH-ADM-01 | Comunicación administrativa | Comunicación general relacionada con la gestión del préstamo |

---

# 5. Convención de nombres

Las plantillas deberán utilizar una nomenclatura uniforme.

Formato:

```text
wh_<modulo>_<evento>_<version>
```

Ejemplos:

```text
wh_pago_recordatorio_v1
wh_pago_vencido_v1
wh_morosidad_aviso_v1
wh_morosidad_critica_v1
wh_pago_registrado_v1
wh_cliente_reactivacion_v1
```

## 5.1 Reglas de nomenclatura

Los nombres deberán:

- Utilizar letras minúsculas.
- No contener espacios.
- Utilizar guiones bajos para separar palabras.
- Identificar el módulo.
- Identificar el evento.
- Incluir una versión cuando sea necesario.

Ejemplo:

```text
wh_pago_recordatorio_v1
```

Donde:

- `wh` = WhatsApp.
- `pago` = módulo.
- `recordatorio` = evento.
- `v1` = versión.

---

# 6. Variables de las plantillas

Meta exige sintaxis posicional `{{1}}, {{2}}, ...` en la plantilla aprobada. La tabla usa nombres legibles solo como documentación; el contrato API envía `params {"1":"...","2":"..."}` (ver `05-API/03 §5` y `02-Casos-Prueba.md CP-WA-01`).

Ejemplos de variables (nombre legible → posición Meta):

| Variable | Descripción |
|---|---|
| `{{nombre_cliente}}` | Nombre del cliente |
| `{{monto_prestamo}}` | Monto inicial del préstamo |
| `{{monto_pago}}` | Monto correspondiente al pago |
| `{{fecha_pago}}` | Fecha esperada del pago |
| `{{semanas_vencidas}}` | Número de semanas vencidas |
| `{{saldo_capital}}` | Saldo pendiente de capital |
| `{{interes_pendiente}}` | Interés pendiente |
| `{{numero_prestamo}}` | Identificador del préstamo |
| `{{nombre_negocio}}` | Nombre del negocio |
| `{{telefono_contacto}}` | Número de contacto del negocio |

Las variables serán reemplazadas por el backend antes del envío del mensaje.

---

# 7. Plantilla de recordatorio de pago

## 7.1 Identificación

**Nombre:**

```text
wh_pago_recordatorio_v1
```

**Código interno:**

```text
WH-PAGO-01
```

**Objetivo:**

Recordar al cliente que se aproxima la fecha establecida para realizar su pago semanal.

---

## 7.2 Contenido propuesto

### Encabezado

```text
Recordatorio de pago
```

### Cuerpo

```text
Hola {{nombre_cliente}}.

Te recordamos que tienes un pago correspondiente a tu préstamo programado para el {{fecha_pago}}.

Monto de referencia: {{monto_pago}}.

Gracias por mantener tus pagos al día.

{{nombre_negocio}}
```

### Pie de mensaje

```text
Mensaje automático del Sistema de Gestión de Préstamos.
```

---

# 8. Plantilla de pago vencido

## 8.1 Identificación

**Nombre:**

```text
wh_pago_vencido_v1
```

**Código interno:**

```text
WH-PAGO-02
```

**Objetivo:**

Informar al cliente que existe un pago pendiente que ha superado la fecha esperada.

---

## 8.2 Contenido propuesto

### Encabezado

```text
Pago pendiente
```

### Cuerpo

```text
Hola {{nombre_cliente}}.

Nuestro sistema registra un pago pendiente correspondiente a tu préstamo.

Fecha de pago esperada: {{fecha_pago}}.
Interés pendiente: {{interes_pendiente}}.

Por favor, comunícate con nosotros para regularizar tu situación.

{{nombre_negocio}}
```

### Pie

```text
Mensaje automático del Sistema de Gestión de Préstamos.
```

---

# 9. Plantilla de aviso de morosidad

## 9.1 Identificación

**Nombre:**

```text
wh_morosidad_aviso_v1
```

**Código interno:**

```text
WH-MOR-01
```

**Objetivo:**

Comunicar al cliente que presenta una situación de morosidad de acuerdo con las reglas establecidas por el sistema.

---

## 9.2 Contenido propuesto

### Encabezado

```text
Aviso de morosidad
```

### Cuerpo

```text
Hola {{nombre_cliente}}.

Te informamos que actualmente registras pagos pendientes relacionados con tu préstamo.

Semanas vencidas: {{semanas_vencidas}}.
Interés pendiente: {{interes_pendiente}}.

Te recomendamos comunicarte con nosotros para revisar tu situación y coordinar la regularización de tus pagos.

{{nombre_negocio}}
```

### Pie

```text
Mensaje automático del Sistema de Gestión de Préstamos.
```

---

# 10. Plantilla de morosidad crítica

## 10.1 Identificación

**Nombre:**

```text
wh_morosidad_critica_v1
```

**Código interno:**

```text
WH-MOR-02
```

**Objetivo:**

Enviar una comunicación cuando el cliente supera el umbral de morosidad establecido por las reglas de negocio.

En el sistema, la condición definida para este escenario es:

> Más de 2 pagos de interés semanales vencidos.

---

## 10.2 Contenido propuesto

### Encabezado

```text
Atención: situación de morosidad
```

### Cuerpo

```text
Hola {{nombre_cliente}}.

Nuestro sistema registra una situación de morosidad relacionada con tu préstamo.

Actualmente tienes {{semanas_vencidas}} pagos de interés vencidos.

Te solicitamos comunicarte con nosotros para revisar tu situación y evaluar las opciones disponibles.

{{nombre_negocio}}
```

### Pie

```text
Mensaje automático del Sistema de Gestión de Préstamos.
```

---

# 11. Plantilla de confirmación de pago

## 11.1 Identificación

**Nombre:**

```text
wh_pago_registrado_v1
```

**Código interno:**

```text
WH-PAGO-03
```

**Objetivo:**

Confirmar al cliente que un pago ha sido registrado correctamente.

---

## 11.2 Contenido propuesto

### Encabezado

```text
Pago registrado
```

### Cuerpo

```text
Hola {{nombre_cliente}}.

Confirmamos que tu pago ha sido registrado correctamente.

Monto registrado: {{monto_pago}}.

Saldo de capital pendiente: {{saldo_capital}}.

Gracias por realizar tu pago.

{{nombre_negocio}}
```

### Pie

```text
Mensaje automático del Sistema de Gestión de Préstamos.
```

---

# 12. Plantilla de reactivación

## 12.1 Identificación

**Nombre:**

```text
wh_cliente_reactivacion_v1
```

**Código interno:**

```text
WH-CLI-01
```

**Objetivo:**

Enviar una comunicación relacionada con la posibilidad de reactivación de un cliente que haya presentado morosidad.

La utilización de esta plantilla deberá estar condicionada a las reglas y permisos establecidos por el sistema.

---

## 12.2 Contenido propuesto

### Encabezado

```text
Información sobre tu cuenta
```

### Cuerpo

```text
Hola {{nombre_cliente}}.

Queremos informarte que puedes comunicarte con nosotros para revisar tu situación y conocer las opciones disponibles para la regularización de tu cuenta.

Para obtener más información, comunícate con nuestro equipo.

{{nombre_negocio}}

Contacto: {{telefono_contacto}}
```

### Pie

```text
Mensaje automático del Sistema de Gestión de Préstamos.
```

---

# 13. Plantilla de comunicación administrativa

## 13.1 Identificación

**Nombre:**

```text
wh_administrativo_v1
```

**Código interno:**

```text
WH-ADM-01
```

**Objetivo:**

Permitir comunicaciones administrativas relacionadas con la gestión de clientes o préstamos.

---

## 13.2 Contenido

Esta plantilla podrá utilizarse cuando exista una comunicación que no corresponda específicamente a:

- Recordatorio de pago.
- Pago vencido.
- Morosidad.
- Confirmación de pago.
- Reactivación.

El contenido deberá ser definido y autorizado por el administrador.

---

# 14. Componentes de una plantilla

Las plantillas podrán estructurarse utilizando los componentes disponibles para los mensajes de plantilla de WhatsApp.

## 14.1 Header

El encabezado permite presentar información breve y relevante.

Ejemplos:

```text
Recordatorio de pago
```

```text
Pago registrado
```

```text
Aviso de morosidad
```

El uso de encabezados deberá mantenerse simple y coherente.

---

## 14.2 Body

El cuerpo contendrá la información principal del mensaje.

El cuerpo podrá incluir:

- Nombre del cliente.
- Fecha de pago.
- Monto.
- Información sobre pagos pendientes.
- Información sobre morosidad.
- Información del negocio.

---

## 14.3 Footer

El pie podrá utilizarse para indicar:

```text
Mensaje automático del Sistema de Gestión de Préstamos.
```

Su utilización dependerá de la plantilla y de la configuración correspondiente en WhatsApp Business.

---

## 14.4 Buttons

Cuando una plantilla requiera una acción del usuario, podrán utilizarse botones compatibles con la configuración de WhatsApp.

Ejemplos conceptuales:

```text
Contactar
```

```text
Más información
```

La utilización de botones dependerá de las capacidades disponibles para la plantilla configurada.

---

# 15. Reglas para las variables

Las variables deberán cumplir las siguientes reglas:

1. Cada variable debe tener un significado definido.
2. El backend debe proporcionar un valor válido.
3. No se deben enviar variables vacías cuando sean obligatorias.
4. Los valores deben validarse antes del envío.
5. Los montos deben utilizar formato monetario.
6. Las fechas deben utilizar el formato definido por el sistema.
7. No se deben incluir datos personales innecesarios.
8. Las variables deberán corresponder con la plantilla configurada.

---

# 16. Ejemplo de reemplazo de variables

Plantilla:

```text
Hola {{nombre_cliente}}.

Te recordamos que tienes un pago programado para el {{fecha_pago}}.

Monto de referencia: {{monto_pago}}.
```

Datos obtenidos por el backend:

```text
nombre_cliente = Juan
fecha_pago = 10/09/2026
monto_pago = S/ 50.00
```

Mensaje resultante:

```text
Hola Juan.

Te recordamos que tienes un pago programado para el 10/09/2026.

Monto de referencia: S/ 50.00.
```

---

# 17. Proceso de aprobación de plantillas

Las plantillas deberán ser configuradas en el entorno correspondiente de **WhatsApp Business Platform de Meta**.

Proceso general:

```text
Crear plantilla
      ↓
Definir nombre
      ↓
Seleccionar categoría
      ↓
Definir idioma
      ↓
Configurar componentes
      ↓
Agregar variables
      ↓
Enviar para revisión
      ↓
Aprobación
      ↓
Configuración en el sistema
      ↓
Pruebas
      ↓
Uso en producción
```

El sistema no deberá asumir que una plantilla creada estará inmediatamente disponible para producción.

---

# 18. Estados de una plantilla

El sistema deberá considerar que una plantilla puede encontrarse en diferentes estados según la información disponible en la plataforma de Meta.

Conceptualmente:

| Estado | Descripción |
|---|---|
| Pendiente | Plantilla enviada y pendiente de revisión |
| Aprobada | Plantilla disponible para utilizarse |
| Rechazada | Plantilla que no fue aprobada |
| Deshabilitada | Plantilla que ya no debe utilizarse |
| En revisión | Plantilla cuyo proceso de revisión continúa |

La integración deberá validar que la plantilla se encuentre disponible antes de utilizarla.

---

# 19. Versionamiento

Las modificaciones importantes deberán generar nuevas versiones de las plantillas.

Ejemplo:

```text
wh_pago_recordatorio_v1
wh_pago_recordatorio_v2
```

La nueva versión deberá probarse antes de reemplazar la versión anterior.

Se recomienda no modificar la lógica histórica de los mensajes enviados.

La información histórica deberá conservar:

- Plantilla utilizada.
- Versión.
- Fecha de envío.
- Cliente.
- Evento que originó el mensaje.
- Estado del envío.

---

# 20. Relación entre eventos y plantillas

El backend deberá determinar qué plantilla corresponde a cada evento.

| Evento | Plantilla |
|---|---|
| Pago próximo | `wh_pago_recordatorio_v1` |
| Pago vencido | `wh_pago_vencido_v1` |
| Morosidad detectada | `wh_morosidad_aviso_v1` |
| Más de 2 pagos vencidos | `wh_morosidad_critica_v1` |
| Pago registrado | `wh_pago_registrado_v1` |
| Cliente elegible para reactivación | `wh_cliente_reactivacion_v1` |
| Comunicación administrativa | `wh_administrativo_v1` |

---

# 21. Flujo de selección de plantilla

El flujo general será:

```text
Evento del sistema
      ↓
Identificación del cliente
      ↓
Identificación del préstamo
      ↓
Evaluación de reglas de negocio
      ↓
Determinar tipo de mensaje
      ↓
Seleccionar plantilla
      ↓
Obtener variables
      ↓
Validar variables
      ↓
Verificar que no exista envío duplicado
      ↓
Enviar mediante WhatsApp Business Platform
      ↓
Registrar resultado
```

---

# 22. Validaciones antes del envío

Antes de enviar un mensaje, el backend deberá verificar:

### Cliente

- El cliente existe.
- El cliente tiene un número de teléfono válido.
- El cliente está habilitado para recibir comunicaciones según las reglas definidas.

### Préstamo

Cuando corresponda:

- El préstamo existe.
- El préstamo corresponde al cliente.
- El préstamo tiene información válida.

### Plantilla

- La plantilla existe en la configuración.
- La plantilla está habilitada.
- La versión corresponde con la configuración actual.
- Las variables requeridas están disponibles.

### Envío

- No existe un envío duplicado para el mismo evento.
- La solicitud contiene los datos necesarios.
- La comunicación se encuentra permitida por las reglas del sistema.

---

# 23. Prevención de mensajes duplicados

El sistema deberá evitar enviar el mismo mensaje varias veces como consecuencia de:

- Reintentos automáticos.
- Ejecución repetida del programador.
- Fallos temporales de red.
- Reinicio del backend.
- Procesamiento duplicado de eventos.

Para ello, cada comunicación deberá contar con un identificador de evento o mecanismo equivalente que permita detectar envíos previamente procesados.

---

# 24. Registro de mensajes

El sistema deberá almacenar información relacionada con los mensajes enviados.

Datos recomendados:

| Campo | Descripción |
|---|---|
| ID | Identificador del registro |
| Cliente | Cliente destinatario |
| Préstamo | Préstamo relacionado |
| Plantilla | Nombre de la plantilla |
| Versión | Versión utilizada |
| Evento | Evento que originó el mensaje |
| Fecha | Fecha y hora del envío |
| Estado | Estado del mensaje |
| Identificador externo | Identificador proporcionado por la plataforma |
| Error | Información del error cuando corresponda |

---

# 25. Seguridad

Las plantillas no deberán contener:

- Contraseñas.
- Tokens.
- Credenciales.
- Información interna del sistema.
- Datos bancarios completos.
- Información técnica de la API.
- Información confidencial innecesaria.

Las credenciales utilizadas para la integración con Meta deberán permanecer exclusivamente en el backend.

La aplicación .NET MAUI no deberá contener tokens privados de acceso a WhatsApp Business Platform.

---

# 26. Permisos

Los permisos deberán controlar quién puede:

### Administrador

- Configurar plantillas.
- Habilitar o deshabilitar plantillas.
- Revisar historial de mensajes.
- Configurar reglas relacionadas con comunicaciones.
- Revisar errores de envío.

### Cobrador

Dependiendo de los permisos definidos:

- Consultar información de comunicaciones.
- Realizar acciones permitidas relacionadas con clientes.
- Consultar historial de mensajes.

El cobrador no deberá modificar configuraciones críticas sin autorización.

---

# 27. Pruebas de plantillas

Antes de utilizar una plantilla en producción deberán realizarse pruebas.

## 27.1 Pruebas de contenido

Verificar:

- Texto correcto.
- Variables correctas.
- Ortografía.
- Formato de fechas.
- Formato monetario.
- Información del negocio.

## 27.2 Pruebas de variables

Verificar:

- Cliente con nombre válido.
- Cliente con datos incompletos.
- Monto válido.
- Fecha válida.
- Saldo válido.
- Variables faltantes.

## 27.3 Pruebas de envío

Verificar:

- Envío exitoso.
- Número inválido.
- Error de autenticación.
- Error de plantilla.
- Error de conexión.
- Reintento.
- Detección de duplicados.

---

# 28. Manejo de errores

Cuando un mensaje no pueda enviarse, el sistema deberá:

1. Registrar el error.
2. Guardar el evento relacionado.
3. Identificar la causa cuando sea posible.
4. Evitar registrar falsamente el mensaje como enviado.
5. Aplicar reintentos cuando corresponda.
6. Evitar reintentos indefinidos.
7. Informar al administrador cuando exista un problema que requiera intervención.

Ejemplo conceptual:

```text
Intento de envío
      ↓
¿Envío exitoso?
   ┌──┴──┐
  Sí     No
  ↓       ↓
Registrar  Registrar error
enviado       ↓
          ¿Reintentable?
           ┌──┴──┐
          Sí     No
          ↓       ↓
       Reintentar  Finalizar
```

---

# 29. Integración con las notificaciones móviles

WhatsApp y Firebase Cloud Messaging tendrán funciones diferentes.

| Característica | WhatsApp | FCM |
|---|---|---|
| Canal | WhatsApp | Aplicación móvil |
| Destinatario | Cliente | Usuario de la aplicación |
| Uso principal | Comunicación con clientes | Alertas dentro de la aplicación |
| Proveedor | Meta | Firebase |
| Ejemplo | Recordatorio al cliente | Aviso al cobrador |
| Requiere plantilla | Para los casos correspondientes | No utiliza plantillas de WhatsApp |

El backend será responsable de decidir cuándo corresponde utilizar cada canal.

---

# 30. Configuración de plantillas

Se recomienda almacenar la configuración de las plantillas en el backend y/o base de datos.

Ejemplo conceptual:

```text
Plantilla
├── Código
├── Nombre
├── Versión
├── Categoría
├── Idioma
├── Estado
├── Uso automático
├── Evento asociado
└── Fecha de actualización
```

La configuración permitirá cambiar la plantilla utilizada sin modificar directamente la lógica principal del sistema.

---

# 31. Auditoría

Las operaciones relacionadas con las plantillas deberán poder auditarse.

Se deberá registrar cuando corresponda:

- Creación.
- Activación.
- Desactivación.
- Cambio de versión.
- Cambio de configuración.
- Usuario que realizó el cambio.
- Fecha y hora.
- Motivo del cambio.

Esto es especialmente importante para las comunicaciones relacionadas con pagos y morosidad.

---

# 32. Reglas de negocio relacionadas

Las plantillas deberán respetar las reglas financieras establecidas por el sistema.

Por ejemplo:

- El interés semanal corresponde al 5 % del capital inicial.
- El interés no se capitaliza.
- Los pagos se aplican primero al interés.
- El excedente se aplica al capital.
- Se permiten pagos parciales.
- Un cliente puede tener múltiples préstamos.
- La morosidad se controla según las semanas vencidas.
- Más de 2 pagos de interés vencidos constituye el umbral definido para morosidad crítica.

Las plantillas deberán mostrar únicamente información calculada y validada por el backend.

---

# 33. Responsabilidad del backend

El backend ASP.NET Core Web API será responsable de:

1. Determinar el evento.
2. Consultar la información del cliente.
3. Consultar el préstamo cuando corresponda.
4. Calcular o recuperar los valores financieros.
5. Seleccionar la plantilla.
6. Construir las variables.
7. Validar la información.
8. Solicitar el envío a Meta.
9. Procesar la respuesta.
10. Registrar el resultado.

La aplicación móvil no deberá realizar por sí misma las decisiones financieras utilizadas en las plantillas.

---

# 34. Ejemplo completo de flujo

Supongamos que un cliente tiene un préstamo activo y se aproxima su fecha semanal de pago.

El sistema ejecutará:

```text
Programador
    ↓
Detecta pagos próximos
    ↓
Obtiene cliente
    ↓
Obtiene préstamo
    ↓
Obtiene fecha de pago
    ↓
Obtiene monto correspondiente
    ↓
Selecciona wh_pago_recordatorio_v1
    ↓
Construye variables
    ↓
Valida información
    ↓
Verifica duplicados
    ↓
Envía a WhatsApp
    ↓
Registra resultado
```

Mensaje:

```text
Hola Juan.

Te recordamos que tienes un pago correspondiente a tu préstamo programado para el 10/09/2026.

Monto de referencia: S/ 5.00.

Gracias por mantener tus pagos al día.

Sistema de Gestión de Préstamos
```

---

# 35. Criterios de aceptación

El módulo de plantillas será considerado correctamente implementado cuando:

- [ ] Las plantillas estén definidas.
- [ ] Cada plantilla tenga un código identificador.
- [ ] Las plantillas utilicen nombres consistentes.
- [ ] Las variables estén documentadas.
- [ ] El backend pueda seleccionar una plantilla según el evento.
- [ ] Las variables sean validadas antes del envío.
- [ ] Se eviten mensajes duplicados.
- [ ] Los mensajes enviados queden registrados.
- [ ] Los errores de envío sean registrados.
- [ ] Las plantillas puedan versionarse.
- [ ] Las plantillas estén configuradas correctamente en Meta.
- [ ] Las credenciales permanezcan protegidas en el backend.
- [ ] Los mensajes financieros utilicen información validada por el backend.
- [ ] Las comunicaciones respeten los permisos establecidos.
- [ ] Se puedan realizar pruebas antes de utilizar las plantillas en producción.

---

# 36. Dependencias

Este módulo depende de:

- WhatsApp Business Platform de Meta.
- Cuenta empresarial configurada en Meta.
- Configuración de WhatsApp Business.
- Número telefónico habilitado para la integración.
- ASP.NET Core Web API.
- PostgreSQL.
- Entity Framework Core.
- Sistema de gestión de préstamos.
- Sistema de programación de eventos.
- Sistema de autenticación y autorización.
- Registro de auditoría.

---

# 37. Consideraciones futuras

En futuras versiones podrán incorporarse:

- Plantillas adicionales.
- Diferentes idiomas.
- Mensajes personalizados por segmento.
- Botones de acción.
- Integración con respuestas recibidas.
- Métricas de entrega.
- Métricas de lectura cuando estén disponibles.
- Reportes de efectividad.
- Configuración avanzada de campañas.
- Historial detallado de conversaciones.
- Automatización de diferentes tipos de comunicaciones.

---

# 38. Resumen

Las plantillas de WhatsApp constituyen un componente fundamental para automatizar las comunicaciones entre el Sistema de Gestión de Préstamos y sus clientes.

El sistema contará principalmente con plantillas para:

- Recordatorios de pago.
- Pagos vencidos.
- Morosidad.
- Morosidad crítica.
- Confirmación de pagos.
- Reactivación.
- Comunicaciones administrativas.

El backend ASP.NET Core Web API será responsable de seleccionar la plantilla adecuada, proporcionar los valores de las variables, validar la información, solicitar el envío mediante la WhatsApp Business Platform de Meta y registrar el resultado.

La aplicación móvil .NET MAUI funcionará como interfaz de gestión, mientras que las reglas financieras y la lógica crítica permanecerán centralizadas en el backend.

De esta manera se garantiza una comunicación automatizada, trazable y coherente con las reglas de negocio del sistema.