using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common;
using Fittify.DataModels.Models.Sport;
using Microsoft.Data.Edm;

namespace Fittify.Api.Helpers
{
    public static class RelatedDataOfm
    {
        public static Dictionary<Type, Type> RelatedClasses = new Dictionary<Type, Type>()
        {
            { typeof(CardioSetOfmForGet), typeof(CardioSet) },
            { typeof(CategoryOfmForGet), typeof(Category) },
            { typeof(ExerciseHistoryOfmForGet), typeof(ExerciseHistory) },
            { typeof(ExerciseOfmForGet), typeof(Exercise) },
            { typeof(WeightLiftingSetOfmForGet), typeof(WeightLiftingSet) },
            { typeof(WorkoutHistoryOfmForGet), typeof(WorkoutHistory) },
            { typeof(WorkoutOfmForGet), typeof(Workout) }
        };

        //public static Dictionary<IUniqueIdentifierDataModels<TId>, IUniqueIdentifierDataModels<TId>> RelatedClasses = new Dictionary<IUniqueIdentifierDataModels<TId>, IUniqueIdentifierDataModels<TId>>()
        //{
        //    { new WorkoutHistoryOfmForGet(), WorkoutHistory }
        //};
    }
}
