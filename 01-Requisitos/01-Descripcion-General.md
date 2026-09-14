# Descripción General

## 1. Nombre del proyecto

Sistema de Gestión de Préstamos y Cobranzas

## 2. Descripción

El proyecto consiste en una aplicación móvil orientada a la gestión de clientes, préstamos y cobranzas para una persona que desempeña las funciones de administrador, prestamista y cobrador.

La aplicación permitirá registrar la información de los clientes, sus avales, solicitudes de préstamo, préstamos aprobados, pagos, intereses y estados de cobranza.

Además, contará con un sistema de notificaciones dirigido al administrador y una integración con WhatsApp Business Platform de Meta para el envío de mensajes mediante plantillas, principalmente para recordatorios de pago, cobranza, confirmaciones y marketing.

## 3. Usuario principal

Inicialmente el sistema tendrá un único usuario operativo:

- Administrador
- Prestamista
- Cobrador

Estas funciones serán realizadas por la misma persona.

El sistema se diseñará de forma que posteriormente pueda incorporar diferentes usuarios y roles.

## 4. Objetivo general

Desarrollar una aplicación móvil que permita administrar de manera centralizada y organizada el ciclo de vida de los clientes, préstamos, intereses y pagos, reduciendo la dependencia de registros manuales.

## 5. Objetivos específicos

- Registrar y consultar clientes.
- Registrar y gestionar avales.
- Registrar solicitudes de préstamo.
- Aprobar o rechazar solicitudes.
- Gestionar préstamos.
- Calcular y controlar intereses semanales.
- Registrar pagos parciales o completos.
- Aplicar los pagos primero a intereses y posteriormente a capital.
- Controlar préstamos morosos.
- Permitir la reactivación de préstamos morosos.
- Enviar notificaciones al administrador.
- Enviar mensajes mediante WhatsApp.
- Mantener un historial de operaciones.

## 6. Característica financiera principal

El interés por defecto corresponde al 5% semanal del monto inicial del préstamo (TASA_SEMANAL_DEFAULT = 0.05, moneda PEN, cálculo `ROUND(monto_inicial * tasa, 2, HALF_UP)`).

El interés se mantiene calculado sobre el monto inicial y no sobre el capital pendiente. La tasa se fija al crear el préstamo (snapshot) y es inmutable después; ver RN-PRE-003.

Ejemplo:

Préstamo inicial: S/ 100  
Interés semanal: S/ 5

Si el cliente paga S/ 50 de capital:

Capital pendiente: S/ 50  
Interés semanal: S/ 5

El interés no se reduce proporcionalmente al capital pendiente.

## 7. Principio de diseño

La aplicación deberá mantener separación entre:

- Información del cliente.
- Información del préstamo.
- Periodos de interés.
- Pagos.
- Estado de cobranza.
- Notificaciones.
- Mensajería de WhatsApp.

Esto permitirá mantener trazabilidad y facilitar futuras ampliaciones.