using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Quantus.IDP.Entities;
using Quantus.IDP.Services;

namespace Quantus.IDP
{
    public class Startup
    {
        private IConfiguration AppConfiguration { get; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(IConfiguration appConfiguration, IHostingEnvironment hostingEnvironment)
        {
            // From kevindockx
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            //    .AddEnvironmentVariables();

            //Configuration = builder.Build();

            //AppConfiguration = appConfiguration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // includes appsettings.json configuartion file
                //.AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true); // includes appsettings.json configuartion file

            AppConfiguration = builder.Build();

            HostingEnvironment = hostingEnvironment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var connectionString = AppConfiguration.GetValue<string>("ConnectionStrings:DefaultConnection");
            services.AddDbContext<QuantusUserContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IQuantusUserRepository, QuantusUserRepository>();

            services.AddMvc();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddTestUsers(Config.GetUsers())
                .AddQuantusUserStore()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, QuantusUserContext quantusUserContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            quantusUserContext.Database.Migrate(); // Ensure database is created and pending migrations are executed
            quantusUserContext.EnsureSeedDataForContext();

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();


        }
    }
}
