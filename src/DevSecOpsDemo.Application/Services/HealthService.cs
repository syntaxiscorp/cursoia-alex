using DevSecOpsDemo.Application.Interfaces;
using DevSecOpsDemo.Domain.Models;

namespace DevSecOpsDemo.Application.Services;

/// <summary>
/// Servicio para manejar el health check de la aplicación
/// </summary>
public class HealthService : IHealthService
{
    public async Task<HealthResponse> GetHealthStatusAsync()
    {
        // Simular una verificación asíncrona
        await Task.Delay(1);

        return new HealthResponse
        {
            Status = "ok",
            Timestamp = DateTime.UtcNow,
            Message = "DevSecOpsDemo API is running successfully"
        };
    }
}