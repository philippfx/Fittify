using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common.Helpers.ResourceParameters.Sport
{
    public class WorkoutHistoryResourceParameters : DateTimeStartEndResourceParameters
    {
        public int? WorkoutId { get; set; }
    }
}
