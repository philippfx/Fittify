using System;
using System.Collections.Generic;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModels.Models.Sport;

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

        //public static Dictionary<IEntityUniqueIdentifier<TId>, IEntityUniqueIdentifier<TId>> RelatedClasses = new Dictionary<IEntityUniqueIdentifier<TId>, IEntityUniqueIdentifier<TId>>()
        //{
        //    { new WorkoutHistoryOfmForGet(), WorkoutHistory }
        //};
    }
}
