using System.IO;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AspNetCoreRateLimit;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.Middleware;
using Fittify.Api.Middleware.Extensions.ConfigureServices;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport;
using Fittify.Api.OfmRepository.OfmRepository.Sport;
using Fittify.Api.OfmRepository.Services;
using Fittify.Api.OfmRepository.Services.PropertyMapping;
using Fittify.Api.OfmRepository.Services.TypeHelper;
using Fittify.Api.Services.ConfigureServices;
using Fittify.DataModelRepository;
using IdentityServer4.AccessTokenValidation;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Fittify.DbResetter.Seed;

namespace Fittify.Api
{
    [ExcludeFromCodeCoverage] 
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(/*IConfiguration configuration, */IHostingEnvironment hostingEnvironment)
        {
            //Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json"); // includes appsettings.json configuartion file
            Configuration = builder.Build();

            HostingEnvironment = hostingEnvironment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Fittify API", Version = "v1" });
            });

            services.AddCors();

            //services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton(Configuration);

            services.AddAuthentication(
                    IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44364/"; // Auth Server
                    options.RequireHttpsMetadata = true; // only for development
                    options.ApiName = "fittifyapi"; // API Resource Id
                    options.ApiSecret = "apisecret";
                });

            services.AddMvc
                (setupAction =>
                {
                    setupAction.ReturnHttpNotAcceptable = true; // returns 406 for header "Accept application/xml2" for example (or any other unsupported content type)
                    setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                })
                .AddJsonOptions(options => // ensures camel cased properties for data shaped properties
                {
                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                });
                ////.ConfigureApplicationPartManager(p => // supports generic controllers 
                ////    p.FeatureProviders.Add(new GenericControllerFeatureProvider())); 



            string dbConnectionString = Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

            if (HostingEnvironment.IsTestInMemoryDb())
            {
                services.AddDbContext<FittifyContext>(options => options.UseSqlite("Data Source = FittifyTestDb.db"));
                var intermediateServiceProvider = services.BuildServiceProvider();
                using (var fittifyContext = intermediateServiceProvider.GetService<FittifyContext>())
                {
                    fittifyContext.Database.EnsureDeleted();
                    fittifyContext.Database.EnsureCreated();
                    FittifyContextSeeder.Seed(fittifyContext);
                }
            }
            else if (HostingEnvironment.IsProduction())
            {
                dbConnectionString = Configuration.GetValue<string>("ConnectionStrings:SmarterAspConnection");
                services.AddDbContext<FittifyContext>(options => options.UseSqlServer(dbConnectionString));
            }
            else
            {
                services.AddDbContext<FittifyContext>(options => options.UseSqlServer(dbConnectionString));
            }
            
            //try
            //{
            //    services.AddAuthorization(authorizationOptions =>
            //    {
            //        authorizationOptions.AddPolicy(
            //            "MustOwnWorkout",
            //            policyBuilder =>
            //            {
            //                policyBuilder.RequireAuthenticatedUser();
            //                policyBuilder.AddRequirements(new MustOwnWorkoutRequirement());
            //            });
            //    });
            //    services.AddScoped<IAuthorizationHandler, MustOwnWorkoutHandler>();
            //}
            //catch (Exception e)
            //{
            //    var msg = e.Message;
            //}

            //try
            //{
            //    services.AddAuthorization(authorizationOptions =>
            //    {
            //        authorizationOptions.AddPolicy(
            //            "MustOwnEntityIntId",
            //            policyBuilder =>
            //            {
            //                policyBuilder.RequireAuthenticatedUser();
            //                policyBuilder.AddRequirements(new MustOwnEntityIntIdRequirement());
            //            });
            //    });
            //    services.AddScoped<IAuthorizationHandler, MustOwnEntityIntIdHandler>();
            //    //services.AddScoped(typeof(IAuthorizationHandlerT<,,>), typeof(MustOwnEntityHandler<,,>));
            //    //services.AddScoped(typeof(IAuthorizationHandler), typeof(MustOwnEntityHandler<,,>));
            //}
            //catch (Exception e)
            //{
            //    var msg = e.Message;
            //}


            // Is required for the UrlHelper, because it creates url to ACTIONS
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = 
                    implementationFactory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddHttpCacheHeaders(
                expirationModelOptions =>
                {
                    expirationModelOptions.MaxAge = 600;
                },
                validationModelOptions =>
                {
                    validationModelOptions.AddMustRevalidate = true;
                });

            //services.AddResponseCaching();

            services.AddMemoryCache(); // Necessary for rate limit

            if (!HostingEnvironment.IsDevelopment() && !HostingEnvironment.IsTestInMemoryDb())
            {
                services.Configure<IpRateLimitOptions>((options) =>
                {
                    options.GeneralRules = new System.Collections.Generic.List<RateLimitRule>()
                    {
                        new RateLimitRule()
                        {
                            Endpoint = "*",
                            Limit = 500,
                            Period = "1m"
                        },
                        new RateLimitRule()
                        {
                            Endpoint = "*",
                            Limit = 300,
                            Period = "10s"
                        }
                    };
                });
                services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
                services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            }

            
            services.AddFittifyDataRepositoryServices();
            services.AddFittifyOfmRepositoryServices();
            services.AddScoped<IAsyncOfmRepositoryForWorkoutHistory, WorkoutHistoryOfmRepository>();
            services.AddScoped<IAsyncOfmRepositoryForWorkout, WorkoutOfmRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment() || env.IsTestInMemoryDb())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async httpContext =>
                    {
                        // Required for logging exceptions when it is not caught later in code, see plural sight api course by https://app.pluralsight.com/library/courses/asp-dot-net-core-restful-api-building/
                        var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                        }

                        httpContext.Response.StatusCode = 500;
                        await httpContext.Response.WriteAsync("An unexpected error happened. Please try again later.");
                    });
                });
            }
            
            app.UseFittifyHeaderValidation(Configuration);
            app.EnsureFittifyHeaderDefaultValues(Configuration);

            loggerFactory.AddDebug(LogLevel.Debug);

            // Allows cross domain api consumption
            app.UseCors(builder =>
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
            );
            
            AutoMapperForFittify.Initialize();

            Debug.Write(string.Format("Creating a foo: {0}", JsonConvert.SerializeObject(new WeightLiftingSet())));

            if (!HostingEnvironment.IsDevelopment() && !HostingEnvironment.IsTestInMemoryDb())
            {
                app.UseIpRateLimiting();
            }

            //app.UseResponseCaching();
            //app.UseHttpCacheHeaders();

            app.UseAuthentication();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fittify API V1");
            });

            if (env.IsDevelopment())
            {
                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("<h1>Environment Development</h1>");
                });
            }
            if (env.IsProduction())
            {
                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("<h1>Environment Production</h1>");
                });
            }
            if (env.IsTestInMemoryDb())
            {
                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("<h1>Environment TestInMemoryDb</h1>");
                });
            }
        }
    }
}
