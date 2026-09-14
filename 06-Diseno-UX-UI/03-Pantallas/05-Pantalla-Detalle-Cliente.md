# 05. Pantalla de Detalle de Cliente

## 1. Información general

| Campo | Descripción |
|---|---|
| Nombre | Detalle de Cliente |
| Archivo | `05-Pantalla-Detalle-Cliente.md` |
| Módulo | Gestión de Clientes |
| Tipo | Detalle / Consulta |
| Prioridad | Alta |
| Acceso | Usuarios autenticados con permisos |
| Plataforma | Aplicación móvil |
| Estado | Diseño |

---

## 2. Objetivo

La pantalla de Detalle de Cliente permite consultar toda la información relevante de un cliente registrado y acceder rápidamente a las operaciones relacionadas con sus préstamos, pagos, morosidad y comunicación.

La pantalla deberá centralizar la información del cliente sin necesidad de navegar entre múltiples módulos para consultar su situación.

---

## 3. Acceso

La pantalla podrá abrirse desde:

```text
Dashboard
   │
   ▼
Clientes
   │
   ▼
Seleccionar cliente
   │
   ▼
Detalle del cliente
```

También podrá accederse desde:

- Resultados de búsqueda.
- Listados de préstamos.
- Registros de pagos.
- Módulo de morosidad.
- Notificaciones relacionadas con el cliente.

---

## 4. Estructura general

```text
┌─────────────────────────────────────┐
│ ←  Detalle del Cliente         ⋮   │
├─────────────────────────────────────┤
│                                     │
│             ┌─────────┐             │
│             │  FOTO   │             │
│             └─────────┘             │
│                                     │
│          Juan Carlos Pérez          │
│             Estado: Activo          │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ DATOS PERSONALES                │ │
│ │ DNI: 72456321                   │ │
│ │ Teléfono: 987654321             │ │
│ └─────────────────────────────────┘ │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ RESUMEN                         │ │
│ │ Préstamos activos: 2            │ │
│ │ Capital pendiente: S/ 1,500     │ │
│ │ Pagos vencidos: 1               │ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ Préstamos ] [ Pagos ]             │
│                                     │
│ ┌─────────────────────────────────┐ │
│ │ OBSERVACIONES                   │ │
│ │ Cliente registrado recientemente│ │
│ └─────────────────────────────────┘ │
│                                     │
│ [ 💬 WhatsApp ] [ Editar ]         │
│                                     │
└─────────────────────────────────────┘
```

---

## 5. Encabezado

El encabezado deberá contener:

- Botón para regresar.
- Título `Detalle del Cliente`.
- Menú de acciones adicionales.

Ejemplo:

```text
←  Detalle del Cliente              ⋮
```

---

## 6. Información principal

La parte superior deberá mostrar la información básica del cliente.

Ejemplo:

```text
┌─────────────┐
│             │
│    FOTO     │
│             │
└─────────────┘

Juan Carlos Pérez

Estado: Activo
```

La imagen podrá corresponder a la fotografía del cliente cuando exista o utilizar una imagen predeterminada.

---

## 7. Estado del cliente

El estado actual deberá mostrarse claramente.

Estados contemplados:

```text
Activo
Observación
Moroso
```

Ejemplo:

```text
Estado: Activo
```

El estado deberá provenir de la lógica de negocio y no ser calculado únicamente por la interfaz.

---

## 8. Datos personales

La pantalla deberá mostrar:

### Identificación

- Tipo de documento.
- Número de documento.
- Nombres.
- Apellidos.

### Contacto

- Número de teléfono.

Ejemplo:

```text
DATOS PERSONALES

Tipo de documento
DNI

Número
72456321

Nombres
Juan Carlos

Apellidos
Pérez López

Teléfono
987654321
```

---

## 9. Documentación

Se deberá permitir consultar la imagen del recibo de agua o electricidad asociada al cliente.

Ejemplo:

```text
DOCUMENTACIÓN

Recibo de agua / electricidad

[ Ver documento ]
```

Al seleccionar la opción, se podrá abrir una vista ampliada de la imagen.

---

## 10. Resumen financiero

La pantalla podrá mostrar un resumen de la situación financiera del cliente.

Información recomendada:

- Número de préstamos activos.
- Capital pendiente.
- Capital recuperado.
- Intereses cobrados.
- Intereses pendientes.
- Pagos vencidos.

Ejemplo:

```text
RESUMEN FINANCIERO

Préstamos activos       2
Capital pendiente       S/ 1,500.00
Capital recuperado      S/ 2,000.00
Intereses cobrados      S/ 350.00
Intereses pendientes    S/ 50.00
Pagos vencidos          1
```

Los valores deberán provenir del backend.

---

## 11. Préstamos del cliente

La pantalla deberá proporcionar acceso a los préstamos asociados al cliente.

Un cliente podrá tener múltiples préstamos.

Ejemplo:

```text
PRÉSTAMOS

Préstamo #001
Capital inicial: S/ 1,000
Capital pendiente: S/ 500
Estado: Activo

Préstamo #002
Capital inicial: S/ 2,000
Capital pendiente: S/ 2,000
Estado: Activo
```

Cada préstamo deberá permitir acceder a su detalle.

---

## 12. Pagos del cliente

Se deberá proporcionar acceso al historial de pagos.

Ejemplo:

```text
PAGOS RECIENTES

01/09/2026
S/ 50.00
Interés

25/08/2026
S/ 150.00
Interés + Capital

18/08/2026
S/ 50.00
Interés
```

La información detallada estará disponible en el módulo de Pagos.

---

## 13. Morosidad

Si el cliente presenta pagos vencidos, la pantalla deberá mostrar esta situación.

Ejemplo:

```text
⚠ MOROSIDAD

Pagos vencidos: 2

Interés pendiente:
S/ 100.00

[Ver morosidad]
```

Si el cliente no presenta morosidad:

```text
✓ Sin pagos vencidos
```

---

## 14. Observaciones

Las observaciones administrativas del cliente deberán mostrarse en una sección independiente.

Ejemplo:

```text
OBSERVACIONES

Cliente registrado recientemente.
```

Los usuarios autorizados podrán editar las observaciones.

---

## 15. Acciones principales

La pantalla podrá proporcionar las siguientes acciones:

```text
[ Editar cliente ]

[ Ver préstamos ]

[ Registrar préstamo ]

[ Ver pagos ]

[ WhatsApp ]
```

Las acciones disponibles deberán depender de los permisos del usuario.

---

## 16. Editar cliente

La opción:

```text
[ Editar ]
```

permitirá modificar la información editable del cliente.

Podrán modificarse, según permisos:

- Nombres.
- Apellidos.
- Teléfono.
- Imagen del recibo.
- Observaciones.

El tipo y número de documento deberán estar sujetos a restricciones para evitar inconsistencias o duplicidades.

---

## 17. Registrar préstamo

Si el usuario tiene permisos suficientes, podrá iniciar el registro de un préstamo directamente desde el cliente.

Flujo:

```text
Detalle Cliente
      │
      ▼
Registrar préstamo
      │
      ▼
Registro de préstamo
      │
      ▼
Seleccionar / confirmar cliente
```

La información del cliente deberá aparecer precargada cuando corresponda.

---

## 18. WhatsApp

La pantalla podrá incluir una acción para comunicarse con el cliente.

Ejemplo:

```text
[ 💬 WhatsApp ]
```

Antes de enviar el mensaje se deberá:

1. Verificar que exista un teléfono.
2. Validar el número.
3. Seleccionar una plantilla o mensaje autorizado.
4. Mostrar una vista previa.
5. Confirmar el envío.

---

## 19. Historial del cliente

El sistema deberá permitir consultar el historial relacionado con el cliente.

Este historial podrá incluir:

- Fecha de registro.
- Modificaciones de información.
- Préstamos.
- Pagos.
- Cambios de estado.
- Eventos de morosidad.
- Mensajes enviados.
- Notificaciones relevantes.

El nivel de detalle dependerá de los permisos del usuario.

---

## 20. Línea de tiempo

Como alternativa para representar el historial, se podrá utilizar una línea de tiempo.

Ejemplo:

```text
HISTORIAL

● 01/09/2026
  Pago registrado
  S/ 50.00

● 25/08/2026
  Préstamo actualizado

● 20/08/2026
  Préstamo registrado
  S/ 1,000.00

● 15/08/2026
  Cliente registrado
```

---

## 21. Menú de acciones adicionales

El menú `⋮` podrá contener:

```text
Editar cliente
Ver documentación
Ver préstamos
Ver pagos
Ver historial
Enviar WhatsApp
```

Las opciones deberán mostrarse según permisos.

---

## 22. Estado cargando

Mientras se obtiene la información:

```text
Cargando información del cliente...
```

Se podrá utilizar skeleton loading para mejorar la experiencia visual.

---

## 23. Cliente no encontrado

Si el cliente solicitado no existe o fue eliminado:

```text
Cliente no encontrado.

Es posible que el registro
ya no esté disponible.

[Volver a Clientes]
```

---

## 24. Error de conexión

Si no se puede obtener la información:

```text
No se pudo cargar la información
del cliente.

[Reintentar]
```

---

## 25. Estado sin préstamos

Si el cliente todavía no tiene préstamos:

```text
PRÉSTAMOS

Este cliente todavía no tiene
préstamos registrados.

[+ Registrar préstamo]
```

---

## 26. Estado sin pagos

Si no existen pagos registrados:

```text
PAGOS

No existen pagos registrados
para este cliente.
```

---

## 27. Estado sin morosidad

Si el cliente se encuentra al día:

```text
✓ Cliente al día

No existen pagos vencidos.
```

---

## 28. Reglas financieras

Los valores financieros mostrados en esta pantalla deberán respetar las reglas definidas para los préstamos.

La tasa inicial establecida es:

```text
Interés semanal = 5% del capital inicial
```

Ejemplo:

```text
Capital inicial: S/ 1,000.00

5% = S/ 50.00 de interés semanal
```

El interés:

- Se calcula sobre el capital inicial.
- No es compuesto.
- Se genera semanalmente.
- Se registra primero en los pagos.
- El excedente del pago podrá aplicarse al capital.

La pantalla únicamente deberá presentar los resultados calculados por el sistema.

---

## 29. Múltiples préstamos

El cliente podrá tener múltiples préstamos.

Ejemplo:

```text
Juan Pérez

Préstamo #001
S/ 1,000
Activo

Préstamo #002
S/ 500
Activo

Préstamo #003
S/ 2,000
Finalizado
```

El sistema deberá mantener cada préstamo como una operación independiente.

---

## 30. Permisos

| Acción | Administrador | Cobrador |
|---|---:|---:|
| Ver cliente | Sí | Sí |
| Editar cliente | Sí | Según permiso |
| Ver documentación | Sí | Según permiso |
| Ver préstamos | Sí | Sí |
| Registrar préstamo | Sí | Según permiso |
| Ver pagos | Sí | Sí |
| Registrar pago | Sí | Sí |
| Ver morosidad | Sí | Sí |
| Editar observaciones | Sí | Según permiso |
| WhatsApp | Sí | Según permiso |
| Ver historial | Sí | Según permiso |

---

## 31. Componentes funcionales

La pantalla podrá estructurarse mediante:

```text
ClientDetailScreen
│
├── ClientDetailHeader
│
├── ClientProfile
│   ├── ClientImage
│   ├── ClientName
│   └── ClientStatus
│
├── PersonalInformation
│
├── ContactInformation
│
├── DocumentSection
│
├── FinancialSummary
│
├── LoansSection
│
├── PaymentsSection
│
├── DelinquencySection
│
├── ObservationsSection
│
├── ClientHistory
│
└── ClientActions
    ├── EditClient
    ├── AddLoan
    └── WhatsApp
```

---

## 32. Datos requeridos

La pantalla podrá requerir una estructura equivalente a:

```text
ClientDetail

- id
- documentType
- documentNumber
- firstNames
- lastNames
- phone
- receiptImage
- status
- observations
- createdAt
- activeLoans
- pendingCapital
- recoveredCapital
- collectedInterest
- pendingInterest
- overduePayments
- loans
- recentPayments
- history
```

Los nombres definitivos dependerán del contrato de la API.

---

## 33. Seguridad

La pantalla deberá:

- Requerir autenticación.
- Validar la sesión.
- Aplicar autorización.
- Proteger los datos personales.
- Restringir el acceso a documentos.
- Utilizar HTTPS.
- Evitar exposición innecesaria de información.
- Registrar acciones sensibles cuando corresponda.

---

## 34. Accesibilidad

La pantalla deberá:

- Utilizar etiquetas claras.
- Mantener textos legibles.
- Utilizar iconos acompañados de texto cuando sea necesario.
- Mantener tamaños táctiles adecuados.
- Proporcionar información alternativa para imágenes.
- No depender exclusivamente de colores para indicar estados.

---

## 35. Diseño responsive

La pantalla deberá adaptarse a dispositivos móviles de diferentes dimensiones.

Ejemplo:

```text
┌───────────────────────┐
│ ← Detalle cliente ⋮   │
├───────────────────────┤
│                       │
│        [ FOTO ]       │
│                       │
│    Juan Pérez         │
│    Estado: Activo     │
│                       │
├───────────────────────┤
│ DATOS PERSONALES      │
│ DNI: 72456321         │
│ Tel: 987654321        │
├───────────────────────┤
│ RESUMEN               │
│ Activos: 2            │
│ Pendiente: S/ 1,500   │
├───────────────────────┤
│ PRÉSTAMOS             │
│ Préstamo #001         │
│ S/ 1,000              │
├───────────────────────┤
│ PAGOS                 │
│ S/ 50 - Interés       │
├───────────────────────┤
│ [ WhatsApp ] [Editar] │
└───────────────────────┘
```

---

## 36. Criterios de aceptación

La pantalla será considerada terminada cuando:

- [ ] Se pueda acceder al detalle desde Clientes.
- [ ] Se muestre la información básica del cliente.
- [ ] Se muestre el estado actual.
- [ ] Se muestre el tipo de documento.
- [ ] Se muestre el número de documento.
- [ ] Se muestre el nombre completo.
- [ ] Se muestre el teléfono.
- [ ] Se pueda consultar la documentación.
- [ ] Se muestre el resumen financiero.
- [ ] Se puedan consultar múltiples préstamos.
- [ ] Se puedan consultar pagos.
- [ ] Se pueda identificar la morosidad.
- [ ] Se puedan visualizar observaciones.
- [ ] Se pueda editar información según permisos.
- [ ] Se pueda iniciar el registro de un préstamo.
- [ ] Se pueda acceder a WhatsApp según permisos.
- [ ] Se pueda consultar el historial.
- [ ] Se gestione correctamente el estado de carga.
- [ ] Se gestione correctamente el cliente no encontrado.
- [ ] Se gestione correctamente el error de conexión.
- [ ] Se gestione correctamente la ausencia de préstamos.
- [ ] Se gestione correctamente la ausencia de pagos.
- [ ] Se respeten las reglas financieras.
- [ ] Se respeten los permisos.
- [ ] Se protejan los datos personales.
- [ ] La pantalla sea responsive.

---

## 37. Resultado esperado

La pantalla de Detalle de Cliente debe concentrar la información relevante de cada cliente y proporcionar acceso directo a las operaciones relacionadas.

El usuario autorizado deberá poder conocer rápidamente:

- Quién es el cliente.
- Cómo contactarlo.
- Qué documentación tiene registrada.
- Cuántos préstamos posee.
- Cuánto capital tiene pendiente.
- Qué pagos ha realizado.
- Si presenta morosidad.
- Qué observaciones existen.
- Qué acciones puede realizar sobre el cliente.

Esta pantalla será el punto central de consulta de la información individual del cliente.