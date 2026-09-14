# Pantalla de Préstamos

**Archivo:** `06-Diseno-UX-UI/03-Pantallas/06-Pantalla-Prestamos.md`

---

## 1. Información General

| Elemento | Descripción |
|---|---|
| Nombre | Pantalla de Préstamos |
| Identificador | SCR-PRESTAMOS-001 |
| Módulo | Préstamos |
| Tipo | Pantalla de listado |
| Usuarios | Administrador / Cobrador |
| Acceso | Menú principal → Préstamos |
| Pantalla relacionada | Registro de Préstamo |
| Pantalla relacionada | Detalle de Préstamo |

---

## 2. Objetivo

La pantalla de Préstamos permite visualizar, buscar, filtrar y administrar todos los préstamos registrados en el sistema.

Desde esta pantalla el usuario autorizado podrá:

- Consultar préstamos.
- Buscar préstamos por diferentes criterios.
- Filtrar préstamos por estado.
- Visualizar información financiera resumida.
- Identificar préstamos próximos a vencerse.
- Identificar préstamos con pagos atrasados.
- Registrar nuevos préstamos.
- Acceder al detalle de un préstamo.
- Acceder a la información del cliente asociado.
- Registrar pagos.
- Consultar la morosidad asociada al préstamo.

El sistema permite 1 préstamo ACTIVO/MOROSO por cliente (UX_Prestamo_Activo) + N históricos CANCELADO/ANULADO. Vocabulario único `PENDIENTE/ACTIVO/MOROSO/CANCELADO/ANULADO`, código `PR-######`.

---

# 3. Acceso a la Pantalla

El usuario podrá ingresar desde:

**Menú principal → Préstamos**

También podrá acceder desde:

- Dashboard.
- Detalle del cliente.
- Notificaciones de pago.
- Pantalla de morosidad.

### 3.1 Restricción de acceso

El acceso estará controlado mediante autenticación y autorización.

Los usuarios deben iniciar sesión previamente.

Las operaciones disponibles dependerán del rol del usuario.

### Administrador

Puede:

- Consultar préstamos.
- Registrar préstamos.
- Modificar información permitida.
- Consultar detalles.
- Registrar pagos.
- Consultar morosidad.
- Gestionar información financiera según permisos.
- Acceder a información histórica.

### Cobrador

Puede:

- Consultar préstamos.
- Consultar información del cliente.
- Registrar pagos.
- Consultar cuotas/intereses pendientes.
- Consultar morosidad.
- Ejecutar acciones relacionadas con la cobranza.

Las operaciones sensibles deberán ser validadas por el backend.

---

# 4. Estructura General de la Pantalla

La pantalla estará organizada en las siguientes secciones:

1. Encabezado.
2. Resumen de préstamos.
3. Barra de búsqueda.
4. Filtros.
5. Ordenamiento.
6. Lista de préstamos.
7. Acciones rápidas.
8. Navegación inferior.

---

# 5. Encabezado

El encabezado mostrará:

- Título: **Préstamos**
- Botón de búsqueda, si corresponde.
- Botón de actualización.
- Botón para registrar nuevo préstamo.

### Ejemplo visual

    ┌──────────────────────────────────────┐
    │ ←  Préstamos                  +      │
    ├──────────────────────────────────────┤
    │                                      │
    │   Administración de préstamos       │
    │                                      │
    └──────────────────────────────────────┘

El botón `+` permitirá acceder directamente a:

**Registrar nuevo préstamo**

---

# 6. Resumen de Préstamos

Antes del listado se mostrará un resumen general.

### Indicadores

- Total de préstamos.
- Préstamos activos.
- Préstamos completados.
- Préstamos atrasados.

### Ejemplo

    ┌──────────────┬──────────────┐
    │ Total        │ Activos      │
    │    125       │     78       │
    ├──────────────┼──────────────┤
    │ Completados  │ Atrasados    │
    │     35       │     12       │
    └──────────────┴──────────────┘

Los valores deberán obtenerse desde el backend.

No se deberán calcular únicamente con información parcial cargada en el dispositivo.

---

# 7. Barra de Búsqueda

La pantalla tendrá una barra de búsqueda.

### Placeholder

**Buscar préstamo...**

La búsqueda podrá realizarse utilizando:

- Código del préstamo.
- Número de documento del cliente.
- Nombre del cliente.
- Apellido del cliente.
- Número telefónico.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ 🔍 Buscar préstamo...                │
    └──────────────────────────────────────┘

La búsqueda debe actualizar los resultados sin afectar la información financiera almacenada.

---

# 8. Filtros

El usuario podrá filtrar los préstamos.

### Filtros principales

- Todos.
- Activos.
- Completados.
- Atrasados.

### Ejemplo

    [Todos] [Activos] [Completados] [Atrasados]

El filtro seleccionado deberá mostrarse visualmente diferenciado.

---

# 9. Filtros Adicionales

Se podrá implementar un filtro avanzado para:

- Fecha de inicio.
- Fecha de último pago.
- Próximo vencimiento.
- Cliente.
- Estado del préstamo.
- Préstamos con pagos pendientes.
- Préstamos con morosidad.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ Filtros avanzados                    │
    ├──────────────────────────────────────┤
    │ Cliente:        [Seleccionar]        │
    │ Estado:         [Todos ▼]            │
    │ Desde:          [DD/MM/YYYY]         │
    │ Hasta:          [DD/MM/YYYY]         │
    │                                      │
    │          [Aplicar filtros]           │
    └──────────────────────────────────────┘

---

# 10. Ordenamiento

El usuario podrá ordenar los préstamos mediante diferentes criterios.

### Opciones

- Más recientes.
- Más antiguos.
- Mayor capital.
- Menor capital.
- Próximo vencimiento.
- Mayor saldo pendiente.
- Mayor morosidad.

### Ejemplo

    Ordenar por: [Próximo vencimiento ▼]

El ordenamiento será visual y no deberá modificar los datos almacenados.

---

# 11. Lista de Préstamos

Los préstamos se mostrarán mediante tarjetas.

Cada tarjeta representará un préstamo.

### Información mínima

- Código del préstamo.
- Nombre del cliente.
- Documento del cliente.
- Capital inicial.
- Saldo de capital pendiente.
- Interés semanal.
- Próxima fecha de pago.
- Estado.
- Indicador de morosidad.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ PRÉSTAMO #000125                     │
    │ Juan Pérez                           │
    │ DNI: 12345678                        │
    │                                      │
    │ Capital inicial      S/ 1,000.00     │
    │ Capital pendiente    S/   600.00     │
    │ Interés semanal      S/    50.00     │
    │ Próximo pago         07/09/2026      │
    │                                      │
    │ Estado: ACTIVO                       │
    │                                      │
    │ [Ver detalle]        [Registrar pago]│
    └──────────────────────────────────────┘

---

# 12. Identificación del Préstamo

Cada préstamo deberá contar con un identificador único.

### Ejemplo

**PRÉSTAMO #000125**

El identificador permitirá:

- Consultar el préstamo.
- Asociar pagos.
- Asociar notificaciones.
- Consultar historial.
- Realizar seguimiento.

El código no deberá repetirse.

---

# 13. Información del Cliente

Cada préstamo estará asociado a un cliente.

La tarjeta deberá mostrar:

- Nombre completo.
- Número de documento.
- Teléfono, cuando sea necesario.
- Estado del cliente.

Al seleccionar el cliente se podrá acceder a su información.

Un mismo cliente puede tener múltiples préstamos.

---

# 14. Capital Inicial

La pantalla mostrará el capital originalmente entregado al cliente.

### Ejemplo

**Capital inicial: S/ 1,000.00**

Este valor será la base utilizada para calcular el interés semanal.

---

# 15. Interés Semanal

La regla financiera inicial del sistema establece:

**Interés semanal = 5% del capital inicial**

Ejemplo:

    Capital inicial = S/ 1,000.00
    Interés semanal = 5%

    Interés semanal = S/ 50.00

El interés se calcula sobre el capital inicial del préstamo.

No se calculará sobre el saldo de capital restante.

---

# 16. Interés No Compuesto

El sistema utilizará interés no compuesto.

Esto significa que los intereses pendientes no se agregarán al capital para generar nuevos intereses.

### Ejemplo

Préstamo:

    Capital inicial: S/ 1,000.00
    Interés semanal: S/ 50.00

El interés semanal continuará siendo:

    S/ 50.00

Mientras el préstamo permanezca vigente, independientemente de que el capital pendiente disminuya.

---

# 17. Frecuencia de Pago

La frecuencia establecida inicialmente será:

**Semanal**

Si un préstamo comienza un lunes, el siguiente interés semanal tendrá como fecha de referencia el lunes siguiente.

### Ejemplo

    Inicio del préstamo:
    Lunes 07/09/2026

    Primer vencimiento:
    Lunes 14/09/2026

    Segundo vencimiento:
    Lunes 21/09/2026

La generación de fechas deberá realizarse según las reglas definidas por el backend.

---

# 18. Saldo de Capital

La pantalla mostrará el capital pendiente.

### Ejemplo

    Capital inicial:       S/ 1,000.00
    Capital pagado:        S/   400.00
    Capital pendiente:     S/   600.00

El saldo de capital será actualizado únicamente después de registrar correctamente un pago.

---

# 19. Próximo Pago

Cada préstamo activo deberá mostrar la próxima fecha de pago correspondiente.

### Información

- Fecha.
- Interés esperado.
- Estado del pago.

### Ejemplo

    Próximo pago
    07/09/2026

    Interés esperado
    S/ 50.00

---

# 20. Estados del Préstamo

La interfaz deberá manejar como mínimo los siguientes estados:

### 20.1 Activo

El préstamo todavía tiene capital pendiente.

Ejemplo:

    Estado: ACTIVO

---

### 20.2 Completado

El capital pendiente ha sido cancelado.

Ejemplo:

    Estado: COMPLETADO

Cuando el préstamo esté completado no deberá mostrarse como préstamo activo.

---

### 20.3 Atrasado

El préstamo presenta pagos de intereses pendientes después de la fecha establecida.

Ejemplo:

    Estado: ATRASADO

La pantalla deberá proporcionar una indicación visual clara.

---

# 21. Morosidad

La pantalla deberá identificar préstamos relacionados con morosidad.

El sistema llevará un control de los intereses semanales vencidos.

La regla establecida para el proyecto considera una situación crítica cuando existen:

**Más de 2 pagos de interés atrasados.**

Esta condición deberá ser determinada por el backend.

---

# 22. Indicador de Morosidad

Cuando exista morosidad, la tarjeta podrá mostrar:

    ⚠ Pagos atrasados: 2

o:

    ⚠ Morosidad

El usuario podrá acceder al detalle de morosidad.

---

# 23. Acciones de Cada Préstamo

Cada tarjeta podrá ofrecer las siguientes acciones:

### Ver detalle

Abre:

**Pantalla de Detalle de Préstamo**

### Registrar pago

Abre:

**Pantalla de Pagos**

### Ver cliente

Abre:

**Detalle del Cliente**

### WhatsApp

Permite iniciar una acción de comunicación con el cliente cuando el usuario tenga autorización.

### Morosidad

Permite consultar los pagos atrasados asociados.

---

# 24. Botón Nuevo Préstamo

La pantalla tendrá un botón para registrar un préstamo.

### Texto

**Nuevo préstamo**

o:

**+ Nuevo préstamo**

Al seleccionar el botón se abrirá:

`07-Pantalla-Registro-Prestamo.md`

---

# 25. Múltiples Préstamos por Cliente

El sistema permitirá múltiples préstamos asociados a un mismo cliente.

### Ejemplo

    Cliente: Juan Pérez

    Préstamo #000101
    Capital inicial: S/ 500.00
    Estado: Completado

    Préstamo #000125
    Capital inicial: S/ 1,000.00
    Estado: Activo

La pantalla deberá mostrar cada préstamo como una operación independiente.

Los pagos de un préstamo no deberán afectar directamente a otro préstamo del mismo cliente.

---

# 26. Acceso al Detalle

Al seleccionar una tarjeta se abrirá:

**Detalle del Préstamo**

El detalle deberá incluir:

- Información del cliente.
- Capital inicial.
- Capital pendiente.
- Capital pagado.
- Interés semanal.
- Historial de intereses.
- Historial de pagos.
- Próximos pagos.
- Pagos atrasados.
- Estado.
- Fecha de inicio.
- Información del aval.
- Observaciones.
- Historial de operaciones.

---

# 27. Próximos Pagos

La pantalla podrá mostrar una sección de préstamos próximos a vencer.

### Ejemplo

    PRÓXIMOS PAGOS

    Juan Pérez
    S/ 50.00
    Vence: 07/09/2026

    María López
    S/ 75.00
    Vence: 08/09/2026

Esta sección facilita las actividades de cobranza.

---

# 28. Pagos Atrasados

También podrá mostrarse una sección específica para préstamos atrasados.

### Ejemplo

    PAGOS ATRASADOS

    Juan Pérez
    Atrasos: 2 semanas
    Interés pendiente: S/ 100.00

    Carlos Ramos
    Atrasos: 3 semanas
    Interés pendiente: S/ 150.00

Los valores deben ser calculados por las reglas financieras del sistema.

---

# 29. Registro de Pagos

Desde la lista de préstamos se podrá seleccionar:

**Registrar pago**

Antes de registrar el pago, el sistema deberá mostrar:

- Cliente.
- Préstamo.
- Capital inicial.
- Intereses pendientes.
- Capital pendiente.
- Monto ingresado.

La aplicación deberá solicitar confirmación antes de guardar la operación.

---

# 30. Aplicación de los Pagos

La regla de aplicación de pagos será:

**Primero intereses y posteriormente capital.**

### Ejemplo

Interés pendiente:

    S/ 50.00

Capital pendiente:

    S/ 500.00

Pago recibido:

    S/ 100.00

Aplicación:

    Interés: S/ 50.00
    Capital: S/ 50.00

Nuevo capital pendiente:

    S/ 450.00

---

# 31. Pagos Parciales

El sistema permitirá pagos parciales.

Ejemplo:

    Interés pendiente: S/ 50.00
    Pago realizado:     S/ 30.00

Resultado:

    Interés pagado:     S/ 30.00
    Interés pendiente:  S/ 20.00

El sistema deberá mantener correctamente los saldos.

---

# 32. Validaciones Financieras

La interfaz no deberá ser responsable de determinar por sí sola las reglas financieras.

El backend deberá validar:

- Capital inicial.
- Interés semanal.
- Intereses pendientes.
- Capital pendiente.
- Aplicación del pago.
- Estado del préstamo.
- Morosidad.
- Fechas de vencimiento.
- Saldos resultantes.

La aplicación móvil únicamente mostrará los resultados proporcionados por el servidor.

---

# 33. Estados de Carga

Mientras se recupera información del servidor se mostrará un indicador de carga.

### Ejemplo

    ┌──────────────────────────────────────┐
    │                                      │
    │          Cargando préstamos...       │
    │                                      │
    │                ◌                     │
    │                                      │
    └──────────────────────────────────────┘

No se deberán mostrar datos financieros incompletos como si fueran definitivos.

---

# 34. Estado Sin Resultados

Cuando no existan préstamos que coincidan con la búsqueda o filtros:

    ┌──────────────────────────────────────┐
    │                                      │
    │          No se encontraron           │
    │             préstamos                │
    │                                      │
    │       Prueba otro criterio           │
    │          de búsqueda.                │
    │                                      │
    └──────────────────────────────────────┘

Si no existen préstamos registrados, podrá mostrarse:

    No hay préstamos registrados.

    [Registrar préstamo]

---

# 35. Manejo de Errores

Si ocurre un error al consultar los préstamos:

    No fue posible cargar los préstamos.

    [Reintentar]

Los errores técnicos no deberán mostrar información sensible al usuario.

---

# 36. Actualización de Información

La pantalla deberá permitir actualizar los datos mediante:

- Botón de actualizar.
- Pull-to-refresh, si la plataforma lo soporta.

La actualización deberá consultar nuevamente el backend.

Esto permitirá reflejar:

- Nuevos préstamos.
- Nuevos pagos.
- Cambios de estado.
- Cambios de morosidad.
- Cambios de saldo.

---

# 37. Paginación

Si existe una cantidad elevada de préstamos, la aplicación deberá utilizar paginación o carga incremental.

Esto permitirá:

- Reducir consumo de memoria.
- Mejorar tiempos de carga.
- Reducir transferencia de datos.
- Mantener una experiencia fluida.

---

# 38. Responsividad

La pantalla deberá adaptarse a diferentes tamaños de dispositivos móviles.

Debe considerar:

- Teléfonos pequeños.
- Teléfonos medianos.
- Teléfonos grandes.
- Diferentes densidades de pantalla.

La información financiera debe mantenerse legible.

---

# 39. Accesibilidad

La pantalla deberá considerar:

- Contraste adecuado.
- Tamaño de texto legible.
- Áreas táctiles adecuadas.
- Iconos acompañados por texto cuando sea necesario.
- Estados identificables no solamente mediante color.
- Mensajes claros para errores y confirmaciones.

---

# 40. Seguridad

La pantalla manejará información financiera y personal.

Por lo tanto:

- Requiere autenticación.
- Requiere autorización.
- No deberá almacenar información sensible innecesariamente.
- Las solicitudes deberán utilizar HTTPS.
- El backend deberá validar las operaciones.
- Las acciones financieras deberán quedar registradas.
- No se deberán confiar cálculos críticos únicamente al cliente móvil.

---

# 41. Auditoría

Las operaciones importantes deberán poder asociarse con:

- Usuario que realizó la operación.
- Fecha.
- Hora.
- Préstamo afectado.
- Tipo de operación.

Ejemplos:

- Registro de préstamo.
- Modificación autorizada.
- Registro de pago.
- Reactivación.
- Cambio de información financiera.

---

# 42. Componentes de Interfaz

La pantalla estará compuesta por:

### Header

- Título.
- Botón regresar.
- Botón actualizar.
- Botón nuevo préstamo.

### Resumen

- Total.
- Activos.
- Completados.
- Atrasados.

### Búsqueda

- Campo de búsqueda.
- Icono de búsqueda.
- Opción para limpiar.

### Filtros

- Todos.
- Activos.
- Completados.
- Atrasados.
- Filtros avanzados.

### Lista

- Tarjetas de préstamo.
- Estados.
- Información financiera.
- Acciones.

### Navegación

- Dashboard.
- Clientes.
- Préstamos.
- Pagos.
- Más.

---

# 43. Datos Requeridos

Para construir esta pantalla se requerirá como mínimo:

- `id`
- `codigo`
- `clienteId`
- `clienteNombre`
- `clienteDocumento`
- `capitalInicial`
- `capitalPagado`
- `capitalPendiente`
- `porcentajeInteres`
- `interesSemanal`
- `interesPendiente`
- `fechaInicio`
- `proximoVencimiento`
- `pagosAtrasados`
- `estado`
- `tieneMorosidad`
- `createdAt`
- `updatedAt`

Los nombres definitivos de los campos deberán coincidir con los modelos definidos en la API y base de datos.

---

# 44. Reglas de Negocio Aplicadas

La pantalla deberá respetar las siguientes reglas:

1. Un cliente puede tener múltiples préstamos.
2. Cada préstamo debe tener un identificador único.
3. El capital inicial determina el interés semanal.
4. El interés semanal inicial es del 5%.
5. El interés se calcula sobre el capital inicial.
6. El interés no es compuesto.
7. La frecuencia de interés es semanal.
8. Los pagos se aplican primero a intereses.
9. El excedente de un pago se aplica al capital.
10. Se permiten pagos parciales.
11. El capital pendiente se actualiza después de registrar correctamente el pago.
12. Los préstamos completados dejan de considerarse activos.
13. Los pagos atrasados deben ser identificados.
14. Más de 2 pagos de interés atrasados representan una condición crítica de morosidad.
15. Las reglas financieras deben ser validadas por el backend.

---

# 45. Confirmación de Acciones

Las operaciones importantes deberán solicitar confirmación.

### Ejemplo

    ┌──────────────────────────────────────┐
    │ ¿Registrar pago?                     │
    │                                      │
    │ Cliente: Juan Pérez                  │
    │ Préstamo: #000125                    │
    │ Monto: S/ 100.00                     │
    │                                      │
    │ [Cancelar]          [Confirmar]      │
    └──────────────────────────────────────┘

---

# 46. Mensajes de Éxito

Después de una operación exitosa:

    ✓ Operación realizada correctamente.

Ejemplo:

    ✓ Pago registrado correctamente.

La información de la tarjeta deberá actualizarse posteriormente.

---

# 47. Flujo Principal

El flujo principal será:

    Usuario inicia sesión
            ↓
       Dashboard
            ↓
        Préstamos
            ↓
    Consulta de préstamos
            ↓
    Buscar / Filtrar
            ↓
    Seleccionar préstamo
            ↓
     Detalle del préstamo
            ↓
    ┌────────┴─────────┐
    ↓                  ↓
Registrar pago     Ver cliente
    ↓
Confirmar operación
    ↓
Actualizar préstamo
    ↓
Actualizar saldos
    ↓
Actualizar morosidad
    ↓
Actualizar notificaciones

---

# 48. Flujo para Registrar Nuevo Préstamo

    Pantalla Préstamos
            ↓
      Nuevo préstamo
            ↓
    Seleccionar cliente
            ↓
    Ingresar información
            ↓
    Validar datos
            ↓
      Calcular interés
            ↓
       Confirmación
            ↓
    Registrar préstamo
            ↓
    Mostrar resultado
            ↓
    Actualizar listado

---

# 49. Flujo de Consulta de Morosidad

    Pantalla Préstamos
            ↓
    Seleccionar préstamo
            ↓
     Detalle préstamo
            ↓
        Morosidad
            ↓
    Consultar pagos vencidos
            ↓
    Mostrar estado actual

---

# 50. Criterios de Aceptación

La pantalla se considerará correctamente implementada cuando:

- [ ] El usuario autenticado puede acceder a Préstamos.
- [ ] Se muestran los préstamos registrados.
- [ ] Se muestra el código de cada préstamo.
- [ ] Se muestra el cliente asociado.
- [ ] Se muestra el capital inicial.
- [ ] Se muestra el capital pendiente.
- [ ] Se muestra el interés semanal.
- [ ] Se muestra la próxima fecha de pago.
- [ ] Se muestra el estado del préstamo.
- [ ] Se puede buscar un préstamo.
- [ ] Se pueden aplicar filtros.
- [ ] Se puede ordenar el listado.
- [ ] Se puede acceder al detalle.
- [ ] Se puede registrar un nuevo préstamo.
- [ ] Se puede iniciar el registro de un pago.
- [ ] Se identifican préstamos atrasados.
- [ ] Se identifica la morosidad.
- [ ] Se soportan múltiples préstamos por cliente.
- [ ] Los valores financieros provienen del backend.
- [ ] Se muestran correctamente los estados de carga.
- [ ] Se muestran correctamente los estados sin resultados.
- [ ] Se manejan los errores de conexión.
- [ ] Se aplican permisos según el rol.
- [ ] Las operaciones importantes requieren confirmación.
- [ ] Las operaciones financieras quedan disponibles para auditoría.

---

# 51. Resultado Esperado

La pantalla de Préstamos proporcionará una vista centralizada para administrar y supervisar todos los préstamos del sistema.

El usuario podrá identificar rápidamente:

- Quién tiene un préstamo.
- Cuánto recibió.
- Cuánto capital debe.
- Cuánto interés semanal corresponde.
- Cuándo debe realizarse el siguiente pago.
- Qué préstamos están atrasados.
- Qué préstamos presentan morosidad.
- Qué préstamos ya fueron completados.

La pantalla deberá priorizar la **claridad financiera, rapidez de consulta, control de cobranza y consistencia de los datos**.

---

# 52. Relación con Otras Pantallas

| Pantalla | Relación |
|---|---|
| Dashboard | Acceso y resumen general |
| Clientes | Consulta del cliente asociado |
| Registro de Cliente | Alta de nuevos clientes |
| Detalle de Cliente | Consulta de préstamos del cliente |
| Registro de Préstamo | Creación de nuevos préstamos |
| Detalle de Préstamo | Consulta completa del préstamo |
| Pagos | Registro y consulta de pagos |
| Morosidad | Control de pagos atrasados |
| Notificaciones | Recordatorios y alertas |
| WhatsApp | Comunicación con el cliente |

---

# 53. Archivo Siguiente

El siguiente documento de esta sección será:

`06-Diseno-UX-UI/03-Pantallas/07-Pantalla-Registro-Prestamo.md`

Este documento definirá detalladamente la pantalla utilizada para registrar un nuevo préstamo.