# 🚀 DevSecOpsDemo - .NET 9 Minimal API with CI Pipeline

Una API moderna construida con **.NET 9 Minimal API** implementando **Arquitectura Limpia** y **Continuous Integration (CI)** con artifacts descargables.

## 📋 Características

### 🏗️ Arquitectura
- ✅ **.NET 9** con **Minimal API**
- ✅ **Clean Architecture** (Domain, Application, API)
- ✅ **Dependency Injection** nativo
- ✅ **Middleware personalizado** para manejo de excepciones
- ✅ **Validación de requests** con Data Annotations

### 🧪 Testing & Quality
- ✅ **14 Tests automatizados** con xUnit
- ✅ **Integration Testing** con WebApplicationFactory
- ✅ **100% Test Coverage** en endpoints críticos
- ✅ **Validación de edge cases**

### 🔄 CI Pipeline
- ✅ **GitHub Actions** para automatización
- ✅ **Build Artifacts** descargables desde GitHub
- ✅ **Automated Testing** en cada PR y main branch
- ✅ **Code Quality Gates** con análisis estático
- ✅ **Security Scanning** con CodeQL
- ✅ **Multi-platform builds** (Windows, Linux, macOS)

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

## 🔄 CI Pipeline - Build & Download

### CI Workflow Features

**🔀 Pull Request Validation:**
- ✅ Code quality validation
- ✅ All tests execution (14 tests)
- ✅ Build verification (Release mode)
- ✅ **PR Build Artifact** generation

**🌿 Main Branch Pipeline:**
- ✅ Multi-platform builds (Windows, Linux, macOS)
- ✅ Security analysis with CodeQL
- ✅ Code quality gates
- ✅ **Release Build Artifacts** generation
- ✅ NuGet packages caching

### 📦 Downloading Build Artifacts

#### From GitHub Actions:
1. **Navigate** to your repository on GitHub
2. **Click** on "Actions" tab
3. **Select** the completed workflow run
4. **Download** artifacts in the "Artifacts" section:
   - `devsecopsdemo-windows` - Windows build
   - `devsecopsdemo-linux` - Linux build  
   - `devsecopsdemo-macos` - macOS build
   - `pr-build-{number}-{sha}` - PR builds

#### Artifact Contents:
- ✅ **Compiled API** ready to run
- ✅ **Dependencies** included
- ✅ **Configuration** files
- ✅ **Build info** with version details
- ✅ **Platform-specific** executables

### Pipeline Stages

1. **🔍 Code Quality** - Linting & static analysis
2. **🏗️ Multi-Platform Build** - Windows, Linux, macOS
3. **🧪 Automated Testing** - All 14 tests
4. **🛡️ Security Scan** - CodeQL analysis  
5. **📦 Package Artifacts** - Downloadable builds
6. **⬆️ Upload Builds** - Available for download

### 📋 Workflows Implementados

#### 1. **CI/CD Principal** (`.github/workflows/ci-cd.yml`)
Se ejecuta en `push` y `pull_request` en ramas `main` y `develop`:

**🏗️ Build and Test:**
- ✅ Checkout del código
- ✅ Setup .NET 9
- ✅ Cache de paquetes NuGet
- ✅ Restaurar dependencias (`dotnet restore`)
- ✅ Compilar en modo Release (`dotnet build`)
- ✅ Ejecutar pruebas (`dotnet test`)
- ✅ Generar artefactos de build
- ✅ Reportes de pruebas con resultados

**🔍 Code Quality:**
- ✅ Análisis estático de código
- ✅ CodeQL Security Analysis
- ✅ Análisis de vulnerabilidades

**🔒 Security Scan:**
- ✅ Escaneo de vulnerabilidades de dependencias
- ✅ Detección de paquetes desactualizados
- ✅ Análisis de seguridad automático

### 🎯 CI Pipeline Benefits

**✨ Build Automation:**
- Multi-platform builds on every push
- Automated testing on PRs
- Downloadable artifacts generation
- Zero-configuration setup

**🛡️ Security & Quality:**
- CodeQL security analysis
- Vulnerability scanning
- Quality gates on PRs
- Code coverage tracking

**📦 Artifact Management:**
- Windows, Linux, macOS builds
- Self-contained executables
- PR-specific build artifacts
- 14-day retention policy

**🔧 Developer Experience:**
- **No Deployment Complexity**: Just download and run
- **Local Testing**: Ready-to-use builds
- **Cross-Platform**: Works everywhere

### 🚀 Using the CI Pipeline

**GitHub Actions (Automatic):**
```bash
# Trigger full CI pipeline
git push origin main

# Trigger PR validation
git push origin feature-branch
# Create Pull Request → GitHub → main

# Check build results and download artifacts
# GitHub → Actions → Workflow runs → Artifacts section
```

**Local Testing Scripts:**
```bash
# Linux/macOS - Run all tests
./test.sh

# Windows - Run all tests  
.\test.ps1

# With code coverage
./test.sh --coverage
.\test.ps1 -Coverage

# Watch mode (development)
./test.sh --watch
.\test.ps1 -Watch

# Filter specific project
./test.sh --project "HealthEndpointTests"
.\test.ps1 -Project "HealthEndpointTests"

# Show help
./test.sh --help
.\test.ps1 -Help
```

#### 📈 CI Metrics & Reports

The pipeline automatically generates:
- 📊 **Test Reports**: Detailed test results
- 📈 **Code Coverage**: Coverage percentages
- 🔍 **Security Reports**: Vulnerability findings  
- 📦 **Build Artifacts**: Ready-to-run executables
- ⚡ **Performance Data**: Response time metrics

### 📁 CI Pipeline Files

```
.github/workflows/
├── ci-cd.yml              # Main CI pipeline
└── pr-validation.yml      # PR-specific validation

# Utility scripts
test.sh                    # Linux/macOS testing script
test.ps1                   # Windows PowerShell script
CodeCoverage.runsettings   # Code coverage configuration
```

## ✅ Project Status

🎉 **Fully functional project with:**
- ✅ REST API with .NET 9 and Minimal API
- ✅ Clean Architecture by layers
- ✅ 14 automated tests (100% passing)
- ✅ Exception handling middleware
- ✅ Complete CI pipeline with GitHub Actions
- ✅ Automatic security analysis
- ✅ Code coverage reporting
- ✅ **Downloadable build artifacts**
- ✅ Cross-platform support
- ✅ Local automation scripts
- ✅ Complete documentation

## 🚀 Running Downloaded Builds

### Windows Build:
```cmd
# Extract devsecopsdemo-windows.zip
cd extracted-folder
DevSecOpsDemo.Api.exe
```

### Linux/macOS Build:
```bash
# Extract devsecopsdemo-linux.tar.gz
cd extracted-folder
chmod +x DevSecOpsDemo.Api
./DevSecOpsDemo.Api
```

### Alternative with .NET Runtime:
```bash
dotnet DevSecOpsDemo.Api.dll
```

## 🤝 Contributing

1. Fork the project
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request
6. **Download PR build** from GitHub Actions to test

---

**🎯 Built with ❤️ using .NET 9, Clean Architecture, and CI best practices**
**📦 Ready-to-download builds available on every commit!**