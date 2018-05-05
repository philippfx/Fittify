using System.Collections.Generic;

namespace Fittify.Web.ApiModelRepositories
{
    public static class ApiUriMapping
    {
        public static IReadOnlyDictionary<string, string> ActionUris { get; } = 
            new Dictionary<string, string>
        {
            { "CardioSetOfmResourceParameters", "api/cardiosets" },
            { "CategoryResourceParameters", "api/categories" },
            { "ExerciseOfmResourceParameters", "api/exercises" },
            { "ExerciseHistoryOfmResourceParameters", "api/exercisehistories" },
            { "MapExerciseWorkoutOfmResourceParameters", "api/mapexerciseworkouts" },
            { "WeightLiftingSetOfmResourceParameters", "api/weightliftingsets" },
            { "WorkoutOfmResourceParameters", "api/workouts" },
            { "WorkoutHistoryOfmResourceParameters", "api/workouthistories" }
        };
    }
}
