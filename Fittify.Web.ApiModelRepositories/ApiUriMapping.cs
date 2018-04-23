using System.Collections.Generic;

namespace Fittify.Web.ApiModelRepositories
{
    public static class ApiUriMapping
    {
        public static IReadOnlyDictionary<string, string> ActionUris { get; } = 
            new Dictionary<string, string>
        {
            { "CardioSet", "api/cardiosets" },
            { "Category", "api/categories" },
            { "Exercise", "api/exercises" },
            { "ExerciseHistory", "api/exercisehistories" },
            { "MapExerciseWorkout", "api/mapexerciseworkouts" },
            { "WeightLiftingSet", "api/weightliftingsets" },
            { "Workout", "api/workouts" },
            { "WorkoutHistory", "api/workouthistories" }
        };
    }
}
