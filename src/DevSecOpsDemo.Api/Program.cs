using DevSecOpsDemo.Api.Configuration;
using DevSecOpsDemo.Api.Middleware;
using DevSecOpsDemo.Application.Interfaces;
using DevSecOpsDemo.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "DevSecOps Demo API", 
        Version = "v1",
        Description = "API de demostración con arquitectura limpia usando Minimal API"
    });
});

// Registrar servicios de aplicación
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IMathService, MathService>();

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configurar pipeline de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevSecOps Demo API V1");
        c.RoutePrefix = string.Empty; // Swagger UI en la raíz
    });
}

// Middleware personalizado para manejo de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Configurar endpoints usando la clase dedicada
app.ConfigureEndpoints();

// Mensaje de inicio
app.Logger.LogInformation("DevSecOps Demo API iniciada correctamente");

app.Run();

// Hacer la clase Program accesible para las pruebas
public partial class Program { }