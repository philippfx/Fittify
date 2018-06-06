using Fittify.Client.ApiModelRepository;
using Fittify.Web.View;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fittify.Web.Test.TestHelpers
{
    public class ClientTestServerStartup : Startup
    {
        public ClientTestServerStartup(IHostingEnvironment env) : base(env)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddScoped<IHttpRequestExecuter, HttpRequestExecuterForIntegrationTest>();
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>();
            base.Configure(app, env, loggerFactory);
        }
    }
}
