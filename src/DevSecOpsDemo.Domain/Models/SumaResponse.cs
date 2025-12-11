namespace DevSecOpsDemo.Domain.Models;

/// <summary>
/// Modelo para la respuesta del endpoint de suma
/// </summary>
public class SumaResponse
{
    public int A { get; set; }
    public int B { get; set; }
    public int Resultado { get; set; }
    public string Operacion { get; set; } = "suma";
}