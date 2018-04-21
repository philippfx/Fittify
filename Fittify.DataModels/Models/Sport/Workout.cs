using System;
using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class Workout : IEntityName<int>, IEntityOwner
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //[ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int? CategoryId { get; set; }

        public virtual IEnumerable<MapExerciseWorkout> MapExerciseWorkout { get; set; }
        public virtual IEnumerable<WorkoutHistory> WorkoutHistories { get; set; }

        public Guid? OwnerGuid { get; set; }
    }
}
