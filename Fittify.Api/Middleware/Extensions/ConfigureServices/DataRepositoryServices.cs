using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.Repository.Sport;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Api.Middleware.Extensions.ConfigureServices
{
    public static class DataRepositoryServices
    {
        public static IServiceCollection AddFittifyDataRepositoryServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<WorkoutRepository>()
                .AddClasses(classes => classes.AssignableTo(typeof(IAsyncCrud<,,>)))
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
