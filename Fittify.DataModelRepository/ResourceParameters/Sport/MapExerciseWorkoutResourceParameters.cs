using System;
using Fittify.Common;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class MapExerciseWorkoutResourceParameters : EntityResourceParametersBase, IEntityOwner
    {
        public Guid? OwnerGuid { get; set; }

        public int? WorkoutId { get; set; }
        public int? ExerciseId { get; set; }
    }
}
