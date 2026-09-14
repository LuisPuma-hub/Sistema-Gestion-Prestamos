# Requisitos No Funcionales

## RNF-001 - Seguridad

La comunicación entre aplicación móvil y servidor deberá realizarse mediante HTTPS.

## RNF-002 - Autenticación

Las credenciales deberán almacenarse de forma segura utilizando hash de contraseñas.

## RNF-003 - Integridad financiera

Los cálculos monetarios deberán utilizar tipos decimales.

No se utilizarán FLOAT ni DOUBLE para importes financieros.

## RNF-004 - Disponibilidad

El sistema de notificaciones deberá funcionar independientemente de que la aplicación esté abierta.

## RNF-005 - Rendimiento

Las operaciones habituales de consulta deberán responder en un tiempo adecuado para una aplicación móvil.

## RNF-006 - Escalabilidad

La arquitectura deberá permitir posteriormente:

- Múltiples usuarios.
- Múltiples cobradores.
- Múltiples préstamos por cliente.
- Nuevas integraciones.

## RNF-007 - Mantenibilidad

El código deberá organizarse por responsabilidades y capas.

## RNF-008 - Trazabilidad

Las operaciones financieras importantes deberán conservar historial.

## RNF-009 - Integridad de datos

La base de datos deberá utilizar claves primarias, claves foráneas, restricciones e índices apropiados.

## RNF-010 - Usabilidad

La aplicación deberá proporcionar una interfaz sencilla para registrar clientes, préstamos y pagos con el menor número posible de pasos.

## RNF-011 - Compatibilidad

La aplicación móvil deberá desarrollarse utilizando .NET MAUI para permitir una arquitectura preparada para diferentes plataformas.

## RNF-012 - Privacidad

La información personal y financiera de los clientes deberá estar protegida y solamente ser accesible a usuarios autorizados.