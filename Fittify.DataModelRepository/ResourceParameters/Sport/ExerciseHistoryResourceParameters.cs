using System;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class ExerciseHistoryResourceParameters : BaseResourceParameters, IEntityOwner
    {
        public int? ExerciseId { get; set; }
        public int? WorkoutHistoryId { get; set; }
        public Guid? OwnerGuid { get; set; }
    }
}
