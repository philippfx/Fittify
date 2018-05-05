using System;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class WorkoutHistoryResourceParameters : EntityResourceParametersBase, IDateTimeStartEndResourceParameters, IEntityOwner
    {
        public DateTime? FromDateTimeStart { get; set; }
        public DateTime? UntilDateTimeEnd { get; set; }
        public Guid? OwnerGuid { get; set; }

        public int? WorkoutId { get; set; }
    }
}
