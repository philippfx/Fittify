using Fittify.Api.OfmRepository;
using Fittify.Api.OfmRepository.Sport;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Api.Extensions.ConfigureServices
{
    public static class GppdRepositoryServices
    {
        public static IServiceCollection AddFittifyGppdRepositoryServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                    .FromAssemblyOf<CardioSetOfmRepository>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IAsyncGppd<,,,,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime())
                    //.AddClasses(classes => classes.AssignableTo<IScopedService>())
                    //.As<IScopedService>()
                    //.WithScopedLifetime())
                    ;

            return services;
        }
    }
}
