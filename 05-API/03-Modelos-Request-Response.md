# 03. Modelos Request / Response (OpenAPI 3.1)

Contrato canónico. Fechas RFC3339 `America/Lima` (ej. `2026-09-15T08:00:00-05:00`). Dinero: `integer céntimos + currency PEN` en API (render `S/ 1,234.56 es-PE` solo en plantilla/UI). IDs `uuid v4`. Enum case `UPPER_SNAKE`.

## 1. Envelope

```json
{"success": true, "data": {}, "meta": {"total": 0, "page": 1, "pageSize": 20}, "traceId": "uuid"}
{"success": false, "error": {"code": "VALIDATION_ERROR", "message": "...", "details": [{"field":"monto","msg":"..."}]}, "traceId": "uuid"}
```

## 2. Auth

`LoginRequest {"email":"a@b.com (max 254, RFC5322)","password":"8..72 chars (límite transporte; la política de nuevas contraseñas exige mín 10 + mayús/min/dígito, ver 05-API/02 §25)"}`
`LoginResponse {"success":true,"user":{"id":"uuid","email":"..."},"accessToken":"jwt RS256","refreshToken":"opaque 256-bit","expiresIn":900,"tokenType":"Bearer","roles":["ADMIN"]}`
`RefreshRequest {"refreshToken":"opaque"}`
`MeResponse {"id":"uuid","nombre":"...","email":"...","estado":"ACTIVO","roles":["ADMIN"]}`

## 3. Cliente

`Cliente {id:uuid, tipoDoc:DNI|CE, nroDoc:string, nombres:string(2..60), apellidos:string(2..60), celular:string ^9\d{8}$ (E.164 +51), direccion:string, estado:ACTIVO|INACTIVO|BLOQUEADO|MOROSO, aceptaWhatsApp:bool, fechaConsentimiento:date-time|null}`

## 4. Préstamo / Cuota / Pago

`Prestamo {id:uuid, clienteId:uuid, montoCapitalCents:int>0, tasaSemanal:0.05 (tanto por uno; DB guarda 5.00 porcentaje, conversión tasa_api=tasa_db/100), estado:PENDIENTE|ACTIVO|MOROSO|CANCELADO|ANULADO}`
`Cuota {id:uuid, prestamoId:uuid, nro:int, vencimiento:date-time, interesCents:int, capitalCents:int, estado:PENDIENTE|PARCIAL|PAGADA|VENCIDA}`
`PagoCreate {prestamoId:uuid, montoCents:int>0, fechaValor:YYYY-MM-DD, metodo:EFECTIVO|YAPE|TRANSFERENCIA|PLIN|DEPOSITO|OTRO, idempotenciaKey:uuid}`
`Pago {+ distribucion:{moraCents:int, interesCents:int, capitalCents:int}, estado:REGISTRADO|ANULADO}`

## 5. Dispositivo / WhatsApp / Notificación

`DispositivoToken {"token":"fcm:...","plataforma":"android","deviceId":"uuid","appVersion":"1.0"}`
`WhatsAppSend {clienteId:uuid, prestamoId:uuid, template:"wh_pago_recordatorio_v1", lang:"es_PE", params:{"1":"Juan","2":"15/09/2026"}}`
`WhatsAppProgramar {same + scheduledAt:date-time}`
`Notification {id:uuid, tipo:PAYMENT_REMINDER|PAYMENT_OVERDUE|DELINQUENCY|CRITICAL_DELINQUENCY|PAYMENT_REGISTERED|LOAN_REACTIVATED, prestamoId:uuid, dedupeKey:"prestamoId:cuotaId:tipo:fechaISO:slot", estado:PENDIENTE|ENVIADO|FALLIDO}`
