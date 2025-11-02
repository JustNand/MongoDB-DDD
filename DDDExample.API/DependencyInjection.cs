using DDDExample.API.Settings;
using DDDExample.Application.Interfaces;
using DDDExample.Application.Mappings;
using DDDExample.Application.Services;
using DDDExample.Domain.Repositories;
using DDDExample.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DDDExample.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar MongoDB
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
            var settings = configuration.GetSection("MongoDB").Get<MongoDBSettings>();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            services.AddSingleton(database);

            // Registrar repositorios
            services.AddScoped<IProductRepository, ProductRepository>();

            // Registrar servicios de aplicación
            services.AddScoped<IProductService, ProductService>();

            // Registrar AutoMapper correctamente según versión
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return services;
        }
    }
}
