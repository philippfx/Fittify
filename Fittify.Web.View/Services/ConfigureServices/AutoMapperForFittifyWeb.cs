using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Client.ViewModels.Sport;

namespace Fittify.Web.View.Services.ConfigureServices
{
    public static class AutoMapperForFittifyWeb
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Reset();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                // OfmGet to ViewModel
                cfg.CreateMap<WorkoutOfmForGet, WorkoutViewModel>()
                    .ForMember(dest => dest.MapsExerciseWorkout, opt => opt.MapFrom(src => src.MapsExerciseWorkout));
                cfg.CreateMap<CategoryOfmForGet, CategoryViewModel>();
                ////.ForMember(dest => dest.RangeOfWorkoutIds, opt => opt.MapFrom(src => src.Workouts.Select(w => w.Id).ToList().ToStringOfIds()));
                cfg.CreateMap<CardioSetOfmForGet, CardioSetViewModel>();
                cfg.CreateMap<WeightLiftingSetOfmForGet, WeightLiftingSetViewModel>();
                cfg.CreateMap<ExerciseOfmForGet, ExerciseViewModel>();
                cfg.CreateMap<ExerciseHistoryOfmForGet, ExerciseHistoryViewModel>();
                cfg.CreateMap<WorkoutHistoryOfmForGet, WorkoutHistoryViewModel>()
                    .ForMember(dest => dest.ExerciseHistories, opt => opt.MapFrom(src => src.ExerciseHistories));
                cfg.IgnoreUnmapped();
            });
            
        }
    }
}
