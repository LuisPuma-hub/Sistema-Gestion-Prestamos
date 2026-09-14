# 03. Pantalla de Clientes

## 1. Información general

| Campo | Descripción |
|---|---|
| Nombre | Clientes |
| Archivo | `03-Pantalla-Clientes.md` |
| Módulo | Gestión de Clientes |
| Tipo | Listado |
| Prioridad | Alta |
| Acceso | Usuarios autenticados con permisos |
| Plataforma | Aplicación móvil |
| Estado | Diseño |

---

## 2. Objetivo

La pantalla de Clientes permite consultar, buscar, registrar y administrar la información de las personas que forman parte del sistema de gestión de préstamos.

Desde esta pantalla el usuario autorizado podrá:

- Consultar clientes registrados.
- Buscar clientes.
- Filtrar clientes por estado.
- Acceder al detalle de un cliente.
- Registrar nuevos clientes.
- Identificar clientes activos.
- Identificar clientes con observaciones.
- Identificar clientes morosos.

---

## 3. Estructura general

```text
┌─────────────────────────────────────┐
│ ←  Clientes                    🔍   │
├─────────────────────────────────────┤
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ 🔍 Buscar cliente               │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [Todos] [Activos] [Morosos]         │
│                                     │
│ 125 clientes                        │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ Juan Pérez                      │ │
│ │ DNI: 72456321                   │ │
│ │ 📱 987654321                    │ │
│ │ Estado: Activo                  │ │
│ │                         >       │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ María López                     │ │
│ │ DNI: 70123456                   │ │
│ │ 📱 956123456                    │ │
│ │ Estado: Moroso                  │ │
│ │                         >       │ │
│ └─────────────────────────────────┘ │
│                                     │
│                              [+]    │
└─────────────────────────────────────┘
```

---

## 4. Encabezado

El encabezado deberá contener:

- Botón para regresar o abrir el menú.
- Título `Clientes`.
- Acción de búsqueda cuando corresponda.

Ejemplo:

```text
←  Clientes                         🔍
```

---

## 5. Buscador

La pantalla deberá incluir un campo de búsqueda.

```text
┌─────────────────────────────────────┐
│ 🔍 Buscar cliente                   │
└─────────────────────────────────────┘
```

La búsqueda podrá realizarse utilizando:

- Número de documento.
- Nombres.
- Apellidos.
- Número de teléfono.

La búsqueda deberá ejecutarse de manera eficiente sin afectar la experiencia del usuario.

---

## 6. Filtros

Se podrán utilizar filtros para facilitar la consulta.

Filtros principales:

```text
[Todos] [Activos] [Observación] [Morosos]
```

### Todos

Muestra todos los clientes registrados.

### Activos

Muestra únicamente clientes con estado activo.

### Observación

Muestra clientes que tienen alguna observación administrativa.

### Morosos

Muestra clientes que cumplen las condiciones establecidas para morosidad.

---

## 7. Contador de clientes

La pantalla mostrará el número total de resultados.

Ejemplo:

```text
125 clientes
```

Cuando se aplique un filtro:

```text
7 clientes morosos
```

---

## 8. Tarjeta de cliente

Cada cliente será mostrado mediante una tarjeta o elemento de lista.

Ejemplo:

```text
┌─────────────────────────────────┐
│ Juan Pérez                      │
│ DNI: 72456321                   │
│ 📱 987654321                    │
│                                 │
│ Estado: Activo              >  │
└─────────────────────────────────┘
```

La información mínima mostrada será:

- Nombres.
- Apellidos.
- Tipo de documento.
- Número de documento.
- Teléfono.
- Estado.

---

## 9. Estados del cliente

Los clientes podrán presentar diferentes estados.

### 9.1 Activo

Indica que el cliente se encuentra habilitado para operar normalmente.

```text
Estado: Activo
```

### 9.2 Observación

Indica que existe una observación administrativa asociada al cliente.

```text
Estado: Observación
```

### 9.3 Moroso

Indica que el cliente presenta incumplimientos de pago de acuerdo con las reglas del sistema.

```text
Estado: Moroso
```

El estado deberá determinarse de acuerdo con las reglas de negocio y no únicamente mediante una modificación visual desde la aplicación.

---

## 10. Registro de nuevo cliente

La pantalla deberá proporcionar una acción para registrar un nuevo cliente.

Botón:

```text
+ Nuevo cliente
```

En dispositivos móviles podrá representarse mediante un botón flotante:

```text
                 [+]
```

Al seleccionar esta acción se abrirá:

`04-Pantalla-Registro-Cliente.md`

---

## 11. Acceso al detalle

Al seleccionar un cliente de la lista, el sistema deberá abrir su información detallada.

Flujo:

```text
Clientes
   │
   ▼
Seleccionar cliente
   │
   ▼
Detalle del cliente
```

La pantalla de detalle permitirá consultar información adicional, préstamos y pagos asociados.

---

## 12. Información del cliente

Los datos principales gestionados por el sistema serán:

### Identificación

- Tipo de documento.
- Número de documento.
- Nombres.
- Apellidos.

### Contacto

- Número de teléfono.

### Documentación

- Imagen del recibo de agua o electricidad.

### Estado

- Activo.
- Observación.
- Moroso.

### Información administrativa

- Observaciones.

---

## 13. Ordenamiento

La lista podrá permitir ordenar los clientes mediante diferentes criterios.

Opciones posibles:

- Nombre ascendente.
- Nombre descendente.
- Fecha de registro más reciente.
- Fecha de registro más antigua.
- Clientes con morosidad.
- Clientes con préstamos activos.

El criterio seleccionado deberá mantenerse durante la navegación cuando sea conveniente para la experiencia del usuario.

---

## 14. Paginación y carga

Cuando exista una cantidad considerable de clientes, la aplicación deberá evitar cargar todos los registros simultáneamente.

Se recomienda utilizar:

- Paginación.
- Carga incremental.
- Scroll infinito.

Ejemplo:

```text
Mostrando 20 de 125 clientes

      [Cargar más]
```

La estrategia definitiva dependerá de la implementación del backend.

---

## 15. Estado inicial

Cuando existan clientes registrados:

```text
Clientes

125 clientes

[Lista de clientes]
```

---

## 16. Estado sin clientes

Cuando no existan clientes:

```text
        👥

No existen clientes registrados.

Registra el primer cliente para comenzar.

[+ Registrar cliente]
```

---

## 17. Estado sin resultados

Cuando una búsqueda no encuentre coincidencias:

```text
        🔍

No se encontraron clientes.

Prueba con otro nombre,
documento o teléfono.
```

---

## 18. Estado de carga

Mientras se obtienen los clientes:

```text
Cargando clientes...
```

Se podrá utilizar un indicador visual de carga o skeleton loading.

---

## 19. Estado de error

Si existe un problema al obtener los clientes:

```text
No se pudieron cargar los clientes.

[Reintentar]
```

El usuario deberá poder intentar nuevamente la operación.

---

## 20. Acciones sobre un cliente

Dependiendo de los permisos, el usuario podrá:

- Ver detalle.
- Editar información.
- Consultar préstamos.
- Consultar pagos.
- Consultar morosidad.
- Enviar mensaje de WhatsApp.
- Modificar observaciones.

Las acciones disponibles deberán depender del rol y permisos.

---

## 21. Acceso a WhatsApp

Desde el detalle o acciones del cliente se podrá acceder a la funcionalidad de WhatsApp.

Flujo:

```text
Clientes
   │
   ▼
Seleccionar cliente
   │
   ▼
Detalle
   │
   ▼
WhatsApp
   │
   ▼
Seleccionar plantilla
   │
   ▼
Enviar mensaje
```

Antes de enviar un mensaje se deberá validar que el cliente tenga un número de teléfono válido.

---

## 22. Acceso a préstamos

Desde un cliente se podrá consultar sus préstamos asociados.

Esto es especialmente importante porque el sistema permite que un cliente pueda tener múltiples préstamos.

Flujo:

```text
Cliente
   │
   ▼
Préstamos del cliente
   │
   ├── Préstamo 001
   ├── Préstamo 002
   └── Préstamo 003
```

Cada préstamo deberá poder abrirse para consultar su detalle.

---

## 23. Acceso a pagos

El usuario autorizado podrá consultar los pagos asociados al cliente.

La información podrá incluir:

- Fecha.
- Monto.
- Tipo de pago.
- Interés pagado.
- Capital pagado.
- Saldo pendiente.

---

## 24. Integración con morosidad

La pantalla deberá identificar visualmente los clientes que presentan morosidad.

La condición deberá utilizar la lógica definida en:

`02-Reglas-Negocio/04-Reglas-Morosidad.md`

La interfaz no deberá determinar por sí sola si un cliente es moroso.

El backend deberá proporcionar el estado correspondiente.

---

## 25. Permisos

| Acción | Administrador | Cobrador |
|---|---:|---:|
| Ver clientes | Sí | Sí |
| Buscar clientes | Sí | Sí |
| Filtrar clientes | Sí | Sí |
| Registrar cliente | Sí | Según permiso |
| Editar cliente | Sí | Según permiso |
| Ver detalle | Sí | Sí |
| Ver préstamos | Sí | Sí |
| Ver pagos | Sí | Sí |
| Ver morosidad | Sí | Sí |
| Enviar WhatsApp | Sí | Según permiso |

La matriz definitiva deberá coincidir con la documentación de autorización del sistema.

---

## 26. Componentes funcionales

La pantalla podrá estructurarse mediante:

```text
ClientsScreen
│
├── ClientsHeader
│
├── ClientSearch
│
├── ClientFilters
│   ├── AllFilter
│   ├── ActiveFilter
│   ├── ObservationFilter
│   └── DelinquentFilter
│
├── ClientCounter
│
├── ClientList
│   └── ClientCard
│
├── EmptyState
│
├── ErrorState
│
└── AddClientButton
```

---

## 27. Datos requeridos

Cada elemento de la lista podrá requerir:

```text
ClientSummary

- id
- documentType
- documentNumber
- firstNames
- lastNames
- phone
- status
- hasActiveLoans
- hasOverduePayments
- createdAt
```

Los nombres definitivos dependerán del contrato de la API.

---

## 28. Seguridad

La pantalla deberá:

- Requerir autenticación.
- Validar el token de sesión.
- Aplicar autorización según rol.
- Evitar mostrar información a usuarios no autorizados.
- Proteger los datos personales.
- Utilizar HTTPS.
- Evitar almacenar información sensible innecesariamente.

---

## 29. Accesibilidad

La interfaz deberá:

- Utilizar textos legibles.
- Mantener contraste adecuado.
- Proporcionar etiquetas para iconos.
- Utilizar tamaños táctiles adecuados.
- Permitir navegación mediante tecnologías de asistencia.
- No depender exclusivamente del color para identificar estados.

---

## 30. Diseño responsive

La lista deberá adaptarse a diferentes tamaños de pantalla.

En dispositivos pequeños:

```text
┌───────────────────────┐
│ ← Clientes            │
├───────────────────────┤
│ 🔍 Buscar cliente     │
├───────────────────────┤
│ [Todos] [Activos]     │
├───────────────────────┤
│ Juan Pérez            │
│ DNI: 72456321         │
│ 📱 987654321           │
│ Estado: Activo        │
├───────────────────────┤
│ María López           │
│ DNI: 70123456         │
│ 📱 956123456           │
│ Estado: Moroso        │
└───────────────────────┘
```

---

## 31. Criterios de aceptación

La pantalla será considerada terminada cuando:

- [ ] Se pueda acceder al módulo Clientes.
- [ ] Se muestre la lista de clientes.
- [ ] Se muestre información básica de cada cliente.
- [ ] Se pueda buscar por nombre.
- [ ] Se pueda buscar por apellido.
- [ ] Se pueda buscar por documento.
- [ ] Se pueda buscar por teléfono.
- [ ] Se puedan filtrar clientes.
- [ ] Se pueda visualizar el estado del cliente.
- [ ] Se pueda registrar un nuevo cliente.
- [ ] Se pueda acceder al detalle.
- [ ] Se puedan consultar los préstamos del cliente.
- [ ] Se puedan consultar los pagos del cliente.
- [ ] Se pueda identificar la morosidad.
- [ ] Se controle el acceso según permisos.
- [ ] Se gestione el estado de carga.
- [ ] Se gestione el estado sin datos.
- [ ] Se gestione el estado sin resultados.
- [ ] Se gestione el error de conexión.
- [ ] La pantalla sea responsive.
- [ ] Se protejan los datos personales.

---

## 32. Resultado esperado

La pantalla de Clientes debe proporcionar una gestión centralizada y eficiente de las personas registradas en el sistema.

El usuario autorizado deberá poder localizar rápidamente un cliente, consultar su información, identificar su estado y acceder a sus préstamos, pagos, morosidad y funcionalidades relacionadas.

La pantalla constituye el punto de entrada principal para la administración de la información de los clientes.