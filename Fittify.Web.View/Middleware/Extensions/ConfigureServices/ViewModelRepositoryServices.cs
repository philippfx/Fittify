using System.Linq;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ViewModelRepository;
using Fittify.Client.ViewModelRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Web.View.Middleware.Extensions.ConfigureServices
{
    public static class ViewModelRepositoryServices
    {
        public static IServiceCollection AddFittifyViewModelRepositoryServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                    .FromAssemblyOf<WorkoutViewModelRepository>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IViewModelRepository<,,,,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime())
                    //.AddClasses(classes => classes.AssignableTo<IScopedService>())
                    //.As<IScopedService>()
                    //.WithScopedLifetime())
                    ;

            //var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IViewModelRepository<int,WeightLiftingSetViewModel,WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>));
            //services.Remove(serviceDescriptor);

            //services.AddTransient<IViewModelRepository<int, WeightLiftingSetViewModel, WeightLiftingSetOfmForPost, WeightLiftingSetOfmResourceParameters, WeightLiftingSetOfmCollectionResourceParameters>, WeightLiftingSetViewModelRepository>();
            return services;
        }
    }
}
