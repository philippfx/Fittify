using System.Linq;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Extensions;
using Fittify.Common.Helpers;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.Services.ConfigureServices
{
    public static class AutoMapperForFittify
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Reset();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                // Entity to OfmGet
                cfg.CreateMap<Workout, WorkoutOfmForGet>()
                    .ForMember(dest => dest.RangeOfWorkoutHistoryIds, opt => opt.MapFrom(src => src.WorkoutHistories.Select(s => s.Id).ToList().ToStringOfIds()))
                    .ForMember(dest => dest.RangeOfExerciseIds, opt => opt.MapFrom(src => src.MapExerciseWorkout.Select(s => s.ExerciseId.GetValueOrDefault()).ToList().ToStringOfIds()));
                cfg.CreateMap<Category, CategoryOfmForGet>();
                ////.ForMember(dest => dest.RangeOfWorkoutIds, opt => opt.MapFrom(src => src.Workouts.Select(w => w.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<CardioSet, CardioSetOfmForGet>();
                cfg.CreateMap<WeightLiftingSet, WeightLiftingSetOfmForGet>();
                cfg.CreateMap<Exercise, ExerciseOfmForGet>()
                    .ForMember(dest => dest.RangeOfWorkoutIds, opt => opt.MapFrom(src => src.MapExerciseWorkout.Select(w => w.Workout.Id).ToList().ToStringOfIds()))
                    .ForMember(dest => dest.RangeOfExerciseHistoryIds, opt => opt.MapFrom(src => src.ExerciseHistories.Select(s => s.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<ExerciseHistory, ExerciseHistoryOfmForGet>()
                    .ForMember(dest => dest.Exercise, opt => opt.MapFrom(src => src.Exercise))
                    .ForMember(dest => dest.RangeOfWeightLiftingSetIds, opt => opt.MapFrom(src => src.WeightLiftingSets.Select(s => s.Id).ToList().ToStringOfIds()))
                    ////.ForMember(dest => dest.WeightLiftingSets, opt => opt.MapFrom(src => src.WeightLiftingSets.ToList()))
                    .ForMember(dest => dest.RangeOfCardioSetIds, opt => opt.MapFrom(src => src.CardioSets.Select(s => s.Id).ToList().ToStringOfIds()));
                    ////.ForMember(dest => dest.CardioSets, opt => opt.MapFrom(src => src.CardioSets.ToList()));
                cfg.CreateMap<WorkoutHistory, WorkoutHistoryOfmForGet>()
                    .ForMember(dest => dest.Workout, opt => opt.MapFrom(src => src.Workout))
                    .ForMember(dest => dest.RangeOfExerciseHistoryIds, opt => opt.MapFrom(src => src.ExerciseHistories.Select(eH => eH.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<IncomingRawHeaders, IncomingHeaders>()
                    .ForMember(dest => dest.IncludeHateoas, opt => opt.MapFrom(src => src.IncludeHateoas.ToBool()))
                    .ForMember(dest => dest.IncludeHateoas, opt => opt.MapFrom(src => int.Parse(src.IncludeHateoas)));

                //cfg.IgnoreUnmapped<WorkoutHistoryOfmForPpp, WorkoutHistoryOfmCollectionResourceParameters>(); // does not work as expected

                // Must be last statement
                cfg.IgnoreUnmapped();
            });
        }
    }
}
