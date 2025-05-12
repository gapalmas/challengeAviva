using App.Application.Services;
using App.Core.Interfaces;
using App.Infrastructure.Repository;

namespace App.API.Extension
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
        {
            // Repositorios en memoria
            services.AddScoped<IProductRepository, InMemoryProductRepository>();
            services.AddScoped<IOrderRepository, InMemoryOrderRepository>();

            // Lógica de negocio
            services.AddScoped<IOrderManager, OrderManager>();

            // Proveedores externos
            services.AddScoped<IPaymentProvider, PagaFacilProvider>();
            services.AddScoped<IPaymentProvider, CazaPagosProvider>();

            return services;
        }
    }
}
