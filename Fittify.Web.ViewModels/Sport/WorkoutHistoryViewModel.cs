using System.Collections.Generic;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class WorkoutHistoryViewModel : WorkoutHistoryOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
        //public DateTime? DateTimeStart { get; set; }
        //public DateTime? DateTimeEnd { get; set; }

        public WorkoutViewModel Workout{ get; set; }

        //[ForeignKey("WorkoutId")]
        //public int WorkoutId { get; set; }
        //public string WorkoutName { get; set; }

        public virtual IEnumerable<ExerciseHistoryViewModel> ExerciseHistories { get; set; }

        public virtual IEnumerable<ExerciseViewModel> AllExercises { get; set; }
    }
}
