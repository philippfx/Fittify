using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Api.Middleware.Extensions.ConfigureServices
{
    public static class OfmRepositoryServices
    {
        public static IServiceCollection AddFittifyOfmRepositoryServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                    .FromAssemblyOf<CardioSetOfmRepository>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IAsyncOfmRepository<,,,,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime())
                    //.AddClasses(classes => classes.AssignableTo<IScopedService>())
                    //.As<IScopedService>()
                    //.WithScopedLifetime())
                    ;

            services.Scan(scan => scan
                .FromAssemblyOf<CardioSetOfmRepository>()
                .AddClasses()
                .AsSelf()
                .WithScopedLifetime());

            return services;
        }
    }
}
