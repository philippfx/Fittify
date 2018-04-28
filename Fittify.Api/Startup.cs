using System.IO;
using System.Linq;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepositories;
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
using AspNetCoreRateLimit;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.Middleware;
using Fittify.Common.Helpers;
using Fittify.DataModelRepositories.Services;
using Fittify.Test.Core.Seed;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Fittify.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            //app.UseMiddleware<HeaderValidation>();
            app.UseFittifyHeaderValidation(Configuration);
            app.EnsureFittifyHeaderDefaultValues(Configuration);

            loggerFactory.AddDebug(LogLevel.Debug);

            // Allows cross domain api consumption
            app.UseCors(builder =>
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
            );
            
            AutoMapper.Mapper.Initialize(cfg =>
            {
                // Entity to OfmGet
                cfg.CreateMap<Workout, WorkoutOfmForGet>()
                    .ForMember(dest => dest.RangeOfWorkoutHistoryIds, opt => opt.MapFrom(src => src.WorkoutHistories.Select(s => s.Id).ToList().ToStringOfIds()))
                    .ForMember(dest => dest.RangeOfExerciseIds, opt => opt.MapFrom(src => src.MapExerciseWorkout.Select(s => s.ExerciseId.GetValueOrDefault()).ToList().ToStringOfIds()));
                cfg.CreateMap<Category, CategoryOfmForGet>()
                    .ForMember(dest => dest.RangeOfWorkoutIds, opt => opt.MapFrom(src => src.Workouts.Select(w => w.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<CardioSet, CardioSetOfmForGet>();
                cfg.CreateMap<WeightLiftingSet, WeightLiftingSetOfmForGet>();
                cfg.CreateMap<Exercise, ExerciseOfmForGet>()
                    .ForMember(dest => dest.RangeOfWorkoutIds, opt => opt.MapFrom(src => src.MapExerciseWorkout.Select(w => w.Workout.Id).ToList().ToStringOfIds()))
                    .ForMember(dest => dest.RangeOfExerciseHistoryIds, opt => opt.MapFrom(src => src.ExerciseHistories.Select(s => s.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<ExerciseHistory, ExerciseHistoryOfmForGet>()
                    .ForMember(dest => dest.Exercise, opt => opt.MapFrom(src => src.Exercise))
                    .ForMember(dest => dest.RangeOfWeightLiftingSetIds, opt => opt.MapFrom(src => src.WeightLiftingSets.Select(s => s.Id).ToList().ToStringOfIds()))
                    .ForMember(dest => dest.RangeOfCardioSetIds, opt => opt.MapFrom(src => src.CardioSets.Select(s => s.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<WorkoutHistory, WorkoutHistoryOfmForGet>()
                    .ForMember(dest => dest.Workout, opt => opt.MapFrom(src => src.Workout))
                    .ForMember(dest => dest.RangeOfExerciseHistoryIds, opt => opt.MapFrom(src => src.ExerciseHistories.Select(eH => eH.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<IncomingRawHeaders, IncomingHeaders>()
                    .ForMember(dest => dest.IncludeHateoas, opt => opt.MapFrom(src => src.IncludeHateoas.ToBool()))
                    .ForMember(dest => dest.IncludeHateoas, opt => opt.MapFrom(src => int.Parse(src.IncludeHateoas)));

                //cfg.CreateMap<CategoryOfmForPatch, Category>()
                //    .ForMember(dest => dest.Workouts, opt => opt.);

                // OfmPpp to Entity
                //cfg.CreateMap<WorkoutHistoryOfmForPatch, WorkoutHistory>();
                //cfg.IgnoreUnmapped<WorkoutHistoryOfmForPpp, WorkoutHistory>(); // does not work as expected

                // Must be last statement
                cfg.IgnoreUnmapped();
            });

            Debug.Write(string.Format("Creating a foo: {0}", JsonConvert.SerializeObject(new WeightLiftingSet())));

            app.UseIpRateLimiting();

            //app.UseResponseCaching();
            //app.UseHttpCacheHeaders();

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
                    await context.Response.WriteAsync("<h1>Environment DEVELOPMENT</h1>");
                });
            }
            if (env.IsProduction())
            {
                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("<h1>Environment PRODUCTION</h1>");
                });
            }
            if (env.IsTestInMemoryDb())
            {
                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("<h1>Environment InMemoryDb</h1>");
                });
            }
        }
    }
}
