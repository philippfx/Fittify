using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Get
{
    public class MapExerciseWorkoutForGet : UniqueIdentifier<int>
    {
        public int WorkoutId { get; set; }
        
        public int? ExerciseId { get; set; }
    }
}
