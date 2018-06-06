using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Web.View.Middleware.Extensions.ConfigureServices
{
    public static class ViewModelRepositoryServices
    {
        public static IServiceCollection AddFittifyViewModelRepositoryServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                    .FromAssemblyOf<WorkoutViewModelRepository>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IViewModelRepository<,,,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime())
                    //.AddClasses(classes => classes.AssignableTo<IScopedService>())
                    //.As<IScopedService>()
                    //.WithScopedLifetime())
                    ;


            return services;
        }
    }
}
