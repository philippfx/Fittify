using Fittify.Client.ApiModelRepository;
using Fittify.Client.ApiModelRepository.OfmRepository.Sport;
using Fittify.DataModelRepository.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Web.View.Middleware.Extensions.ConfigureServices
{
    public static class ApiModelRepositoryServices
    {
        public static IServiceCollection AddFittifyApiModelRepositoryServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<WorkoutApiModelRepository>()
                .AddClasses(classes => classes.AssignableTo(typeof(IApiModelRepository<,,,>)))
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
