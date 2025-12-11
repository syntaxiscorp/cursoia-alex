using DevSecOpsDemo.Application.Interfaces;
using DevSecOpsDemo.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevSecOpsDemo.Api.Configuration;

/// <summary>
/// Clase estática para configurar todos los endpoints de la API
/// </summary>
public static class EndpointsConfiguration
{
    /// <summary>
    /// Configura todos los endpoints de la aplicación
    /// </summary>
    /// <param name="app">La aplicación web</param>
    public static void ConfigureEndpoints(this WebApplication app)
    {
        ConfigureHealthEndpoints(app);
        ConfigureMathEndpoints(app);
    }

    /// <summary>
    /// Configura los endpoints relacionados con health check
    /// </summary>
    /// <param name="app">La aplicación web</param>
    private static void ConfigureHealthEndpoints(WebApplication app)
    {
        app.MapGet("/api/health", async (IHealthService healthService) =>
        {
            var healthResponse = await healthService.GetHealthStatusAsync();
            return Results.Ok(healthResponse);
        })
        .WithName("GetHealth")
        .WithTags("Health")
        .WithSummary("Verificar el estado de la API")
        .WithDescription("Endpoint para verificar que la API está funcionando correctamente")
        .Produces<HealthResponse>(200)
        .WithOpenApi();
    }

    /// <summary>
    /// Configura los endpoints relacionados con operaciones matemáticas
    /// </summary>
    /// <param name="app">La aplicación web</param>
    private static void ConfigureMathEndpoints(WebApplication app)
    {
        app.MapPost("/api/suma", async ([FromBody] SumaRequest request, IMathService mathService) =>
        {
            if (request == null)
            {
                return Results.BadRequest(new 
                { 
                    error = true,
                    message = "El body de la request no puede estar vacío",
                    timestamp = DateTime.UtcNow
                });
            }

            var response = await mathService.SumarAsync(request);
            return Results.Ok(response);
        })
        .WithName("PostSuma")
        .WithTags("Math")
        .WithSummary("Realizar suma de dos números")
        .WithDescription("Endpoint para sumar dos números enteros (A + B)")
        .Accepts<SumaRequest>("application/json")
        .Produces<SumaResponse>(200)
        .Produces(400)
        .WithOpenApi();
    }
}