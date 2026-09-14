# 09 - Autorización

## 1. Objetivo

Este documento define el sistema de autorización del **Sistema de Gestión de Préstamos**.

La autorización determina qué acciones puede realizar cada usuario después de haberse autenticado correctamente.

El objetivo es garantizar que:

- Cada usuario acceda únicamente a las funcionalidades permitidas.
- Los permisos sean controlados por el backend.
- Las operaciones financieras estén protegidas.
- Las configuraciones críticas estén restringidas.
- Los usuarios no puedan acceder a información fuera de sus permisos.
- Todas las operaciones relevantes mantengan trazabilidad.

La autorización complementará al mecanismo de autenticación definido en:

```text
09-Seguridad/01-Autenticacion.md
```

---

# 2. Alcance

La autorización se aplicará a las funcionalidades:

- Dashboard.
- Clientes.
- Préstamos.
- Pagos.
- Morosidad.
- Notificaciones.
- WhatsApp.
- Perfil.
- Configuración.
- Gestión de usuarios.
- Auditoría.

El control deberá realizarse principalmente en:

```text
ASP.NET Core Web API
```

La aplicación .NET MAUI únicamente mostrará las opciones disponibles de acuerdo con la información proporcionada por el backend.

---

# 3. Autenticación vs autorización

La autenticación y la autorización cumplen funciones diferentes.

### Autenticación

Determina:

> ¿Quién es el usuario?

### Autorización

Determina:

> ¿Qué puede hacer ese usuario?

Flujo:

```text
Usuario
   ↓
Autenticación
   ↓
Identidad confirmada
   ↓
Autorización
   ↓
Evaluar rol y permisos
   ↓
Permitir / Denegar
```

---

# 4. Principio de mínimo privilegio

El sistema aplicará el principio de **mínimo privilegio**.

Cada usuario deberá disponer únicamente de los permisos necesarios para realizar sus funciones.

Por ejemplo:

```text
Cobrador
   ↓
Consultar clientes
   ↓
Registrar pagos
   ↓
Consultar préstamos
```

No deberá tener automáticamente acceso a:

```text
Configuración financiera
Gestión de usuarios
Cambio de permisos
Credenciales de integración
```

---

# 5. Roles del sistema

Los roles iniciales serán:

| Rol | Código (canónico en DB/API/logs) | Descripción |
|---|---|---|
| Administrador | ADMIN | Gestiona y configura el sistema |
| Cobrador | COBRADOR | Realiza operaciones de gestión y cobranza |

Los permisos se asignarán de acuerdo con las responsabilidades de cada rol.

---

# 6. Rol Administrador

El administrador tendrá el nivel más alto de permisos dentro del sistema.

Podrá realizar, según la configuración:

- Gestionar clientes.
- Gestionar préstamos.
- Registrar pagos.
- Consultar morosidad.
- Gestionar notificaciones.
- Gestionar comunicaciones de WhatsApp.
- Administrar usuarios.
- Configurar parámetros del sistema.
- Consultar auditoría.
- Gestionar permisos.
- Modificar configuraciones autorizadas.

Las operaciones críticas deberán continuar siendo validadas por el backend.

---

# 7. Rol Cobrador

El cobrador estará orientado principalmente a las actividades de cobranza y gestión operativa.

Podrá disponer de permisos para:

- Consultar clientes.
- Registrar clientes cuando esté permitido.
- Consultar préstamos.
- Registrar pagos.
- Consultar morosidad.
- Consultar notificaciones.
- Consultar comunicaciones.
- Realizar determinadas acciones de WhatsApp.

No deberá modificar configuraciones críticas del sistema salvo que el administrador le otorgue explícitamente el permiso correspondiente.

---

# 8. Matriz general de permisos

La matriz inicial será:

| Funcionalidad | Administrador | Cobrador |
|---|:---:|:---:|
| Dashboard | ✅ | ✅ |
| Consultar clientes | ✅ | ✅ |
| Registrar clientes | ✅ | Según permiso |
| Editar clientes | ✅ | Según permiso |
| Eliminar clientes | ✅ | ❌ |
| Consultar préstamos | ✅ | ✅ |
| Registrar préstamos | ✅ | Según permiso |
| Editar préstamos | ✅ | Según permiso |
| Cancelar préstamo | ✅ | ❌ |
| Registrar pagos | ✅ | ✅ |
| Modificar pagos | ✅ | ❌ |
| Anular pagos | ✅ | ❌ |
| Consultar morosidad | ✅ | ✅ |
| Gestionar notificaciones | ✅ | Según permiso |
| Consultar WhatsApp | ✅ | Según permiso |
| Enviar WhatsApp manual | ✅ | Según permiso |
| Configuración general | ✅ | ❌ |
| Configuración financiera | ✅ | ❌ |
| Gestión de usuarios | ✅ | ❌ |
| Gestión de permisos | ✅ | ❌ |
| Auditoría | ✅ | ❌ |

La matriz podrá modificarse durante la implementación de acuerdo con las necesidades reales del negocio.

---

# 9. Permisos por módulo

Para facilitar la administración, los permisos se organizarán por módulo.

Ejemplo:

```text
Clientes
├── clientes.consultar
├── clientes.crear
├── clientes.editar
└── clientes.eliminar

Préstamos
├── prestamos.consultar
├── prestamos.crear
├── prestamos.editar
└── prestamos.cancelar

Pagos
├── pagos.consultar
├── pagos.registrar
├── pagos.editar
└── pagos.anular

Morosidad
├── morosidad.consultar
└── morosidad.gestionar

WhatsApp
├── whatsapp.consultar
└── whatsapp.enviar

Configuración
├── configuracion.consultar
├── configuracion.editar
└── configuracion.financiera
```

---

# 10. Convención de nombres de permisos

Los permisos utilizarán una nomenclatura consistente:

```text
<modulo>.<accion>
```

Ejemplos:

```text
clientes.consultar
clientes.crear
clientes.editar

prestamos.consultar
prestamos.crear

pagos.consultar
pagos.registrar

morosidad.consultar

whatsapp.consultar
whatsapp.enviar

configuracion.consultar
configuracion.editar
```

---

# 11. Acciones disponibles

Las acciones principales serán:

| Acción | Descripción |
|---|---|
| consultar | Visualizar información |
| crear | Crear un registro |
| editar | Modificar un registro |
| eliminar | Eliminar un registro |
| registrar | Registrar una operación |
| anular | Anular una operación |
| gestionar | Ejecutar acciones administrativas |
| enviar | Enviar una comunicación |
| configurar | Modificar configuración |

---

# 12. Autorización basada en roles

El sistema podrá utilizar autorización basada en roles.

Ejemplo:

```text
Usuario
   ↓
Rol = ADMIN
   ↓
Permitir acceso
```

Otro ejemplo:

```text
Usuario
   ↓
Rol = COBRADOR
   ↓
Solicita configuración financiera
   ↓
Permiso insuficiente
   ↓
Acceso denegado
```

---

# 13. Autorización basada en permisos

Además de los roles, el sistema podrá comprobar permisos específicos.

Ejemplo:

```text
Usuario
   ↓
Rol COBRADOR
   ↓
Permiso pagos.registrar
   ↓
Registrar pago
```

Mientras que:

```text
Usuario
   ↓
Rol COBRADOR
   ↓
No tiene configuracion.financiera
   ↓
Modificar interés
   ↓
Acceso denegado
```

---

# 14. Control en el backend

El backend deberá realizar siempre la validación de autorización.

La aplicación móvil no deberá ser considerada un mecanismo de seguridad.

Flujo:

```text
.NET MAUI
    ↓
Solicitud HTTPS
    ↓
ASP.NET Core Web API
    ↓
Autenticación
    ↓
Autorización
    ↓
Validación de datos
    ↓
Reglas de negocio
    ↓
Operación
```

---

# 15. No confiar en la interfaz móvil

Ocultar un botón en la aplicación no significa que una operación esté protegida.

Por ejemplo:

```text
Cobrador
   ↓
No aparece botón "Eliminar cliente"
```

Esto no es suficiente.

El backend también deberá rechazar:

```text
DELETE /api/clientes/{id}
```

cuando el usuario no tenga el permiso correspondiente.

---

# 16. Respuesta de acceso denegado

Cuando un usuario esté autenticado pero no tenga autorización para realizar una operación, el backend deberá devolver una respuesta apropiada.

Conceptualmente:

```text
403 Forbidden
```

Ejemplo:

```text
Usuario autenticado
        ↓
Sin permiso requerido
        ↓
403 Forbidden
```

La aplicación deberá mostrar un mensaje adecuado.

Ejemplo:

```text
No tienes permisos para realizar esta operación.
```

---

# 17. Diferencia entre 401 y 403

El sistema deberá distinguir:

### 401 - No autenticado

El usuario no está autenticado o su autenticación no es válida.

```text
No existe una sesión válida.
```

### 403 - No autorizado

El usuario está autenticado, pero no tiene permisos suficientes.

```text
La sesión es válida,
pero la operación no está permitida.
```

---

# 18. Protección de clientes

Las operaciones sobre clientes deberán estar protegidas.

### Consultar

Permiso:

```text
clientes.consultar
```

### Crear

Permiso:

```text
clientes.crear
```

### Editar

Permiso:

```text
clientes.editar
```

### Eliminar

Permiso:

```text
clientes.eliminar
```

La eliminación deberá estar especialmente restringida.

---

# 19. Protección de préstamos

Las operaciones sobre préstamos deberán estar protegidas.

Permisos:

```text
prestamos.consultar
prestamos.crear
prestamos.editar
prestamos.cancelar
```

Las operaciones que puedan afectar información financiera deberán requerir permisos apropiados.

---

# 20. Protección de pagos

Los pagos representan operaciones financieras críticas.

Permisos:

```text
pagos.consultar
pagos.registrar
pagos.editar
pagos.anular
```

El registro de un pago deberá realizarse exclusivamente mediante el backend.

El backend deberá validar:

- Usuario.
- Permiso.
- Cliente.
- Préstamo.
- Monto.
- Fecha.
- Estado.
- Reglas financieras.

---

# 21. Protección contra modificación indebida de pagos

Un usuario no autorizado no deberá modificar directamente los valores financieros.

Ejemplo:

```text
Cobrador
   ↓
Intentar modificar pago confirmado
   ↓
Backend
   ↓
Verificar permiso
   ↓
No autorizado
   ↓
403 Forbidden
```

Las modificaciones o anulaciones deberán seguir las reglas definidas para las operaciones financieras.

---

# 22. Protección de morosidad

Los permisos relacionados con morosidad serán:

```text
morosidad.consultar
morosidad.gestionar
```

La consulta podrá estar disponible para el cobrador.

Las acciones administrativas relacionadas con la gestión de morosidad podrán estar restringidas al administrador.

---

# 23. Protección de WhatsApp

Los permisos relacionados con WhatsApp serán:

```text
whatsapp.consultar
whatsapp.enviar
```

El administrador podrá disponer de ambos permisos.

El cobrador podrá tener acceso a ellos únicamente si han sido habilitados.

Las credenciales de Meta nunca deberán estar disponibles para el usuario.

---

# 24. Protección de configuración

La configuración del sistema tendrá diferentes niveles.

## Configuración general

Ejemplos:

```text
Nombre del negocio
Moneda
Zona horaria
Formato de fecha
```

Permiso:

```text
configuracion.editar
```

## Configuración financiera

Ejemplos:

```text
Interés semanal
Frecuencia
Reglas de pagos
Reglas de morosidad
```

Permiso:

```text
configuracion.financiera
```

La configuración financiera deberá estar especialmente protegida.

---

# 25. Protección de usuarios

La administración de usuarios estará reservada al administrador.

Permisos conceptuales:

```text
usuarios.consultar
usuarios.crear
usuarios.editar
usuarios.desactivar
usuarios.gestionar_permisos
```

El cobrador no deberá gestionar usuarios administrativos.

---

# 26. Gestión de roles

La asignación de roles deberá estar restringida.

Ejemplo:

```text
Administrador
      ↓
Crear usuario
      ↓
Asignar rol
      ↓
ADMIN / COBRADOR
      ↓
Guardar
      ↓
Auditar operación
```

Un cobrador no deberá poder elevar sus propios privilegios.

---

# 27. Prevención de escalamiento de privilegios

El sistema deberá impedir que un usuario:

- Modifique su propio rol.
- Se asigne permisos adicionales.
- Modifique permisos de otro usuario sin autorización.
- Acceda a endpoints administrativos.
- Manipule información para obtener privilegios.

Ejemplo:

```text
Cobrador
   ↓
Intenta cambiar rol
   ↓
Backend
   ↓
Validar permiso
   ↓
Denegar
```

---

# 28. Control de acceso por recurso

Además de verificar el permiso, el backend deberá verificar que el usuario pueda acceder al recurso solicitado.

Ejemplo:

```text
GET /api/prestamos/100
```

El backend deberá comprobar:

```text
Usuario autenticado
        +
Permiso de consulta
        +
Préstamo existente
        +
Acceso permitido
```

No deberá bastar únicamente con conocer el identificador del préstamo.

---

# 29. Protección contra IDOR

El sistema deberá prevenir vulnerabilidades de tipo **Insecure Direct Object Reference (IDOR)**.

Ejemplo de riesgo:

```text
/api/clientes/100
```

Un usuario podría intentar cambiar:

```text
/api/clientes/101
```

para consultar información que no debería estar disponible.

El backend deberá validar que el acceso al recurso esté autorizado.

---

# 30. Validación de propiedad y contexto

Cuando sea necesario, el backend deberá comprobar:

- Usuario.
- Rol.
- Permiso.
- Recurso.
- Relación entre entidades.
- Estado del recurso.

Ejemplo:

```text
Usuario
  ↓
Permiso
  ↓
Cliente
  ↓
Préstamo
  ↓
Pago
```

Las relaciones deberán validarse antes de ejecutar la operación.

---

# 31. Autorización de operaciones financieras

Las operaciones financieras deberán tener controles adicionales.

Ejemplos:

```text
Registrar préstamo
Registrar pago
Anular pago
Modificar préstamo
Modificar interés
Modificar reglas financieras
```

Estas operaciones deberán:

1. Autenticar al usuario.
2. Verificar el permiso.
3. Validar los datos.
4. Ejecutar las reglas de negocio.
5. Registrar auditoría cuando corresponda.

---

# 32. Autorización de reactivación

La reactivación de clientes con morosidad deberá respetar las reglas de negocio.

Flujo:

```text
Cliente moroso
      ↓
Evaluar condición
      ↓
¿Puede reactivarse?
      ↓
Verificar permiso
      ↓
Confirmar operación
      ↓
Registrar cambio
      ↓
Auditar
```

No deberá permitirse una reactivación únicamente porque la aplicación móvil envíe una solicitud.

---

# 33. Confirmación de operaciones críticas

Las operaciones críticas podrán requerir una confirmación adicional.

Ejemplos:

- Anular pago.
- Cancelar préstamo.
- Modificar configuración financiera.
- Desactivar usuario.
- Modificar permisos.

Flujo:

```text
Solicitar operación
      ↓
Verificar permiso
      ↓
Mostrar confirmación
      ↓
Usuario confirma
      ↓
Backend valida nuevamente
      ↓
Ejecutar
```

---

# 34. Auditoría de autorización

Las operaciones administrativas y financieras importantes deberán generar registros de auditoría.

Ejemplos:

- Acceso permitido.
- Acceso denegado.
- Cambio de rol.
- Cambio de permisos.
- Modificación de configuración.
- Registro de pago.
- Anulación de pago.
- Modificación de préstamo.
- Reactivación de cliente.

---

# 35. Información de auditoría

El registro podrá contener:

| Campo | Descripción |
|---|---|
| ID | Identificador |
| Usuario | Usuario que ejecutó la acción |
| Rol | Rol utilizado |
| Acción | Operación ejecutada |
| Recurso | Entidad afectada |
| Recurso ID | Identificador del recurso |
| Resultado | Permitido / Denegado |
| Fecha | Fecha y hora |
| Descripción | Información adicional |

No se deberán registrar contraseñas ni secretos.

---

# 36. Autorización desde la aplicación móvil

La aplicación .NET MAUI podrá utilizar la información de permisos para mejorar la experiencia de usuario.

Ejemplo:

```text
Usuario = COBRADOR
        ↓
Consultar permisos
        ↓
Mostrar únicamente funciones disponibles
```

Sin embargo:

> Ocultar una funcionalidad en la aplicación no reemplaza la autorización del backend.

---

# 37. Menú dinámico

El menú podrá adaptarse al rol y permisos.

Administrador:

```text
Dashboard
Clientes
Préstamos
Pagos
Morosidad
Notificaciones
WhatsApp
Usuarios
Configuración
Auditoría
Perfil
```

Cobrador:

```text
Dashboard
Clientes
Préstamos
Pagos
Morosidad
Notificaciones
WhatsApp
Perfil
```

Las opciones definitivas dependerán de los permisos asignados.

---

# 38. Cambio de permisos

Cuando el administrador modifique los permisos de un usuario:

```text
Administrador
      ↓
Modificar permisos
      ↓
Guardar
      ↓
Registrar auditoría
      ↓
Aplicar nuevos permisos
```

Los cambios deberán afectar las solicitudes posteriores de ese usuario.

---

# 39. Cambio de rol

Cuando se modifique el rol de un usuario:

```text
Usuario
  ↓
Rol anterior
  ↓
Administrador modifica
  ↓
Nuevo rol
  ↓
Actualizar permisos
  ↓
Registrar auditoría
```

El sistema deberá evitar inconsistencias entre el rol y los permisos asociados.

---

# 40. Desactivación de usuario

Cuando un usuario sea desactivado:

```text
Usuario activo
      ↓
Administrador
      ↓
Desactivar
      ↓
Estado = INACTIVO
      ↓
Invalidar sesiones según política
      ↓
Registrar auditoría
```

El usuario no deberá poder realizar nuevas operaciones.

---

# 41. Revocación de permisos

Si un usuario pierde un permiso:

```text
Cobrador
   ↓
Tenía pagos.anular
   ↓
Administrador elimina permiso
   ↓
Nuevo intento
   ↓
403 Forbidden
```

El backend deberá aplicar la configuración actual.

---

# 42. Protección de configuración crítica

Los siguientes parámetros deberán estar especialmente protegidos:

```text
Interés semanal
Frecuencia de pago
Reglas de morosidad
Reglas de reactivación
Horarios de notificaciones
Configuración de WhatsApp
Configuración de Firebase
```

El acceso deberá estar limitado a usuarios autorizados.

---

# 43. Principio de separación de funciones

Cuando sea necesario, las operaciones críticas deberán mantener una separación de responsabilidades.

Por ejemplo:

```text
Usuario A
   ↓
Registra operación

Usuario B
   ↓
Revisa / autoriza operación crítica
```

La aplicación de este principio dependerá de las necesidades finales del negocio.

---

# 44. Autorización en la API

Los endpoints deberán definir los requisitos de autorización.

Ejemplo conceptual (prefijo `/api/v1` omitido por brevedad; el contrato normativo está en `05-API/01-Endpoints.md`):

```text
GET /api/clientes
→ clientes.consultar

POST /api/clientes
→ clientes.crear

PUT /api/clientes/{id}
→ clientes.editar

GET /api/prestamos
→ prestamos.consultar

POST /api/pagos
→ pagos.registrar

GET /api/morosidad
→ morosidad.consultar

POST /api/whatsapp/send
→ whatsapp.enviar
```

La definición definitiva de endpoints se documentará en:

```text
05-API/01-Endpoints.md
```

---

# 45. Flujo general de autorización

```text
Solicitud HTTP
      ↓
¿Usuario autenticado?
   ┌──┴──┐
  No     Sí
  ↓       ↓
401    Obtener identidad
          ↓
       Obtener rol
          ↓
     Obtener permisos
          ↓
    ¿Tiene permiso?
      ┌───┴───┐
     No       Sí
     ↓         ↓
    403    Validar datos
                ↓
         Reglas de negocio
                ↓
            Ejecutar
                ↓
             Auditar
```

---

# 46. Ejemplo de acceso permitido

```text
Usuario: Juan
Rol: COBRADOR
Permiso: pagos.registrar

Solicitud:
POST /api/pagos

Resultado:

Autenticado → Sí
Permiso → Sí
Datos → Válidos
Reglas → Cumplidas

Resultado:
Operación permitida
```

---

# 47. Ejemplo de acceso denegado

```text
Usuario: Juan
Rol: COBRADOR

Solicitud:
Modificar configuración financiera

Autenticado → Sí
Permiso → No

Resultado:
403 Forbidden
```

---

# 48. Ejemplo de intento de escalamiento

```text
Usuario: COBRADOR

Solicitud:
Cambiar su propio rol a ADMIN

        ↓

Autenticación
        ↓
Correcta
        ↓
Autorización
        ↓
Sin permiso
        ↓
403 Forbidden
        ↓
Registrar intento
```

---

# 49. Seguridad de la autorización

La autorización deberá:

- Ejecutarse en el backend.
- Utilizar la identidad autenticada.
- Validar permisos.
- Validar recursos.
- Aplicar mínimo privilegio.
- Proteger operaciones financieras.
- Proteger configuraciones críticas.
- Registrar eventos importantes.
- Evitar confiar en parámetros enviados por el cliente.

---

# 50. Pruebas de autorización

Se deberán realizar pruebas para cada rol.

## 50.1 Pruebas del administrador

Verificar:

- Acceso al dashboard.
- Gestión de clientes.
- Gestión de préstamos.
- Registro de pagos.
- Gestión de morosidad.
- Gestión de WhatsApp.
- Gestión de usuarios.
- Configuración.
- Auditoría.

## 50.2 Pruebas del cobrador

Verificar:

- Consulta de clientes.
- Consulta de préstamos.
- Registro de pagos.
- Consulta de morosidad.
- Acceso permitido a WhatsApp.
- Restricción de configuración.
- Restricción de usuarios.
- Restricción de permisos.

---

# 51. Pruebas de seguridad

Se deberán probar:

- Solicitud sin autenticación.
- Token inválido.
- Token expirado.
- Permiso inexistente.
- Rol incorrecto.
- Manipulación de identificadores.
- Acceso a recursos no autorizados.
- Intento de modificar el propio rol.
- Intento de modificar permisos.
- Acceso a configuración financiera.
- Acceso a endpoints administrativos.

---

# 52. Pruebas de IDOR

Ejemplo:

```text
Usuario autorizado:
GET /api/clientes/100
```

Posteriormente:

```text
GET /api/clientes/101
```

El backend deberá verificar que el acceso al segundo recurso también esté autorizado.

Nunca se deberá asumir que conocer un ID implica tener permiso para acceder al recurso.

---

# 53. Criterios de aceptación

El módulo de autorización será considerado correctamente implementado cuando:

- [ ] Existan roles definidos.
- [ ] Existan permisos por módulo.
- [ ] El administrador tenga permisos administrativos.
- [ ] El cobrador tenga permisos operativos según configuración.
- [ ] El backend valide todos los permisos.
- [ ] Las operaciones no autorizadas devuelvan una respuesta adecuada.
- [ ] Se utilice mínimo privilegio.
- [ ] Las operaciones financieras estén protegidas.
- [ ] La configuración financiera esté restringida.
- [ ] La gestión de usuarios esté restringida.
- [ ] La gestión de permisos esté restringida.
- [ ] Se prevenga el escalamiento de privilegios.
- [ ] Se validen los recursos solicitados.
- [ ] Se contemplen controles contra IDOR.
- [ ] Los cambios críticos queden registrados.
- [ ] La aplicación móvil no sea el único mecanismo de seguridad.
- [ ] Se realicen pruebas para cada rol.
- [ ] Los usuarios desactivados no puedan realizar nuevas operaciones.

---

# 54. Dependencias

Este módulo depende de:

- `09-Seguridad/01-Autenticacion.md`
- ASP.NET Core Web API.
- .NET MAUI.
- PostgreSQL.
- Entity Framework Core.
- Sistema de usuarios.
- Sistema de roles.
- Sistema de permisos.
- Sistema de auditoría.

---

# 55. Consideraciones futuras

En futuras versiones podrán incorporarse:

- Permisos personalizados por usuario.
- Roles adicionales.
- Control de acceso basado en atributos.
- Control de acceso por sucursal.
- Control de acceso por cartera de clientes.
- Flujos de aprobación.
- Autorización multifactor para operaciones críticas.
- Segregación avanzada de funciones.
- Políticas de acceso contextual.
- Alertas ante intentos de acceso no autorizado.

---

# 56. Resumen

El sistema de autorización garantizará que cada usuario pueda realizar únicamente las operaciones correspondientes a su rol y permisos.

Los roles iniciales serán:

```text
ADMIN
COBRADOR
```

El administrador tendrá capacidades de gestión y configuración, mientras que el cobrador estará orientado principalmente a las operaciones de cobranza y gestión.

La autorización será ejecutada principalmente en **ASP.NET Core Web API**, independientemente de las restricciones visuales implementadas en .NET MAUI.

Las operaciones financieras, configuraciones críticas, gestión de usuarios y permisos deberán contar con controles adicionales y trazabilidad.

El sistema aplicará el principio de mínimo privilegio y deberá prevenir problemas como escalamiento de privilegios y acceso indebido a recursos.

De esta manera, la autenticación permitirá determinar **quién es el usuario**, mientras que la autorización determinará **qué puede hacer dentro del sistema**.