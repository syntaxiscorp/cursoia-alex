using DevSecOpsDemo.Domain.Models;

namespace DevSecOpsDemo.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de health check
/// </summary>
public interface IHealthService
{
    Task<HealthResponse> GetHealthStatusAsync();
}