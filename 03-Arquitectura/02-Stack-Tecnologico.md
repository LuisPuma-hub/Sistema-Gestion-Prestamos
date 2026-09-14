# Stack Tecnológico

## 1. Objetivo

Definir las tecnologías, frameworks, herramientas y servicios que serán utilizados para desarrollar el Sistema de Gestión de Préstamos.

---

## 2. Entorno de desarrollo

### IDE

Visual Studio Code (versión Stable de 64 bits, 1.90 o superior) como único IDE oficial y obligatorio del proyecto.

Será el entorno exclusivo para el desarrollo de:

- Aplicación móvil.
- Backend.
- Pruebas.
- Depuración (mediante `launch.json` / `tasks.json`).

No se utilizará Visual Studio 2026 ni ninguna edición de Visual Studio. Toda la documentación, scripts y tutoriales del proyecto asumen Visual Studio Code.

Requisitos complementarios obligatorios en cada máquina de desarrollo:

1. .NET SDK LTS vigente (versión exacta fijada en `global.json`, p. ej. .NET 8 LTS) con workload MAUI instalado (`dotnet workload install maui`).
2. Extensiones obligatorias de Visual Studio Code: `C# (ms-dotnettools.csharp)`, `C# Dev Kit (ms-dotnettools.csdevkit)`, `.NET MAUI (ms-dotnettools.dotnet-maui)`.
3. Android SDK + JDK compatible con MAUI para compilar y depurar en Android (plataforma inicial).
4. Git + cuenta de GitHub.

### Lenguaje principal

C#.

C# será utilizado tanto para el desarrollo de la aplicación móvil como para el backend.

---

## 3. Control de versiones

### Git

Git será utilizado para controlar las versiones del código fuente.

Permitirá:

- Registrar cambios.
- Crear ramas.
- Recuperar versiones anteriores.
- Trabajar con diferentes funcionalidades.
- Mantener un historial del proyecto.

### GitHub

GitHub será utilizado como plataforma de alojamiento del repositorio.

Permitirá:

- Almacenar el código fuente.
- Gestionar ramas.
- Registrar Issues.
- Gestionar Pull Requests.
- Mantener el historial del proyecto.
- Trabajar con GitHub Copilot y Muse Spark (vía OpenCode).

---

## 4. Inteligencia artificial para desarrollo

Se utilizarán dos asistentes de IA con roles complementarios y sin solapamiento ambiguo:

### 4.1 GitHub Copilot (asistente inline en Visual Studio Code)

GitHub Copilot será utilizado como herramienta de asistencia inline durante el desarrollo en Visual Studio Code.

Podrá utilizarse para:

- Generación de código.
- Autocompletado.
- Refactorización.
- Explicación de código.
- Generación de pruebas.
- Detección de posibles errores.
- Documentación.

### 4.2 Muse Spark vía OpenCode (agente de ingeniería)

Muse Spark, operado mediante OpenCode, será utilizado como agente de ingeniería para tareas multi-archivo y verificación.

Se utilizará obligatoriamente para:

- Revisión del proyecto sin codificar cuando se solicite.
- Edición multi-archivo trazable (Read/Grep/Glob + Edit).
- Verificación por terminal (dotnet, git, gh) y lectura de evidencia.
- Planificación con TodoWrite y delegación con Task cuando aplique.

Regla: Copilot sugiere en el editor; Muse Spark (OpenCode) ejecuta cambios transversales y los verifica. Ninguno reemplaza la validación del desarrollador.

GitHub Copilot y Muse Spark serán herramientas de apoyo y no reemplazarán la validación del desarrollador.

---

## 5. Aplicación móvil

### Tecnología

.NET MAUI.

### Lenguaje

C#.

### Arquitectura

MVVM.

.NET MAUI permitirá desarrollar la aplicación móvil utilizando una base de código común.

La aplicación estará orientada inicialmente a dispositivos Android.

La arquitectura deberá permitir considerar otras plataformas posteriormente.

---

## 6. Backend

### Framework

ASP.NET Core Web API.

### Lenguaje

C#.

### Tipo de aplicación

API REST.

El backend será responsable de:

- Autenticación.
- Autorización.
- Reglas de negocio.
- Gestión de clientes.
- Gestión de préstamos.
- Gestión de pagos.
- Notificaciones.
- WhatsApp.
- Acceso a la base de datos.

---

## 7. ORM

### Entity Framework Core

Entity Framework Core será utilizado como ORM para la comunicación entre el backend y PostgreSQL.

Sus responsabilidades principales serán:

- Mapeo de entidades.
- Consultas.
- Persistencia.
- Relaciones.
- Migraciones.
- Control de cambios.

---

## 8. Base de datos

### Motor

PostgreSQL.

### Tipo

Base de datos relacional.

PostgreSQL será utilizado para almacenar la información persistente del sistema.

Se almacenarán, entre otros:

- Usuarios.
- Clientes.
- Avales.
- Solicitudes.
- Préstamos.
- Periodos de interés.
- Pagos.
- Notificaciones.
- Plantillas de WhatsApp.
- Historial de mensajes.

---

## 9. Notificaciones

### Servicio

Firebase Cloud Messaging.

### Abreviatura

FCM.

FCM será utilizado para enviar notificaciones push al dispositivo móvil.

Permitirá enviar:

- Recordatorios de cobro.
- Avisos de interés próximo.
- Avisos de vencimiento.
- Avisos de morosidad.
- Notificaciones administrativas.

---

## 10. Comunicación mediante WhatsApp

### Plataforma

WhatsApp Business Platform.

La integración permitirá utilizar WhatsApp como canal de comunicación con los clientes.

No se desarrollará una aplicación de chat.

La integración estará orientada a:

- Recordatorios.
- Confirmaciones.
- Avisos de pago.
- Mensajes de marketing.
- Comunicación relacionada con préstamos.

---

## 11. API

La comunicación entre la aplicación móvil y el backend se realizará mediante:

- HTTPS.
- REST.
- JSON.

Ejemplo conceptual:

Aplicación móvil
→ HTTP Request
→ ASP.NET Core API
→ Procesamiento
→ JSON Response
→ Aplicación móvil.

---

## 12. Diagramas

### PlantUML

PlantUML será utilizado para documentar:

- Diagramas ER.
- Diagramas de arquitectura.
- Diagramas de secuencia.
- Diagramas de componentes.
- Otros diagramas necesarios.

---

## 13. Pruebas de API

Durante el desarrollo se utilizará una herramienta de pruebas de API para verificar:

- Endpoints.
- Requests.
- Responses.
- Autenticación.
- Errores.
- Validaciones.

La herramienta específica podrá definirse durante la implementación.

---

## 14. Resumen tecnológico

| Área | Tecnología |
|---|---|
| IDE | Visual Studio Code (Stable 64 bits, único IDE oficial) |
| Lenguaje | C# |
| Mobile | .NET MAUI |
| Arquitectura Mobile | MVVM |
| Backend | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Base de datos | PostgreSQL |
| Notificaciones | Firebase Cloud Messaging |
| WhatsApp | WhatsApp Business Platform |
| Control de versiones | Git |
| Repositorio | GitHub |
| IA de desarrollo | GitHub Copilot (inline) + Muse Spark vía OpenCode (agente) |
| Diagramas | PlantUML |

---

## 15. Principios tecnológicos

El proyecto seguirá los siguientes principios:

- Separación de responsabilidades.
- Código mantenible.
- Reutilización de componentes.
- Seguridad.
- Validación en backend.
- Uso de tipos adecuados para valores monetarios.
- Control de versiones.
- Documentación técnica.
- Preparación para escalabilidad.

Las tecnologías podrán actualizarse durante el desarrollo si aparece una alternativa técnicamente superior, siempre que el cambio sea documentado.