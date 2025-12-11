# DevSecOpsDemo API

API de demostración desarrollada con .NET 9 usando Minimal API y arquitectura limpia (Clean Architecture) sin capa de infraestructura.

## Arquitectura

El proyecto está organizado en las siguientes capas:

- **DevSecOpsDemo.Api**: Capa de presentación con endpoints y configuración
- **DevSecOpsDemo.Application**: Capa de aplicación con lógica de negocio y servicios
- **DevSecOpsDemo.Domain**: Capa de dominio con modelos y excepciones

## Características

- ✅ .NET 9 con Minimal API
- ✅ Arquitectura limpia por capas
- ✅ Middleware personalizado para manejo de excepciones
- ✅ Configuración de endpoints separada del Program.cs
- ✅ Documentación automática con Swagger/OpenAPI
- ✅ Inyección de dependencias
- ✅ Logging estructurado

## Endpoints

### 1. Health Check

**GET** `/api/health`

Verifica el estado de la API.

**Respuesta exitosa (200):**
```json
{
  "status": "ok",
  "timestamp": "2024-12-11T10:30:00.000Z",
  "message": "DevSecOpsDemo API is running successfully"
}
```

### 2. Suma de Números

**POST** `/api/suma`

Realiza la suma de dos números enteros.

**Request Body:**
```json
{
  "a": 5,
  "b": 3
}
```

**Respuesta exitosa (200):**
```json
{
  "a": 5,
  "b": 3,
  "resultado": 8,
  "operacion": "suma"
}
```

**Respuesta de error (400):**
```json
{
  "error": true,
  "message": "Error de validación",
  "details": "El request no puede ser nulo",
  "timestamp": "2024-12-11T10:30:00.000Z"
}
```

## Ejecución

### Requisitos

- .NET 9 SDK

### Comandos

```bash
# Clonar o navegar al directorio del proyecto
cd DevSecOpsDemo

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run --project src/DevSecOpsDemo.Api
```

### URL de la aplicación

- **API Base**: http://localhost:5000
- **Swagger UI**: http://localhost:5000 (configurado como página de inicio)
- **Health Check**: http://localhost:5000/api/health

## Estructura del Proyecto

```
DevSecOpsDemo/
├── DevSecOpsDemo.sln
├── README.md
└── src/
    ├── DevSecOpsDemo.Api/
    │   ├── Configuration/
    │   │   └── EndpointsConfiguration.cs
    │   ├── Middleware/
    │   │   └── ExceptionHandlingMiddleware.cs
    │   ├── DevSecOpsDemo.Api.csproj
    │   ├── Program.cs
    │   ├── appsettings.json
    │   └── appsettings.Development.json
    ├── DevSecOpsDemo.Application/
    │   ├── Interfaces/
    │   │   ├── IHealthService.cs
    │   │   └── IMathService.cs
    │   ├── Services/
    │   │   ├── HealthService.cs
    │   │   └── MathService.cs
    │   └── DevSecOpsDemo.Application.csproj
    ├── DevSecOpsDemo.Domain/
    │   ├── Exceptions/
    │   │   └── ValidationException.cs
    │   ├── Models/
    │   │   ├── HealthResponse.cs
    │   │   ├── SumaRequest.cs
    │   │   └── SumaResponse.cs
    │   └── DevSecOpsDemo.Domain.csproj
    └── DevSecOpsDemo.Tests/
        ├── Infrastructure/
        │   ├── DevSecOpsApiWebApplicationFactory.cs
        │   └── IntegrationTestBase.cs
        ├── Integration/
        │   ├── HealthEndpointTests.cs
        │   ├── SumaEndpointTests.cs
        │   └── SumaEndpointErrorTests.cs
        └── DevSecOpsDemo.Tests.csproj
```

## Características Técnicas

### Middleware de Excepciones

El `ExceptionHandlingMiddleware` maneja automáticamente todas las excepciones no controladas:

- **ValidationException**: Retorna 400 (Bad Request)
- **ArgumentException**: Retorna 400 (Bad Request)
- **Otras excepciones**: Retorna 500 (Internal Server Error)

### Configuración de Endpoints

La clase `EndpointsConfiguration` centraliza toda la configuración de endpoints, manteniendo limpio el `Program.cs`.

### Validaciones

- Validación de requests nulos
- Validación de overflow en operaciones matemáticas
- Manejo de errores estructurado con mensajes descriptivos

## Ejemplos de Uso

### Probar Health Check

```bash
curl -X GET http://localhost:5000/api/health
```

### Probar Suma

```bash
curl -X POST http://localhost:5000/api/suma \
  -H "Content-Type: application/json" \
  -d '{"a": 10, "b": 5}'
```

### Probar Error de Validación

```bash
curl -X POST http://localhost:5000/api/suma \
  -H "Content-Type: application/json" \
  -d '{}'
```

## Pruebas Automatizadas

El proyecto incluye un completo conjunto de pruebas automatizadas con **xUnit** y **WebApplicationFactory** para pruebas de integración.

### Ejecutar Pruebas

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar con detalles
dotnet test --verbosity normal

# Ejecutar solo un proyecto de pruebas
dotnet test src/DevSecOpsDemo.Tests
```

### Cobertura de Pruebas

**📊 Total: 14 pruebas - Todas pasando ✅**

#### GET /api/health (3 pruebas)
- ✅ Retorna código HTTP 200 con estructura JSON correcta
- ✅ Verifica Content-Type application/json
- ✅ Valida timestamp dentro de rango razonable

#### POST /api/suma - Casos Exitosos (4 pruebas)
- ✅ Suma de números positivos
- ✅ Suma con números negativos
- ✅ Suma con cero
- ✅ Suma con números grandes (sin overflow)

#### POST /api/suma - Casos de Error (7 pruebas)
- ✅ Body nulo retorna BadRequest
- ✅ Body vacío retorna BadRequest con mensaje de error
- ✅ JSON inválido retorna BadRequest
- ✅ Content-Type faltante retorna UnsupportedMediaType
- ✅ Overflow de números retorna BadRequest con ValidationException
- ✅ JSON incompleto funciona (valores por defecto)
- ✅ Estructura de respuesta de error correcta

### Configuración de Pruebas

Las pruebas utilizan:
- **WebApplicationFactory**: Levanta la API completa en memoria
- **HttpClient**: Realiza peticiones HTTP reales
- **xUnit**: Framework de pruebas
- **Integration Tests**: Pruebas end-to-end de los endpoints

## Tecnologías Utilizadas

- .NET 9
- ASP.NET Core Minimal API
- Swagger/OpenAPI
- Clean Architecture
- Dependency Injection
- Structured Logging
- **xUnit + WebApplicationFactory (Pruebas de Integración)**