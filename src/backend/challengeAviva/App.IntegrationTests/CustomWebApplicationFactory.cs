using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace App.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Si deseas reemplazar servicios para pruebas (por ejemplo mocks), puedes hacerlo aquí
            builder.ConfigureServices(services =>
            {
                // Ejemplo: reemplazar servicios reales por fakes si se desea
            });

            return base.CreateHost(builder);
        }
    }
}
