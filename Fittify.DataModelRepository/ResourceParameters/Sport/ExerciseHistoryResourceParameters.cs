using System;
using Fittify.Common;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class ExerciseHistoryResourceParameters : EntityResourceParametersBase, IEntityOwner
    {
        public Guid? OwnerGuid { get; set; }

        public int? ExerciseId { get; set; }
        public int? WorkoutHistoryId { get; set; }
    }
}
