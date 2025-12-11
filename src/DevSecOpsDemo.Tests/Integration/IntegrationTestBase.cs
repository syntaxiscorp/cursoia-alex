using DevSecOpsDemo.Tests.Infrastructure;

namespace DevSecOpsDemo.Tests.Integration;

/// <summary>
/// Clase base para todas las pruebas de integración
/// </summary>
public class IntegrationTestBase : IClassFixture<DevSecOpsApiWebApplicationFactory>
{
    protected readonly HttpClient _client;
    protected readonly DevSecOpsApiWebApplicationFactory _factory;

    public IntegrationTestBase(DevSecOpsApiWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
}