using System;
using Fittify.Common;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class WeightLiftingSetResourceParameters : EntityResourceParametersBase, IEntityOwner
    {
        public Guid? OwnerGuid { get; set; }

        public int? ExerciseHistoryId { get; set; }
    }
}
