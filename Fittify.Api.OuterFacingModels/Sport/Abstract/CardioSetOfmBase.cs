using System;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class CardioSetOfmBase
    {
        public virtual DateTime? DateTimeStart { get; set; }
        public virtual DateTime? DateTimeEnd { get; set; }
        public int? ExerciseHistoryId { get; set; }
    }
}
