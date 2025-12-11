namespace DevSecOpsDemo.Domain.Models;

/// <summary>
/// Modelo para la solicitud del endpoint de suma
/// </summary>
public class SumaRequest
{
    public int A { get; set; }
    public int B { get; set; }
}