using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Get
{
    public class WorkoutHistoryDetailsForGet : UniqueIdentifier<int>
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }

        public string RangeOfExerciseHistoryIds { get; set; }
    }
}
