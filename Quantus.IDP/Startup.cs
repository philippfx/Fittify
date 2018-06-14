using System.IO;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Quantus.IDP.Entities;
using Quantus.IDP.Entities.Default;
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

            if (hostingEnvironment.IsProduction())
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);
                //.AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true); // includes appsettings.json configuartion file

                AppConfiguration = builder.Build();
            }
            else
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // includes appsettings.json configuartion file
                //.AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true); // includes appsettings.json configuartion file

                AppConfiguration = builder.Build();
            }

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

            services.AddIdentity<QuantusUser, QuantusRole>()
                .AddEntityFrameworkStores<QuantusUserContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                //facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                //facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                facebookOptions.AppId = "587753821609898";
                facebookOptions.AppSecret = "b929897c7e523d4293ce599ef1739190";
            });

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                googleOptions.ClientId = "945734666949-t5pp08tu9jodh77b5dh5bsj3q1c6h457.apps.googleusercontent.com";
                googleOptions.ClientSecret = "rFxtQ5VKebOMt3lv7lOQgnFO";
            });

            //services.AddIdentity<QuantusUser, IdentityRole>()
            //    //.AddEntityFrameworkStores<QuantusUserContext>()
            //    .AddDefaultTokenProviders();

            //services.AddAuthentication().AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = "1741305999290145";
            //    facebookOptions.AppSecret = "b3b626a2ef941df5ad8bce152a56a5d2";
            //    facebookOptions.CallbackPath = new PathString("/signin-facebook");
            //});

            //services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //googleOptions.ClientId = AppConfiguration["Authentication:Google:ClientId"];
            //googleOptions.ClientSecret = AppConfiguration["Authentication:Google:ClientSecret"];
            //    googleOptions.ClientId = "945734666949-t5pp08tu9jodh77b5dh5bsj3q1c6h457.apps.googleusercontent.com";
            //    googleOptions.ClientSecret = "rFxtQ5VKebOMt3lv7lOQgnFO";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, QuantusUserContext quantusUserContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}
            
            //quantusUserContext.Database.Migrate(); // Ensure database is created and pending migrations are executed
            //quantusUserContext.EnsureSeedDataForContext();

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();


        }
    }
}
