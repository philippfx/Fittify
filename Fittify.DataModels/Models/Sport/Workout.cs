using System;
using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class Workout : IEntityName<int>, IEntityOwner
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        ////public virtual Category Category { get; set; }
        ////public int? CategoryId { get; set; }

        public IEnumerable<MapExerciseWorkout> MapExerciseWorkout { get; set; }
        public IEnumerable<WorkoutHistory> WorkoutHistories { get; set; }

        public Guid? OwnerGuid { get; set; }
    }
}
