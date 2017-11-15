using System.IO;
using Fittify.Api.OuterFacingModels.Sport;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fittify.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json"); // includes appsettings.json configuartion file
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var dbConnectionString = Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            services.AddDbContext<FittifyContext>(options => options.UseSqlServer(dbConnectionString));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Category, CategoryOfm>();
            });

            app.UseMvc(ConfigureRoute);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World from startup.cs If you clicked a linked after landing on this page, then the link was dead!");
            });
        }

        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default",
                "{controller=Category}/{id?}");
        }
    }
}
