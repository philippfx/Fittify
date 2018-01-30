using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Get
{
    public class ExerciseHistoryForGet : UniqueIdentifier<int>
    {
        public int? ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        
        public int WorkoutHistoryId { get; set; }
        
        public int? PreviousExerciseHistoryId { get; set; }


        public virtual string RangeOfWeightLiftingSetIds { get; set; }
        public virtual string RangeOfCardioSetIds { get; set; }
    }
}
