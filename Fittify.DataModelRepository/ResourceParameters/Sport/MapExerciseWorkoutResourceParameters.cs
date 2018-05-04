using System;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class MapExerciseWorkoutResourceParameters : BaseResourceParameters, IEntityOwner
    {
        public int? WorkoutId { get; set; }
        public int? ExerciseId { get; set; }
        public Guid? OwnerGuid { get; set; }
    }
}
