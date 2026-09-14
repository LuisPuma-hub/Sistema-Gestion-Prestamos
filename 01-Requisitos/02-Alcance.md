# Alcance

## 1. Alcance inicial

El sistema incluirá los siguientes componentes:

### Gestión de clientes

- Registro de clientes.
- Consulta de clientes.
- Actualización de información.
- Estado del cliente.
- Observaciones.
- Registro de documentos de identidad.
- Registro de recibos de luz y agua.

### Gestión de avales

- Asociar un cliente existente como aval.
- Registrar una nueva persona como aval.
- Consultar información del aval.

### Gestión de solicitudes

- Registrar solicitud.
- Consultar solicitudes.
- Aprobar solicitud.
- Rechazar solicitud.
- Mantener historial de solicitudes.

### Gestión de préstamos

- Crear préstamo aprobado.
- Registrar monto inicial.
- Registrar tasa de interés (v1: solo lectura, default 5% fijo según RN-PRE-003; editable solo en futuras versiones con rango 0-10% y auditoría).
- Generar periodos semanales bajo demanda: al crear se genera N°1 con `inicio = fecha_desembolso 00:00 America/Lima`, `vencimiento = inicio + 7 días 23:59:59`; job diario crea el siguiente si el préstamo sigue ACTIVO/MOROSO.
- Consultar capital pendiente.
- Consultar intereses pendientes.
- Consultar historial.

### Gestión de pagos

- Registrar pagos.
- Aplicar pagos a intereses.
- Aplicar excedente a capital.
- Consultar historial de pagos.
- Consultar distribución de cada pago.

### Gestión de morosidad

- Detectar periodos vencidos (job diario 00:05 America/Lima, regla RN-MOR-002).
- Marcar préstamo como moroso automáticamente cuando COUNT(vencidos_impagos) >= 3 (RN-MOR-005).
- Registrar historial de morosidad en `Historial_Morosidad`.
- Permitir reactivación manual MOROSO -> ACTIVO solo con pago >=1 vencido o motivo + actor (no resetea contador).

### Notificaciones

- Recordatorios de cobranza.
- Recordatorios de cobros próximos.
- Notificaciones de morosidad.
- Horarios configurables.
- Notificaciones mediante FCM.

### WhatsApp

- Integración con WhatsApp Business Platform.
- Uso de plantillas.
- Recordatorios de pago.
- Avisos de cobro próximo.
- Avisos de cobro vencido.
- Confirmaciones.
- Mensajes de marketing.

## 2. Fuera del alcance inicial

No se contempla inicialmente:

- Chat entre administrador y cliente.
- Pasarela de pagos.
- Integración bancaria.
- Contabilidad empresarial.
- Facturación electrónica.
- Scoring crediticio automático.
- Múltiples cobradores.
- Múltiples administradores.
- Múltiples préstamos activos simultáneamente por cliente.

## 3. Preparación para futuras versiones

Aunque inicialmente se limitará a un préstamo activo por cliente, la base de datos utilizará una relación:

CLIENTE 1:N PRESTAMO

Esto permitirá habilitar múltiples préstamos para un mismo cliente posteriormente.

## 4. Alcance tecnológico

La solución estará compuesta por:

- Aplicación móvil.
- API backend.
- Base de datos.
- Servicio de notificaciones.
- Integración con WhatsApp.