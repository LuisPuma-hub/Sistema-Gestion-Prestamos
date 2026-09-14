# 02. Pantalla de Dashboard

## 1. Información general

| Campo | Descripción |
|---|---|
| Nombre | Dashboard |
| Archivo | `02-Pantalla-Dashboard.md` |
| Módulo | Inicio / Resumen |
| Tipo | Pantalla principal |
| Prioridad | Alta |
| Acceso | Usuarios autenticados |
| Plataforma | Aplicación móvil |
| Estado | Diseño |

---

## 2. Objetivo

El Dashboard proporciona una vista general del estado operativo y financiero del sistema de gestión de préstamos.

Su objetivo es permitir al usuario consultar rápidamente:

- Cantidad de clientes.
- Cantidad de préstamos.
- Préstamos activos.
- Préstamos finalizados.
- Pagos registrados.
- Intereses cobrados.
- Capital pendiente.
- Pagos próximos.
- Pagos vencidos.
- Clientes en situación de morosidad.

La información mostrada deberá depender de los permisos y rol del usuario autenticado.

---

## 3. Usuarios

El Dashboard podrá ser utilizado por:

- Administrador.
- Cobrador.

El contenido disponible podrá variar según los permisos asignados.

---

## 4. Estructura general

La pantalla tendrá una estructura similar a la siguiente:

```text
┌─────────────────────────────────────┐
│ ☰  Dashboard                 🔔 👤 │
├─────────────────────────────────────┤
│                                     │
│ Hola, Usuario                       │
│ Resumen de hoy                      │
│                                     │
│ ┌─────────────┐ ┌─────────────┐    │
│ │  CLIENTES   │ │  PRÉSTAMOS  │    │
│ │     125     │ │      48     │    │
│ └─────────────┘ └─────────────┘    │
│                                     │
│ ┌─────────────┐ ┌─────────────┐    │
│ │   ACTIVOS   │ │  VENCIDOS   │    │
│ │      32     │ │       7     │    │
│ └─────────────┘ └─────────────┘    │
│                                     │
│ Resumen financiero                  │
│                                     │
│ Capital pendiente    S/ 12,500.00  │
│ Intereses cobrados   S/  2,350.00  │
│                                     │
│ Próximos pagos                      │
│ ┌───────────────────────────────┐   │
│ │ Juan Pérez       S/ 50  Hoy   │   │
│ │ María López      S/ 75  Mañana│   │
│ └───────────────────────────────┘   │
│                                     │
│ Pagos vencidos                      │
│ ┌───────────────────────────────┐   │
│ │ Carlos Ruiz      S/ 100  2d   │   │
│ └───────────────────────────────┘   │
│                                     │
├─────────────────────────────────────┤
│ 🏠      👥       💰       ⋮       │
│ Inicio  Clientes  Préstamos  Más  │
└─────────────────────────────────────┘
```

---

## 5. Encabezado

El encabezado estará ubicado en la parte superior.

### Elementos

- Botón de menú.
- Título de la pantalla.
- Icono de notificaciones.
- Acceso al perfil.

Ejemplo:

```text
☰   Dashboard                         🔔  👤
```

---

## 6. Menú lateral

El botón de menú permitirá acceder a las diferentes funcionalidades del sistema.

El menú podrá contener:

```text
MENÚ

🏠 Dashboard

👥 Clientes

💰 Préstamos

💵 Pagos

⚠ Morosidad

🔔 Notificaciones

📱 WhatsApp

👤 Perfil

⚙ Configuración

🚪 Cerrar sesión
```

Las opciones deberán mostrarse según los permisos del usuario.

---

## 7. Saludo al usuario

Debajo del encabezado se mostrará un saludo personalizado.

Ejemplo:

```text
Hola, Luis

Resumen de la actividad del sistema
```

El nombre deberá obtenerse de la información del usuario autenticado.

---

## 8. Tarjetas de indicadores

El Dashboard deberá mostrar indicadores principales mediante tarjetas.

### 8.1 Total de clientes

Muestra el número total de clientes registrados.

Ejemplo:

```text
CLIENTES

125
```

Al seleccionar la tarjeta, el sistema podrá dirigir al módulo de Clientes.

---

### 8.2 Total de préstamos

Muestra el número total de préstamos registrados.

Ejemplo:

```text
PRÉSTAMOS

48
```

---

### 8.3 Préstamos activos

Muestra la cantidad de préstamos que actualmente tienen capital pendiente.

Ejemplo:

```text
PRÉSTAMOS ACTIVOS

32
```

---

### 8.4 Préstamos vencidos

Muestra la cantidad de préstamos asociados a pagos de interés vencidos.

Ejemplo:

```text
VENCIDOS

7
```

Este indicador debe permitir acceder rápidamente al módulo de morosidad.

---

## 9. Resumen financiero

El Dashboard deberá mostrar información financiera relevante.

### 9.1 Capital pendiente

Representa la suma del capital que aún deben pagar los clientes.

Ejemplo:

```text
Capital pendiente

S/ 12,500.00
```

El cálculo deberá considerar únicamente el capital pendiente de los préstamos activos.

---

### 9.2 Intereses cobrados

Representa el total de intereses registrados como pagados.

Ejemplo:

```text
Intereses cobrados

S/ 2,350.00
```

Los intereses deberán calcularse de acuerdo con las reglas financieras definidas para el sistema.

---

### 9.3 Capital recuperado

Se podrá mostrar el total de capital que ya ha sido recuperado mediante pagos.

Ejemplo:

```text
Capital recuperado

S/ 25,000.00
```

---

## 10. Próximos pagos

Se mostrará una lista de los pagos de interés próximos a vencer.

Ejemplo:

```text
PRÓXIMOS PAGOS

Juan Pérez
S/ 50.00
Vence hoy

María López
S/ 75.00
Vence mañana

Pedro García
S/ 100.00
Vence en 3 días
```

Cada registro deberá permitir acceder al detalle del préstamo correspondiente.

---

## 11. Pagos vencidos

Se mostrará una lista de pagos que no fueron realizados dentro del periodo establecido.

Ejemplo:

```text
PAGOS VENCIDOS

Carlos Ruiz
S/ 100.00
2 días de atraso

Ana Torres
S/ 150.00
5 días de atraso
```

Al seleccionar un registro, el usuario podrá consultar el detalle del préstamo y registrar el pago correspondiente si tiene autorización.

---

## 12. Indicador de morosidad

El Dashboard deberá mostrar un resumen de clientes que presentan morosidad.

Ejemplo:

```text
MOROSIDAD

7 clientes con pagos vencidos

[Ver morosidad]
```

La condición de morosidad deberá determinarse utilizando las reglas establecidas en:

`02-Reglas-Negocio/04-Reglas-Morosidad.md`

---

## 13. Acciones rápidas

El Dashboard podrá incluir botones para operaciones frecuentes.

Ejemplo:

```text
ACCIONES RÁPIDAS

[ + Nuevo cliente ]

[ + Nuevo préstamo ]

[ Registrar pago ]

[ Ver morosidad ]
```

Las acciones deberán respetar los permisos del usuario.

---

## 14. Acceso rápido a clientes

Se podrá incluir un campo de búsqueda para encontrar rápidamente un cliente.

```text
┌───────────────────────────────────┐
│ 🔍 Buscar cliente                 │
└───────────────────────────────────┘
```

La búsqueda podrá realizarse mediante:

- Número de documento.
- Nombre.
- Apellidos.
- Número de teléfono.

---

## 15. Notificaciones

El icono de notificaciones mostrará información relevante para el usuario.

Ejemplos:

```text
🔔

3 nuevas notificaciones
```

Las notificaciones podrán corresponder a:

- Pagos próximos.
- Pagos vencidos.
- Clientes en morosidad.
- Recordatorios.
- Eventos importantes del sistema.

---

## 16. Estados del Dashboard

### 16.1 Estado cargando

Mientras se obtiene información del backend se mostrará un indicador de carga.

```text
Cargando información...
```

Las tarjetas podrán utilizar placeholders mientras los datos son procesados.

---

### 16.2 Estado con información

Cuando la información sea obtenida correctamente, se mostrarán todos los indicadores disponibles.

---

### 16.3 Estado sin información

Si todavía no existen registros:

```text
No existen datos suficientes para mostrar información.
```

Por ejemplo, cuando no existen clientes registrados:

```text
CLIENTES

0

Aún no existen clientes registrados.
```

---

### 16.4 Error de conexión

Si no se puede obtener información del servidor:

```text
No se pudo cargar la información.

[Reintentar]
```

---

## 17. Actualización de información

La información del Dashboard deberá actualizarse cuando:

- Se registre un nuevo cliente.
- Se registre un nuevo préstamo.
- Se registre un pago.
- Se modifique el estado de un préstamo.
- Se actualice información relacionada con morosidad.
- Se realice una acción que afecte los indicadores.

También podrá existir una acción manual de actualización:

```text
↻ Actualizar
```

---

## 18. Navegación desde el Dashboard

El Dashboard deberá funcionar como punto central de navegación.

Flujo general:

```text
                         ┌──────────────┐
                         │   DASHBOARD  │
                         └──────┬───────┘
                                │
            ┌───────────┬───────┼────────┬────────────┐
            │           │       │        │            │
            ▼           ▼       ▼        ▼            ▼
        Clientes    Préstamos  Pagos  Morosidad  Notificaciones
            │           │       │        │            │
            ▼           ▼       ▼        ▼            ▼
         Detalle     Detalle  Registro  Detalle     Detalle
```

---

## 19. Navegación inferior

En dispositivos móviles se podrá utilizar una barra de navegación inferior.

Ejemplo:

```text
┌─────────────────────────────────────┐
│                                     │
│            CONTENIDO                │
│                                     │
├─────────────────────────────────────┤
│  🏠       👥        💰       ⋮     │
│ Inicio  Clientes  Préstamos  Más   │
└─────────────────────────────────────┘
```

Las opciones principales serán:

- Inicio.
- Clientes.
- Préstamos.
- Más.

Las funcionalidades adicionales estarán disponibles desde el menú.

---

## 20. Información financiera

Los cálculos mostrados en el Dashboard deberán respetar las reglas financieras del sistema.

Para los préstamos:

- La tasa inicial de interés semanal es del 5%.
- El interés se calcula sobre el capital inicial.
- El interés no es compuesto.
- El interés se genera semanalmente.
- El capital pendiente se reduce cuando se realizan pagos de capital.
- El pago debe aplicar primero al interés pendiente.
- El excedente podrá aplicarse al capital.

Ejemplo:

```text
Capital inicial:       S/ 1,000.00
Interés semanal: 5%
Interés semanal:         S/ 50.00
```

El Dashboard no deberá recalcular estas reglas de forma independiente. Los valores financieros deberán provenir de los datos y cálculos establecidos por el backend.

---

## 21. Información de morosidad

El Dashboard deberá reflejar el estado de morosidad de acuerdo con las reglas del negocio.

La información podrá incluir:

- Número de clientes morosos.
- Número de préstamos con pagos vencidos.
- Monto de intereses vencidos.
- Días de atraso.
- Clientes con múltiples periodos vencidos.

Cuando un cliente supere el límite establecido por las reglas del negocio, deberá identificarse como un caso de morosidad que requiere atención.

---

## 22. Permisos

El contenido del Dashboard deberá respetar el sistema de autorización.

Ejemplo:

| Funcionalidad | Administrador | Cobrador |
|---|---:|---:|
| Ver Dashboard | Sí | Sí |
| Ver clientes | Sí | Sí |
| Registrar cliente | Sí | Sí |
| Ver préstamos | Sí | Sí |
| Registrar préstamo | Sí | Según permiso |
| Registrar pagos | Sí | Sí |
| Ver morosidad | Sí | Sí |
| Configuración | Sí | No |
| Gestión de usuarios | Sí | No |
| Integración WhatsApp | Sí | Según permiso |

La matriz definitiva de permisos será establecida en la documentación de seguridad y autorización.

---

## 23. Componentes funcionales

La pantalla podrá estructurarse mediante los siguientes componentes:

```text
DashboardScreen
│
├── DashboardHeader
│   ├── MenuButton
│   ├── NotificationButton
│   └── ProfileButton
│
├── UserGreeting
│
├── SearchClient
│
├── SummaryCards
│   ├── ClientsCard
│   ├── LoansCard
│   ├── ActiveLoansCard
│   └── OverdueLoansCard
│
├── FinancialSummary
│   ├── PendingCapital
│   ├── CollectedInterest
│   └── RecoveredCapital
│
├── QuickActions
│
├── UpcomingPayments
│
├── OverduePayments
│
├── DelinquencySummary
│
└── BottomNavigation
```

---

## 24. Datos requeridos

Para construir el Dashboard, la aplicación podrá requerir información como:

```text
dashboardSummary

- totalClients
- totalLoans
- activeLoans
- completedLoans
- overdueLoans
- pendingCapital
- collectedInterest
- recoveredCapital
- upcomingPayments
- overduePayments
- delinquentClients
- notificationsCount
```

Los nombres definitivos dependerán del contrato de la API.

---

## 25. Rendimiento

El Dashboard deberá evitar consultas innecesarias al backend.

Se recomienda:

- Utilizar un endpoint de resumen.
- Evitar realizar una consulta independiente para cada tarjeta.
- Implementar caché cuando sea apropiado.
- Actualizar únicamente los datos necesarios.
- Mostrar indicadores de carga.
- Evitar bloquear toda la interfaz mientras se obtienen los datos.

Una respuesta consolidada del backend podrá proporcionar la información principal del Dashboard.

---

## 26. Seguridad

El Dashboard solo podrá ser accesible después de una autenticación válida.

Se deberá:

- Validar el token de sesión.
- Verificar permisos.
- No mostrar información financiera a usuarios no autorizados.
- No almacenar información sensible innecesariamente.
- Cerrar la sesión cuando corresponda.
- Gestionar correctamente la expiración del token.

---

## 27. Accesibilidad

La interfaz deberá:

- Utilizar textos legibles.
- Mantener contraste adecuado.
- Utilizar iconos acompañados de etiquetas cuando sea necesario.
- Permitir interacción mediante lectores de pantalla.
- Mantener áreas táctiles adecuadas.
- Evitar depender exclusivamente del color.
- Mostrar mensajes comprensibles.

---

## 28. Diseño responsive

El Dashboard deberá adaptarse a diferentes tamaños de pantalla.

En pantallas pequeñas:

- Las tarjetas se mostrarán en una o dos columnas.
- Las listas deberán ser desplazables verticalmente.
- Los textos deberán evitar desbordamientos.
- La navegación inferior deberá permanecer accesible.

Ejemplo:

```text
Pantalla pequeña

┌─────────────────────┐
│ CLIENTES            │
│ 125                 │
├─────────────────────┤
│ PRÉSTAMOS           │
│ 48                  │
├─────────────────────┤
│ ACTIVOS             │
│ 32                  │
├─────────────────────┤
│ VENCIDOS            │
│ 7                   │
└─────────────────────┘
```

---

## 29. Criterios de aceptación

La pantalla será considerada terminada cuando:

- [ ] El usuario autenticado pueda acceder al Dashboard.
- [ ] Se muestre el nombre del usuario.
- [ ] Se muestre el total de clientes.
- [ ] Se muestre el total de préstamos.
- [ ] Se muestre el número de préstamos activos.
- [ ] Se muestre el número de préstamos vencidos.
- [ ] Se muestre el capital pendiente.
- [ ] Se muestre el total de intereses cobrados.
- [ ] Se puedan visualizar próximos pagos.
- [ ] Se puedan visualizar pagos vencidos.
- [ ] Se muestre un resumen de morosidad.
- [ ] Existan acciones rápidas.
- [ ] Se pueda acceder a Clientes.
- [ ] Se pueda acceder a Préstamos.
- [ ] Se pueda acceder a Pagos.
- [ ] Se pueda acceder a Morosidad.
- [ ] Se puedan consultar las notificaciones.
- [ ] Se respeten los permisos del usuario.
- [ ] Se gestione correctamente el estado de carga.
- [ ] Se gestione correctamente el estado sin datos.
- [ ] Se gestione correctamente el error de conexión.
- [ ] Los datos financieros provengan del backend.
- [ ] La pantalla sea responsive.
- [ ] Se cumplan las condiciones de seguridad.

---

## 30. Resultado esperado

El Dashboard debe proporcionar una visión general, rápida y confiable del estado del sistema.

El usuario deberá poder identificar inmediatamente:

- Cuántos clientes existen.
- Cuántos préstamos están activos.
- Cuánto capital está pendiente.
- Cuánto interés ha sido cobrado.
- Qué pagos están próximos.
- Qué pagos están vencidos.
- Qué clientes requieren atención por morosidad.

Desde esta pantalla el usuario podrá acceder rápidamente a las principales operaciones del sistema de gestión de préstamos.