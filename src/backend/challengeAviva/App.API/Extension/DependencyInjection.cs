using App.Application.Mappings;
using App.Application.Services;
using App.Core.Interfaces;
using App.Infrastructure.Repository;
using AutoMapper;

namespace App.API.Extension
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
        {
            // AutoMapper 
            services.AddAutoMap();

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

        private static IServiceCollection AddAutoMap(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(MC =>
            {
                MC.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
