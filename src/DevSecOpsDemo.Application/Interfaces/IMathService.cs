using DevSecOpsDemo.Domain.Models;

namespace DevSecOpsDemo.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de operaciones matemáticas
/// </summary>
public interface IMathService
{
    Task<SumaResponse> SumarAsync(SumaRequest request);
}