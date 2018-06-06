using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.RouteAnalyzer;
using Fittify.Client.ApiModelRepositories;
using Fittify.Web.View.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Fittify.Web.View
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            ////var currentDirectory = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                ////.SetBasePath(@"C:\VS_2017_Projects\Fittify\Fittify.Web.View")
                .AddJsonFile("appsettings.json"); // includes appsettings.json configuartion file
            Configuration = builder.Build();

            HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options => {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/authorization/accessdenied";
                })
                .AddOpenIdConnect(options =>
                {
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Authority = "https://localhost:44364/";
                    options.RequireHttpsMetadata = true;
                    options.ClientId = "fittifyclient";
                    options.ClientSecret = "secret";
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("address");
                    options.Scope.Add("roles");
                    options.Scope.Add("fittifyapi");
                    options.Scope.Add("subscriptionlevel");
                    options.Scope.Add("country");
                    options.Scope.Add("offline_access");
                    options.ClaimActions.MapCustomJson("role", jobj => jobj["role"]?.ToString());
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "role"
                    };
                    options.ClaimActions.MapJsonKey("country", "country");
                    options.ClaimActions.MapUniqueJsonKey("subscriptionlevel", "subscriptionlevel");

                    options.ResponseType = "code id_token";
                    //options.CallbackPath = new PathString("..."); // default is "https://[fittifywebhost]/signin-oidc
                    //options.SignedOutCallbackPath = new PathString("...");
                    options.SignInScheme = "Cookies";
                    //options.SignOutScheme = "oidc";
                    options.SaveTokens = true; // saves tokens in memory to reuse them

                    // In Asp.Net Core 2.0, redundant claims are deleted before returning the token. You can remove this "deletion", for example for "amr" claims, like this:
                    //options.ClaimActions.Remove("amr");
                    // The following code does not apply to asp.net core 2.0 anymore
                    options.Events = new OpenIdConnectEvents()
                    {
                        OnTokenValidated = tokenValidatedContext =>
                        {

                            //var identity = tokenValidatedContext.Principal.Identity as ClaimsIdentity;
                            //var subjectClaim = identity.Claims.FirstOrDefault(z => z.Type == "sub");
                            //var newClaimsIdentity = new ClaimsIdentity(
                            //    tokenValidatedContext.Scheme);
                            //Add claims
                            //var claims = new List<Claim>
                            //{
                            //    new Claim(ClaimTypes.Role, "role")
                            //};
                            //var claimsIdentity = new ClaimsIdentity(claims);
                            //tokenValidatedContext.Principal.AddIdentity(claimsIdentity);

                            //[...]

                            return Task.FromResult(0);
                        },
                        OnUserInformationReceived = userInformationReceivedContext =>
                        {
                            //userInformationReceivedContext.User.Remove("iss");
                            //userInformationReceivedContext.User.Remove("iat");
                            //userInformationReceivedContext.User.Remove("address");
                            //userInformationReceivedContext.User.Remove("country");
                            //userInformationReceivedContext.User.Remove("subscriptionlevel");

                            ClaimsIdentity claimsIdentity = userInformationReceivedContext.Principal.Identity as ClaimsIdentity;

                            var user = userInformationReceivedContext.User;
                            var children = user.Children();
                            var claims = userInformationReceivedContext.User.Children().Values().ToList();
                            //claimsIdentity.AddClaims(claims.Select(r => new Claim(JwtClaimTypes., r.Value<String>())));
                            var country = claims.FirstOrDefault(j => j.Path == "country")?.Value<string>();
                            //var countryClaim = new Claim("country", claims.FirstOrDefault(j => j.Path == "country")?.Value<string>());
                            var subscriptionlevelClaim = new Claim("subscriptionlevel", claims.FirstOrDefault(j => j.Path == "subscriptionlevel")?.Value<string>());
                            if (!String.IsNullOrWhiteSpace(country)) claimsIdentity.AddClaim(new Claim("country", country));
                            claimsIdentity.AddClaim(subscriptionlevelClaim);

                            return Task.FromResult(0);
                        }
                    };
                });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthorization(authOptions =>
            {
                authOptions.AddPolicy(
                    "PayingUserFromNL",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("country", "nl"/*, "be", "..."*/);
                        policyBuilder.RequireClaim("subscriptionlevel", "PayingUser");
                        //policyBuilder.RequireRole("PayingUser");
                    });
            });

            services.AddMvc();
            services.AddRouteAnalyzer();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IHttpRequestExecuter, HttpRequestExecuter>();
            services.AddScoped<IHttpRequestBuilder, HttpRequestBuilder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory/*, FittifyContext fittifyContext*/)
        {
            if (!env.IsClientTestServer()) // need to exclude automapper initialization for full integration test of client app, because mapper is also initialized in Api Startup 
            {
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.IgnoreUnmapped();
                });
            }

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
