using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Api.Extensions.ConfigureServices
{
    public static class CustomServices
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            //services.AddScoped<IBrowserConfigService, BrowserConfigService>();
            //services.AddScoped<IManifestService, ManifestService>();
            //services.AddScoped<IRobotsService, RobotsService>();
            //services.AddScoped<ISitemapService, SitemapService>();
            //services.AddScoped<ISitemapPingerService, SitemapPingerService>();

            // Add your own custom services here e.g.

            // Singleton - Only one instance is ever created and returned.
            //services.AddSingleton<IExampleService, ExampleService>();

            // Scoped - A new instance is created and returned for each request/response cycle.
            //services.AddScoped<IExampleService, ExampleService>();

            // Transient - A new instance is created and returned each time.
            //services.AddTransient<IExampleService, ExampleService>();

            return services;
        }
    }
}
