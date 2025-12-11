using DevSecOpsDemo.Application.Interfaces;
using DevSecOpsDemo.Domain.Exceptions;
using DevSecOpsDemo.Domain.Models;

namespace DevSecOpsDemo.Application.Services;

/// <summary>
/// Servicio para operaciones matemáticas
/// </summary>
public class MathService : IMathService
{
    public async Task<SumaResponse> SumarAsync(SumaRequest request)
    {
        // Validación básica
        if (request == null)
        {
            throw new ValidationException("El request no puede ser nulo");
        }

        // Validación de overflow
        try
        {
            var resultado = checked(request.A + request.B);
            
            await Task.Delay(1); // Simular operación asíncrona

            return new SumaResponse
            {
                A = request.A,
                B = request.B,
                Resultado = resultado,
                Operacion = "suma"
            };
        }
        catch (OverflowException)
        {
            throw new ValidationException($"La suma de {request.A} + {request.B} produce un overflow");
        }
    }
}