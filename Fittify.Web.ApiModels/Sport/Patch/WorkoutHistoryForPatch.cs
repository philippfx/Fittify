using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Patch
{
    public class WorkoutHistoryForPatch : UniqueIdentifier<int>
    {
        [ForeignKey("DateTimeStartEndId")]
        public virtual DateTimeStartEndForPatch DateTimeStartEnd { get; set; }
        public int? DateTimeStartEndId { get; set; }

        [ForeignKey("WorkoutId")]
        public virtual WorkoutForPatch Workout { get; set; }
        public int WorkoutId { get; set; }

        public virtual IEnumerable<ExerciseHistoryForPatch> ExerciseHistories { get; set; }
    }
}