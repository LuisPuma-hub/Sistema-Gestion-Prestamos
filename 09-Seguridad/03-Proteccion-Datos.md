# Protección de Datos

## 1. Objetivo

Definir las medidas técnicas y funcionales necesarias para proteger la información almacenada y procesada por el **Sistema de Gestión de Préstamos**, evitando accesos no autorizados, pérdida de información, exposición de datos sensibles, alteración de registros y uso indebido de las credenciales o integraciones externas.

La protección de datos se aplicará a:

- Aplicación móvil.
- Backend.
- API REST.
- Base de datos PostgreSQL.
- Firebase Cloud Messaging.
- WhatsApp Business Platform de Meta.
- Registros de auditoría.
- Copias de seguridad.
- Información almacenada temporalmente en los dispositivos móviles.

---

## 2. Alcance

Esta política comprende la protección de:

- Datos personales de clientes.
- Datos de avales.
- Información de préstamos.
- Información de pagos.
- Información de morosidad.
- Credenciales de usuarios.
- Tokens y sesiones.
- Configuraciones del sistema.
- Datos de auditoría.
- Credenciales de Firebase.
- Credenciales y tokens de Meta.
- Información intercambiada mediante la API.
- Información almacenada en PostgreSQL.
- Información almacenada localmente en dispositivos móviles.

---

## 3. Clasificación de la Información

La información del sistema se clasificará de acuerdo con su nivel de sensibilidad.

| Clasificación | Descripción | Ejemplos |
|---|---|---|
| Pública | Información que no representa un riesgo significativo | Información general del sistema |
| Interna | Información destinada al uso interno | Configuraciones generales |
| Confidencial | Información cuyo acceso debe estar restringido | Préstamos, pagos, reportes |
| Sensible | Información que requiere controles adicionales | DNI, teléfono, dirección, credenciales |

El sistema deberá aplicar controles de acceso de acuerdo con la clasificación de cada información.

---

## 4. Protección de Datos Personales

Los datos personales registrados por el sistema deberán protegerse contra accesos, modificaciones o divulgaciones no autorizadas.

Entre los datos considerados personales se encuentran:

- Tipo de documento.
- Número de documento.
- Nombres.
- Apellidos.
- Número de teléfono.
- Dirección.
- Correo electrónico, cuando corresponda.
- Imagen del recibo de agua o electricidad.
- Información relacionada con avales.

La aplicación no deberá mostrar información personal innecesaria a usuarios que no tengan permisos para visualizarla.

---

## 5. Protección de Información Financiera

La información financiera tendrá un nivel de protección alto debido a su importancia para el funcionamiento del sistema.

Se deberá proteger:

- Capital inicial del préstamo.
- Tasa de interés.
- Interés generado.
- Interés pagado.
- Capital pagado.
- Saldo pendiente.
- Historial de pagos.
- Fechas de vencimiento.
- Estado del préstamo.
- Estado de morosidad.
- Información relacionada con reactivaciones.

Las operaciones financieras deberán ser procesadas y validadas por el backend.

La aplicación móvil no deberá considerarse una fuente confiable para determinar por sí sola:

- Intereses.
- Saldos.
- Estados de préstamos.
- Morosidad.
- Montos pendientes.

---

## 6. Protección de Credenciales

Las contraseñas de los usuarios nunca deberán almacenarse en texto plano.

El backend deberá almacenar únicamente representaciones seguras mediante algoritmos de hashing apropiados.

No se deberá registrar en logs:

- Contraseñas.
- Tokens de autenticación.
- Refresh tokens.
- Claves privadas.
- Secretos de API.
- Credenciales de Meta.
- Credenciales de Firebase.

---

## 7. Protección de Tokens y Sesiones

Los tokens de autenticación deberán manejarse de forma segura.

Se deberán considerar las siguientes medidas:

- Tiempo de expiración de sesiones.
- Invalidación de sesiones cuando corresponda.
- Protección contra reutilización indebida de tokens.
- Cierre de sesiones desde el sistema.
- Cierre de otras sesiones desde el perfil del usuario.
- No almacenar tokens en archivos de texto sin protección.
- No enviar tokens mediante parámetros de URL.
- Utilizar HTTPS para las comunicaciones.

En caso de detectar una sesión comprometida, esta deberá poder invalidarse.

---

## 8. Cifrado de Comunicaciones

Todas las comunicaciones entre componentes del sistema deberán utilizar HTTPS.

El flujo general será:

```text
Aplicación .NET MAUI
        │
        │ HTTPS
        ▼
ASP.NET Core Web API
        │
        │ Conexión segura
        ▼
PostgreSQL
```

También deberán utilizarse comunicaciones seguras con servicios externos como:

- Firebase Cloud Messaging.
- WhatsApp Business Platform de Meta.

No se deberán transmitir datos sensibles mediante conexiones HTTP sin cifrado.

---

## 9. Cifrado de Datos Almacenados

La información sensible deberá protegerse durante su almacenamiento cuando corresponda.

Se deberán considerar mecanismos de protección para:

- Base de datos.
- Copias de seguridad.
- Archivos temporales.
- Información almacenada localmente.
- Documentos o imágenes asociados a clientes.

Las contraseñas deberán utilizar hashing seguro en lugar de cifrado reversible.

Para otros datos sensibles podrá utilizarse cifrado cuando el diseño técnico lo requiera.

---

## 10. Almacenamiento Seguro en .NET MAUI

La aplicación móvil desarrollada con **.NET MAUI** no deberá almacenar información sensible de forma insegura.

Se evitará guardar directamente en archivos de texto:

- Contraseñas.
- Tokens.
- Secretos.
- Credenciales.
- Información financiera innecesaria.

Cuando sea necesario almacenar información de sesión en el dispositivo, se deberán utilizar mecanismos de almacenamiento seguro proporcionados por la plataforma y/o las capacidades de seguridad disponibles en .NET MAUI.

La aplicación deberá almacenar únicamente la información local necesaria para su funcionamiento.

---

## 11. Seguridad de PostgreSQL

La base de datos PostgreSQL deberá configurarse aplicando el principio de mínimo privilegio.

Las medidas deberán incluir:

- Utilizar usuarios de base de datos con permisos limitados.
- Evitar utilizar el usuario administrador para operaciones normales de la aplicación.
- Proteger las credenciales de conexión.
- Restringir conexiones innecesarias.
- Mantener PostgreSQL actualizado.
- Proteger las copias de seguridad.
- Aplicar controles de acceso a la infraestructura.
- Registrar operaciones administrativas importantes.

El usuario utilizado por el backend deberá disponer únicamente de los permisos necesarios.

---

## 12. Protección de la API

La **ASP.NET Core Web API** será responsable de controlar el acceso a los datos.

Cada endpoint deberá validar:

1. Autenticación.
2. Autorización.
3. Parámetros recibidos.
4. Identificadores de recursos.
5. Reglas de negocio.
6. Integridad de los datos.

No deberá confiarse exclusivamente en las validaciones realizadas desde la aplicación móvil.

Ejemplo:

```text
.NET MAUI
    │
    │ Solicitud
    ▼
ASP.NET Core Web API
    │
    ├── Autenticación
    ├── Autorización
    ├── Validación
    ├── Reglas de negocio
    └── Acceso a datos
            │
            ▼
       PostgreSQL
```

---

## 13. Minimización de Datos

El sistema deberá recopilar únicamente la información necesaria para cumplir con sus funciones.

Por ejemplo, para registrar un cliente se utilizarán los datos definidos por el sistema:

- Tipo de documento.
- Número de documento.
- Nombres.
- Apellidos.
- Teléfono.
- Información requerida del domicilio.
- Imagen del recibo de servicio cuando corresponda.
- Observaciones administrativas.

No deberán solicitarse datos personales que no tengan una finalidad definida dentro del sistema.

---

## 14. Control de Acceso a Datos

El acceso a la información dependerá del rol y permisos del usuario.

Los roles principales serán:

- Administrador.
- Cobrador.

El backend deberá determinar si un usuario puede:

- Consultar clientes.
- Crear clientes.
- Modificar clientes.
- Consultar préstamos.
- Registrar préstamos.
- Registrar pagos.
- Consultar morosidad.
- Enviar mensajes.
- Modificar configuraciones.
- Consultar información financiera.

La interfaz móvil podrá ocultar opciones no autorizadas, pero la seguridad deberá aplicarse principalmente en el backend.

---

## 15. Protección contra Acceso Indebido a Recursos

La API deberá verificar que el usuario tenga autorización para acceder al recurso solicitado.

Por ejemplo:

```text
GET /api/prestamos/125
```

No deberá ser suficiente con conocer el identificador `125`.

El backend deberá comprobar:

```text
Usuario autenticado
        +
Permiso correspondiente
        +
Acceso al recurso
        +
Recurso existente
        =
Acceso permitido
```

Esto ayudará a prevenir accesos indebidos mediante la manipulación de identificadores.

---

## 16. Protección de Imágenes y Documentos

Las imágenes de recibos de agua o electricidad deberán considerarse información sensible.

Se deberá controlar:

- Quién puede cargarlas.
- Quién puede consultarlas.
- Quién puede modificarlas.
- Quién puede eliminarlas.
- Dónde se almacenan.
- Cómo se descargan.
- Cómo se registran sus accesos.

No deberán exponerse mediante rutas públicas sin protección cuando contengan información personal.

---

## 17. Integración con Firebase

La integración con **Firebase Cloud Messaging (FCM)** deberá proteger las credenciales y configuraciones utilizadas para enviar notificaciones.

No deberán almacenarse credenciales sensibles directamente en el código fuente de la aplicación.

La información utilizada para enviar notificaciones deberá limitarse a lo necesario.

Los mensajes no deberán incluir información financiera excesivamente detallada si no es necesario.

Ejemplo recomendado:

```text
Tiene un pago próximo. Ingrese a la aplicación
para consultar los detalles.
```

En lugar de incluir información financiera sensible directamente en una notificación visible en la pantalla bloqueada.

---

## 18. Integración con WhatsApp

La integración con **WhatsApp Business Platform de Meta** deberá proteger:

- Access tokens.
- Identificadores de la cuenta.
- Identificadores del número telefónico.
- Credenciales de configuración.
- Configuración de webhooks.
- Datos intercambiados con Meta.

Estas credenciales deberán permanecer en el backend y nunca deberán incluirse directamente en la aplicación .NET MAUI.

El backend será responsable de comunicarse con Meta.

```text
.NET MAUI
    │
    ▼
ASP.NET Core Web API
    │
    ▼
WhatsApp Business Platform
```

---

## 19. Preferencias de Comunicación

El sistema deberá permitir controlar las preferencias relacionadas con las comunicaciones cuando corresponda.

Se podrán considerar preferencias como:

- Notificaciones móviles.
- Comunicaciones mediante WhatsApp.
- Mensajes administrativos.
- Recordatorios de pago.

Estas preferencias deberán almacenarse y respetarse de acuerdo con las reglas configuradas por el sistema y la normativa aplicable.

---

## 20. Protección de Logs

Los logs deberán utilizarse para diagnosticar errores y realizar auditoría sin exponer información sensible.

No se deberán registrar:

```text
Contraseñas
Tokens
Secretos
Claves privadas
Credenciales
```

Cuando sea necesario registrar información personal, deberá limitarse a la información necesaria para identificar técnicamente la operación.

Ejemplo:

```text
Usuario: 25
Operación: REGISTRO_PAGO
Préstamo: 125
Resultado: EXITOSO
Fecha: 2026-08-08 16:20
```

En lugar de registrar información innecesaria del cliente.

---

## 21. Auditoría de Operaciones

Las operaciones importantes deberán generar registros de auditoría.

Se deberán considerar especialmente:

- Creación de clientes.
- Modificación de clientes.
- Creación de préstamos.
- Modificación de préstamos.
- Registro de pagos.
- Cambios de estado.
- Reactivación por morosidad.
- Modificación de configuraciones financieras.
- Cambio de permisos.
- Cambio de contraseña.
- Cierre de sesiones.
- Envío de comunicaciones importantes.

La auditoría permitirá determinar:

- Quién realizó la operación.
- Qué operación realizó.
- Cuándo ocurrió.
- Sobre qué recurso se realizó.
- Resultado de la operación.

---

## 22. Integridad de la Información

El sistema deberá evitar modificaciones inconsistentes de los datos.

Las operaciones financieras deberán utilizar mecanismos que permitan mantener la integridad de la información.

Por ejemplo, al registrar un pago:

```text
Validar pago
     │
     ▼
Calcular aplicación del pago
     │
     ▼
Actualizar interés
     │
     ▼
Actualizar capital
     │
     ▼
Actualizar saldo
     │
     ▼
Actualizar estado del préstamo
     │
     ▼
Registrar auditoría
```

Estas operaciones deberán realizarse de manera consistente.

---

## 23. Protección de Operaciones Financieras

Las operaciones financieras deberán ejecutarse bajo controles estrictos.

En particular:

- El interés deberá calcularse en el backend.
- El monto del pago deberá validarse.
- No deberán aceptarse valores negativos.
- No deberán aceptarse valores inválidos.
- Los saldos deberán calcularse utilizando tipos numéricos apropiados para valores monetarios.
- Los cambios financieros deberán quedar registrados.
- El usuario deberá tener autorización para realizar la operación.

La regla del sistema establece que el interés semanal corresponde al **5 % del capital inicial** y no deberá calcularse como interés compuesto.

---

## 24. Copias de Seguridad

La base de datos deberá contar con mecanismos de respaldo.

Las copias de seguridad deberán:

- Realizarse periódicamente.
- Protegerse contra accesos no autorizados.
- Mantenerse en una ubicación segura.
- Tener procedimientos de recuperación.
- Verificarse mediante pruebas de restauración.
- Mantener políticas de retención definidas.

No deberá considerarse una copia de seguridad válida si nunca se ha probado su restauración.

---

## 25. Recuperación ante Pérdida de Información

El sistema deberá disponer de procedimientos para recuperar información ante:

- Fallos de hardware.
- Fallos del servidor.
- Corrupción de datos.
- Eliminación accidental.
- Errores de actualización.
- Incidentes de seguridad.

El objetivo será minimizar:

- Pérdida de datos.
- Tiempo de indisponibilidad.
- Impacto sobre las operaciones financieras.

---

## 26. Retención y Eliminación de Datos

Los datos deberán conservarse durante el tiempo necesario para cumplir con las necesidades operativas, administrativas y legales aplicables.

Antes de eliminar información deberá determinarse si:

- Forma parte del historial financiero.
- Es necesaria para auditoría.
- Está relacionada con un préstamo activo.
- Es necesaria para reportes.
- Existe una obligación de conservación.

Cuando corresponda, podrá utilizarse eliminación lógica o **soft delete** para mantener la trazabilidad histórica.

---

## 27. Separación de Entornos

Se deberán mantener separados los entornos de:

- Desarrollo.
- Pruebas.
- Producción.

No deberán utilizarse credenciales reales de producción en el entorno de desarrollo.

Asimismo, no se deberán utilizar datos reales de clientes en pruebas sin controles y autorización adecuados.

Ejemplo:

```text
DESARROLLO
PostgreSQL desarrollo
Firebase desarrollo
Meta desarrollo/pruebas

PRUEBAS
PostgreSQL pruebas
Firebase pruebas
Meta pruebas

PRODUCCIÓN
PostgreSQL producción
Firebase producción
Meta producción
```

---

## 28. Gestión de Secretos

Las claves y secretos deberán mantenerse fuera del código fuente.

Se deberán proteger especialmente:

- Contraseñas de PostgreSQL.
- JWT secrets u otros secretos de autenticación.
- Access tokens.
- Credenciales de Firebase.
- Credenciales de Meta.
- Secretos de webhooks.
- Variables de configuración sensibles.

Los archivos que contengan secretos no deberán subirse al repositorio de GitHub.

Se deberá utilizar una estrategia adecuada de configuración y gestión de secretos para cada entorno.

---

## 29. Protección del Repositorio

El repositorio Git deberá evitar la exposición accidental de información sensible.

No deberán subirse:

- Contraseñas.
- Tokens.
- Claves privadas.
- Archivos `.env` con secretos.
- Credenciales de servicios externos.
- Copias de bases de datos con información real.

Se deberá utilizar un archivo `.gitignore` apropiado.

También deberán revisarse los cambios antes de realizar un `git push`.

---

## 30. Actualizaciones y Vulnerabilidades

Las tecnologías utilizadas deberán mantenerse actualizadas dentro de versiones compatibles y estables.

Se deberán revisar periódicamente:

- .NET.
- .NET MAUI.
- ASP.NET Core.
- Entity Framework Core.
- PostgreSQL.
- Paquetes NuGet.
- Bibliotecas utilizadas en la aplicación.
- Componentes de Firebase.
- Integraciones externas.

Cuando se detecte una vulnerabilidad crítica deberá evaluarse su actualización o mitigación.

---

## 31. Gestión de Incidentes de Seguridad

Ante un posible incidente de seguridad se deberá:

1. Detectar el incidente.
2. Identificar el componente afectado.
3. Contener el problema.
4. Revocar credenciales comprometidas cuando corresponda.
5. Invalidar sesiones comprometidas.
6. Analizar los logs.
7. Recuperar los servicios.
8. Verificar la integridad de los datos.
9. Documentar el incidente.
10. Aplicar medidas preventivas.

Los incidentes relacionados con información financiera deberán recibir prioridad alta.

---

## 32. Monitoreo

El sistema deberá contar con mecanismos de monitoreo para detectar:

- Intentos repetidos de autenticación.
- Errores anormales de API.
- Accesos no autorizados.
- Fallos de envío de WhatsApp.
- Fallos de notificaciones FCM.
- Errores de base de datos.
- Operaciones financieras fallidas.
- Comportamientos anómalos.

Los eventos relevantes deberán poder relacionarse con usuarios, fechas y operaciones.

---

## 33. Seguridad en la Aplicación Móvil

La aplicación .NET MAUI deberá:

- Utilizar HTTPS.
- Validar las respuestas del backend.
- No almacenar secretos en el código.
- No confiar en cálculos financieros locales.
- Evitar exponer información innecesaria.
- Gestionar correctamente la sesión.
- Utilizar almacenamiento seguro para información sensible.
- Limpiar información temporal cuando corresponda.

La seguridad definitiva deberá implementarse en el backend.

---

## 34. Seguridad en el Backend

El backend ASP.NET Core deberá:

- Validar todas las solicitudes.
- Aplicar autenticación.
- Aplicar autorización.
- Validar datos de entrada.
- Controlar acceso a recursos.
- Aplicar reglas de negocio.
- Proteger endpoints.
- Gestionar errores sin exponer información interna.
- Registrar eventos relevantes.
- Proteger secretos.
- Controlar operaciones financieras.

---

## 35. Manejo Seguro de Errores

Los mensajes enviados al usuario no deberán revelar información interna del sistema.

No se deberán mostrar detalles como:

```text
Stack traces
Contraseñas
Credenciales
Consultas SQL
Rutas internas
Configuraciones del servidor
```

Ejemplo:

```text
Mensaje para el usuario:
"No fue posible procesar la operación. Intente nuevamente."
```

Mientras que el detalle técnico deberá almacenarse en logs protegidos.

---

## 36. Validación de Datos

Toda información recibida desde la aplicación móvil deberá validarse en el backend.

Se deberán validar:

- Tipos de datos.
- Longitudes.
- Formatos.
- Valores permitidos.
- Campos obligatorios.
- Montos monetarios.
- Fechas.
- Identificadores.
- Estados.
- Relaciones entre entidades.

La validación deberá realizarse antes de modificar la base de datos.

---

## 37. Protección contra Manipulación de Datos

El sistema deberá evitar que un usuario modifique directamente información que no tiene autorización para cambiar.

Por ejemplo, un usuario no autorizado no deberá poder modificar mediante una petición HTTP:

```text
interesSemanal
saldoCapital
estadoPrestamo
estadoMorosidad
```

Estos valores deberán ser determinados por las reglas de negocio implementadas en el backend.

---

## 38. Pruebas de Protección de Datos

Se deberán realizar pruebas para verificar:

### Autenticación

- Contraseñas incorrectas.
- Sesiones expiradas.
- Tokens inválidos.
- Tokens revocados.

### Autorización

- Acceso con permisos insuficientes.
- Acceso a recursos de otro contexto.
- Acceso a endpoints protegidos.

### Datos personales

- Visualización restringida.
- Modificación no autorizada.
- Acceso a imágenes protegidas.

### Información financiera

- Manipulación de saldos.
- Montos negativos.
- Modificación no autorizada de préstamos.
- Alteración de pagos.

### Integraciones

- Protección de credenciales Meta.
- Protección de credenciales Firebase.
- Validación de webhooks.

### Logs

- Verificación de ausencia de contraseñas.
- Verificación de ausencia de tokens.
- Verificación de ausencia de secretos.

---

## 39. Criterios de Aceptación

La protección de datos se considerará implementada cuando:

- [ ] Las contraseñas no se almacenen en texto plano.
- [ ] Las comunicaciones utilicen HTTPS.
- [ ] Los tokens se gestionen de forma segura.
- [ ] La información financiera sea validada en el backend.
- [ ] Los usuarios solo accedan a información autorizada.
- [ ] Los datos personales estén protegidos.
- [ ] Las imágenes de clientes tengan acceso controlado.
- [ ] Las credenciales de Meta no estén en la aplicación móvil.
- [ ] Las credenciales de Firebase estén protegidas.
- [ ] Los secretos no estén incluidos en GitHub.
- [ ] Los logs no almacenen información sensible innecesaria.
- [ ] Existan copias de seguridad.
- [ ] Se pueda restaurar una copia de seguridad.
- [ ] Existan mecanismos de auditoría.
- [ ] Exista separación entre ambientes.
- [ ] Las operaciones financieras mantengan integridad.
- [ ] Existan procedimientos para incidentes de seguridad.

---

## 40. Dependencias

La implementación de esta protección depende de:

- .NET 2026.
- .NET MAUI.
- C#.
- ASP.NET Core Web API.
- Entity Framework Core.
- PostgreSQL.
- Firebase Cloud Messaging.
- WhatsApp Business Platform de Meta.
- Git.
- GitHub.
- GitHub Copilot (inline).
- Muse Spark vía OpenCode (agente).
- Infraestructura donde se despliegue el backend y la base de datos.

---

## 41. Consideraciones Futuras

En futuras versiones se podrá incorporar:

- Cifrado avanzado de campos sensibles.
- Gestión centralizada de secretos.
- Autenticación multifactor.
- Políticas avanzadas de sesión.
- Detección de comportamiento anómalo.
- Alertas de seguridad.
- Rotación automática de credenciales.
- Escaneo automatizado de vulnerabilidades.
- Auditoría avanzada.
- Monitoreo centralizado.
- Políticas automatizadas de retención.
- Gestión avanzada de dispositivos móviles.

La aplicación de medidas legales o regulatorias específicas deberá evaluarse de acuerdo con la normativa vigente y el contexto de operación del sistema.

---

## 42. Resumen de Seguridad

La protección de datos se basa en los siguientes principios:

```text
                 PROTECCIÓN DE DATOS
                         │
        ┌────────────────┼────────────────┐
        │                │                │
   Confidencialidad   Integridad     Disponibilidad
        │                │                │
        ▼                ▼                ▼
   Control de acceso  Validaciones     Backups
   Cifrado            Auditoría        Recuperación
   HTTPS              Reglas negocio   Monitoreo
        │                │                │
        └────────────────┼────────────────┘
                         ▼
                 SISTEMA SEGURO
```

El backend ASP.NET Core constituye el principal punto de control de seguridad, mientras que la aplicación .NET MAUI deberá utilizar mecanismos seguros de comunicación y almacenamiento.

La información financiera, personal y de autenticación deberá recibir controles diferenciados de acuerdo con su nivel de sensibilidad.

El objetivo final es garantizar la **confidencialidad, integridad y disponibilidad** de la información del Sistema de Gestión de Préstamos.