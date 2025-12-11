using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevSecOpsDemo.Tests.Infrastructure;

/// <summary>
/// Factory personalizada para pruebas de integración de la API
/// </summary>
public class DevSecOpsApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Configuraciones específicas para pruebas si fuera necesario
            // Por ejemplo, usar base de datos en memoria, mocks, etc.
            
            // Configurar logging para pruebas
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Warning);
            });
        });

        builder.UseEnvironment("Testing");
    }
}