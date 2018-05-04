using System;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class ExerciseResourceParameters : SearchQueryResourceParameters, IEntityOwner
    {
        public Guid? OwnerGuid { get; set; }
    }
}
