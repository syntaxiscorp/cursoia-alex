namespace DevSecOpsDemo.Domain.Models;

/// <summary>
/// Modelo para la respuesta del endpoint de health
/// </summary>
public class HealthResponse
{
    public string Status { get; set; } = "ok";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Message { get; set; } = "Service is running";
}