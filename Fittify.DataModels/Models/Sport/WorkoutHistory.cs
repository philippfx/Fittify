using System;
using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class WorkoutHistory : IEntityDateTimeStartEnd<int>, IEntityOwner
    {
        public int Id { get; set; }

        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }

        //[ForeignKey("WorkoutId")]
        public virtual Workout Workout { get; set; }
        public int? WorkoutId { get; set; }

        public virtual IEnumerable<ExerciseHistory> ExerciseHistories { get; set; }

        public Guid OwnerGuid { get; set; }
    }
}