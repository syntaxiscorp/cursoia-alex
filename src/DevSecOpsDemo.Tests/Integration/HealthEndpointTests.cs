using DevSecOpsDemo.Domain.Models;
using DevSecOpsDemo.Tests.Infrastructure;
using System.Net;
using System.Text.Json;
using Xunit;

namespace DevSecOpsDemo.Tests.Integration;

/// <summary>
/// Pruebas de integración para el endpoint de health check
/// </summary>
public class HealthEndpointTests : IntegrationTestBase
{
    public HealthEndpointTests(DevSecOpsApiWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetHealth_ShouldReturnOk_WithCorrectStatusInformation()
    {
        // Arrange
        const string endpoint = "/api/health";

        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(content));

        var healthResponse = JsonSerializer.Deserialize<HealthResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(healthResponse);
        Assert.Equal("ok", healthResponse.Status);
        Assert.NotEqual(default(DateTime), healthResponse.Timestamp);
        Assert.Contains("DevSecOpsDemo API is running successfully", healthResponse.Message);
    }

    [Fact]
    public async Task GetHealth_ShouldReturnCorrectContentType()
    {
        // Arrange
        const string endpoint = "/api/health";

        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("application/json", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task GetHealth_ShouldReturnTimestampWithinReasonableTime()
    {
        // Arrange
        const string endpoint = "/api/health";
        var beforeRequest = DateTime.UtcNow;

        // Act
        var response = await _client.GetAsync(endpoint);
        var afterRequest = DateTime.UtcNow;

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var healthResponse = JsonSerializer.Deserialize<HealthResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(healthResponse);
        Assert.True(healthResponse.Timestamp >= beforeRequest.AddSeconds(-1));
        Assert.True(healthResponse.Timestamp <= afterRequest.AddSeconds(1));
    }
}