using System;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class WorkoutHistoryOfmBase
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        
        //public WorkoutOfmBase Workout { get; set; }

    }
}
