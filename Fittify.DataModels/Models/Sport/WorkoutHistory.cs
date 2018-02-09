using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class WorkoutHistory : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }

        [ForeignKey("DateTimeStartEndId")]
        public virtual DateTimeStartEnd DateTimeStartEnd { get; set; }
        public int? DateTimeStartEndId { get; set; }

        [ForeignKey("WorkoutId")]
        public virtual Workout Workout { get; set; }
        public int WorkoutId { get; set; }

        public virtual IEnumerable<ExerciseHistory> ExerciseHistories { get; set; }
    }
}