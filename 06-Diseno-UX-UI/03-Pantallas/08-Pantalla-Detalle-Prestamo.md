# Pantalla de Detalle de Préstamo

**Archivo:** `06-Diseno-UX-UI/03-Pantallas/08-Pantalla-Detalle-Prestamo.md`

---

## 1. Información General

| Elemento | Descripción |
|---|---|
| Nombre | Pantalla de Detalle de Préstamo |
| Identificador | SCR-PRESTAMO-DET-001 |
| Módulo | Préstamos |
| Tipo | Pantalla de detalle |
| Usuarios | Administrador / Cobrador autorizado |
| Acceso | Préstamos → Seleccionar préstamo |
| Pantalla anterior | Pantalla de Préstamos |
| Pantallas relacionadas | Pagos, Morosidad, Cliente, WhatsApp |

---

# 2. Objetivo

La pantalla de Detalle de Préstamo permite consultar toda la información relacionada con una operación de préstamo.

El usuario podrá visualizar:

- Datos del préstamo.
- Información del cliente.
- Información del aval.
- Capital inicial.
- Capital pagado.
- Capital pendiente.
- Interés semanal.
- Intereses pagados.
- Intereses pendientes.
- Fecha de inicio.
- Próximos vencimientos.
- Historial de pagos.
- Pagos atrasados.
- Estado del préstamo.
- Estado de morosidad.
- Observaciones.
- Historial de operaciones.

También permitirá ejecutar acciones autorizadas relacionadas con la cobranza.

---

# 3. Acceso

Se podrá acceder desde:

**Préstamos → Seleccionar préstamo**

También desde:

**Detalle del Cliente → Préstamos → Seleccionar préstamo**

La pantalla deberá recibir el identificador único del préstamo seleccionado.

---

# 4. Estructura General

La pantalla estará organizada en:

1. Encabezado.
2. Estado del préstamo.
3. Información del cliente.
4. Resumen financiero.
5. Próximo pago.
6. Intereses.
7. Capital.
8. Información del aval.
9. Historial de pagos.
10. Morosidad.
11. Observaciones.
12. Historial de operaciones.
13. Acciones.

---

# 5. Encabezado

El encabezado mostrará:

- Botón regresar.
- Título.
- Código del préstamo.
- Menú de acciones, según permisos.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ ←  Préstamo #000125             ⋮   │
    └──────────────────────────────────────┘

El menú podrá contener:

- Editar información permitida.
- Registrar pago.
- Ver cliente.
- Ver morosidad.
- Enviar WhatsApp.
- Actualizar información.

Las opciones dependerán del rol y permisos.

---

# 6. Estado del Préstamo

En la parte superior se mostrará el estado actual.

### Estados

- Activo.
- Atrasado.
- Completado.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ PRÉSTAMO #000125                     │
    │                                      │
    │ Estado: ACTIVO                       │
    └──────────────────────────────────────┘

El estado deberá provenir del backend.

---

# 7. Información Principal

Se mostrará información básica:

- Código.
- Fecha de inicio.
- Estado.
- Frecuencia.
- Tasa de interés.

### Ejemplo

    Código:
    #000125

    Fecha de inicio:
    07/09/2026

    Frecuencia:
    Semanal

    Tasa:
    5%

---

# 8. Información del Cliente

La pantalla mostrará el cliente asociado.

### Datos

- Nombre completo.
- Tipo de documento.
- Número de documento.
- Teléfono.
- Estado del cliente.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ CLIENTE                              │
    ├──────────────────────────────────────┤
    │ Juan Pérez                           │
    │ DNI: 12345678                        │
    │ Teléfono: 987654321                  │
    │ Estado: Activo                       │
    │                                      │
    │ [Ver cliente]                        │
    └──────────────────────────────────────┘

---

# 9. Resumen Financiero

La sección principal mostrará un resumen de la situación financiera.

### Información

- Capital inicial.
- Capital pagado.
- Capital pendiente.
- Interés semanal.
- Interés pagado.
- Interés pendiente.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ RESUMEN FINANCIERO                   │
    ├──────────────────────────────────────┤
    │ Capital inicial       S/ 1,000.00    │
    │ Capital pagado        S/   400.00    │
    │ Capital pendiente     S/   600.00    │
    │                                      │
    │ Interés semanal       S/    50.00    │
    │ Interés pagado        S/   100.00    │
    │ Interés pendiente     S/    50.00    │
    └──────────────────────────────────────┘

---

# 10. Capital Inicial

El capital inicial representa el monto original entregado.

Ejemplo:

    Capital inicial:
    S/ 1,000.00

Este valor permanecerá como referencia para el cálculo del interés semanal.

---

# 11. Capital Pagado

Representa la cantidad de capital que ya fue recuperada mediante los pagos registrados.

Ejemplo:

    Capital inicial: S/ 1,000.00
    Capital pagado:  S/   400.00

---

# 12. Capital Pendiente

Representa el capital que todavía debe ser recuperado.

Ejemplo:

    Capital inicial:   S/ 1,000.00
    Capital pagado:    S/   400.00
    Capital pendiente: S/   600.00

La fórmula conceptual es:

    Capital pendiente =
    Capital inicial - Capital pagado

El valor definitivo deberá ser validado por el backend.

---

# 13. Interés Semanal

La regla inicial establece:

**5% semanal sobre el capital inicial.**

### Ejemplo

    Capital inicial: S/ 1,000.00
    Tasa: 5%

    Interés semanal:
    S/ 50.00

Este interés no depende directamente del capital pendiente.

---

# 14. Interés No Compuesto

Los intereses no se capitalizan.

Esto significa que un interés pendiente no se suma al capital para generar nuevos intereses.

### Ejemplo

    Capital inicial: S/ 1,000.00
    Interés semanal: S/ 50.00

Aunque el capital pendiente disminuya, el interés semanal continuará calculándose sobre el capital inicial según las reglas del sistema.

---

# 15. Interés Pagado

Se mostrará el total de intereses que ya fueron cancelados.

Ejemplo:

    Interés pagado:
    S/ 150.00

El valor deberá considerar los pagos correctamente registrados.

---

# 16. Interés Pendiente

Se mostrará el total de intereses que todavía no han sido cancelados.

Ejemplo:

    Interés pendiente:
    S/ 100.00

El sistema deberá mantener este valor actualizado.

---

# 17. Próximo Pago

La pantalla mostrará la siguiente fecha de pago.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ PRÓXIMO PAGO                         │
    ├──────────────────────────────────────┤
    │ Fecha: 14/09/2026                    │
    │ Interés esperado: S/ 50.00           │
    │ Estado: Pendiente                    │
    └──────────────────────────────────────┘

---

# 18. Fecha de Vencimiento

La fecha de vencimiento deberá calcularse de acuerdo con la frecuencia semanal y las reglas del sistema.

Ejemplo:

    Inicio:
    07/09/2026

    Vencimiento:
    14/09/2026

Las fechas definitivas deberán ser proporcionadas o validadas por el backend.

---

# 19. Historial de Pagos

La pantalla deberá mostrar los pagos realizados.

Cada registro podrá contener:

- Fecha.
- Monto total.
- Monto aplicado a interés.
- Monto aplicado a capital.
- Usuario que registró el pago.
- Número o código de operación.

### Ejemplo

    HISTORIAL DE PAGOS

    14/09/2026
    Total: S/ 100.00
    Interés: S/ 50.00
    Capital: S/ 50.00

    21/09/2026
    Total: S/ 75.00
    Interés: S/ 50.00
    Capital: S/ 25.00

---

# 20. Aplicación de Pagos

El sistema aplicará los pagos en el siguiente orden:

**1. Intereses**

**2. Capital**

### Ejemplo

    Interés pendiente: S/ 50.00
    Capital pendiente: S/ 500.00
    Pago: S/ 100.00

    Aplicación:

    Interés: S/ 50.00
    Capital: S/ 50.00

    Nuevo capital pendiente:
    S/ 450.00

---

# 21. Pagos Parciales

Se permitirán pagos parciales.

### Ejemplo

    Interés pendiente:
    S/ 50.00

    Pago recibido:
    S/ 30.00

    Resultado:

    Interés pagado:
    S/ 30.00

    Interés pendiente:
    S/ 20.00

El saldo deberá mantenerse correctamente.

---

# 22. Pago Superior al Interés

Cuando el pago sea superior al interés pendiente, el excedente se aplicará al capital.

### Ejemplo

    Interés pendiente: S/ 50.00
    Capital pendiente: S/ 500.00

    Pago:
    S/ 200.00

    Aplicación:

    Interés: S/ 50.00
    Capital: S/ 150.00

    Nuevo capital:
    S/ 350.00

---

# 23. Botón Registrar Pago

La pantalla deberá mostrar una acción principal:

**Registrar pago**

Ejemplo:

    ┌──────────────────────────────────────┐
    │                                      │
    │       [ + Registrar pago ]           │
    │                                      │
    └──────────────────────────────────────┘

Al seleccionar esta acción se abrirá la pantalla correspondiente al registro del pago.

---

# 24. Información del Aval

Se mostrará la información del aval asociado.

### Datos

- Nombre completo.
- Teléfono.
- Dirección.
- Tipo de aval.
- Relación con el préstamo.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ AVAL                                 │
    ├──────────────────────────────────────┤
    │ María López                          │
    │ Teléfono: 999888777                  │
    │ Dirección: Arequipa                  │
    │                                      │
    │ [Ver información]                    │
    └──────────────────────────────────────┘

Si el préstamo no tiene aval:

    Aval:
    No registrado

---

# 25. Morosidad

La pantalla deberá mostrar la situación de morosidad del préstamo.

### Información

- Cantidad de pagos atrasados.
- Intereses pendientes.
- Última fecha de pago.
- Días o períodos de atraso.
- Estado de morosidad.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ MOROSIDAD                            │
    ├──────────────────────────────────────┤
    │ Pagos atrasados: 2                   │
    │ Interés pendiente: S/ 100.00         │
    │ Estado: En seguimiento               │
    └──────────────────────────────────────┘

---

# 26. Condición de Morosidad Crítica

El sistema considera una condición crítica cuando existen:

**Más de 2 pagos de interés atrasados.**

La interfaz deberá mostrar una advertencia clara.

### Ejemplo

    ⚠ MOROSIDAD CRÍTICA

    El préstamo presenta más de 2 pagos
    de interés atrasados.

El estado definitivo deberá ser determinado por el backend.

---

# 27. Reactivación

Si el negocio permite reactivar un préstamo o cliente después de una situación de morosidad, la acción deberá estar disponible únicamente para usuarios autorizados.

Ejemplo:

    [ Reactivar ]

Antes de ejecutar la operación deberá solicitarse confirmación.

La reactivación deberá quedar registrada para auditoría.

---

# 28. Observaciones

Se mostrará la información adicional relacionada con el préstamo.

### Ejemplo

    OBSERVACIONES

    Cliente solicita mantener comunicación
    mediante WhatsApp para recordatorios.

El usuario autorizado podrá modificar las observaciones cuando corresponda.

---

# 29. Historial de Operaciones

La pantalla podrá mostrar un historial de eventos.

### Ejemplo

    HISTORIAL

    07/09/2026
    Préstamo registrado
    Usuario: Administrador

    14/09/2026
    Pago registrado
    Monto: S/ 100.00
    Usuario: Cobrador

    21/09/2026
    Pago registrado
    Monto: S/ 75.00
    Usuario: Cobrador

El historial permitirá realizar trazabilidad.

---

# 30. Acciones Disponibles

Según los permisos del usuario se podrán mostrar:

- Registrar pago.
- Ver cliente.
- Ver aval.
- Consultar morosidad.
- Enviar WhatsApp.
- Editar información permitida.
- Actualizar.
- Reactivar, cuando corresponda.

---

# 31. Acción WhatsApp

Cuando el cliente tenga un teléfono válido y el usuario tenga autorización, podrá mostrarse:

**Enviar WhatsApp**

Esta acción permitirá iniciar el flujo de comunicación definido por el módulo de WhatsApp.

Podrán utilizarse plantillas para:

- Recordatorio de pago.
- Pago próximo.
- Pago atrasado.
- Comunicación relacionada con morosidad.

---

# 32. Confirmación de Acciones

Las operaciones sensibles deberán solicitar confirmación.

### Ejemplo

    ¿Deseas registrar este pago?

    Cliente: Juan Pérez
    Préstamo: #000125
    Monto: S/ 100.00

    [Cancelar]      [Confirmar]

---

# 33. Actualización

La pantalla deberá permitir actualizar la información.

Esto será importante después de:

- Registrar un pago.
- Cambiar el estado.
- Actualizar información.
- Registrar una operación.

El usuario podrá utilizar:

- Botón actualizar.
- Pull-to-refresh.

---

# 34. Estado de Carga

Mientras se obtiene información:

    Cargando información del préstamo...

Durante una operación:

    Procesando operación...

Los botones correspondientes deberán deshabilitarse temporalmente para evitar acciones duplicadas.

---

# 35. Estado Sin Historial

Si el préstamo todavía no tiene pagos:

    No existen pagos registrados.

Esto no deberá interpretarse como un error.

---

# 36. Manejo de Errores

Si no se puede cargar la información:

    No fue posible cargar el préstamo.

    [Reintentar]

Si el préstamo ya no existe:

    El préstamo solicitado no está disponible.

La aplicación deberá manejar correctamente los errores provenientes de la API.

---

# 37. Seguridad

La pantalla contiene información financiera y personal.

Por lo tanto:

- Requiere autenticación.
- Requiere autorización.
- Utiliza HTTPS.
- El backend valida las operaciones.
- No se deberán confiar datos financieros enviados desde el dispositivo.
- Las operaciones sensibles deberán registrarse.
- Se deberá evitar exponer información innecesaria.

---

# 38. Auditoría

Las operaciones deberán registrar como mínimo:

- Usuario.
- Fecha.
- Hora.
- Préstamo.
- Tipo de operación.
- Información relevante de la operación.

Ejemplos:

- Registro de préstamo.
- Registro de pago.
- Modificación.
- Reactivación.
- Cambio de estado.

---

# 39. Datos Requeridos

La pantalla deberá recibir información como:

    id
    codigo
    cliente
    aval
    capitalInicial
    capitalPagado
    capitalPendiente
    tasaInteres
    interesSemanal
    interesPagado
    interesPendiente
    fechaInicio
    proximoVencimiento
    estado
    pagosAtrasados
    estadoMorosidad
    observaciones
    historialPagos
    historialOperaciones
    createdAt
    updatedAt

Los nombres definitivos deberán coincidir con los modelos de la API.

---

# 40. Reglas de Negocio

La pantalla deberá respetar:

1. Un cliente puede tener múltiples préstamos.
2. Cada préstamo es una operación independiente.
3. El capital inicial es la base para calcular el interés.
4. La tasa inicial es 5%.
5. El interés es semanal.
6. El interés no es compuesto.
7. Los pagos se aplican primero a intereses.
8. El excedente se aplica al capital.
9. Se permiten pagos parciales.
10. El capital pendiente se actualiza después de los pagos.
11. Un préstamo se completa cuando el capital pendiente llega a cero, según las reglas del sistema.
12. Los pagos atrasados deben ser identificados.
13. Más de 2 pagos de interés atrasados representan una condición crítica.
14. Las reglas financieras son validadas por el backend.

---

# 41. Navegación

### Regresar a Préstamos

    Detalle préstamo
          ↓
       ← Atrás
          ↓
      Préstamos

### Ir al Cliente

    Detalle préstamo
          ↓
      Ver cliente
          ↓
    Detalle cliente

### Registrar Pago

    Detalle préstamo
          ↓
    Registrar pago
          ↓
       Pagos

### Consultar Morosidad

    Detalle préstamo
          ↓
       Morosidad

---

# 42. Flujo Principal

    Préstamos
        ↓
    Seleccionar préstamo
        ↓
    Detalle del préstamo
        ↓
    Consultar información
        ↓
    ┌──────────┬──────────┬──────────┐
    ↓          ↓          ↓          ↓
   Pago     Cliente    Morosidad  WhatsApp
    ↓
    Registrar pago
    ↓
    Confirmar
    ↓
    Backend procesa
    ↓
    Actualizar préstamo
    ↓
    Actualizar saldos
    ↓
    Actualizar morosidad
    ↓
    Actualizar historial

---

# 43. Diseño de Información Financiera

La información financiera deberá tener prioridad visual.

Se recomienda mostrar primero:

1. Capital pendiente.
2. Interés pendiente.
3. Próximo pago.
4. Estado.
5. Pagos atrasados.

Esto permitirá que el cobrador pueda conocer rápidamente la situación del préstamo.

---

# 44. Accesibilidad

La pantalla deberá considerar:

- Texto legible.
- Contraste adecuado.
- Botones suficientemente grandes.
- Iconos comprensibles.
- Estados claramente identificables.
- Mensajes de error descriptivos.
- Compatibilidad con lectores de pantalla cuando corresponda.

Los estados no deberán depender exclusivamente del color.

---

# 45. Criterios de Aceptación

La pantalla se considerará correctamente implementada cuando:

- [ ] Muestra el código del préstamo.
- [ ] Muestra el estado.
- [ ] Muestra la información del cliente.
- [ ] Muestra la información del aval.
- [ ] Muestra el capital inicial.
- [ ] Muestra el capital pagado.
- [ ] Muestra el capital pendiente.
- [ ] Muestra el interés semanal.
- [ ] Muestra los intereses pagados.
- [ ] Muestra los intereses pendientes.
- [ ] Muestra la fecha de inicio.
- [ ] Muestra el próximo vencimiento.
- [ ] Muestra el historial de pagos.
- [ ] Muestra la distribución de cada pago.
- [ ] Permite registrar pagos cuando el usuario tenga permisos.
- [ ] Permite pagos parciales.
- [ ] Aplica primero los pagos a intereses.
- [ ] Aplica el excedente al capital.
- [ ] Muestra información de morosidad.
- [ ] Identifica más de 2 pagos atrasados.
- [ ] Permite consultar al cliente.
- [ ] Permite acceder a WhatsApp cuando corresponda.
- [ ] Muestra observaciones.
- [ ] Muestra historial de operaciones.
- [ ] Maneja estados de carga.
- [ ] Maneja errores.
- [ ] Permite actualizar la información.
- [ ] Respeta permisos.
- [ ] Protege la información financiera.
- [ ] Mantiene trazabilidad de operaciones.

---

# 46. Resultado Esperado

La pantalla de Detalle de Préstamo proporcionará una vista completa de cada operación financiera.

El usuario podrá conocer rápidamente:

- Quién recibió el préstamo.
- Cuánto dinero recibió.
- Cuánto capital ha pagado.
- Cuánto capital falta pagar.
- Cuánto interés corresponde semanalmente.
- Cuánto interés está pendiente.
- Cuándo debe realizarse el próximo pago.
- Cuántos pagos están atrasados.
- Si existe una condición de morosidad.
- Qué pagos se han realizado.
- Cómo fueron aplicados los pagos.
- Quién realizó las operaciones.

La pantalla será el centro de consulta y seguimiento individual de cada préstamo.

---

# 47. Relación con Otras Pantallas

| Pantalla | Relación |
|---|---|
| Préstamos | Lista de préstamos |
| Registro de Préstamo | Creación de la operación |
| Clientes | Información del cliente |
| Detalle de Cliente | Historial del cliente |
| Pagos | Registro y consulta de pagos |
| Morosidad | Seguimiento de atrasos |
| Notificaciones | Recordatorios relacionados |
| WhatsApp | Comunicación con el cliente |

---

# 48. Archivo Siguiente

El siguiente documento será:

`06-Diseno-UX-UI/03-Pantallas/09-Pantalla-Pagos.md`

Este documento definirá la pantalla utilizada para consultar y registrar los pagos de los préstamos.