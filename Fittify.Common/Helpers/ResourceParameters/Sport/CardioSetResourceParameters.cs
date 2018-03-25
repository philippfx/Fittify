using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Common.Helpers.ResourceParameters.Sport
{
    public class CardioSetResourceParameters : DateTimeStartEndResourceParameters
    {
        public int? ExerciseHistoryId { get; set; }
    }
}
