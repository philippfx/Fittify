using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Fittify.Api.OuterFacingModels.Sport.Abstract;

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
