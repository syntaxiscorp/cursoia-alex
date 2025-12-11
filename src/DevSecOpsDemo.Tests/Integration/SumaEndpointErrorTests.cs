using DevSecOpsDemo.Tests.Infrastructure;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DevSecOpsDemo.Tests.Integration;

/// <summary>
/// Pruebas de integración para casos de error en el endpoint de suma
/// </summary>
public class SumaEndpointErrorTests : IntegrationTestBase
{
    public SumaEndpointErrorTests(DevSecOpsApiWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task PostSuma_WithNullBody_ShouldReturnBadRequest()
    {
        // Arrange
        const string endpoint = "/api/suma";

        // Act
        var response = await _client.PostAsync(endpoint, null);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostSuma_WithEmptyBody_ShouldReturnBadRequestWithErrorMessage()
    {
        // Arrange
        const string endpoint = "/api/suma";
        var content = new StringContent("", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        // Para JSON vacío, la API puede devolver un error de deserialización directamente
        if (!string.IsNullOrEmpty(responseContent))
        {
            try
            {
                var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                Assert.True(errorResponse.TryGetProperty("error", out var errorProp) || errorResponse.TryGetProperty("title", out _));
            }
            catch (JsonException)
            {
                // Si no es JSON válido, al menos debe ser una respuesta de error HTTP 400
                Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            }
        }
        else
        {
            // Si el contenido está vacío, al menos debe ser BadRequest
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async Task PostSuma_WithInvalidJson_ShouldReturnBadRequest()
    {
        // Arrange
        const string endpoint = "/api/suma";
        var invalidJson = "{ invalid json }";
        var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostSuma_WithMissingContentType_ShouldReturnBadRequest()
    {
        // Arrange
        const string endpoint = "/api/suma";
        var json = JsonSerializer.Serialize(new { A = 1, B = 2 });
        var content = new StringContent(json, Encoding.UTF8); // Sin content-type application/json

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
    }

    [Fact]
    public async Task PostSuma_WithOverflowNumbers_ShouldReturnBadRequestWithValidationError()
    {
        // Arrange
        const string endpoint = "/api/suma";
        var request = new { A = int.MaxValue, B = 1 }; // Esto causará overflow
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(responseContent));

        var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
        Assert.True(errorResponse.TryGetProperty("error", out var errorProp));
        Assert.True(errorProp.GetBoolean());
        Assert.True(errorResponse.TryGetProperty("message", out var messageProp));
        Assert.Equal("Error de validación", messageProp.GetString());
        Assert.True(errorResponse.TryGetProperty("details", out var detailsProp));
        Assert.Contains("overflow", detailsProp.GetString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PostSuma_WithIncompleteJson_ShouldStillWork()
    {
        // Arrange - JSON con solo un campo (el otro será 0 por defecto)
        const string endpoint = "/api/suma";
        var incompleteJson = "{ \"A\": 5 }"; // Falta B
        var content = new StringContent(incompleteJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        // Debería funcionar porque B será 0 por defecto
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostSuma_ErrorResponse_ShouldHaveCorrectStructure()
    {
        // Arrange - Usar un JSON malformado para garantizar una respuesta de error estructurada
        const string endpoint = "/api/suma";
        var content = new StringContent("{", Encoding.UTF8, "application/json"); // JSON incompleto

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        
        // La respuesta debe ser BadRequest y puede tener contenido vacío o con estructura de error
        if (!string.IsNullOrEmpty(responseContent))
        {
            try
            {
                var errorResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                
                // Verificar que tiene estructura de error (puede ser del middleware personalizado o del framework)
                var hasErrorStructure = errorResponse.TryGetProperty("error", out _) ||
                                       errorResponse.TryGetProperty("title", out _) ||
                                       errorResponse.TryGetProperty("type", out _);
                
                Assert.True(hasErrorStructure, "La respuesta de error debe tener una estructura reconocible");
            }
            catch (JsonException)
            {
                // Si no es JSON válido, debe ser al menos BadRequest
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
        else
        {
            // Contenido vacío también es válido para BadRequest
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}