using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class WeightLiftingSetOfmBase
    {
        public int? WeightFull { get; set; }
        public int? RepetitionsFull { get; set; }

        public int? WeightReduced { get; set; }
        public int? RepetitionsReduced { get; set; }

        public int? WeightBurn { get; set; }
        
        public int? ExerciseHistoryId { get; set; }
    }
}
