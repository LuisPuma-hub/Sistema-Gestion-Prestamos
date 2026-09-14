# Plan de Pruebas

## 1. Objetivo

Definir la estrategia, alcance, metodología y criterios para verificar la calidad del **Sistema de Gestión de Préstamos**, asegurando que sus funcionalidades cumplan con los requisitos establecidos y que las operaciones financieras sean correctas, seguras y consistentes.

El plan contempla pruebas sobre:

- Aplicación móvil .NET MAUI.
- Backend ASP.NET Core Web API.
- Base de datos PostgreSQL.
- Autenticación y autorización.
- Gestión de clientes.
- Gestión de préstamos.
- Registro de pagos.
- Cálculo de intereses.
- Control de morosidad.
- Notificaciones mediante Firebase Cloud Messaging.
- Mensajería mediante WhatsApp Business Platform de Meta.
- Configuraciones del sistema.
- Seguridad.
- Integridad de datos.

---

## 2. Alcance

Las pruebas cubrirán los principales módulos funcionales y técnicos del sistema.

### 2.1 Módulos incluidos

| Módulo | Pruebas |
|---|---|
| Autenticación | Inicio de sesión, cierre de sesión, recuperación y cambio de contraseña |
| Usuarios | Roles y permisos |
| Clientes | Registro, consulta, modificación y estados |
| Avales | Registro y asociación |
| Préstamos | Registro, aprobación, consulta y estados |
| Pagos | Registro, aplicación y validación |
| Morosidad | Detección, seguimiento y reactivación |
| Notificaciones | Programación y envío |
| WhatsApp | Plantillas, envío y estados |
| Configuración | Parámetros generales y financieros |
| Seguridad | Autenticación, autorización y protección de datos |
| Base de datos | Integridad y restricciones |
| API | Endpoints, validaciones y respuestas |

---

## 3. Objetivos de las Pruebas

Los objetivos principales son:

1. Verificar que los requisitos funcionales se implementen correctamente.
2. Detectar errores antes de la puesta en producción.
3. Validar las reglas financieras.
4. Comprobar la integridad de los datos.
5. Verificar los permisos de cada rol.
6. Comprobar la seguridad de la información.
7. Validar las integraciones externas.
8. Verificar el comportamiento ante datos inválidos.
9. Comprobar la correcta comunicación entre aplicación móvil, API y base de datos.
10. Reducir el riesgo de errores en operaciones financieras.

---

## 4. Estrategia de Pruebas

Se utilizará una estrategia basada en diferentes niveles de prueba.

```text
Pruebas unitarias
       │
       ▼
Pruebas de integración
       │
       ▼
Pruebas de API
       │
       ▼
Pruebas funcionales
       │
       ▼
Pruebas de seguridad
       │
       ▼
Pruebas de aceptación
```

Cada nivel tendrá un objetivo específico.

---

## 5. Niveles de Prueba

### 5.1 Pruebas Unitarias

Validarán componentes individuales del sistema.

Se podrán probar:

- Servicios.
- Métodos.
- Validadores.
- Cálculos financieros.
- Reglas de negocio.
- Conversión de datos.

Ejemplo:

```text
Capital inicial = S/ 100.00
Interés semanal = 5 %

Interés = 100 × 0.05
Interés = S/ 5.00
```

---

### 5.2 Pruebas de Integración

Verificarán la comunicación entre diferentes componentes.

Se probarán integraciones como:

```text
.NET MAUI
    │
    ▼
ASP.NET Core Web API
    │
    ▼
Entity Framework Core
    │
    ▼
PostgreSQL
```

También:

```text
ASP.NET Core
    ├── Firebase Cloud Messaging
    └── WhatsApp Business Platform
```

---

### 5.3 Pruebas de API

Se validarán los endpoints de la API REST.

Se comprobará:

- Código HTTP.
- Autenticación.
- Autorización.
- Datos enviados.
- Datos recibidos.
- Validaciones.
- Manejo de errores.
- Reglas de negocio.

Ejemplo:

```text
POST /api/v1/pagos
```

La prueba deberá verificar que:

1. El usuario esté autenticado.
2. Tenga permiso para registrar pagos.
3. El préstamo exista.
4. El monto sea válido.
5. El pago pueda aplicarse correctamente.
6. El saldo sea actualizado.
7. La operación quede registrada.

---

## 6. Pruebas Funcionales

Las pruebas funcionales comprobarán que cada funcionalidad produzca el resultado esperado.

Se deberán cubrir como mínimo:

- Inicio de sesión.
- Registro de clientes.
- Registro de avales.
- Registro de préstamos.
- Aprobación de préstamos.
- Registro de pagos.
- Cálculo de intereses.
- Actualización de saldos.
- Detección de morosidad.
- Reactivación.
- Envío de notificaciones.
- Envío de mensajes WhatsApp.
- Configuración del sistema.

---

## 7. Pruebas de Regresión

Después de realizar modificaciones en el sistema se ejecutarán pruebas para comprobar que las funcionalidades existentes continúen funcionando correctamente.

Se deberán repetir especialmente las pruebas relacionadas con:

- Préstamos.
- Pagos.
- Intereses.
- Saldos.
- Morosidad.
- Autenticación.
- Permisos.
- Notificaciones.
- WhatsApp.

Una modificación en una regla financiera deberá generar una nueva ejecución de las pruebas financieras relacionadas.

---

## 8. Pruebas de Seguridad

Se verificarán los mecanismos de protección del sistema.

Se deberán comprobar:

- Inicio de sesión.
- Contraseñas incorrectas.
- Sesiones expiradas.
- Tokens inválidos.
- Roles.
- Permisos.
- Acceso a recursos no autorizados.
- Protección de endpoints.
- Protección de información personal.
- Protección de información financiera.
- Protección de secretos.
- Manejo seguro de errores.

---

## 9. Pruebas de Base de Datos

Se comprobará la integridad de PostgreSQL.

Las pruebas deberán verificar:

- Claves primarias.
- Claves foráneas.
- Restricciones.
- Campos obligatorios.
- Tipos de datos.
- Valores únicos.
- Relaciones entre entidades.
- Integridad de préstamos.
- Integridad de pagos.
- Integridad de clientes.

No deberán existir registros financieros huérfanos.

---

## 10. Pruebas de Interfaz Móvil

La aplicación .NET MAUI deberá probarse en diferentes situaciones de uso.

Se verificará:

- Navegación.
- Formularios.
- Validaciones.
- Mensajes de error.
- Mensajes de confirmación.
- Listados.
- Detalles.
- Botones.
- Estados de carga.
- Conectividad.
- Manejo de errores de API.
- Sesiones.

También se comprobará que la aplicación no permita operaciones que el backend rechazaría.

---

## 11. Pruebas de Reglas Financieras

Las reglas financieras tendrán prioridad alta.

La configuración base será (normativo, ver RN-PRE-003):

```text
Interés semanal = 5 % (TASA_SEMANAL_DEFAULT = 0.05, v1 fija CHECK=5.00)
Base de cálculo = capital inicial (snapshot inmutable por préstamo)
Interés compuesto = No
Frecuencia = Semanal
Fórmula = interes_periodo = ROUND(capital_inicial * tasa_snapshot, 2, HALF_UP)
Dinero API = integer céntimos + PEN (S/ solo render UI)
```

Se deberán probar diferentes escenarios.

### Ejemplo 1

```text
Capital inicial: S/ 100.00
Interés: 5 %

Interés semanal:
100 × 0.05 = S/ 5.00
```

### Ejemplo 2

```text
Capital inicial: S/ 500.00
Interés: 5 %

Interés semanal:
500 × 0.05 = S/ 25.00
```

### Ejemplo 3

```text
Capital inicial: S/ 1,000.00
Interés: 5 %

Interés semanal:
1000 × 0.05 = S/ 50.00
```

El interés deberá calcularse siempre sobre el capital inicial y no sobre el saldo restante.

---

## 12. Pruebas de Aplicación de Pagos

Los pagos deberán aplicarse respetando la regla normativa (RN-PAG-003, no duplicar):

```text
Pago recibido
      │
      ▼
Mora pendiente
      │
      ▼
Interés pendiente FIFO (P1, P2, ... por fecha_vencimiento asc)
      │
      ▼
Excedente
      │
      ▼
Capital
```

Todo `POST /api/v1/pagos` exige `idempotenciaKey: uuid`. Excedente sobre deuda total → `422 E-PAG-09` (v1 sin vuelto).

Ejemplo:

```text
Interés pendiente: S/ 5.00
Capital pendiente: S/ 100.00
Pago: S/ 20.00
```

Resultado esperado:

```text
Interés pagado: S/ 5.00
Capital pagado: S/ 15.00
Capital pendiente: S/ 85.00
```

---

## 13. Pruebas de Pagos Parciales

Se deberán probar pagos inferiores al interés pendiente.

Ejemplo:

```text
Interés pendiente: S/ 5.00
Pago: S/ 3.00
```

Resultado esperado:

```text
Interés pagado: S/ 3.00
Interés pendiente: S/ 2.00
Capital pagado: S/ 0.00
```

El sistema no deberá aplicar el pago parcialmente recibido al capital mientras exista interés pendiente, de acuerdo con la regla establecida.

---

## 14. Pruebas de Múltiples Préstamos

En v1 el cliente tiene relación histórica 1:N pero **un solo préstamo en `ACTIVO`/`MOROSO`** (ver `01-Requisitos/02-Alcance.md §2-3` e índice `ux_prestamo_activo`). Intentar un segundo activo → `409 E-LOAN-ACTIVE`.

Se deberá verificar que:

```text
Cliente
 ├── Préstamo 001
 ├── Préstamo 002
 └── Préstamo 003
```

Cada préstamo mantenga independientemente:

- Capital.
- Intereses.
- Pagos.
- Saldo.
- Fechas.
- Estado.
- Morosidad.

Un pago registrado para un préstamo no deberá modificar accidentalmente otro préstamo del mismo cliente.

---

## 15. Pruebas de Morosidad

La morosidad deberá evaluarse según las reglas definidas.

El sistema deberá detectar los intereses vencidos y contabilizar los retrasos.

Se deberá verificar especialmente el escenario:

```text
0 pagos vencidos (ACTIVO al día)
        │
        ▼
1-2 pagos vencidos (ACTIVO con cuotas VENCIDA/PARCIAL)
        │
        ▼
>= 3 pagos vencidos
        │
        ▼
MOROSO (+ Historial_Morosidad + notif CRITICAL_DELINQUENCY)
```

Cuando `COUNT(vencidos_impagos) >= 3` (job `00:05 America/Lima`, RN-MOR-005) el préstamo pasa a `MOROSO`. `CRITICAL_DELINQUENCY` es tipo de notificación/plantilla, no estado. La reactivación `MOROSO → ACTIVO` es manual con pago ≥1 vencido o motivo 10–200 + actor (RN-PRE-012), sin resetear contador.

---

## 16. Pruebas de Notificaciones

Se deberán probar las notificaciones enviadas mediante Firebase Cloud Messaging.

Se deberán verificar:

- Recordatorios.
- Pagos próximos.
- Pagos vencidos.
- Morosidad.
- Mensajes informativos.
- Estados de envío.
- Manejo de errores.

Los horarios configurados inicialmente serán:

```text
08:00
16:00
```

Se deberá comprobar que las notificaciones sean procesadas incluso cuando la aplicación no se encuentre abierta, dependiendo de la configuración y capacidades del servicio.

---

## 17. Pruebas de WhatsApp

Se deberá validar la integración con WhatsApp Business Platform de Meta (`POST /api/v1/whatsapp/mensajes` con `template` aprobado + `idempotenciaKey`, sintaxis Meta `{{1}}` posicional).

Se probarán:

- Selección de plantilla aprobada (`wh_pago_recordatorio_v1`, `es_PE`).
- Variables posicionales completas.
- Número del destinatario.
- Envío.
- Confirmación (`identificador_externo wamid.xxx`).
- Error de envío.
- Reintento (solo temporales).
- Mensajes duplicados (misma key/evento → no reenviar).
- Webhooks (`GET+POST /api/v1/whatsapp/webhook`, `X-Hub-Signature-256`, 200 < 5 s, proceso async).
- Registro de estados unificados (`PENDIENTE/ENVIADO/ENTREGADO/LEIDO/FALLIDO/CANCELADO`).

Los mensajes deberán utilizar plantillas aprobadas cuando corresponda (100 % plantilla en v1).

---

## 18. Pruebas de Autenticación

Se deberán realizar como mínimo las siguientes pruebas (contrato `05-API/02` + `02-Casos-Prueba.md CP-LOGIN-01`):

| Caso | Resultado esperado |
|---|---|
| Credenciales correctas | `200 LoginResponse {accessToken JWT 15 min, refreshToken opaque 7 d, expiresIn:900}` |
| Contraseña incorrecta o usuario inexistente | `401 INVALID_CREDENTIALS "Correo o contraseña incorrectos"` (genérico, sin revelar cuál falló), p95 < 3 s |
| Campos vacíos | `400` validación |
| Sesión expirada / token inválido | `401`, renovar vía `POST /api/v1/auth/refresh` (rotación obligatoria) |
| Usuario `INACTIVO/BLOQUEADO` | `403 ACCOUNT_BLOCKED` |
| Cierre de sesión | Refresh invalidado; `GET /api/v1/auth/sesiones` sin la sesión |

---

## 19. Pruebas de Autorización

Se deberá verificar que cada rol solo pueda realizar las operaciones permitidas.

Ejemplo:

| Operación | Administrador | Cobrador |
|---|---:|---:|
| Consultar clientes | Sí | Sí |
| Registrar cliente | Sí | Con `clientes.crear` |
| Registrar préstamo (crea `PENDIENTE`) | Sí | Con `prestamos.crear` |
| Aprobar / anular préstamo | Sí | No (403) |
| Registrar pago | Sí | Sí |
| Consultar morosidad | Sí | Sí |
| `POST /moras/recalcular` | Sí | No (403) |
| Modificar configuración financiera | Sí | No |
| Gestionar permisos | Sí | No |

La matriz canónica es `09-Seguridad/02-Autorizacion.md §8`. Detalle de casos en `02-Casos-Prueba.md CP-AUTHZ-01`.

---

## 20. Pruebas de Validación

Se deberán probar datos inválidos.

Ejemplos:

```text
Nombre vacío
Teléfono inválido
Documento vacío
Monto negativo
Monto igual a cero
Fecha inválida
Interés inválido
Identificador inexistente
```

El backend deberá rechazar información inválida.

---

## 21. Pruebas de Errores

Se deberá comprobar el comportamiento ante:

- Pérdida de conexión.
- Servidor no disponible.
- Error de base de datos.
- Timeout.
- Servicio externo no disponible.
- Token expirado.
- Datos inválidos.
- Error interno.

La aplicación deberá mostrar mensajes comprensibles al usuario sin revelar información técnica sensible.

---

## 22. Pruebas de Rendimiento

Se evaluará el comportamiento del sistema bajo diferentes cargas.

Se podrán medir:

- Tiempo de respuesta de API.
- Tiempo de consulta de clientes.
- Tiempo de consulta de préstamos.
- Tiempo de registro de pagos.
- Tiempo de generación de reportes.
- Tiempo de procesamiento de notificaciones.

Las operaciones críticas deberán responder dentro de tiempos aceptables para el entorno de uso definido.

---

## 23. Pruebas de Compatibilidad

La aplicación móvil deberá probarse en diferentes dispositivos compatibles con .NET MAUI.

Se deberá verificar:

- Diferentes tamaños de pantalla.
- Diferentes resoluciones.
- Orientación soportada.
- Conectividad Wi-Fi.
- Conectividad móvil.
- Diferentes versiones compatibles del sistema operativo.

---

## 24. Pruebas de Usabilidad

Se evaluará si los usuarios pueden realizar las operaciones principales de forma clara.

Se verificará:

- Facilidad de navegación.
- Claridad de formularios.
- Comprensión de mensajes.
- Identificación de errores.
- Confirmación de operaciones.
- Acceso a información relevante.
- Facilidad para registrar pagos.

Las operaciones financieras importantes deberán solicitar confirmación antes de ejecutarse cuando corresponda.

---

## 25. Ambiente de Pruebas

El ambiente de pruebas deberá estar separado del ambiente de producción.

Componentes:

```text
.NET MAUI
     │
     ▼
ASP.NET Core Web API
     │
     ▼
PostgreSQL - PRUEBAS
     │
     ├── Firebase - PRUEBAS
     └── Meta - PRUEBAS
```

No deberán utilizarse datos reales de producción durante las pruebas sin controles adecuados.

---

## 26. Datos de Prueba

Se deberán crear datos controlados para ejecutar los casos de prueba.

Ejemplos:

### Cliente

```text
Cliente de prueba 01
Documento: valor de prueba
Teléfono: número de prueba
Estado: Activo
```

### Préstamo

```text
Capital: S/ 100.00
Interés: 5 %
Frecuencia: Semanal
Estado: Activo
```

### Pago

```text
Monto: S/ 5.00
Tipo: Interés
Estado: Registrado
```

Los datos de prueba no deberán corresponder a información personal real innecesaria.

---

## 27. Gestión de Defectos

Cuando se detecte un defecto deberá registrarse como mínimo:

- Identificador.
- Fecha.
- Módulo.
- Descripción.
- Pasos para reproducir.
- Resultado esperado.
- Resultado obtenido.
- Prioridad.
- Severidad.
- Evidencia.
- Estado.
- Responsable.

Ejemplo:

```text
ID: BUG-001
Módulo: Pagos
Severidad: Alta
Prioridad: Alta

Descripción:
El sistema aplica el pago al capital antes de cubrir
el interés pendiente.

Resultado esperado:
El interés debe pagarse primero.

Resultado obtenido:
El pago se aplica directamente al capital.
```

---

## 28. Severidad de Defectos

Se utilizará la siguiente clasificación:

| Severidad | Descripción |
|---|---|
| Crítica | Impide utilizar el sistema o compromete información importante |
| Alta | Afecta una funcionalidad principal |
| Media | Afecta parcialmente una funcionalidad |
| Baja | Error menor sin impacto significativo |

Los errores relacionados con:

- Cálculo de intereses.
- Saldos.
- Pagos.
- Seguridad.
- Pérdida de datos.

deberán recibir especial prioridad.

---

## 29. Prioridad de Defectos

| Prioridad | Acción |
|---|---|
| P1 - Crítica | Resolver inmediatamente |
| P2 - Alta | Resolver antes de liberar |
| P3 - Media | Resolver en la iteración correspondiente |
| P4 - Baja | Programar para una versión futura |

Un defecto financiero crítico no deberá considerarse aceptable para producción.

---

## 30. Evidencias de Prueba

Cada prueba importante deberá generar evidencia cuando corresponda.

Las evidencias pueden incluir:

- Capturas de pantalla.
- Respuestas de API.
- Logs.
- Registros de base de datos.
- Resultados de pruebas automatizadas.
- Reportes.
- Evidencia de mensajes WhatsApp.
- Evidencia de notificaciones.

Las evidencias deberán almacenarse de forma organizada.

---

## 31. Herramientas de Prueba

Podrán utilizarse herramientas compatibles con el stack del proyecto.

### Desarrollo y pruebas

- Visual Studio Code (Stable 64 bits, único IDE oficial, con extensiones C#, C# Dev Kit y .NET MAUI).
- .NET SDK LTS con workload MAUI + Android SDK.
- .NET.
- C#.
- .NET MAUI.

### Backend

- ASP.NET Core.
- Entity Framework Core.

### Base de datos

- PostgreSQL.

### Control de versiones

- Git.
- GitHub.

### Automatización y asistencia

- GitHub Copilot (inline en VS Code).
- Muse Spark vía OpenCode (agente para edición y verificación).

Las herramientas específicas para pruebas automatizadas podrán incorporarse conforme avance la implementación.

---

## 32. Criterios de Entrada

Las pruebas podrán comenzar cuando:

- [ ] Los requisitos principales estén definidos.
- [ ] La funcionalidad a probar esté implementada.
- [ ] El ambiente de pruebas esté disponible.
- [ ] La base de datos de pruebas esté configurada.
- [ ] Existan datos de prueba.
- [ ] Los servicios requeridos estén disponibles.
- [ ] Los casos de prueba estén definidos.

---

## 33. Criterios de Salida

Una funcionalidad podrá considerarse validada cuando:

- [ ] Los casos críticos hayan sido ejecutados.
- [ ] No existan defectos críticos abiertos.
- [ ] Los defectos de alta prioridad hayan sido resueltos o aceptados formalmente.
- [ ] Las reglas financieras hayan sido verificadas.
- [ ] La seguridad básica haya sido validada.
- [ ] La integración con servicios externos funcione correctamente.
- [ ] La información mantenga su integridad.
- [ ] Los resultados hayan sido documentados.

---

## 34. Criterios de Aceptación General

El sistema podrá considerarse listo para una versión de producción cuando:

```text
Requisitos
    +
Funcionalidad
    +
Reglas financieras
    +
Seguridad
    +
Integridad de datos
    +
Integraciones
    +
Pruebas de aceptación
    =
Versión aprobada
```

---

## 35. Responsabilidades

### Desarrollo

Responsable de:

- Corregir defectos.
- Implementar funcionalidades.
- Ejecutar pruebas unitarias.
- Mantener la calidad del código.

### Responsable de pruebas

Responsable de:

- Diseñar casos de prueba.
- Ejecutar pruebas.
- Registrar defectos.
- Validar correcciones.
- Generar evidencias.

### Administrador

Responsable de:

- Validar reglas del negocio.
- Revisar resultados.
- Realizar pruebas de aceptación.
- Confirmar que las funcionalidades satisfacen las necesidades operativas.

---

## 36. Seguimiento de Pruebas

El avance podrá controlarse mediante indicadores como:

```text
Casos planificados
Casos ejecutados
Casos aprobados
Casos rechazados
Casos bloqueados
Defectos encontrados
Defectos corregidos
Defectos pendientes
```

También podrá calcularse:

```text
% Ejecución =
Casos ejecutados / Casos planificados × 100
```

y:

```text
% Aprobación =
Casos aprobados / Casos ejecutados × 100
```

---

## 37. Matriz General de Pruebas

| ID (plan) | ID detallado (02) | Módulo | Tipo | Prioridad |
|---|---|---|---|---|
| CP-001 | CP-LOGIN-01 | Login | Funcional | Alta |
| CP-002 | CP-CLI-DUP-01 | Clientes | Funcional | Alta |
| CP-003 | CP-LOAN-MONTO-01, CP-LOAN-ACTIVO-01 | Préstamos | Funcional | Alta |
| CP-004 | F-01–F-08 (03) | Intereses | Financiera | Crítica |
| CP-005 | CP-PAGO-01–04, CP-PAGO-IDEM-01 | Pagos | Financiera | Crítica |
| CP-006 | CP-MORO-01 | Morosidad | Financiera | Alta |
| CP-007 | CP-NOTIF-01 | Notificaciones | Integración | Media |
| CP-008 | CP-WA-01 | WhatsApp | Integración | Media |
| CP-009 | CP-AUTHZ-01 | Autorización | Seguridad | Crítica |
| CP-010 | CP-AUTHZ-01, CP-NOTIF-01 | Protección de datos | Seguridad | Crítica |
| CP-011 | CP-PAGO-IDEM-01 | API | Integración | Alta |
| CP-012 | §9 (BD) | PostgreSQL | Integridad | Alta |

El detalle request/response exacto vive en `02-Casos-Prueba.md` y `03-Pruebas-Reglas-Financieras.md`; esta tabla es índice, no duplica casos.

---

## 38. Pruebas de Aceptación

Las pruebas de aceptación deberán validar los principales escenarios reales de uso.

### Escenario 1: Registro de cliente

```text
Iniciar sesión
      ↓
Clientes
      ↓
Nuevo cliente
      ↓
Ingresar información
      ↓
Guardar
      ↓
Cliente registrado
```

### Escenario 2: Registro de préstamo

```text
Seleccionar cliente
      ↓
Solicitud APROBADA 1:1 (solicitudId obligatorio en v1)
      ↓
Registrar préstamo
      ↓
Ingresar capital
      ↓
Aplicar configuración financiera
      ↓
Guardar
      ↓
Préstamo creado PENDIENTE → aprobar [ADMIN] → ACTIVO
```

### Escenario 3: Registro de pago

```text
Seleccionar préstamo
      ↓
Registrar pago (POST /api/v1/pagos + idempotenciaKey)
      ↓
Validar monto (montoCents > 0, <= deuda_total o 422 E-PAG-09)
      ↓
Aplicar a mora, luego a interés FIFO
      ↓
Aplicar excedente al capital
      ↓
Verificar SUM(pago_cuotas) = monto_pago y saldo = programado - pagado
      ↓
Actualizar saldo y estado cuota (PARCIAL/PAGADA)
      ↓
Registrar operación
```

### Escenario 4: Morosidad

```text
Interés vencido (pagado_interes < interes a 23:59:59 America/Lima)
      ↓
Job 00:05 cuenta vencidos impagos
      ↓
>= 3 vencidos → MOROSO + Historial_Morosidad
      ↓
Notificar CRITICAL_DELINQUENCY + WhatsApp wh_morosidad_critica_v1
```

---

## 39. Pruebas de Recuperación

Se deberá verificar que el sistema pueda recuperarse después de:

- Caída del backend.
- Reinicio del servidor.
- Pérdida temporal de conexión.
- Fallo de PostgreSQL.
- Error de integración externa.

Después de la recuperación se deberá comprobar que:

- Los datos permanezcan íntegros.
- No se dupliquen pagos.
- No se pierdan operaciones confirmadas.
- Las sesiones se comporten correctamente.
- Las tareas pendientes puedan continuar de forma segura.

---

## 40. Pruebas de Idempotencia

Las operaciones que puedan repetirse accidentalmente deberán contar con mecanismos para evitar duplicaciones.

Especialmente:

- Registro de pagos.
- Envío de WhatsApp.
- Procesamiento de notificaciones.
- Webhooks.
- Operaciones programadas.

Ejemplo:

```text
Solicitud de pago
      │
      ▼
ID de operación
      │
      ├── Primera solicitud → Procesar
      │
      └── Solicitud duplicada → No duplicar operación
```

---

## 41. Pruebas de Configuración

Se deberán comprobar las configuraciones del sistema:

- Moneda.
- Zona horaria.
- Formato de fecha.
- Tasa de interés.
- Frecuencia.
- Horarios de notificación.
- Preferencias de comunicación.
- Parámetros de morosidad.

Los cambios de parámetros financieros deberán estar protegidos y auditados.

---

## 42. Riesgos de Prueba

| Riesgo | Impacto | Mitigación |
|---|---|---|
| Datos de prueba incorrectos | Alto | Validar datos antes de ejecutar |
| Ambiente inestable | Alto | Preparar ambiente separado |
| Cambios constantes | Medio | Ejecutar regresión |
| Servicio externo no disponible | Medio | Utilizar ambientes/pruebas controladas |
| Error en reglas financieras | Crítico | Pruebas específicas y automatizadas |
| Pérdida de datos de prueba | Alto | Respaldos |
| Falta de evidencias | Medio | Registrar resultados sistemáticamente |

---

## 43. Recomendación de Automatización

Las pruebas repetitivas deberán automatizarse progresivamente.

Se recomienda priorizar:

1. Cálculo de intereses.
2. Aplicación de pagos.
3. Cálculo de saldos.
4. Morosidad.
5. Validaciones.
6. Autenticación.
7. Autorización.
8. Endpoints críticos.
9. Integridad de datos.

Las reglas financieras deberán tener una cobertura de pruebas especialmente alta debido al impacto que tendría un error.

---

## 44. Mantenimiento del Plan

Este documento deberá actualizarse cuando:

- Se agreguen nuevos módulos.
- Cambien las reglas de negocio.
- Cambie la arquitectura.
- Se incorporen nuevas integraciones.
- Se modifiquen las reglas financieras.
- Se detecten nuevos riesgos.
- Se incorporen nuevos casos de prueba.

Cada cambio importante del sistema deberá evaluar si requiere actualizar el plan de pruebas.

---

## 45. Resultado Esperado

El proceso de pruebas deberá proporcionar evidencia suficiente para determinar que el Sistema de Gestión de Préstamos:

- Cumple los requisitos establecidos.
- Ejecuta correctamente las reglas financieras.
- Mantiene la integridad de los datos.
- Protege la información.
- Controla correctamente los permisos.
- Gestiona los errores.
- Mantiene la comunicación entre sus componentes.
- Funciona correctamente con Firebase Cloud Messaging.
- Funciona correctamente con WhatsApp Business Platform de Meta.
- Puede utilizarse de forma confiable en las operaciones de gestión de préstamos.

---

## 46. Conclusión

El presente plan establece una estrategia integral para validar la calidad del Sistema de Gestión de Préstamos.

La prioridad de las pruebas estará enfocada en las funcionalidades que puedan generar mayor impacto operativo, especialmente las relacionadas con **préstamos, intereses, pagos, saldos, morosidad, seguridad e integridad de información**.

La aplicación de pruebas unitarias, de integración, funcionales, de seguridad, financieras y de aceptación permitirá reducir riesgos antes de la puesta en producción y proporcionar una base documentada para validar las futuras versiones del sistema.