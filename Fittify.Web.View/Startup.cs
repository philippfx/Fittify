using System.Collections.Generic;
using System.IO;
using AspNetCore.RouteAnalyzer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fittify.Web.View
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json"); // includes appsettings.json configuartion file
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(o => o.LoginPath = new PathString("/account/login"))
            //    .AddOpenIdConnect(o =>
            //    {
            //        //o.SignInScheme = "oidc";
            //        //o.SignOutScheme = "oidc";
            //        o.Authority = "https://localhost:44364/";
            //        o.RequireHttpsMetadata = true; // ensures working over TLS
            //        o.ClientId = "fittifyclient";
            //        //o.Scope = new List<string>() { "openid", "profile" }; // READ ONLY ???
            //        o.ResponseType = "code id_token";
            //        //o.CallbackPath = new PathString(); // can be used to alternate https://localhost:44328/signin-oidc
            //        //o.SignInScheme = "Cookie";
            //        o.SignInScheme = "oidc";
            //        o.SaveTokens = true;
            //    });

            services.AddAuthentication(options => {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.Authority = "https://localhost:44364/";
                    options.RequireHttpsMetadata = true;
                    options.ClientId = "fittifyclient";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.ResponseType = "code id_token";
                    //options.CallbackPath = new PathString("...");
                    options.SignInScheme = "Cookies";
                    //options.SignOutScheme = "oidc";
                    options.SaveTokens = true;
                    options.ClientSecret = "secret";
                    options.GetClaimsFromUserInfoEndpoint = true;
                });

            services.AddMvc();
            services.AddRouteAnalyzer();
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory/*, FittifyContext fittifyContext*/)
        {

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.IgnoreUnmapped();
            });

            loggerFactory.AddConsole();

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}


            app.UseFileServer();
            // is equivalent to:
            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            app.UseAuthentication();

            //app.UseMvcWithDefaultRoute(); // or do it more explicitly:
            app.UseMvc(routes =>
            {
                routes.MapRouteAnalyzer("/routes");
                routes.MapRoute("Default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World from startup.cs If you clicked a linked after landing on this page, then the link was dead!");
            });
        }
    }
}
