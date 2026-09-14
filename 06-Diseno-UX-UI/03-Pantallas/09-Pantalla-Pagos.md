# Pantalla de Pagos

**Archivo:** `06-Diseno-UX-UI/03-Pantallas/09-Pantalla-Pagos.md`

---

# 1. Información General

| Elemento | Descripción |
|---|---|
| Nombre | Pantalla de Pagos |
| Identificador | SCR-PAGOS-001 |
| Módulo | Pagos |
| Tipo | Listado y registro de pagos |
| Usuarios | Administrador / Cobrador autorizado |
| Acceso | Menú principal → Pagos |
| Pantalla relacionada | Detalle de Préstamo |
| Pantalla relacionada | Morosidad |
| Pantalla relacionada | Cliente |

---

# 2. Objetivo

La pantalla de Pagos permitirá consultar y registrar los pagos realizados por los clientes sobre sus préstamos.

El usuario podrá:

- Consultar pagos registrados.
- Buscar pagos.
- Filtrar pagos.
- Consultar pagos recientes.
- Identificar pagos pendientes.
- Registrar un nuevo pago.
- Seleccionar el préstamo asociado.
- Consultar intereses pendientes.
- Consultar capital pendiente.
- Visualizar cómo se aplicó un pago.
- Consultar el historial de pagos.
- Generar o consultar comprobantes cuando corresponda.

---

# 3. Acceso

Se podrá acceder desde:

**Menú principal → Pagos**

También desde:

**Préstamos → Detalle del préstamo → Registrar pago**

y:

**Clientes → Detalle del cliente → Préstamo → Registrar pago**

---

# 4. Estructura General

La pantalla estará organizada en:

1. Encabezado.
2. Resumen de pagos.
3. Barra de búsqueda.
4. Filtros.
5. Lista de pagos.
6. Botón registrar pago.
7. Estados de pago.
8. Detalle de pago.

---

# 5. Encabezado

El encabezado mostrará:

- Botón regresar.
- Título: **Pagos**
- Botón actualizar.
- Botón nuevo pago.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ ←  Pagos                       +     │
    └──────────────────────────────────────┘

El botón `+` permitirá iniciar el registro de un nuevo pago.

---

# 6. Resumen de Pagos

Se mostrará un resumen de las operaciones.

### Indicadores

- Pagos registrados.
- Monto cobrado.
- Intereses cobrados.
- Capital recuperado.

### Ejemplo

    ┌──────────────┬──────────────┐
    │ Pagos        │ Cobrado      │
    │    125       │ S/ 8,500.00  │
    ├──────────────┼──────────────┤
    │ Intereses    │ Capital      │
    │ S/ 2,500.00  │ S/ 6,000.00  │
    └──────────────┴──────────────┘

Los valores deberán obtenerse desde el backend.

---

# 7. Barra de Búsqueda

La pantalla tendrá:

**Buscar pago...**

La búsqueda podrá utilizar:

- Código del pago.
- Código del préstamo.
- Nombre del cliente.
- Documento del cliente.
- Teléfono.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ 🔍 Buscar pago...                    │
    └──────────────────────────────────────┘

---

# 8. Filtros

Los pagos podrán filtrarse por:

- Todos.
- Hoy.
- Esta semana.
- Este mes.
- Rango de fechas.
- Cliente.
- Préstamo.
- Usuario que registró el pago.

### Ejemplo

    [Todos] [Hoy] [Semana] [Mes]

---

# 9. Filtros Avanzados

Se podrá implementar un panel de filtros.

### Campos

- Fecha desde.
- Fecha hasta.
- Cliente.
- Préstamo.
- Usuario.
- Tipo de aplicación.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ FILTROS                              │
    ├──────────────────────────────────────┤
    │ Desde: [DD/MM/YYYY]                  │
    │ Hasta: [DD/MM/YYYY]                  │
    │ Cliente: [Seleccionar]               │
    │ Préstamo: [Seleccionar]              │
    │                                      │
    │        [Aplicar filtros]             │
    └──────────────────────────────────────┘

---

# 10. Lista de Pagos

Los pagos se mostrarán mediante tarjetas.

Cada tarjeta deberá mostrar información resumida.

### Información

- Código del pago.
- Cliente.
- Préstamo.
- Fecha.
- Monto total.
- Interés aplicado.
- Capital aplicado.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ PAGO #000456                         │
    │ Juan Pérez                           │
    │ Préstamo #000125                     │
    │                                      │
    │ Fecha: 14/09/2026                    │
    │ Total: S/ 100.00                     │
    │ Interés: S/ 50.00                    │
    │ Capital: S/ 50.00                    │
    │                                      │
    │ [Ver detalle]                        │
    └──────────────────────────────────────┘

---

# 11. Código del Pago

Cada pago deberá tener un identificador único.

### Ejemplo

**PAGO #000456**

Este identificador permitirá:

- Consultar el pago.
- Asociarlo al préstamo.
- Asociarlo al cliente.
- Consultar auditoría.
- Identificar el comprobante.

---

# 12. Fecha del Pago

Cada operación deberá registrar:

- Fecha.
- Hora.

### Ejemplo

    Fecha:
    14/09/2026

    Hora:
    09:35

La fecha y hora definitivas de la operación deberán registrarse de forma confiable en el backend.

---

# 13. Monto Total

El monto ingresado por el usuario representa el pago total recibido.

### Ejemplo

    Monto recibido:
    S/ 100.00

El sistema deberá validar que:

- Sea numérico.
- Sea mayor que cero.
- Cumpla las reglas establecidas.

---

# 14. Aplicación del Pago

Los pagos deberán aplicarse siguiendo la regla:

**Primero intereses y después capital.**

### Ejemplo

    Interés pendiente: S/ 50.00
    Capital pendiente: S/ 500.00

    Pago:
    S/ 100.00

    Aplicación:

    Interés:
    S/ 50.00

    Capital:
    S/ 50.00

---

# 15. Pago Parcial

Se permitirán pagos parciales.

### Ejemplo

    Interés pendiente:
    S/ 50.00

    Pago:
    S/ 30.00

    Aplicación:

    Interés:
    S/ 30.00

    Interés restante:
    S/ 20.00

El sistema deberá mantener correctamente el saldo pendiente.

---

# 16. Pago Superior al Interés

Cuando el pago supere los intereses pendientes, el excedente se aplicará al capital.

### Ejemplo

    Interés pendiente: S/ 50.00
    Capital pendiente: S/ 500.00

    Pago recibido:
    S/ 200.00

    Interés aplicado:
    S/ 50.00

    Capital aplicado:
    S/ 150.00

    Capital pendiente:
    S/ 350.00

---

# 17. Selección del Préstamo

Para registrar un pago, el usuario deberá seleccionar el préstamo correspondiente.

### Campo

**Préstamo**

El selector permitirá buscar por:

- Código del préstamo.
- Nombre del cliente.
- DNI.
- CE.
- Teléfono.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ Préstamo *                           │
    │ [ 🔍 Seleccionar préstamo        ▼ ] │
    └──────────────────────────────────────┘

---

# 18. Información del Préstamo Seleccionado

Después de seleccionar el préstamo se mostrará:

- Cliente.
- Capital inicial.
- Capital pendiente.
- Interés semanal.
- Interés pendiente.
- Próximo vencimiento.
- Pagos atrasados.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ PRÉSTAMO #000125                     │
    │                                      │
    │ Cliente: Juan Pérez                  │
    │ Capital inicial: S/ 1,000.00         │
    │ Capital pendiente: S/ 600.00         │
    │ Interés semanal: S/ 50.00            │
    │ Interés pendiente: S/ 50.00          │
    │                                      │
    │ Próximo vencimiento: 14/09/2026      │
    └──────────────────────────────────────┘

---

# 19. Registro de Nuevo Pago

El flujo de registro será:

1. Seleccionar préstamo.
2. Consultar información financiera.
3. Ingresar monto.
4. Calcular aplicación.
5. Revisar distribución.
6. Confirmar.
7. Registrar pago.
8. Actualizar saldos.

---

# 20. Formulario de Pago

El formulario deberá contener:

- Préstamo.
- Fecha.
- Monto.
- Observaciones, si corresponde.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ REGISTRAR PAGO                       │
    ├──────────────────────────────────────┤
    │ Préstamo *                           │
    │ [ #000125                       ▼ ]  │
    │                                      │
    │ Fecha *                              │
    │ [ 14/09/2026 ]                       │
    │                                      │
    │ Monto *                              │
    │ [ S/ 100.00 ]                        │
    │                                      │
    │ Observaciones                        │
    │ [_______________________________]    │
    └──────────────────────────────────────┘

---

# 21. Distribución del Pago

Después de ingresar el monto, el sistema mostrará cómo será aplicado.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ DISTRIBUCIÓN DEL PAGO                │
    ├──────────────────────────────────────┤
    │ Monto recibido:      S/ 100.00       │
    │                                      │
    │ Interés:             S/ 50.00        │
    │ Capital:             S/ 50.00        │
    │                                      │
    │ Total aplicado:      S/ 100.00       │
    └──────────────────────────────────────┘

Esta información deberá estar disponible antes de confirmar.

---

# 22. Intereses Pendientes

El sistema deberá identificar los intereses pendientes antes de aplicar el pago.

### Ejemplo

    Interés pendiente:
    S/ 100.00

    Pago recibido:
    S/ 75.00

    Aplicación:

    Interés:
    S/ 75.00

    Capital:
    S/ 0.00

    Interés restante:
    S/ 25.00

---

# 23. Aplicación de Pagos Atrasados

Si existen varios intereses atrasados, el backend deberá determinar cómo se aplicará el pago según las reglas financieras.

La aplicación deberá mostrar al usuario la distribución resultante.

Ejemplo:

    Intereses pendientes:
    S/ 150.00

    Pago:
    S/ 100.00

    Aplicado a intereses:
    S/ 100.00

    Capital:
    S/ 0.00

    Intereses restantes:
    S/ 50.00

---

# 24. Confirmación

Antes de registrar el pago se mostrará una confirmación.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ CONFIRMAR PAGO                       │
    ├──────────────────────────────────────┤
    │ Cliente: Juan Pérez                  │
    │ Préstamo: #000125                    │
    │                                      │
    │ Pago: S/ 100.00                      │
    │ Interés: S/ 50.00                    │
    │ Capital: S/ 50.00                    │
    │                                      │
    │ ¿Deseas registrar el pago?           │
    │                                      │
    │ [Cancelar]      [Confirmar]          │
    └──────────────────────────────────────┘

---

# 25. Registro de la Operación

Después de confirmar:

    Aplicación
        ↓
    Validación local
        ↓
    Envío al backend
        ↓
    Validación financiera
        ↓
    Registro del pago
        ↓
    Actualización del préstamo
        ↓
    Actualización de morosidad
        ↓
    Registro de auditoría
        ↓
    Resultado

---

# 26. Estado de Carga

Durante el registro:

    Registrando pago...

El botón de confirmación deberá quedar deshabilitado temporalmente.

Esto evita múltiples registros accidentales.

---

# 27. Registro Exitoso

Después de registrar:

    ✓ Pago registrado correctamente.

La pantalla podrá mostrar:

- Código del pago.
- Monto.
- Interés aplicado.
- Capital aplicado.
- Nuevo saldo.
- Próximo vencimiento.

---

# 28. Nuevo Saldo

Después de registrar un pago deberá actualizarse la información.

### Ejemplo

    Antes:

    Capital pendiente:
    S/ 600.00

    Pago aplicado a capital:
    S/ 50.00

    Después:

    Capital pendiente:
    S/ 550.00

---

# 29. Estado del Pago

El sistema podrá manejar estados como:

- Registrado.
- Procesando.
- Anulado, si la lógica del negocio posteriormente permite anulaciones.

La anulación de una operación financiera deberá requerir permisos especiales y quedar registrada en auditoría.

---

# 30. Historial de Pagos

La lista permitirá consultar las operaciones históricas.

### Ejemplo

    PAGO #000456
    14/09/2026
    Juan Pérez
    S/ 100.00

    PAGO #000489
    21/09/2026
    Juan Pérez
    S/ 75.00

---

# 31. Detalle de un Pago

Al seleccionar un pago se mostrará:

- Código.
- Fecha.
- Hora.
- Cliente.
- Préstamo.
- Monto total.
- Interés aplicado.
- Capital aplicado.
- Usuario que registró.
- Observaciones.
- Estado.

---

# 32. Comprobante

El sistema podrá generar o mostrar un comprobante del pago.

El comprobante podrá contener:

- Nombre del sistema.
- Código del pago.
- Fecha.
- Cliente.
- Documento.
- Código del préstamo.
- Monto recibido.
- Interés aplicado.
- Capital aplicado.
- Saldo pendiente.
- Usuario responsable.

La generación del comprobante deberá respetar el diseño definido para documentos del sistema.

---

# 33. Envío del Comprobante

Cuando corresponda, el sistema podrá permitir:

- Compartir comprobante.
- Enviar por WhatsApp.
- Guardar comprobante.
- Visualizar comprobante.

Estas acciones dependerán de las integraciones disponibles y de los permisos del usuario.

---

# 34. Morosidad Después del Pago

Después de registrar un pago, el sistema deberá recalcular la situación de morosidad.

Ejemplo:

    Antes:
    Pagos atrasados: 3

    Pago registrado:
    S/ 50.00

    Después:
    Pagos atrasados: 2

La condición definitiva deberá determinarse mediante las reglas del backend.

---

# 35. Notificaciones Después del Pago

Cuando corresponda, el registro de un pago podrá provocar:

- Actualización de recordatorios.
- Actualización de próximos vencimientos.
- Cambio del estado de morosidad.
- Actualización de notificaciones.

El backend será responsable de determinar las acciones necesarias.

---

# 36. Búsqueda de Pagos

El usuario podrá buscar pagos mediante:

- Código.
- Cliente.
- Documento.
- Préstamo.
- Fecha.

Los resultados deberán actualizarse según los criterios introducidos.

---

# 37. Ordenamiento

Los pagos podrán ordenarse por:

- Más recientes.
- Más antiguos.
- Mayor monto.
- Menor monto.

### Ejemplo

    Ordenar:
    [Más recientes ▼]

---

# 38. Estado Sin Resultados

Cuando no existan resultados:

    No se encontraron pagos.

    Prueba con otro criterio de búsqueda
    o modifica los filtros.

---

# 39. Estado Sin Pagos

Cuando todavía no existan pagos registrados:

    No hay pagos registrados.

    [Registrar pago]

---

# 40. Manejo de Errores

Si ocurre un error:

    No fue posible cargar los pagos.

    [Reintentar]

Si falla el registro:

    No fue posible registrar el pago.

    Verifica la información e inténtalo
    nuevamente.

No deberán mostrarse detalles técnicos innecesarios.

---

# 41. Validaciones

Antes de registrar un pago se deberá validar:

### Préstamo

- Debe existir.
- Debe estar disponible.
- Debe pertenecer al cliente seleccionado.

### Monto

- Debe ser obligatorio.
- Debe ser numérico.
- Debe ser mayor que cero.

### Fecha

- Debe ser válida.
- Debe cumplir las reglas del sistema.

### Usuario

- Debe estar autenticado.
- Debe tener permisos.

---

# 42. Validación Financiera

El backend deberá validar:

- Intereses pendientes.
- Capital pendiente.
- Monto recibido.
- Distribución del pago.
- Estado del préstamo.
- Morosidad.
- Saldos posteriores.

La aplicación no deberá modificar directamente los saldos financieros.

---

# 43. Seguridad

La pantalla manejará información financiera.

Por lo tanto:

- Requiere autenticación.
- Requiere autorización.
- Utiliza HTTPS.
- El backend valida operaciones.
- Se registra auditoría.
- Se evita almacenar información sensible innecesariamente.
- Se evita el doble registro de pagos.

---

# 44. Auditoría

Cada pago registrado deberá quedar asociado a:

- Usuario.
- Fecha.
- Hora.
- Cliente.
- Préstamo.
- Monto.
- Aplicación a interés.
- Aplicación a capital.

Esto permitirá conocer quién realizó cada operación.

---

# 45. Permisos

### Administrador

Puede:

- Consultar pagos.
- Registrar pagos.
- Consultar detalles.
- Consultar historial.
- Acceder a comprobantes.
- Ejecutar acciones autorizadas.

### Cobrador

Puede:

- Consultar pagos.
- Registrar pagos si tiene permiso.
- Consultar historial.
- Consultar comprobantes según autorización.

Las restricciones deberán aplicarse también en backend.

---

# 46. Datos Requeridos

La pantalla podrá utilizar información como:

    id
    codigo
    prestamoId
    clienteId
    clienteNombre
    clienteDocumento
    fecha
    hora
    montoTotal
    montoInteres
    montoCapital
    observaciones
    estado
    usuarioId
    usuarioNombre
    createdAt
    updatedAt

Los nombres definitivos deberán coincidir con los modelos de la API.

---

# 47. Reglas de Negocio

La pantalla deberá cumplir:

1. Cada pago pertenece a un préstamo.
2. Cada préstamo pertenece a un cliente.
3. El pago debe registrarse con fecha y hora.
4. Los intereses se aplican antes que el capital.
5. Se permiten pagos parciales.
6. El excedente después de cubrir intereses se aplica al capital.
7. Los intereses no se capitalizan.
8. El interés semanal se calcula sobre el capital inicial.
9. La tasa inicial es 5%.
10. La frecuencia inicial es semanal.
11. Los pagos actualizan los saldos.
12. Los pagos pueden modificar la situación de morosidad.
13. Las operaciones financieras deben ser validadas por backend.
14. Los pagos registrados deben ser auditables.

---

# 48. Flujo Principal

    Pagos
      ↓
    Nuevo pago
      ↓
    Seleccionar préstamo
      ↓
    Consultar información
      ↓
    Ingresar monto
      ↓
    Calcular distribución
      ↓
    Revisar
      ↓
    Confirmar
      ↓
    Backend
      ↓
    Registrar pago
      ↓
    Actualizar préstamo
      ↓
    Actualizar morosidad
      ↓
    Actualizar notificaciones
      ↓
    Mostrar resultado

---

# 49. Flujo Desde el Detalle del Préstamo

    Detalle del préstamo
            ↓
       Registrar pago
            ↓
      Formulario pago
            ↓
      Ingresar monto
            ↓
    Mostrar distribución
            ↓
        Confirmar
            ↓
       Registrar pago
            ↓
      Actualizar detalle

---

# 50. Accesibilidad

La pantalla deberá considerar:

- Texto legible.
- Contraste adecuado.
- Botones suficientemente grandes.
- Campos claramente identificados.
- Mensajes de validación visibles.
- Estados comprensibles.
- Navegación lógica.
- Compatibilidad con lectores de pantalla cuando corresponda.

---

# 51. Responsividad

La interfaz deberá adaptarse a:

- Teléfonos pequeños.
- Teléfonos medianos.
- Teléfonos grandes.
- Diferentes densidades de pantalla.

Los valores monetarios deberán permanecer completamente visibles.

---

# 52. Criterios de Aceptación

La pantalla se considerará correctamente implementada cuando:

- [ ] Permite consultar pagos.
- [ ] Permite buscar pagos.
- [ ] Permite filtrar pagos.
- [ ] Muestra el resumen de pagos.
- [ ] Muestra pagos recientes.
- [ ] Permite seleccionar un préstamo.
- [ ] Muestra información financiera del préstamo.
- [ ] Permite ingresar un monto.
- [ ] Valida que el monto sea mayor que cero.
- [ ] Permite pagos parciales.
- [ ] Calcula la distribución del pago.
- [ ] Aplica primero a intereses.
- [ ] Aplica excedentes a capital.
- [ ] Permite confirmar antes de registrar.
- [ ] Evita registros duplicados.
- [ ] Actualiza el saldo después del pago.
- [ ] Actualiza la morosidad.
- [ ] Actualiza información relacionada.
- [ ] Muestra el historial.
- [ ] Permite consultar el detalle del pago.
- [ ] Permite consultar comprobantes cuando corresponda.
- [ ] Maneja estados de carga.
- [ ] Maneja errores.
- [ ] Respeta permisos.
- [ ] Registra auditoría.
- [ ] Protege la información financiera.

---

# 53. Resultado Esperado

La pantalla de Pagos permitirá gestionar de forma segura y controlada las operaciones de cobranza.

El usuario podrá conocer:

- Cuánto pagó el cliente.
- Cuándo realizó el pago.
- Cuánto se aplicó a intereses.
- Cuánto se aplicó a capital.
- Cuánto capital queda pendiente.
- Cuánto interés queda pendiente.
- Qué pagos están atrasados.
- Cómo cambió la situación del préstamo después del pago.

La pantalla deberá garantizar que cada pago esté correctamente asociado con su préstamo y cliente, manteniendo la integridad de las reglas financieras y la trazabilidad de las operaciones.

---

# 54. Relación con Otras Pantallas

| Pantalla | Relación |
|---|---|
| Dashboard | Resumen de cobranza |
| Préstamos | Acceso a los préstamos |
| Registro de Préstamo | Crea la operación sobre la cual se realizarán pagos |
| Detalle de Préstamo | Consulta y registro de pagos |
| Clientes | Consulta del cliente |
| Morosidad | Control de atrasos |
| Notificaciones | Recordatorios de pago |
| WhatsApp | Comunicación y envío de información |

---

# 55. Archivo Siguiente

El siguiente documento será:

`06-Diseno-UX-UI/03-Pantallas/10-Pantalla-Morosidad.md`

Este documento definirá la pantalla para controlar los préstamos con intereses atrasados, identificar clientes morosos y gestionar el seguimiento de cobranza.