# 07. Pantalla de Registro de Préstamo

| Campo | Descripción |
|---|---|
| Nombre | Registro de Préstamo |
| ID | SCR-PRESTAMO-REG-001 |
| Módulo | Préstamos |
| Tipo | Formulario |
| Roles | ADMIN: sí. COBRADOR: según permiso (crear PENDIENTE, no aprobar). Ver `09-Seguridad/02-Autorizacion.md` |
| Acceso | Préstamos → + Nuevo / Detalle Cliente → + Préstamo |

## 1. Objetivo
Crear deuda con snapshot financiero. Flujo: `PENDIENTE --aprobar(admin)--> ACTIVO`. Código único `PR-######` (`^PR-\d{6}$`, zero-padded, backend secuencial).

## 2. Campos normativos
- `clienteId: uuid` obligatorio, debe estar ACTIVO y con 0 préstamos ACTIVO/MOROSO (si no → 409 E-LOAN-ACTIVE).
- `monto: 100.00–10,000.00 PEN, 2 decimales, >0` (422 si 0/negativo/más decimales).
- `fecha_inicio: hoy ±30 días, no futura >7 días, America/Lima`. `primer_vencimiento = inicio+7d 23:59:59` (domingo no se mueve).
- `tasa: solo lectura, banner "Tasa vigente 5% (config 18/08/2026)"`. Se guarda snapshot; cambio 5→6 solo afecta nuevos (ver RN-PRE-003).
- `aval: OPCIONAL, máx 1`. Si cliente-existente: ACTIVO, 0 vencidos, != cliente. Si nuevo: nombres/apellidos/tel `^9\d{8}$`/dirección (mismas regex cliente).
- `motivo/solicitudId`: préstamo solo desde solicitud APROBADA 1:1.

## 3. Contrato API
`POST /api/v1/prestamos {clienteId, montoCents, fechaDesembolso:YYYY-MM-DD, solicitudId, aval?:{tipo, ...}, idempotenciaKey:uuid} → 201 {id, codigo:PR-000125, estado:PENDIENTE} | 409 | 422`. Nombres exactos según `05-API/03-Modelos-Request-Response.md`.

## 4. Validaciones UI
DNI `^\d{8}$`, CE `^[A-Z0-9]{9,12}$`, celular `^9\d{8}$` (+51), debounce duplicado 300ms → 409 `"Ya existe..."`. Área táctil 48x48dp, padding 16dp, título 18sp bold / cuerpo 14sp.

## 5. Criterios aceptación (Given/When/Then)
- DADO cliente sin activo CUANDO POST monto 1000 inicio hoy ENTONCES 201 PR-###### PENDIENTE <2s.
- DADO cliente con ACTIVO CUANDO intenta otro ENTONCES 409 E-LOAN-ACTIVE.
- DADO monto 0/-5/100.123 CUANDO envía ENTONCES 422 campo exacto.
- DADO tasa cambia 5→6 CUANDO revisa préstamo viejo ENTONCES sigue 5% (snapshot).
- DADO capital 1000 + pago que deja capital 0 + interés 20 ENTONCES sigue ACTIVO (con cuotas vencidas) o MOROSO si ≥3 vencidos; nunca CANCELADO (exige cap==0 AND int==0, RN-PRE-010). `FINALIZADO` prohibido.
