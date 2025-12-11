using DevSecOpsDemo.Domain.Models;
using DevSecOpsDemo.Tests.Infrastructure;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DevSecOpsDemo.Tests.Integration;

/// <summary>
/// Pruebas de integración para el endpoint de suma
/// </summary>
public class SumaEndpointTests : IntegrationTestBase
{
    public SumaEndpointTests(DevSecOpsApiWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task PostSuma_WithValidNumbers_ShouldReturnCorrectSum()
    {
        // Arrange
        const string endpoint = "/api/suma";
        var request = new SumaRequest { A = 10, B = 5 };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(responseContent));

        var sumaResponse = JsonSerializer.Deserialize<SumaResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(sumaResponse);
        Assert.Equal(10, sumaResponse.A);
        Assert.Equal(5, sumaResponse.B);
        Assert.Equal(15, sumaResponse.Resultado);
        Assert.Equal("suma", sumaResponse.Operacion);
    }

    [Fact]
    public async Task PostSuma_WithNegativeNumbers_ShouldReturnCorrectSum()
    {
        // Arrange
        const string endpoint = "/api/suma";
        var request = new SumaRequest { A = -10, B = 15 };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var sumaResponse = JsonSerializer.Deserialize<SumaResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(sumaResponse);
        Assert.Equal(-10, sumaResponse.A);
        Assert.Equal(15, sumaResponse.B);
        Assert.Equal(5, sumaResponse.Resultado);
    }

    [Fact]
    public async Task PostSuma_WithZero_ShouldReturnCorrectSum()
    {
        // Arrange
        const string endpoint = "/api/suma";
        var request = new SumaRequest { A = 0, B = 42 };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var sumaResponse = JsonSerializer.Deserialize<SumaResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(sumaResponse);
        Assert.Equal(0, sumaResponse.A);
        Assert.Equal(42, sumaResponse.B);
        Assert.Equal(42, sumaResponse.Resultado);
    }

    [Fact]
    public async Task PostSuma_WithLargeNumbers_ShouldReturnCorrectSum()
    {
        // Arrange
        const string endpoint = "/api/suma";
        var request = new SumaRequest { A = int.MaxValue - 1, B = 1 };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var sumaResponse = JsonSerializer.Deserialize<SumaResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(sumaResponse);
        Assert.Equal(int.MaxValue - 1, sumaResponse.A);
        Assert.Equal(1, sumaResponse.B);
        Assert.Equal(int.MaxValue, sumaResponse.Resultado);
    }
}