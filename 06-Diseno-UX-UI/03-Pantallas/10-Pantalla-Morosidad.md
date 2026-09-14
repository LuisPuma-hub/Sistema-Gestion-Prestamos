# Pantalla de Morosidad

## 1. Información General

| Campo | Detalle |
|---|---|
| Nombre | Pantalla de Morosidad |
| Archivo | `10-Pantalla-Morosidad.md` |
| Módulo | Morosidad |
| Tipo | Pantalla de gestión y seguimiento |
| Acceso | Administrador y Cobrador |
| Prioridad | Alta |
| Plataforma | Aplicación móvil |
| Moneda | Soles (S/) |
| Zona horaria | `America/Lima` |
| Formato de fecha | `DD/MM/YYYY` |

---

## 2. Objetivo

La pantalla de Morosidad permite identificar, consultar y gestionar los préstamos que presentan pagos de intereses semanales vencidos.

Su objetivo principal es proporcionar al Administrador y al Cobrador una vista centralizada de los clientes morosos, permitiendo realizar acciones de seguimiento y cobranza.

La pantalla debe permitir:

- Identificar préstamos con pagos vencidos.
- Conocer la cantidad de semanas vencidas.
- Visualizar el monto de intereses pendientes.
- Identificar morosidad crítica.
- Consultar el historial de pagos.
- Contactar al cliente mediante WhatsApp.
- Registrar un pago.
- Consultar el detalle del préstamo.
- Realizar seguimiento de cobranza.
- Reactivar un préstamo cuando corresponda según las reglas del negocio.
- Mantener trazabilidad de las acciones realizadas.

---

# 3. Acceso a la Pantalla

La pantalla podrá ser accesible desde diferentes puntos de la aplicación.

## 3.1 Desde el menú principal

Ruta:

```text
Dashboard
   └── Morosidad
```

## 3.2 Desde Dashboard

El usuario podrá seleccionar el indicador de préstamos morosos.

```text
Dashboard
   └── Préstamos Morosos
          ↓
      Morosidad
```

## 3.3 Desde Clientes

Desde el detalle de un cliente se podrá consultar si tiene préstamos en situación de morosidad.

```text
Clientes
   └── Cliente
        └── Préstamos
             └── Morosidad
```

## 3.4 Desde Préstamos

Desde el detalle de un préstamo podrá accederse directamente al seguimiento de morosidad.

```text
Préstamos
   └── Detalle del préstamo
          └── Morosidad
```

---

# 4. Estructura General de la Pantalla

La pantalla estará organizada en las siguientes secciones:

```text
┌──────────────────────────────────────┐
│ ← Morosidad                          │
├──────────────────────────────────────┤
│                                      │
│  Resumen                             │
│                                      │
│  [Morosos] [Intereses]               │
│  [Clientes] [Críticos]               │
│                                      │
├──────────────────────────────────────┤
│ 🔍 Buscar morosidad...               │
├──────────────────────────────────────┤
│ Filtros                              │
│ [Todos] [Vencidos] [Críticos]        │
│ [Seguimiento] [Resueltos]            │
├──────────────────────────────────────┤
│                                      │
│ Préstamos en Morosidad               │
│                                      │
│ ┌──────────────────────────────────┐ │
│ │ Juan Pérez                       │ │
│ │ PR-000125                       │ │
│ │ 3 semanas vencidas              │ │
│ │ Interés pendiente: S/ 15.00     │ │
│ │                                  │ │
│ │ [Ver] [Pagar] [WhatsApp]        │ │
│ └──────────────────────────────────┘ │
│                                      │
└──────────────────────────────────────┘
```

---

# 5. Encabezado

El encabezado deberá mostrar:

- Botón regresar.
- Título `Morosidad`.
- Opcionalmente un botón de actualización.
- Opcionalmente un botón de filtros avanzados.

Ejemplo:

```text
←  Morosidad                         ↻
```

El botón de actualización permitirá consultar nuevamente la información desde el backend.

---

# 6. Resumen de Morosidad

La parte superior mostrará indicadores generales.

## 6.1 Total de préstamos morosos

Muestra la cantidad de préstamos que actualmente presentan intereses vencidos.

```text
Préstamos morosos
       12
```

## 6.2 Intereses pendientes

Muestra el monto total de intereses vencidos.

```text
Intereses pendientes
       S/ 180.00
```

## 6.3 Clientes morosos

Muestra la cantidad de clientes que tienen al menos un préstamo con morosidad.

```text
Clientes morosos
       10
```

## 6.4 Morosidad crítica

Muestra la cantidad de préstamos que superan el límite definido para morosidad crítica.

Regla:

> Más de 2 pagos de intereses semanales vencidos.

Ejemplo:

```text
Morosidad crítica
       5
```

---

# 7. Buscador

El usuario podrá buscar préstamos morosos mediante diferentes datos.

## 7.1 Criterios de búsqueda

El buscador permitirá utilizar:

- Nombre del cliente.
- Apellidos del cliente.
- Número de documento.
- Código del préstamo.
- Número de teléfono.

Ejemplo:

```text
🔍 Buscar cliente, documento o préstamo...
```

La búsqueda deberá realizarse de manera controlada para evitar consultas innecesarias al backend.

---

# 8. Filtros

La pantalla tendrá filtros para facilitar la gestión de los casos.

## 8.1 Todos

Muestra todos los registros de morosidad.

```text
[Todos]
```

## 8.2 Vencidos

Muestra préstamos que tienen al menos un pago de interés vencido.

```text
[Vencidos]
```

## 8.3 Críticos

Muestra préstamos que superan las 2 semanas de intereses vencidos.

```text
[Críticos]
```

## 8.4 En seguimiento

Muestra préstamos sobre los cuales se ha registrado una acción de cobranza o seguimiento.

```text
[Seguimiento]
```

## 8.5 Resueltos

Permite consultar casos anteriormente morosos que ya fueron regularizados.

```text
[Resueltos]
```

---

# 9. Tarjeta de Morosidad

Cada préstamo moroso se mostrará mediante una tarjeta.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ Juan Pérez                           │
│ DNI: 12345678                        │
│                                      │
│ Préstamo: PR-000125                  │
│ Capital inicial: S/ 500.00           │
│ Capital pendiente: S/ 500.00         │
│                                      │
│ Interés semanal: S/ 25.00            │
│ Intereses vencidos: S/ 75.00         │
│ Semanas vencidas: 3                  │
│                                      │
│ Último pago: 10/08/2026              │
│ Vencimiento: 31/08/2026              │
│                                      │
│ Estado: CRÍTICO                      │
│                                      │
│ [Ver] [Pagar] [WhatsApp]             │
└──────────────────────────────────────┘
```

---

# 10. Información de la Tarjeta

Cada tarjeta deberá mostrar como mínimo:

### Cliente

- Nombre completo.
- Número de documento.
- Teléfono.

### Préstamo

- Código del préstamo.
- Capital inicial.
- Capital pendiente.
- Interés semanal.
- Fecha de inicio.

### Morosidad

- Número de intereses vencidos.
- Monto de intereses vencidos.
- Fecha del último pago.
- Fecha del último vencimiento.
- Días o semanas de atraso.
- Estado de morosidad.

---

# 11. Estados de Morosidad

La aplicación deberá manejar diferentes estados.

## 11.1 Sin morosidad

El préstamo no presenta pagos vencidos.

```text
SIN MOROSIDAD
```

## 11.2 Vencido

El préstamo presenta al menos un interés semanal vencido.

```text
VENCIDO
```

## 11.3 Crítico

El préstamo presenta más de 2 pagos de intereses vencidos.

```text
CRÍTICO
```

## 11.4 En seguimiento

El préstamo tiene una acción de cobranza registrada.

```text
EN SEGUIMIENTO
```

## 11.5 Resuelto

La situación de morosidad ha sido regularizada.

```text
RESUELTO
```

---

# 12. Regla de Morosidad Crítica

La aplicación deberá considerar como morosidad crítica cuando el cliente tenga:

```text
Cantidad de intereses vencidos > 2
```

Ejemplo:

| Intereses vencidos | Estado |
|---:|---|
| 0 | Sin morosidad |
| 1 | Vencido |
| 2 | Vencido |
| 3 | Crítico |
| 4 | Crítico |
| 5 | Crítico |

Esta regla deberá ser aplicada por el backend.

La aplicación móvil únicamente mostrará el estado recibido desde el servidor.

---

# 13. Cálculo de Intereses Vencidos

El interés semanal corresponde al:

```text
5% del capital inicial
```

Ejemplo:

```text
Capital inicial = S/ 500.00

Interés semanal:
500 × 0.05 = S/ 25.00
```

Si existen 3 intereses semanales vencidos:

```text
25 × 3 = S/ 75.00
```

Por lo tanto:

```text
Intereses vencidos = S/ 75.00
```

El cálculo definitivo deberá realizarlo el backend.

---

# 14. No Capitalización del Interés

Los intereses no deberán incorporarse al capital para generar nuevos intereses.

Ejemplo:

```text
Capital inicial: S/ 500.00
Interés semanal: S/ 25.00
```

Después de varias semanas:

```text
Semana 1 → S/ 25.00
Semana 2 → S/ 25.00
Semana 3 → S/ 25.00
```

Total:

```text
S/ 75.00
```

No se deberá calcular:

```text
S/ 500 + S/ 25 = S/ 525
```

para posteriormente calcular el 5% sobre S/ 525.

---

# 15. Detalle de Morosidad

Al seleccionar un registro se abrirá el detalle de morosidad.

Ejemplo:

```text
┌──────────────────────────────────────┐
│ Detalle de Morosidad                 │
├──────────────────────────────────────┤
│ Cliente                              │
│ Juan Pérez                           │
│ DNI: 12345678                        │
│                                      │
│ Préstamo                             │
│ PR-000125                            │
│                                      │
│ Capital inicial      S/ 500.00       │
│ Capital pendiente    S/ 400.00       │
│ Interés semanal      S/ 25.00        │
│                                      │
│ Intereses vencidos   3              │
│ Interés pendiente    S/ 75.00        │
│                                      │
│ Estado                CRÍTICO        │
│                                      │
│ [Registrar pago]                     │
│ [Contactar por WhatsApp]             │
│ [Ver préstamo]                       │
└──────────────────────────────────────┘
```

---

# 16. Historial de Morosidad

Se deberá mostrar el historial de eventos relacionados con la morosidad.

Ejemplo:

```text
Historial

31/08/2026
Interés semanal vencido
S/ 25.00

24/08/2026
Interés semanal vencido
S/ 25.00

17/08/2026
Interés semanal vencido
S/ 25.00

10/08/2026
Pago registrado
S/ 100.00
```

El historial permitirá conocer la evolución del préstamo.

---

# 17. Historial de Seguimiento

El usuario autorizado podrá registrar acciones realizadas durante la cobranza.

Ejemplos:

- Llamada telefónica.
- Mensaje enviado.
- Contacto mediante WhatsApp.
- Visita.
- Compromiso de pago.
- Observación administrativa.

Ejemplo:

```text
Seguimiento

02/09/2026
WhatsApp enviado
"Recordatorio de pago enviado"

01/09/2026
Llamada telefónica
Cliente indicó que realizará el pago mañana.
```

Cada acción deberá registrar:

- Fecha.
- Hora.
- Usuario.
- Tipo de acción.
- Observación.

---

# 18. Contacto mediante WhatsApp

Desde la pantalla de morosidad se podrá iniciar una comunicación con el cliente.

Botón:

```text
[WhatsApp]
```

El sistema podrá mostrar un mensaje personalizado.

Ejemplo:

```text
Hola Juan Pérez.

Te recordamos que tienes un pago pendiente
correspondiente a tu préstamo PR-000125.

Interés pendiente: S/ 75.00.

Por favor, comunícate con nosotros para
coordinar tu pago.

Gracias.
```

El envío automático deberá utilizar las plantillas configuradas en el módulo de WhatsApp cuando corresponda.

---

# 19. Registro de Pago

Desde la pantalla de morosidad se podrá acceder directamente al registro de pago.

```text
Morosidad
   ↓
Préstamo moroso
   ↓
[Registrar pago]
   ↓
Pantalla de Pagos
```

El pago deberá aplicar las reglas financieras establecidas.

Orden de aplicación:

```text
1. Intereses pendientes
2. Capital
```

Ejemplo:

```text
Interés pendiente: S/ 75.00
Pago realizado:    S/ 100.00

Aplicación:

Intereses: S/ 75.00
Capital:   S/ 25.00
```

---

# 20. Reactivación

Cuando el préstamo se encuentre en una situación que permita su reactivación, se podrá mostrar la acción:

```text
[Reactivar]
```

La reactivación estará condicionada por las reglas de negocio y los permisos del usuario.

El sistema deberá verificar previamente:

- Estado actual del préstamo.
- Historial de morosidad.
- Pagos realizados.
- Intereses pendientes.
- Condiciones de reactivación.
- Permisos del usuario.

La operación deberá quedar registrada en auditoría.

---

# 21. Acciones Disponibles

Cada registro podrá presentar las siguientes acciones:

| Acción | Descripción |
|---|---|
| Ver | Ver detalle de morosidad |
| Préstamo | Abrir detalle del préstamo |
| Cliente | Abrir información del cliente |
| Pagar | Registrar un pago |
| WhatsApp | Contactar al cliente |
| Seguimiento | Registrar acción de cobranza |
| Reactivar | Reactivar cuando corresponda |

Las acciones disponibles dependerán del rol y del estado del préstamo.

---

# 22. Permisos por Rol

## 22.1 Administrador

El Administrador podrá:

- Consultar morosidad.
- Ver detalles.
- Registrar pagos.
- Contactar clientes.
- Registrar seguimiento.
- Reactivar préstamos cuando corresponda.
- Consultar historial.
- Consultar auditoría.

## 22.2 Cobrador

El Cobrador podrá:

- Consultar morosidad.
- Ver detalles.
- Registrar pagos.
- Contactar clientes.
- Registrar seguimiento.

Las acciones administrativas sensibles deberán estar restringidas según los permisos configurados.

---

# 23. Actualización Automática

La morosidad deberá ser controlada automáticamente.

El backend deberá determinar:

- Fecha de vencimiento.
- Intereses vencidos.
- Cantidad de semanas vencidas.
- Monto pendiente.
- Estado de morosidad.
- Morosidad crítica.

La aplicación móvil consultará esta información mediante la API.

---

# 24. Sincronización con Pagos

Cuando se registre un pago, el sistema deberá actualizar automáticamente:

```text
Pago
  ↓
Saldo de intereses
  ↓
Saldo de capital
  ↓
Estado del préstamo
  ↓
Estado de morosidad
  ↓
Notificaciones
```

Ejemplo:

```text
Antes:

Intereses vencidos: 3
Estado: CRÍTICO

Pago registrado

Después:

Intereses vencidos: 0
Estado: SIN MOROSIDAD
```

El estado exacto dependerá de la distribución del pago y de las reglas financieras.

---

# 25. Notificaciones Relacionadas

Cuando corresponda, el sistema podrá generar notificaciones relacionadas con:

- Próximo vencimiento.
- Interés vencido.
- Morosidad.
- Morosidad crítica.
- Pago registrado.
- Regularización del préstamo.

La configuración de horarios de notificación se encuentra definida en el módulo de Notificaciones.

---

# 26. Paginación

Cuando existan muchos préstamos morosos, la información deberá cargarse mediante paginación.

Ejemplo:

```text
Mostrando 1 - 20 de 85

[Anterior]  1  2  3  4  5  [Siguiente]
```

En dispositivos móviles se podrá utilizar carga progresiva:

```text
[Cargar más]
```

---

# 27. Ordenamiento

Los registros podrán ordenarse por:

- Mayor cantidad de semanas vencidas.
- Mayor monto pendiente.
- Fecha de vencimiento más antigua.
- Fecha del último pago.
- Nombre del cliente.
- Estado de morosidad.

Por defecto se recomienda mostrar primero los casos de mayor prioridad.

Orden recomendado:

```text
1. Críticos
2. Mayor cantidad de semanas vencidas
3. Mayor monto pendiente
4. Vencimientos más antiguos
```

---

# 28. Estado de Carga

Mientras se consulta información del backend se mostrará un indicador de carga.

Ejemplo:

```text
┌──────────────────────────────────────┐
│                                      │
│          Cargando morosidad...       │
│                                      │
│              ⟳                       │
│                                      │
└──────────────────────────────────────┘
```

Se recomienda utilizar skeleton loading cuando sea posible.

---

# 29. Estado Sin Registros

Si no existen préstamos morosos:

```text
┌──────────────────────────────────────┐
│                                      │
│              ✓                       │
│                                      │
│      No existen préstamos            │
│          en morosidad                │
│                                      │
│   Todos los pagos están al día.      │
│                                      │
└──────────────────────────────────────┘
```

Este estado deberá diferenciarse de un error de conexión.

---

# 30. Estado de Error

Si ocurre un error al consultar la información:

```text
┌──────────────────────────────────────┐
│                                      │
│              ⚠                       │
│                                      │
│   No fue posible cargar la           │
│   información de morosidad.         │
│                                      │
│           [Reintentar]               │
│                                      │
└──────────────────────────────────────┘
```

No se deberán mostrar datos financieros desactualizados como si fueran actuales sin indicarlo.

---

# 31. Seguridad

La pantalla deberá cumplir las siguientes medidas:

- Requerir autenticación.
- Validar permisos mediante backend.
- Utilizar HTTPS.
- No almacenar información financiera sensible innecesariamente en el dispositivo.
- No confiar exclusivamente en cálculos realizados en la aplicación móvil.
- Validar todas las operaciones en el backend.
- Registrar operaciones importantes en auditoría.
- Controlar las sesiones activas.

---

# 32. Auditoría

Las operaciones importantes relacionadas con morosidad deberán quedar registradas.

Ejemplos:

```text
Consulta de detalle
Registro de seguimiento
Envío de WhatsApp
Registro de pago
Reactivación
Cambio de estado
```

La auditoría deberá incluir:

- Usuario.
- Fecha.
- Hora.
- Acción.
- Préstamo afectado.
- Cliente afectado.
- Resultado de la operación.

---

# 33. Accesibilidad

La pantalla deberá considerar:

- Contraste adecuado.
- Textos legibles.
- Botones suficientemente grandes.
- No depender únicamente del color para representar estados.
- Etiquetas claras.
- Mensajes comprensibles.
- Compatibilidad con lectores de pantalla cuando corresponda.

Ejemplo:

```text
CRÍTICO
```

Además del indicador visual, deberá existir un texto explícito que identifique el estado.

---

# 34. Diseño Responsive

La interfaz deberá adaptarse a diferentes tamaños de dispositivos móviles.

## Pantalla pequeña

```text
┌─────────────────────┐
│ Morosidad            │
├─────────────────────┤
│ Morosos: 12          │
│ Críticos: 5          │
├─────────────────────┤
│ 🔍 Buscar...         │
├─────────────────────┤
│ Juan Pérez           │
│ PR-000125            │
│ 3 semanas vencidas   │
│ S/ 75.00 pendiente   │
│                      │
│ [Ver] [Pagar]        │
└─────────────────────┘
```

## Pantalla de mayor tamaño

Se podrán utilizar tarjetas distribuidas en columnas.

```text
┌──────────────┬──────────────┐
│ Juan Pérez   │ María López  │
│ PR-000125    │ PR-000130    │
│ CRÍTICO      │ VENCIDO      │
└──────────────┴──────────────┘
```

---

# 35. Confirmación de Operaciones

Las acciones que puedan modificar información deberán solicitar confirmación cuando corresponda.

Ejemplo:

```text
¿Registrar este pago?

Cliente:
Juan Pérez

Préstamo:
PR-000125

Monto:
S/ 100.00

Distribución:
Intereses: S/ 75.00
Capital:   S/ 25.00

[Cancelar]    [Confirmar]
```

Para operaciones sensibles como reactivación también deberá solicitarse confirmación.

---

# 36. Prevención de Duplicados

La aplicación deberá evitar que una acción sea ejecutada múltiples veces accidentalmente.

Por ejemplo:

```text
Usuario presiona:

[Confirmar pago]

       ↓

Botón deshabilitado

       ↓

Procesando...

       ↓

Pago registrado
```

El backend deberá contar también con mecanismos de idempotencia o validación para evitar registros duplicados.

---

# 37. Criterios de Aceptación

La pantalla será considerada correctamente implementada cuando:

- [ ] El usuario pueda consultar los préstamos morosos.
- [ ] Se muestre el número total de préstamos morosos.
- [ ] Se muestre el monto de intereses pendientes.
- [ ] Se muestre la cantidad de clientes morosos.
- [ ] Se identifiquen los casos de morosidad crítica.
- [ ] La búsqueda funcione por cliente.
- [ ] La búsqueda funcione por documento.
- [ ] La búsqueda funcione por código de préstamo.
- [ ] La búsqueda funcione por teléfono.
- [ ] Los filtros funcionen correctamente.
- [ ] Se muestre la cantidad de intereses vencidos.
- [ ] Se muestre el monto pendiente.
- [ ] Se pueda acceder al detalle del préstamo.
- [ ] Se pueda registrar un pago.
- [ ] Se pueda contactar al cliente mediante WhatsApp.
- [ ] Se pueda registrar seguimiento.
- [ ] La morosidad crítica se determine cuando existan más de 2 intereses vencidos.
- [ ] Los cálculos financieros sean validados por el backend.
- [ ] La aplicación actualice el estado después de registrar un pago.
- [ ] Las acciones sensibles respeten los permisos del usuario.
- [ ] Las operaciones importantes queden registradas en auditoría.
- [ ] Se controle el estado de carga.
- [ ] Se controle el estado sin registros.
- [ ] Se controle el estado de error.
- [ ] La pantalla sea responsive.
- [ ] La pantalla cumpla criterios básicos de accesibilidad.

---

# 38. Relación con Otras Pantallas

La pantalla de Morosidad tendrá relación con:

```text
Dashboard
    ↓
Morosidad
    ├── Clientes
    ├── Préstamos
    ├── Pagos
    ├── WhatsApp
    ├── Notificaciones
    └── Seguimiento
```

## Navegación principal

```text
Morosidad
    ↓
Detalle de Morosidad
    ├── Ver Cliente
    ├── Ver Préstamo
    ├── Registrar Pago
    ├── WhatsApp
    ├── Registrar Seguimiento
    └── Reactivar
```

---

# 39. Reglas de Negocio Relacionadas

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

Las reglas financieras y de morosidad deberán mantenerse centralizadas en dichos documentos y no duplicarse de manera contradictoria en la interfaz.

---

# 40. Consideraciones Técnicas

La aplicación móvil deberá recibir desde el backend información similar a:

```text
loanId
loanCode
clientId
clientName
documentNumber
phone
initialCapital
remainingCapital
weeklyInterest
overdueInterest
overdueWeeks
lastPaymentDate
dueDate
delinquencyStatus
followUpStatus
```

Los nombres definitivos dependerán del contrato de la API.

La aplicación no deberá determinar por sí misma si un préstamo es moroso utilizando únicamente la fecha local del dispositivo.

El backend será la fuente oficial para:

- Estado del préstamo.
- Cálculo financiero.
- Fechas de vencimiento.
- Intereses vencidos.
- Morosidad.
- Morosidad crítica.
- Saldos.
- Permisos.
- Estado de las operaciones.

---

# 41. Resultado Esperado

La pantalla deberá proporcionar una herramienta rápida para que el Administrador y el Cobrador puedan identificar y gestionar los casos de morosidad.

El flujo principal será:

```text
Consultar morosidad
       ↓
Identificar cliente
       ↓
Revisar préstamo
       ↓
Analizar deuda vencida
       ↓
Contactar cliente
       ↓
Registrar pago
       ↓
Actualizar estado
       ↓
Registrar seguimiento
```

El sistema deberá priorizar los casos críticos y proporcionar información financiera clara para facilitar la gestión de cobranza.

---

# 42. Siguiente Pantalla

La siguiente pantalla a documentar será:

```text
06-Diseno-UX-UI/03-Pantallas/11-Pantalla-Notificaciones.md
```

Esta pantalla documentará la consulta y gestión de las notificaciones de pagos, vencimientos y morosidad.