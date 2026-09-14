# Casos de Prueba

## 1. Objetivo

Definir los casos de prueba necesarios para verificar el correcto funcionamiento del **Sistema de Gestión de Préstamos**, considerando las funcionalidades, reglas de negocio, operaciones financieras, seguridad, integraciones y manejo de errores.

Los casos de prueba permitirán documentar:

- Qué funcionalidad será evaluada.
- Qué condiciones deben cumplirse.
- Qué datos se utilizarán.
- Qué pasos deben ejecutarse.
- Qué resultado se espera.
- Qué resultado se obtuvo.
- Si la prueba fue aprobada o rechazada.

---

## 2. Alcance

Los casos de prueba comprenden:

- Autenticación.
- Autorización.
- Gestión de usuarios.
- Gestión de clientes.
- Gestión de avales.
- Gestión de préstamos.
- Cálculo de intereses.
- Registro de pagos.
- Pagos parciales.
- Múltiples préstamos por cliente.
- Morosidad.
- Reactivación.
- Notificaciones.
- WhatsApp.
- Configuración.
- API.
- Base de datos.
- Seguridad.
- Protección de datos.

---

## 3. Convenciones

### 3.1 Estados de los casos

| Estado | Descripción |
|---|---|
| Pendiente | El caso todavía no ha sido ejecutado |
| En ejecución | La prueba está siendo ejecutada |
| Aprobado | El resultado coincide con el esperado |
| Rechazado | El resultado no coincide con el esperado |
| Bloqueado | No puede ejecutarse por una dependencia |
| No aplica | El caso no corresponde a la versión actual |

---

## 4. Prioridades

| Prioridad | Descripción |
|---|---|
| Crítica | Un fallo puede afectar operaciones financieras o seguridad |
| Alta | Afecta una funcionalidad principal |
| Media | Afecta parcialmente una funcionalidad |
| Baja | Tiene impacto menor |

---

# 5. Casos de Prueba de Autenticación

## CP-AUT-001 - Inicio de sesión correcto

**Prioridad:** Crítica  
**Tipo:** Funcional  
**Módulo:** Autenticación

### Precondiciones

- El usuario debe estar registrado.
- La cuenta debe estar activa.
- El usuario debe conocer sus credenciales.

### Datos de prueba

```text
Correo: usuario@ejemplo.com
Contraseña: contraseña válida
```

### Pasos

1. Abrir la aplicación.
2. Ingresar el correo.
3. Ingresar la contraseña.
4. Presionar "Iniciar sesión".

### Resultado esperado

El sistema deberá validar las credenciales y permitir el acceso al dashboard.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-AUT-002 - Inicio de sesión con contraseña incorrecta

**Prioridad:** Alta  
**Tipo:** Seguridad

### Pasos

1. Abrir la aplicación.
2. Ingresar un correo válido.
3. Ingresar una contraseña incorrecta.
4. Presionar "Iniciar sesión".

### Resultado esperado

El sistema deberá rechazar el acceso y mostrar un mensaje indicando que las credenciales no son válidas.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-AUT-003 - Inicio de sesión con campos vacíos

**Prioridad:** Alta  
**Tipo:** Validación

### Pasos

1. Abrir la pantalla de login.
2. Dejar el correo vacío.
3. Dejar la contraseña vacía.
4. Presionar "Iniciar sesión".

### Resultado esperado

El sistema deberá mostrar las validaciones correspondientes y no enviar una solicitud de autenticación inválida.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-AUT-004 - Cierre de sesión

**Prioridad:** Alta  
**Tipo:** Funcional

### Precondiciones

El usuario debe tener una sesión activa.

### Pasos

1. Ingresar al sistema.
2. Abrir el perfil.
3. Seleccionar "Cerrar sesión".
4. Confirmar la operación.

### Resultado esperado

La sesión deberá finalizar y el usuario deberá regresar a la pantalla de login.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-AUT-005 - Acceso con sesión expirada

**Prioridad:** Alta  
**Tipo:** Seguridad

### Pasos

1. Iniciar sesión.
2. Esperar o simular la expiración del token.
3. Intentar ejecutar una operación protegida.

### Resultado esperado

El backend deberá rechazar la solicitud y la aplicación deberá solicitar nuevamente la autenticación cuando corresponda.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 6. Casos de Prueba de Autorización

## CP-AUTZ-001 - Acceso autorizado según rol

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Iniciar sesión con un usuario autorizado.
2. Acceder a una funcionalidad permitida.
3. Ejecutar la operación.

### Resultado esperado

La operación deberá ejecutarse correctamente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-AUTZ-002 - Acceso a funcionalidad no autorizada

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Iniciar sesión con un usuario sin el permiso requerido.
2. Intentar acceder a una funcionalidad restringida.

### Resultado esperado

El backend deberá rechazar la operación.

La respuesta deberá indicar que el usuario no tiene autorización suficiente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-AUTZ-003 - Modificación de configuración financiera sin permiso

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Iniciar sesión como usuario sin permiso administrativo.
2. Intentar modificar la tasa de interés.
3. Enviar la solicitud al backend.

### Resultado esperado

El backend deberá rechazar la modificación.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 7. Casos de Prueba de Clientes

## CP-CLI-001 - Registrar cliente correctamente

**Prioridad:** Alta  
**Tipo:** Funcional

### Datos

```text
Tipo de documento: DNI
Número de documento: Documento de prueba
Nombres: Juan
Apellidos: Pérez
Teléfono: Número de prueba
```

### Pasos

1. Ingresar al módulo Clientes.
2. Seleccionar "Nuevo cliente".
3. Completar los campos obligatorios.
4. Guardar.

### Resultado esperado

El cliente deberá registrarse correctamente y aparecer en el listado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-CLI-002 - Registrar cliente con campos obligatorios vacíos

**Prioridad:** Alta  
**Tipo:** Validación

### Pasos

1. Abrir el formulario de cliente.
2. Dejar uno o más campos obligatorios vacíos.
3. Seleccionar "Guardar".

### Resultado esperado

El sistema deberá mostrar las validaciones correspondientes y no registrar el cliente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-CLI-003 - Registrar documento duplicado

**Prioridad:** Alta  
**Tipo:** Integridad

### Precondiciones

Debe existir un cliente con el mismo documento.

### Pasos

1. Intentar registrar otro cliente utilizando el mismo número de documento.
2. Guardar.

### Resultado esperado

El sistema deberá impedir el registro duplicado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-CLI-004 - Modificar información del cliente

**Prioridad:** Media  
**Tipo:** Funcional

### Pasos

1. Buscar un cliente existente.
2. Abrir el detalle.
3. Seleccionar "Editar".
4. Modificar información permitida.
5. Guardar.

### Resultado esperado

La información deberá actualizarse correctamente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 8. Casos de Prueba de Avales

## CP-AVA-001 - Registrar nuevo aval

**Prioridad:** Alta  
**Tipo:** Funcional

### Pasos

1. Iniciar el registro de un préstamo.
2. Seleccionar la opción para registrar un nuevo aval.
3. Ingresar nombres.
4. Ingresar apellidos.
5. Ingresar teléfono.
6. Ingresar dirección.
7. Guardar.

### Resultado esperado

El aval deberá registrarse y asociarse correctamente al préstamo.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-AVA-002 - Seleccionar un cliente existente como aval

**Prioridad:** Alta  
**Tipo:** Funcional

### Pasos

1. Iniciar el registro de un préstamo.
2. Seleccionar "Aval existente".
3. Buscar un cliente.
4. Seleccionarlo.
5. Guardar el préstamo.

### Resultado esperado

El cliente seleccionado deberá asociarse como aval.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 9. Casos de Prueba de Préstamos

## CP-PRE-001 - Registrar préstamo correctamente

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Capital inicial: S/ 100.00
Interés semanal: 5 %
Frecuencia: Semanal
```

### Pasos

1. Seleccionar un cliente.
2. Seleccionar "Nuevo préstamo".
3. Ingresar el capital.
4. Verificar la configuración del interés.
5. Registrar el aval cuando corresponda.
6. Guardar el préstamo.

### Resultado esperado

El préstamo deberá registrarse correctamente con:

```text
Capital inicial: S/ 100.00
Interés semanal: S/ 5.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PRE-002 - Calcular interés sobre capital inicial

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Capital inicial: S/ 500.00
Tasa: 5 %
```

### Cálculo esperado

```text
500 × 0.05 = S/ 25.00
```

### Resultado esperado

El interés semanal deberá ser **S/ 25.00**.

El cálculo deberá utilizar el capital inicial y no el saldo restante.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PRE-003 - Verificar que no exista interés compuesto

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Capital inicial: S/ 100.00
Interés semanal: S/ 5.00
```

### Pasos

1. Registrar el préstamo.
2. Simular el paso de varias semanas sin reducir el capital.
3. Verificar el interés generado.

### Resultado esperado

El interés semanal deberá mantenerse en:

```text
S/ 5.00
```

No deberá convertirse en un interés calculado sobre un capital incrementado por intereses anteriores.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PRE-004 - Registrar múltiples préstamos para un cliente

**Prioridad:** Alta  
**Tipo:** Funcional

### Pasos

1. Seleccionar un cliente existente.
2. Registrar un primer préstamo.
3. Registrar un segundo préstamo para el mismo cliente.
4. Consultar el historial del cliente.

### Resultado esperado

El cliente deberá mostrar ambos préstamos independientemente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PRE-005 - Consultar detalle de préstamo

**Prioridad:** Alta  
**Tipo:** Funcional

### Pasos

1. Abrir el módulo de préstamos.
2. Seleccionar un préstamo.
3. Abrir el detalle.

### Resultado esperado

Deberá mostrarse:

- Cliente.
- Capital inicial.
- Interés.
- Pagos.
- Saldo.
- Fechas.
- Estado.
- Información de morosidad.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 10. Casos de Prueba de Pagos

## CP-PAG-001 - Registrar pago exacto de interés

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Interés pendiente: S/ 5.00
Pago: S/ 5.00
```

### Resultado esperado

```text
Interés pagado: S/ 5.00
Capital pagado: S/ 0.00
```

El interés deberá quedar completamente cubierto.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PAG-002 - Registrar pago superior al interés

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Interés pendiente: S/ 5.00
Capital pendiente: S/ 100.00
Pago: S/ 20.00
```

### Resultado esperado

```text
Interés pagado: S/ 5.00
Capital pagado: S/ 15.00
Capital pendiente: S/ 85.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PAG-003 - Registrar pago parcial de interés

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Interés pendiente: S/ 5.00
Pago: S/ 3.00
```

### Resultado esperado

```text
Interés pagado: S/ 3.00
Interés pendiente: S/ 2.00
Capital pagado: S/ 0.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PAG-004 - Registrar pago con monto negativo

**Prioridad:** Crítica  
**Tipo:** Validación

### Datos

```text
Pago: S/ -10.00
```

### Resultado esperado

El backend deberá rechazar la operación.

No deberá modificarse el préstamo.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PAG-005 - Registrar pago igual a cero

**Prioridad:** Alta  
**Tipo:** Validación

### Datos

```text
Pago: S/ 0.00
```

### Resultado esperado

El sistema deberá rechazar el pago.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PAG-006 - Registrar pago en préstamo inexistente

**Prioridad:** Alta  
**Tipo:** Validación

### Pasos

1. Enviar una solicitud de pago utilizando un identificador inexistente.
2. Procesar la solicitud.

### Resultado esperado

El backend deberá rechazar la operación indicando que el préstamo no existe.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-PAG-007 - Evitar registro duplicado de pago

**Prioridad:** Crítica  
**Tipo:** Integridad

### Pasos

1. Registrar un pago.
2. Repetir accidentalmente la misma operación.
3. Procesar ambas solicitudes.

### Resultado esperado

El sistema deberá evitar registrar dos veces la misma operación.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 11. Casos de Prueba de Morosidad

## CP-MOR-001 - Detectar interés vencido

**Prioridad:** Alta  
**Tipo:** Financiera

### Pasos

1. Crear un préstamo activo.
2. Establecer una fecha de vencimiento.
3. No registrar el pago correspondiente.
4. Ejecutar el proceso de evaluación de morosidad.

### Resultado esperado

El sistema deberá identificar el interés como vencido.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-MOR-002 - Dos pagos de interés vencidos

**Prioridad:** Alta  
**Tipo:** Financiera

### Pasos

1. Registrar un préstamo.
2. Simular dos periodos vencidos.
3. No registrar los pagos.
4. Ejecutar la evaluación de morosidad.

### Resultado esperado

El sistema deberá registrar dos pagos de interés vencidos.

No deberá activar todavía la condición de "más de 2" vencimientos.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-MOR-003 - Más de dos pagos de interés vencidos

**Prioridad:** Crítica  
**Tipo:** Financiera

### Pasos

1. Crear un préstamo activo.
2. Simular tres periodos de interés vencidos.
3. No registrar los pagos.
4. Ejecutar la evaluación de morosidad.

### Resultado esperado

El sistema deberá identificar la condición de morosidad crítica debido a que existen más de dos pagos de interés vencidos.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-MOR-004 - Reactivación de cliente

**Prioridad:** Alta  
**Tipo:** Funcional

### Precondiciones

El cliente deberá encontrarse en una situación de morosidad que permita la reactivación según las reglas configuradas.

### Pasos

1. Abrir el detalle del cliente.
2. Revisar la condición de morosidad.
3. Seleccionar la opción de reactivación.
4. Confirmar.

### Resultado esperado

El sistema deberá actualizar el estado de acuerdo con las reglas establecidas y registrar la operación.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 12. Casos de Prueba de Notificaciones

## CP-NOT-001 - Programación de notificación

**Prioridad:** Media  
**Tipo:** Integración

### Datos

```text
Hora 1: 08:00
Hora 2: 16:00
```

### Resultado esperado

El sistema deberá ejecutar el proceso de notificaciones en los horarios configurados.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-NOT-002 - Notificación de pago próximo

**Prioridad:** Alta  
**Tipo:** Integración

### Precondiciones

Debe existir un préstamo con un pago próximo.

### Pasos

1. Ejecutar el proceso programado.
2. Identificar préstamos con pago próximo.
3. Generar la notificación.

### Resultado esperado

El cliente deberá recibir una notificación de recordatorio cuando corresponda.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-NOT-003 - Notificación de pago vencido

**Prioridad:** Alta  
**Tipo:** Integración

### Precondiciones

Debe existir un pago vencido.

### Resultado esperado

El sistema deberá generar la notificación correspondiente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 13. Casos de Prueba de WhatsApp

## CP-WHA-001 - Selección correcta de plantilla

**Prioridad:** Alta  
**Tipo:** Integración

### Pasos

1. Generar un evento de pago próximo.
2. Ejecutar el proceso de mensajería.
3. Seleccionar la plantilla correspondiente.

### Resultado esperado

El backend deberá seleccionar la plantilla configurada para el evento.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-WHA-002 - Validación de variables de plantilla

**Prioridad:** Alta  
**Tipo:** Integración

### Pasos

1. Seleccionar una plantilla.
2. Preparar las variables requeridas.
3. Omitir intencionalmente una variable.
4. Intentar enviar el mensaje.

### Resultado esperado

El backend deberá detectar la variable faltante y evitar el envío incorrecto.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-WHA-003 - Evitar mensajes duplicados

**Prioridad:** Alta  
**Tipo:** Integridad

### Pasos

1. Generar un evento de comunicación.
2. Ejecutar el proceso de envío.
3. Ejecutar nuevamente el mismo proceso.

### Resultado esperado

El sistema deberá evitar el envío duplicado cuando corresponda.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-WHA-004 - Registrar estado del mensaje

**Prioridad:** Media  
**Tipo:** Integración

### Pasos

1. Enviar un mensaje.
2. Esperar la respuesta correspondiente.
3. Procesar el webhook.

### Resultado esperado

El sistema deberá registrar el estado correspondiente del mensaje.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 14. Casos de Prueba de API

## CP-API-001 - Endpoint protegido sin autenticación

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Ejecutar un endpoint protegido.
2. No enviar credenciales de autenticación.

### Resultado esperado

La API deberá rechazar la solicitud.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-API-002 - Solicitud con datos inválidos

**Prioridad:** Alta  
**Tipo:** Validación

### Pasos

1. Enviar una solicitud con campos inválidos.
2. Procesar la solicitud.

### Resultado esperado

La API deberá devolver una respuesta de validación y no modificar información incorrectamente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-API-003 - Acceso a recurso inexistente

**Prioridad:** Alta  
**Tipo:** Funcional

### Pasos

1. Solicitar un recurso utilizando un identificador inexistente.

### Resultado esperado

La API deberá devolver una respuesta indicando que el recurso no existe.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 15. Casos de Prueba de Base de Datos

## CP-BD-001 - Integridad de clave primaria

**Prioridad:** Alta  
**Tipo:** Integridad

### Resultado esperado

No deberá ser posible crear dos registros con la misma clave primaria.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-BD-002 - Integridad de clave foránea

**Prioridad:** Crítica  
**Tipo:** Integridad

### Pasos

1. Intentar registrar un pago asociado a un préstamo inexistente.

### Resultado esperado

La operación deberá ser rechazada.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-BD-003 - Persistencia de pago

**Prioridad:** Crítica  
**Tipo:** Integridad

### Pasos

1. Registrar un pago mediante la API.
2. Consultar el préstamo.
3. Consultar el historial de pagos.

### Resultado esperado

El pago deberá encontrarse almacenado correctamente y relacionado con el préstamo correspondiente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 16. Casos de Prueba de Seguridad

## CP-SEG-001 - Contraseña no almacenada en texto plano

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Registrar un usuario.
2. Consultar el registro correspondiente en la base de datos.

### Resultado esperado

La contraseña no deberá encontrarse almacenada en texto plano.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-SEG-002 - Token inválido

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Generar o utilizar un token inválido.
2. Ejecutar un endpoint protegido.

### Resultado esperado

La API deberá rechazar la solicitud.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-SEG-003 - Manipulación de saldo

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Intentar modificar directamente el saldo mediante una solicitud manipulada.
2. Enviar la solicitud al backend.

### Resultado esperado

El backend deberá ignorar o rechazar valores que el cliente no tenga permitido modificar.

El saldo deberá determinarse mediante las reglas de negocio.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-SEG-004 - Acceso no autorizado a información personal

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Iniciar sesión con un usuario sin permiso.
2. Intentar acceder a información personal restringida.

### Resultado esperado

El backend deberá impedir el acceso.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 17. Casos de Prueba de Configuración

## CP-CON-001 - Modificar tasa de interés

**Prioridad:** Crítica  
**Tipo:** Funcional

### Precondiciones

El usuario deberá contar con permisos administrativos.

### Pasos

1. Abrir configuración.
2. Modificar la tasa de interés.
3. Confirmar.
4. Guardar.

### Resultado esperado

La nueva configuración deberá almacenarse y quedar registrada mediante auditoría.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-CON-002 - Usuario sin permiso modifica configuración

**Prioridad:** Crítica  
**Tipo:** Seguridad

### Pasos

1. Iniciar sesión con un usuario sin permiso administrativo.
2. Intentar modificar la tasa de interés.

### Resultado esperado

La modificación deberá ser rechazada.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 18. Casos de Prueba de Integridad Financiera

## CP-FIN-001 - Pago aplicado primero al interés

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Capital inicial: S/ 100.00
Interés pendiente: S/ 5.00
Pago: S/ 20.00
```

### Resultado esperado

```text
Interés pagado: S/ 5.00
Capital pagado: S/ 15.00
Capital pendiente: S/ 85.00
```

---

## CP-FIN-002 - Pago inferior al interés

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Interés pendiente: S/ 5.00
Pago: S/ 2.00
```

### Resultado esperado

```text
Interés pagado: S/ 2.00
Interés pendiente: S/ 3.00
Capital pagado: S/ 0.00
```

---

## CP-FIN-003 - Pago igual a interés y capital pendiente

**Prioridad:** Crítica  
**Tipo:** Financiera

### Datos

```text
Interés pendiente: S/ 5.00
Capital pendiente: S/ 100.00
Pago: S/ 105.00
```

### Resultado esperado

```text
Interés pagado: S/ 5.00
Capital pagado: S/ 100.00
Saldo: S/ 0.00
```

El préstamo deberá actualizarse al estado correspondiente de finalizado o pagado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 19. Casos de Prueba de Errores

## CP-ERR-001 - Backend no disponible

**Prioridad:** Alta  
**Tipo:** Disponibilidad

### Pasos

1. Desconectar o detener temporalmente el backend.
2. Intentar realizar una operación desde la aplicación.

### Resultado esperado

La aplicación deberá mostrar un mensaje comprensible y no deberá cerrarse inesperadamente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-ERR-002 - Pérdida de conexión durante una operación

**Prioridad:** Alta  
**Tipo:** Integración

### Pasos

1. Iniciar una operación.
2. Interrumpir la conexión.
3. Finalizar la solicitud.

### Resultado esperado

La aplicación deberá informar el problema y evitar registrar accidentalmente la operación dos veces.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 20. Casos de Prueba de Auditoría

## CP-AUD-001 - Registrar operación financiera

**Prioridad:** Alta  
**Tipo:** Auditoría

### Pasos

1. Registrar un pago.
2. Consultar los registros de auditoría.

### Resultado esperado

Deberá existir un registro que permita identificar:

```text
Usuario
Operación
Recurso
Fecha
Resultado
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## CP-AUD-002 - Registrar cambio de configuración

**Prioridad:** Alta  
**Tipo:** Auditoría

### Pasos

1. Modificar un parámetro financiero.
2. Consultar la auditoría.

### Resultado esperado

El sistema deberá registrar el cambio realizado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 21. Matriz Resumen

| ID | Módulo | Tipo | Prioridad |
|---|---|---|---|
| CP-AUT-001 | Autenticación | Funcional | Crítica |
| CP-AUT-002 | Autenticación | Seguridad | Alta |
| CP-AUT-003 | Autenticación | Validación | Alta |
| CP-AUT-004 | Autenticación | Funcional | Alta |
| CP-AUT-005 | Autenticación | Seguridad | Alta |
| CP-AUTZ-001 | Autorización | Seguridad | Crítica |
| CP-AUTZ-002 | Autorización | Seguridad | Crítica |
| CP-AUTZ-003 | Autorización | Seguridad | Crítica |
| CP-CLI-001 | Clientes | Funcional | Alta |
| CP-CLI-002 | Clientes | Validación | Alta |
| CP-CLI-003 | Clientes | Integridad | Alta |
| CP-CLI-004 | Clientes | Funcional | Media |
| CP-AVA-001 | Avales | Funcional | Alta |
| CP-AVA-002 | Avales | Funcional | Alta |
| CP-PRE-001 | Préstamos | Financiera | Crítica |
| CP-PRE-002 | Intereses | Financiera | Crítica |
| CP-PRE-003 | Intereses | Financiera | Crítica |
| CP-PRE-004 | Préstamos | Funcional | Alta |
| CP-PRE-005 | Préstamos | Funcional | Alta |
| CP-PAG-001 | Pagos | Financiera | Crítica |
| CP-PAG-002 | Pagos | Financiera | Crítica |
| CP-PAG-003 | Pagos | Financiera | Crítica |
| CP-PAG-004 | Pagos | Validación | Crítica |
| CP-PAG-005 | Pagos | Validación | Alta |
| CP-PAG-006 | Pagos | Validación | Alta |
| CP-PAG-007 | Pagos | Integridad | Crítica |
| CP-MOR-001 | Morosidad | Financiera | Alta |
| CP-MOR-002 | Morosidad | Financiera | Alta |
| CP-MOR-003 | Morosidad | Financiera | Crítica |
| CP-MOR-004 | Morosidad | Funcional | Alta |
| CP-NOT-001 | Notificaciones | Integración | Media |
| CP-NOT-002 | Notificaciones | Integración | Alta |
| CP-NOT-003 | Notificaciones | Integración | Alta |
| CP-WHA-001 | WhatsApp | Integración | Alta |
| CP-WHA-002 | WhatsApp | Integración | Alta |
| CP-WHA-003 | WhatsApp | Integridad | Alta |
| CP-WHA-004 | WhatsApp | Integración | Media |
| CP-API-001 | API | Seguridad | Crítica |
| CP-API-002 | API | Validación | Alta |
| CP-API-003 | API | Funcional | Alta |
| CP-BD-001 | Base de datos | Integridad | Alta |
| CP-BD-002 | Base de datos | Integridad | Crítica |
| CP-BD-003 | Base de datos | Integridad | Crítica |
| CP-SEG-001 | Seguridad | Seguridad | Crítica |
| CP-SEG-002 | Seguridad | Seguridad | Crítica |
| CP-SEG-003 | Seguridad | Seguridad | Crítica |
| CP-SEG-004 | Seguridad | Seguridad | Crítica |
| CP-CON-001 | Configuración | Funcional | Crítica |
| CP-CON-002 | Configuración | Seguridad | Crítica |
| CP-FIN-001 | Finanzas | Financiera | Crítica |
| CP-FIN-002 | Finanzas | Financiera | Crítica |
| CP-FIN-003 | Finanzas | Financiera | Crítica |
| CP-ERR-001 | Errores | Disponibilidad | Alta |
| CP-ERR-002 | Errores | Integración | Alta |
| CP-AUD-001 | Auditoría | Auditoría | Alta |
| CP-AUD-002 | Auditoría | Auditoría | Alta |

---

# 22. Criterios Generales de Aprobación

Un caso de prueba será considerado **Aprobado** cuando:

- El sistema ejecute correctamente los pasos definidos.
- El resultado obtenido coincida con el resultado esperado.
- No se produzcan efectos secundarios inesperados.
- Los datos permanezcan íntegros.
- Las reglas de negocio se respeten.

Será considerado **Rechazado** cuando:

- El resultado sea diferente al esperado.
- Se produzca un error no controlado.
- Se modifique información incorrectamente.
- Se incumpla una regla de negocio.
- Se genere una vulnerabilidad.
- Se pierda o duplique información.

---

# 23. Registro de Resultados

Para cada ejecución se deberá registrar:

| Campo | Información |
|---|---|
| ID del caso | Identificador del caso |
| Fecha | Fecha de ejecución |
| Ejecutor | Usuario responsable |
| Resultado | Aprobado/Rechazado/Bloqueado |
| Evidencia | Captura, log o resultado |
| Defecto | ID del defecto si existe |
| Observaciones | Comentarios adicionales |

Ejemplo:

```text
ID: CP-PAG-002
Fecha: ____/____/________
Ejecutor: __________________
Resultado: APROBADO
Evidencia: __________________
Defecto: Ninguno
Observaciones: ______________
```

---

# 24. Priorización de Casos

Antes de cada versión se deberán ejecutar prioritariamente los casos relacionados con:

1. Autenticación.
2. Autorización.
3. Préstamos.
4. Cálculo de intereses.
5. Pagos.
6. Saldos.
7. Morosidad.
8. Integridad de base de datos.
9. Seguridad.
10. Integraciones críticas.

Los casos de prioridad **Crítica** deberán ejecutarse antes de considerar una versión apta para producción.

---

# 25. Reejecución de Casos

Cuando un defecto sea corregido, se deberá:

1. Ejecutar nuevamente el caso que falló.
2. Verificar que el defecto haya sido solucionado.
3. Ejecutar pruebas relacionadas.
4. Ejecutar pruebas de regresión cuando corresponda.
5. Actualizar el estado del caso.

```text
Caso fallido
     │
     ▼
Registrar defecto
     │
     ▼
Corregir
     │
     ▼
Reejecutar caso
     │
     ├── Aprobado → Cerrar defecto
     │
     └── Rechazado → Mantener defecto abierto
```

---

# 26. Criterios de Aceptación

Los casos de prueba deberán permitir demostrar que:

- [ ] La autenticación funciona correctamente.
- [ ] La autorización se aplica según los permisos.
- [ ] Los clientes pueden registrarse y gestionarse correctamente.
- [ ] Los avales pueden registrarse o seleccionarse.
- [ ] Los préstamos se registran correctamente.
- [ ] Se permiten múltiples préstamos por cliente.
- [ ] El interés semanal se calcula sobre el capital inicial.
- [ ] La tasa inicial es del 5 %.
- [ ] No se aplica interés compuesto.
- [ ] Los pagos se aplican primero al interés.
- [ ] Los pagos parciales funcionan correctamente.
- [ ] Los excedentes se aplican al capital.
- [ ] Los saldos se actualizan correctamente.
- [ ] La morosidad se detecta correctamente.
- [ ] La condición de más de dos pagos vencidos funciona correctamente.
- [ ] La reactivación respeta las reglas establecidas.
- [ ] Las notificaciones se procesan correctamente.
- [ ] Los mensajes WhatsApp utilizan las plantillas correspondientes.
- [ ] No se generan mensajes duplicados.
- [ ] La API valida las solicitudes.
- [ ] PostgreSQL mantiene la integridad de los datos.
- [ ] Los datos sensibles están protegidos.
- [ ] Las operaciones importantes generan auditoría.

---

# 27. Conclusión

Los casos de prueba definidos en este documento proporcionan una base para validar funcional y técnicamente el Sistema de Gestión de Préstamos.

Se dará especial prioridad a las pruebas relacionadas con **operaciones financieras, seguridad, integridad de datos y reglas de negocio**, debido a que cualquier error en estas áreas puede afectar directamente la información y operación del sistema.

Los casos deberán mantenerse actualizados conforme se agreguen funcionalidades, cambien las reglas de negocio o se modifique la arquitectura del sistema.