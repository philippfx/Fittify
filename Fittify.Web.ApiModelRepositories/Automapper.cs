using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Web.ViewModels.Sport;

namespace Fittify.Web.ApiModelRepositories
{
    public static class Automapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<WorkoutOfmForGet, WorkoutViewModel>();
                cfg.CreateMap<ExerciseOfmForGet, ExerciseViewModel>();
            });
        }
    }
}
