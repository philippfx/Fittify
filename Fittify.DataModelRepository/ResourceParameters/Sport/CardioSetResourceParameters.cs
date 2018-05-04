using System;
using Fittify.Common;
using Fittify.Common.ResourceParameters;

namespace Fittify.DataModelRepository.ResourceParameters.Sport
{
    public class CardioSetResourceParameters : DateTimeStartEndResourceParameters, IEntityOwner
    {
        public int? ExerciseHistoryId { get; set; }
        public Guid? OwnerGuid { get; set; }
    }
}
