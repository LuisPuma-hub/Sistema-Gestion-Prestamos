# Requisitos Funcionales

## RF-001 - Autenticación

El sistema deberá permitir al administrador iniciar y cerrar sesión.

## RF-002 - Gestión de clientes

El sistema deberá permitir registrar clientes con:

- Tipo de documento: DNI o CE.
- Número de documento.
- Nombres.
- Apellidos.
- Número de celular.
- Imagen de recibo de luz (opcional, se guarda en `documentos`).
- Imagen de recibo de agua (opcional, se guarda en `documentos`).
- Observaciones.
- Estado.

## RF-003 - Consulta de clientes

El sistema deberá permitir buscar y consultar clientes registrados.

## RF-004 - Gestión de avales

El sistema deberá permitir seleccionar:

- Cliente existente como aval.
- Nueva persona como aval.

## RF-005 - Registro de aval nuevo

Cuando el aval sea una persona nueva, el sistema deberá solicitar:

- Nombres.
- Apellidos.
- Número de teléfono.
- Dirección.

## RF-006 - Solicitudes de préstamo

El sistema deberá permitir registrar solicitudes de préstamo asociadas a un cliente.

## RF-007 - Aprobación de préstamos

El administrador deberá poder aprobar o rechazar una solicitud.

## RF-008 - Creación del préstamo

Una solicitud aprobada deberá permitir crear un préstamo.

> Excepto la importación masiva, que crea el préstamo directo en ACTIVO sin solicitud previa.

## RF-009 - Cálculo del interés

El sistema deberá calcular el interés semanal equivalente al 5% del monto inicial.

## RF-010 - Periodos de interés

El sistema deberá generar periodos semanales de interés.

## RF-011 - Registro de pagos

El sistema deberá permitir registrar pagos asociados a un préstamo.

> Incluye préstamos en estado MOROSO; rechaza Pendiente, Cancelado y Anulado.

## RF-012 - Aplicación de pagos

El sistema deberá aplicar el pago en el siguiente orden:

1. Intereses pendientes.
2. Capital.

## RF-013 - Reducción de capital

El capital pendiente únicamente se reducirá con la parte del pago que exceda los intereses pendientes.

## RF-014 - Control de morosidad

El sistema deberá identificar periodos de interés vencidos.

## RF-015 - Reactivación

El administrador deberá poder reactivar manualmente un préstamo moroso.

## RF-016 - Notificaciones

El sistema deberá enviar notificaciones al dispositivo del administrador.

## RF-017 - Horarios

El administrador deberá poder establecer horarios de notificación, por ejemplo:

- 08:00
- 16:00

## RF-018 - Funcionamiento en segundo plano

Las notificaciones deberán poder recibirse aunque la aplicación esté cerrada o el administrador no tenga la aplicación abierta.

## RF-019 - WhatsApp

El sistema deberá permitir enviar mensajes mediante WhatsApp Business Platform utilizando plantillas.

## RF-020 - Historial

El sistema deberá conservar el historial de:

- Clientes.
- Solicitudes.
- Préstamos.
- Periodos.
- Pagos.
- Notificaciones.
- Mensajes de WhatsApp.

## RF-021 - Importación masiva

El sistema deberá permitir al Administrador importar préstamos vía CSV en dos pasos: previsualizar (valida cabecera, DNI de 8 dígitos, celular de 9 dígitos que empiece con 9, máximo 500 filas, duplicados y préstamo vigente) y confirmar con creación atómica de cliente, préstamo, periodos y pago de ajuste (`CARGA INICIAL`).

## RF-022 - Actividad reciente

El dashboard deberá mostrar el evento más reciente entre el último pago (ordenado por fecha de registro) y el último préstamo aprobado.