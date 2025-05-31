using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sigma.Application.Interfaces;
using Sigma.Application.Services;
using Sigma.Domain.Interfaces.Repositories;
using Sigma.Infra.Data.Context;
using Sigma.Infra.Data.Repositories;

namespace Sigma.Infra.CrossCutting.IoC
{

    public static class ContainerService
    {
        public static IServiceCollection AddApplicationServicesCollentions(this IServiceCollection services)
        {
            services.AddServices();
            services.AddRepositories();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProjetoService, ProjetoService>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProjetoRepository, ProjetoRepository>();
            return services;
        }

        public static IServiceCollection AddApplicationContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SigmaContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Sigma.Infra.Data")));
            return services;
        }

        
    }
}
