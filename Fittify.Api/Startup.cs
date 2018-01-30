using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Fittify.Api.OfmRepository;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models.Sport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace Fittify.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json"); // includes appsettings.json configuartion file
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true; // returns 406 for header "Accept applicatioon/xml2" for example (or any other unsupported content type)
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            }); 

            var dbConnectionString = Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            services.AddDbContext<FittifyContext>(options => options.UseSqlServer(dbConnectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
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
            
            loggerFactory.AddDebug(LogLevel.Debug);

            // Allows cross domain api consumption
            app.UseCors(builder =>
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
            );

            AutoMapper.Mapper.Initialize(cfg =>
            {
                // Entity to Ofm
                cfg.CreateMap<Workout, WorkoutOfmForGet>()
                    .ForMember(dest => dest.RangeOfWorkoutHistoryIds, opt => opt.MapFrom(src => src.WorkoutHistories.Select(s => s.Id).ToList().ToStringOfIds()))
                    .ForMember(dest => dest.RangeOfExerciseIds, opt => opt.MapFrom(src => src.ExercisesWorkoutsMap.Select(s => s.ExerciseId).ToList().ToStringOfIds()));
                cfg.CreateMap<Category, CategoryOfmForGet>()
                    .ForMember(dest => dest.RangeOfWorkoutIds, opt => opt.MapFrom(src => src.Workouts.Select(w => w.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<CardioSet, CardioSetOfmForGet>()
                    .ForMember(dest => dest.DateTimeStart, opt => opt.MapFrom(src => src.DateTimeStartEnd.DateTimeStart))
                    .ForMember(dest => dest.DateTimeEnd, opt => opt.MapFrom(src => src.DateTimeStartEnd.DateTimeEnd));
                cfg.CreateMap<WeightLiftingSet, WeightLiftingSetOfmForGet>();
                cfg.CreateMap<Exercise, ExerciseOfmForGet>()
                    .ForMember(dest => dest.RangeOfWorkoutIds, opt => opt.MapFrom(src => src.ExercisesWorkoutsMap.Select(w => w.Workout.Id).ToList().ToStringOfIds()))
                    .ForMember(dest => dest.RangeOfExerciseHistoryIds, opt => opt.MapFrom(src => src.ExerciseHistories.Select(s => s.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<ExerciseHistory, ExerciseHistoryOfmForGet>()
                    .ForMember(dest => dest.Exercise, opt => opt.MapFrom(src => src.Exercise))
                    .ForMember(dest => dest.RangeOfWeightLiftingSetIds, opt => opt.MapFrom(src => src.WeightLiftingSets.Select(s => s.Id).ToList().ToStringOfIds()))
                    .ForMember(dest => dest.RangeOfCardioSetIds, opt => opt.MapFrom(src => src.CardioSets.Select(s => s.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<WorkoutHistory, WorkoutHistoryOfmForGet>()
                    .ForMember(dest => dest.DateTimeStart, opt => opt.MapFrom(src => src.DateTimeStartEnd.DateTimeStart))
                    .ForMember(dest => dest.DateTimeEnd, opt => opt.MapFrom(src => src.DateTimeStartEnd.DateTimeEnd))
                    .ForMember(dest => dest.Workout, opt => opt.MapFrom(src => src.Workout))
                    .ForMember(dest => dest.ExerciseHistoryIds, opt => opt.MapFrom(src => src.ExerciseHistories.Select(eH => eH.Id)));

                // OfmGet to Entity
                //cfg.CreateMap<WorkoutHistoryOfmForGet, WorkoutHistory>()
                //    .ForMember(dest => dest.WorkoutId, opt => opt.MapFrom(src => src.Workout))
                //    .ForMember(dest => dest.Id, opt => opt.Ignore());
                //cfg.CreateMap<WeightLiftingSetOfmForGet, WeightLiftingSet>()
                //    .ForMember(dest => dest.Id, opt => opt.Ignore());
                //cfg.CreateMap<ExerciseHistoryOfmForGet, ExerciseHistory>()
                //    .ForMember(dest => dest.Id, opt => opt.Ignore());

                // OfmPpp to Entity
                cfg.CreateMap<WorkoutHistoryOfmForPatch, WorkoutHistory>();
                //cfg.IgnoreUnmapped<WorkoutHistoryOfmForPpp, WorkoutHistory>(); // does not work as expected

                // Must be last statement
                cfg.IgnoreUnmapped();
            });

            Debug.Write(string.Format("Creating a foo: {0}", JsonConvert.SerializeObject(new WeightLiftingSet())));
            

            app.UseMvc();

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

        public class BaseClassConstructor
        {
            public static D Construct<S, D>(S source, ResolutionContext context) where D : class where S: IUniqueIdentifierDataModels<int>
            {
                D instance = context.Options.CreateInstance<D>();
                IUniqueIdentifierDataModels<int> completeInstance = null;
                if (typeof(IUniqueIdentifierDataModels<int>).IsAssignableFrom(typeof(D)))
                {
                    completeInstance = instance as IUniqueIdentifierDataModels<int>;
                    if (completeInstance != null)
                    {
                        completeInstance.Id = source.Id;
                        instance = completeInstance as D;
                    }
                }
                return instance;
            }
        }
    }
}
