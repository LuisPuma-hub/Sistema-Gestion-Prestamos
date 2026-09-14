# Pruebas de Reglas Financieras

## 1. Objetivo

Definir y documentar las pruebas específicas destinadas a verificar que las operaciones financieras del **Sistema de Gestión de Préstamos** se ejecuten de acuerdo con las reglas de negocio establecidas.

Estas pruebas tendrán prioridad crítica debido a que cualquier error en los cálculos de intereses, pagos, saldos o morosidad puede afectar directamente la información financiera del sistema.

---

## 2. Alcance

Las pruebas comprenden:

- Cálculo del interés semanal.
- Aplicación del 5 % de interés.
- Cálculo sobre el capital inicial.
- Verificación de interés no compuesto.
- Periodicidad semanal.
- Registro de pagos.
- Pagos parciales.
- Aplicación de pagos al interés.
- Aplicación del excedente al capital.
- Actualización de saldos.
- Finalización de préstamos.
- Múltiples préstamos por cliente.
- Intereses vencidos.
- Control de morosidad.
- Más de dos pagos de interés vencidos.
- Reactivación.
- Integridad de las operaciones financieras.
- Prevención de duplicidad de pagos.

---

# 3. Reglas Financieras Base

Las pruebas deberán utilizar como referencia las siguientes reglas:

| Regla | Configuración |
|---|---|
| Tasa de interés | 5 % |
| Frecuencia | Semanal |
| Base del interés | Capital inicial |
| Interés compuesto | No |
| Aplicación del pago | Primero interés |
| Excedente | Capital |
| Pagos parciales | Permitidos |
| Múltiples préstamos | Permitidos |
| Morosidad crítica | Más de 2 intereses vencidos |

---

# 4. Fórmula del Interés

El interés semanal se calculará mediante:

```text
Interés semanal = Capital inicial × 0.05
```

Ejemplo:

```text
Capital inicial = S/ 100.00

Interés = 100 × 0.05
Interés = S/ 5.00
```

El cálculo deberá ejecutarse en el backend mediante **ASP.NET Core Web API**.

---

# 5. Consideraciones Monetarias

Los valores monetarios deberán manejarse utilizando tipos apropiados para dinero.

En C#, se recomienda utilizar:

```text
decimal
```

para:

- Capital.
- Interés.
- Pagos.
- Saldos.
- Montos pendientes.

No se deberán utilizar tipos de punto flotante como `float` o `double` para representar directamente valores monetarios.

---

# 6. Casos de Prueba del Cálculo de Interés

## PF-001 - Interés sobre S/ 100

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 100.00
Tasa: 5 %
```

### Operación

```text
100 × 0.05 = 5
```

### Resultado esperado

```text
Interés semanal = S/ 5.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-002 - Interés sobre S/ 500

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 500.00
Tasa: 5 %
```

### Operación

```text
500 × 0.05 = 25
```

### Resultado esperado

```text
Interés semanal = S/ 25.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-003 - Interés sobre S/ 1,000

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 1,000.00
Tasa: 5 %
```

### Operación

```text
1000 × 0.05 = 50
```

### Resultado esperado

```text
Interés semanal = S/ 50.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-004 - Interés sobre monto decimal

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 350.50
Tasa: 5 %
```

### Operación

```text
350.50 × 0.05 = 17.525
```

### Resultado esperado

El sistema deberá aplicar la política de precisión monetaria definida para el proyecto.

Si se utiliza redondeo monetario a dos decimales:

```text
Interés = S/ 17.53
```

La regla de redondeo deberá ser consistente en toda la aplicación.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 7. Pruebas de Capital Inicial

## PF-005 - El interés debe utilizar el capital inicial

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 100.00
Tasa: 5 %
Capital pendiente: S/ 60.00
```

### Resultado esperado

El interés semanal deberá continuar calculándose sobre:

```text
S/ 100.00
```

y no sobre:

```text
S/ 60.00
```

Por lo tanto:

```text
Interés = S/ 5.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-006 - El pago de capital no debe cambiar la base del interés

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 500.00
Pago aplicado al capital: S/ 200.00
Capital pendiente: S/ 300.00
Tasa: 5 %
```

### Resultado esperado

El interés de acuerdo con la regla del sistema deberá continuar tomando como base:

```text
Capital inicial = S/ 500.00
```

Por lo tanto:

```text
Interés semanal = S/ 25.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 8. Pruebas de Interés No Compuesto

## PF-007 - Verificar que el interés no se capitalice

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 100.00
Interés semanal: S/ 5.00
```

### Escenario

Después de una semana:

```text
Interés = S/ 5.00
```

Después de dos semanas:

```text
Interés de la semana = S/ 5.00
```

Después de tres semanas:

```text
Interés de la semana = S/ 5.00
```

### Resultado esperado

El sistema no deberá calcular:

```text
105 × 0.05
```

ni:

```text
110 × 0.05
```

como consecuencia de acumular intereses anteriores al capital.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 9. Pruebas de Periodicidad

## PF-008 - Generación de interés semanal

**Prioridad:** Alta

### Datos

```text
Capital inicial: S/ 200.00
Tasa: 5 %
Frecuencia: Semanal
```

### Resultado esperado

El sistema deberá generar el interés correspondiente según la periodicidad semanal configurada.

```text
200 × 0.05 = S/ 10.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-009 - Fecha de vencimiento semanal

**Prioridad:** Alta

### Escenario

Si un préstamo inicia un lunes, el siguiente vencimiento de interés deberá corresponder al lunes siguiente según la configuración de fechas del sistema.

### Resultado esperado

El sistema deberá mantener una separación de una semana entre los periodos correspondientes.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 10. Pruebas de Aplicación de Pagos

## PF-010 - Pago exacto del interés

**Prioridad:** Crítica

### Datos

```text
Interés pendiente: S/ 5.00
Pago: S/ 5.00
Capital pendiente: S/ 100.00
```

### Resultado esperado

```text
Interés pagado: S/ 5.00
Capital pagado: S/ 0.00
Interés pendiente: S/ 0.00
Capital pendiente: S/ 100.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-011 - Pago superior al interés

**Prioridad:** Crítica

### Datos

```text
Interés pendiente: S/ 5.00
Capital pendiente: S/ 100.00
Pago: S/ 20.00
```

### Aplicación

```text
Pago total:       S/ 20.00
Interés:          S/  5.00
Excedente:        S/ 15.00
Capital:          S/ 15.00
```

### Resultado esperado

```text
Interés pendiente: S/ 0.00
Capital pendiente: S/ 85.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-012 - Pago inferior al interés

**Prioridad:** Crítica

### Datos

```text
Interés pendiente: S/ 5.00
Pago: S/ 3.00
Capital pendiente: S/ 100.00
```

### Resultado esperado

```text
Interés pagado: S/ 3.00
Interés pendiente: S/ 2.00
Capital pagado: S/ 0.00
Capital pendiente: S/ 100.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-013 - Pago igual a interés más una parte del capital

**Prioridad:** Crítica

### Datos

```text
Interés pendiente: S/ 5.00
Capital pendiente: S/ 100.00
Pago: S/ 55.00
```

### Resultado esperado

```text
Interés pagado: S/ 5.00
Capital pagado: S/ 50.00
Capital pendiente: S/ 50.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-014 - Pago total del préstamo

**Prioridad:** Crítica

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
Saldo pendiente: S/ 0.00
```

El préstamo deberá pasar al estado correspondiente de préstamo pagado/finalizado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 11. Pruebas de Pagos Parciales

## PF-015 - Pago parcial menor al interés

**Prioridad:** Crítica

### Datos

```text
Interés pendiente: S/ 10.00
Pago: S/ 4.00
```

### Resultado esperado

```text
Interés pagado: S/ 4.00
Interés pendiente: S/ 6.00
Capital pagado: S/ 0.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-016 - Pago parcial que cubre exactamente el interés

**Prioridad:** Crítica

### Datos

```text
Interés pendiente: S/ 10.00
Pago: S/ 10.00
```

### Resultado esperado

```text
Interés pendiente: S/ 0.00
Capital pagado: S/ 0.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 12. Pruebas de Pagos Excesivos

## PF-017 - Pago superior al saldo total

**Prioridad:** Crítica

### Datos

```text
Interés pendiente: S/ 5.00
Capital pendiente: S/ 100.00
Saldo total: S/ 105.00
Pago: S/ 120.00
```

### Resultado esperado

El sistema deberá aplicar correctamente el monto permitido y deberá tener definida una política para el excedente.

Las posibles políticas deberán definirse explícitamente, por ejemplo:

```text
Pago máximo permitido = saldo total
```

o

```text
Excedente registrado como saldo a favor
```

El sistema no deberá perder ni asignar incorrectamente el excedente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 13. Pruebas de Montos Inválidos

## PF-018 - Pago negativo

**Prioridad:** Crítica

### Datos

```text
Pago: S/ -10.00
```

### Resultado esperado

La operación deberá ser rechazada.

No deberá modificarse ningún saldo.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-019 - Pago igual a cero

**Prioridad:** Alta

### Datos

```text
Pago: S/ 0.00
```

### Resultado esperado

El sistema deberá rechazar el pago.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-020 - Capital inicial negativo

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ -100.00
```

### Resultado esperado

El préstamo no deberá registrarse.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-021 - Capital inicial igual a cero

**Prioridad:** Alta

### Datos

```text
Capital inicial: S/ 0.00
```

### Resultado esperado

El sistema deberá rechazar el préstamo si la regla de negocio establece un capital positivo obligatorio.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 14. Pruebas de Múltiples Préstamos

## PF-022 - Independencia de préstamos

**Prioridad:** Crítica

### Datos

```text
Cliente: Cliente de prueba

Préstamo 1:
Capital inicial: S/ 100.00

Préstamo 2:
Capital inicial: S/ 500.00
```

### Resultado esperado

Cada préstamo deberá mantener independientemente:

- Capital.
- Interés.
- Pagos.
- Saldo.
- Estado.
- Morosidad.

El pago del préstamo 1 no deberá modificar el préstamo 2.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-023 - Pago aplicado al préstamo correcto

**Prioridad:** Crítica

### Pasos

1. Seleccionar el préstamo 1.
2. Registrar un pago.
3. Consultar ambos préstamos.

### Resultado esperado

Solo el préstamo seleccionado deberá actualizarse.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 15. Pruebas de Morosidad

## PF-024 - Un interés vencido

**Prioridad:** Alta

### Escenario

Un préstamo tiene un interés vencido sin pago.

### Resultado esperado

El sistema deberá registrar:

```text
Intereses vencidos: 1
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-025 - Dos intereses vencidos

**Prioridad:** Alta

### Resultado esperado

El sistema deberá registrar:

```text
Intereses vencidos: 2
```

La condición de "más de 2" todavía no deberá considerarse cumplida.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-026 - Tres intereses vencidos

**Prioridad:** Crítica

### Resultado esperado

El sistema deberá registrar:

```text
Intereses vencidos: 3
```

Y deberá activar la condición definida para:

```text
Más de 2 intereses vencidos
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 16. Pruebas de Reactivación

## PF-027 - Reactivación permitida

**Prioridad:** Alta

### Precondiciones

El cliente deberá cumplir las condiciones definidas para la reactivación.

### Pasos

1. Consultar el cliente.
2. Verificar su estado.
3. Seleccionar reactivación.
4. Confirmar.
5. Guardar.

### Resultado esperado

El sistema deberá cambiar el estado según las reglas de negocio y registrar la operación.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-028 - Reactivación no autorizada

**Prioridad:** Alta

### Precondiciones

El usuario no deberá tener permiso para ejecutar la reactivación.

### Resultado esperado

La operación deberá ser rechazada.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 17. Pruebas de Integridad de Pagos

## PF-029 - Pago duplicado

**Prioridad:** Crítica

### Escenario

Se envía dos veces la misma solicitud de pago.

### Resultado esperado

El sistema deberá identificar la duplicidad y evitar que el mismo pago sea contabilizado dos veces.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-030 - Error durante registro de pago

**Prioridad:** Crítica

### Escenario

Se produce un error durante una operación financiera.

### Resultado esperado

La operación deberá mantener la consistencia de los datos.

No deberá producirse una situación como:

```text
Pago registrado
pero
saldo sin actualizar
```

o:

```text
Saldo actualizado
pero
pago no registrado
```

La operación deberá completarse correctamente o revertirse de forma consistente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 18. Pruebas de Transacciones

## PF-031 - Registro transaccional de pago

**Prioridad:** Crítica

### Operaciones

Un registro de pago deberá considerar de manera consistente:

```text
1. Validar préstamo.
2. Validar monto.
3. Determinar interés pendiente.
4. Aplicar pago al interés.
5. Aplicar excedente al capital.
6. Actualizar saldo.
7. Actualizar estado.
8. Registrar pago.
9. Registrar auditoría.
```

### Resultado esperado

Todas las operaciones deberán completarse correctamente como una operación consistente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 19. Pruebas de Precisión Monetaria

## PF-032 - Mantener precisión decimal

**Prioridad:** Crítica

### Datos

```text
Capital: S/ 333.33
Tasa: 5 %
```

### Operación

```text
333.33 × 0.05 = 16.6665
```

### Resultado esperado

El sistema deberá aplicar una política de precisión monetaria consistente.

Si se establece redondeo a dos decimales:

```text
Interés = S/ 16.67
```

La misma política deberá utilizarse en cálculos posteriores.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 20. Pruebas de Configuración de la Tasa

## PF-033 - Tasa configurada correctamente

**Prioridad:** Crítica

### Datos

```text
Capital: S/ 1,000.00
Tasa configurada: 5 %
```

### Resultado esperado

```text
Interés = S/ 50.00
```

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-034 - Cambio de tasa

**Prioridad:** Crítica

### Precondiciones

El usuario deberá tener autorización para modificar la configuración financiera.

### Pasos

1. Modificar la tasa configurada.
2. Guardar.
3. Registrar un nuevo préstamo.
4. Verificar el cálculo.

### Resultado esperado

El nuevo préstamo deberá utilizar la configuración vigente según las reglas de configuración.

Los préstamos existentes deberán mantener el tratamiento definido por las reglas del sistema y la información histórica correspondiente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 21. Pruebas de Historial

## PF-035 - Historial de pagos

**Prioridad:** Alta

### Pasos

1. Registrar varios pagos.
2. Abrir el detalle del préstamo.
3. Consultar el historial.

### Resultado esperado

Cada pago deberá mostrar información como:

- Fecha.
- Monto.
- Aplicación al interés.
- Aplicación al capital.
- Usuario que realizó la operación.
- Estado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 22. Pruebas de Estados del Préstamo

## PF-036 - Préstamo activo

**Prioridad:** Alta

### Resultado esperado

Un préstamo correctamente registrado y vigente deberá aparecer como activo.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-037 - Préstamo pagado

**Prioridad:** Crítica

### Datos

```text
Interés pendiente: S/ 0.00
Capital pendiente: S/ 0.00
```

### Resultado esperado

El préstamo deberá cambiar al estado correspondiente de pagado/finalizado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-038 - Préstamo con saldo pendiente

**Prioridad:** Alta

### Datos

```text
Capital pendiente: S/ 50.00
```

### Resultado esperado

El préstamo no deberá marcarse como completamente pagado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 23. Pruebas de Consistencia del Saldo

## PF-039 - Verificación matemática del saldo

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 500.00
Capital pagado acumulado: S/ 150.00
```

### Resultado esperado

```text
Capital pendiente = 500 - 150
Capital pendiente = S/ 350.00
```

El sistema deberá mantener consistencia entre:

- Capital inicial.
- Capital pagado.
- Capital pendiente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 24. Prueba Integral de Préstamo

## PF-040 - Ciclo completo del préstamo

**Prioridad:** Crítica

### Datos iniciales

```text
Capital inicial: S/ 100.00
Tasa semanal: 5 %
Interés semanal: S/ 5.00
```

### Semana 1

El cliente realiza un pago:

```text
Pago: S/ 5.00
```

Resultado:

```text
Interés pagado: S/ 5.00
Capital pagado: S/ 0.00
Capital pendiente: S/ 100.00
```

### Semana 2

El cliente realiza:

```text
Pago: S/ 20.00
```

Resultado:

```text
Interés: S/ 5.00
Capital: S/ 15.00
Capital pendiente: S/ 85.00
```

### Semana 3

Se genera nuevamente:

```text
Interés semanal: S/ 5.00
```

La base continúa siendo:

```text
Capital inicial: S/ 100.00
```

### Resultado esperado

El sistema deberá mantener correctamente:

- Historial de pagos.
- Intereses.
- Capital.
- Saldo.
- Estado.
- Fechas.
- Morosidad.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 25. Prueba Integral con Pago Parcial

## PF-041 - Ciclo con pago parcial

**Prioridad:** Crítica

### Datos

```text
Capital inicial: S/ 200.00
Tasa: 5 %
Interés semanal: S/ 10.00
```

### Semana 1

Pago:

```text
S/ 4.00
```

Resultado:

```text
Interés pagado: S/ 4.00
Interés pendiente: S/ 6.00
Capital pendiente: S/ 200.00
```

### Semana 2

Pago:

```text
S/ 12.00
```

El pago deberá cubrir:

```text
Interés pendiente anterior: S/ 6.00
Interés correspondiente: según las reglas de periodificación aplicables
Excedente: al capital
```

El sistema deberá determinar correctamente la aplicación de acuerdo con los periodos pendientes y las reglas de negocio implementadas.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 26. Prueba Integral de Morosidad

## PF-042 - Préstamo con más de dos vencimientos

**Prioridad:** Crítica

### Escenario

Un cliente posee un préstamo con tres periodos de interés vencidos.

```text
Periodo 1 → Vencido
Periodo 2 → Vencido
Periodo 3 → Vencido
```

### Resultado esperado

El sistema deberá:

1. Identificar los periodos vencidos.
2. Contabilizar tres vencimientos.
3. Determinar que se supera el límite de dos.
4. Actualizar la condición de morosidad.
5. Generar las acciones configuradas.
6. Registrar la operación correspondiente.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 27. Pruebas de Backend

Las reglas financieras deberán procesarse principalmente en el backend.

El flujo esperado será:

```text
.NET MAUI
     │
     │ Solicitud
     ▼
ASP.NET Core Web API
     │
     ├── Validación
     ├── Reglas financieras
     ├── Cálculo
     └── Transacción
            │
            ▼
      Entity Framework Core
            │
            ▼
        PostgreSQL
```

La aplicación móvil no deberá tener autoridad para modificar directamente los resultados financieros.

---

# 28. Pruebas de Manipulación de Datos

## PF-043 - Manipular tasa desde la aplicación

**Prioridad:** Crítica

### Escenario

Un usuario intenta modificar la tasa mediante una solicitud manipulada.

### Resultado esperado

El backend deberá verificar:

```text
Usuario
Permiso
Configuración
Valor recibido
```

y rechazar la operación si el usuario no está autorizado.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

## PF-044 - Manipular saldo

**Prioridad:** Crítica

### Escenario

El cliente intenta enviar directamente un nuevo saldo.

### Resultado esperado

El backend no deberá confiar en el saldo enviado por la aplicación móvil.

El saldo deberá calcularse mediante las reglas financieras del sistema.

**Resultado obtenido:** ____________________

**Estado:** ____________________

---

# 29. Matriz de Pruebas Financieras

| ID | Regla | Prioridad |
|---|---|---|
| PF-001 | Interés sobre S/ 100 | Crítica |
| PF-002 | Interés sobre S/ 500 | Crítica |
| PF-003 | Interés sobre S/ 1,000 | Crítica |
| PF-004 | Interés con decimales | Crítica |
| PF-005 | Interés sobre capital inicial | Crítica |
| PF-006 | Capital pagado no cambia base | Crítica |
| PF-007 | No interés compuesto | Crítica |
| PF-008 | Periodicidad semanal | Alta |
| PF-009 | Vencimiento semanal | Alta |
| PF-010 | Pago exacto de interés | Crítica |
| PF-011 | Pago superior al interés | Crítica |
| PF-012 | Pago inferior al interés | Crítica |
| PF-013 | Pago con excedente | Crítica |
| PF-014 | Pago total | Crítica |
| PF-015 | Pago parcial | Crítica |
| PF-016 | Pago exacto de interés | Crítica |
| PF-017 | Pago superior al saldo | Crítica |
| PF-018 | Pago negativo | Crítica |
| PF-019 | Pago cero | Alta |
| PF-020 | Capital negativo | Crítica |
| PF-021 | Capital cero | Alta |
| PF-022 | Múltiples préstamos | Crítica |
| PF-023 | Pago al préstamo correcto | Crítica |
| PF-024 | Un vencimiento | Alta |
| PF-025 | Dos vencimientos | Alta |
| PF-026 | Más de dos vencimientos | Crítica |
| PF-027 | Reactivación | Alta |
| PF-028 | Reactivación no autorizada | Alta |
| PF-029 | Pago duplicado | Crítica |
| PF-030 | Error durante pago | Crítica |
| PF-031 | Transacción de pago | Crítica |
| PF-032 | Precisión monetaria | Crítica |
| PF-033 | Tasa configurada | Crítica |
| PF-034 | Cambio de tasa | Crítica |
| PF-035 | Historial de pagos | Alta |
| PF-036 | Préstamo activo | Alta |
| PF-037 | Préstamo pagado | Crítica |
| PF-038 | Saldo pendiente | Alta |
| PF-039 | Consistencia de saldo | Crítica |
| PF-040 | Ciclo completo | Crítica |
| PF-041 | Ciclo con pago parcial | Crítica |
| PF-042 | Morosidad integral | Crítica |
| PF-043 | Manipulación de tasa | Crítica |
| PF-044 | Manipulación de saldo | Crítica |

---

# 30. Criterios de Aprobación

Las pruebas financieras deberán cumplir los siguientes criterios:

- [ ] El interés se calcula correctamente.
- [ ] La tasa inicial es del 5 %.
- [ ] El interés utiliza el capital inicial.
- [ ] No se aplica interés compuesto.
- [ ] La frecuencia es semanal.
- [ ] Los pagos se aplican primero al interés.
- [ ] Los excedentes se aplican al capital.
- [ ] Los pagos parciales funcionan correctamente.
- [ ] Los pagos inválidos son rechazados.
- [ ] Los pagos duplicados son controlados.
- [ ] Los saldos se actualizan correctamente.
- [ ] Los préstamos independientes mantienen sus propios saldos.
- [ ] La morosidad se determina correctamente.
- [ ] Más de dos intereses vencidos activa la condición correspondiente.
- [ ] La reactivación respeta los permisos y reglas.
- [ ] Las operaciones financieras mantienen integridad.
- [ ] Los cálculos utilizan precisión monetaria adecuada.
- [ ] El backend controla las reglas financieras.
- [ ] Los cambios financieros importantes quedan auditados.

---

# 31. Automatización de Pruebas Financieras

Las pruebas financieras deberán automatizarse progresivamente para reducir errores humanos y facilitar las pruebas de regresión.

Se recomienda automatizar especialmente:

```text
Cálculo de interés
        ↓
Aplicación de pagos
        ↓
Cálculo de saldo
        ↓
Morosidad
        ↓
Estados del préstamo
```

Las pruebas deberán ejecutarse cada vez que se modifiquen:

- Reglas financieras.
- Servicios de préstamos.
- Servicios de pagos.
- Cálculos.
- Modelos financieros.
- Configuración de tasas.
- Lógica de morosidad.

---

# 32. Pruebas de Regresión Financiera

Después de cualquier cambio en la lógica financiera se deberán repetir como mínimo:

```text
PF-001
PF-005
PF-007
PF-010
PF-011
PF-012
PF-014
PF-022
PF-026
PF-029
PF-031
PF-039
PF-040
```

Estos casos representan escenarios fundamentales del sistema.

---

# 33. Criterios para Liberación

No deberá liberarse una versión a producción si existe un defecto crítico relacionado con:

- Cálculo de intereses.
- Aplicación de pagos.
- Saldos.
- Morosidad.
- Integridad de datos.
- Autorización de operaciones financieras.
- Duplicación de pagos.

La liberación deberá realizarse únicamente cuando los casos financieros críticos hayan sido aprobados o exista una aceptación formal del riesgo correspondiente.

---

# 34. Evidencias

Para cada prueba financiera se recomienda conservar:

- Datos utilizados.
- Cálculo esperado.
- Resultado obtenido.
- Captura de pantalla cuando corresponda.
- Respuesta de API.
- Registro de base de datos.
- Logs relevantes.
- Estado de la prueba.

Ejemplo:

```text
Caso: PF-011
Capital inicial: S/ 100.00
Interés pendiente: S/ 5.00
Pago: S/ 20.00

Esperado:
Interés = S/ 5.00
Capital = S/ 15.00
Saldo capital = S/ 85.00

Obtenido:
____________________________________

Estado:
____________________________________
```

---

# 35. Conclusión

Las pruebas de reglas financieras constituyen uno de los componentes más críticos del proceso de calidad del Sistema de Gestión de Préstamos.

La validación deberá garantizar que el sistema mantenga de forma consistente:

```text
Capital inicial
      +
Intereses
      +
Pagos
      +
Saldos
      +
Morosidad
      +
Estados
      =
Información financiera consistente
```

El **ASP.NET Core Web API** será responsable de ejecutar y validar las reglas financieras, mientras que **Entity Framework Core** gestionará la persistencia y **PostgreSQL** almacenará la información.

La aplicación **.NET MAUI** actuará como cliente del sistema y no deberá considerarse una fuente de autoridad para los cálculos financieros.

Todas las modificaciones futuras de las reglas financieras deberán acompañarse de nuevos casos de prueba y de pruebas de regresión para asegurar que los cambios no afecten las operaciones existentes.