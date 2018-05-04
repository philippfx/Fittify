using System;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class WorkoutResourceParameters : SearchQueryResourceParameters, IEntityOwner
    {
        public int? CategoryId { get; set; }
        public Guid? OwnerGuid { get; set; }
    }
}
