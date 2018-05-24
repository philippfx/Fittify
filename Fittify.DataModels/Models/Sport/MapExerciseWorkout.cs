using System;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class MapExerciseWorkout : IEntityUniqueIdentifier<int>, IEntityOwner
    {
        public int Id { get; set; }

        public Workout Workout { get; set; }
        public int? WorkoutId { get; set; }
        
        public Exercise Exercise { get; set; }
        public int? ExerciseId { get; set; }

        public Guid? OwnerGuid { get; set; }
    }
}
