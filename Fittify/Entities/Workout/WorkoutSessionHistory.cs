using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Entities.Workout
{
    public class WorkoutSessionHistory
    {
        public int Id { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }

        [ForeignKey("WorkoutSessionId")]
        public WorkoutSession WorkoutSession { get; set; }
        public int WorkoutSessionId { get; set; }

        public virtual ICollection<ExerciseHistory> ExerciseHistories { get; set; }
    }
}
