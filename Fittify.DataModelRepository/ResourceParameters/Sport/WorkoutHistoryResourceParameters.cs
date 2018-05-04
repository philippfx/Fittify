using System;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class WorkoutHistoryResourceParameters : DateTimeStartEndResourceParameters, IEntityOwner
    {
        public int? WorkoutId { get; set; }
        public Guid? OwnerGuid { get; set; }
    }
}
