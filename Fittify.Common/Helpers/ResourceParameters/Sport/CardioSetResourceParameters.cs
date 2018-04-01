using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Common.IForeignKeys;

namespace Fittify.Common.Helpers.ResourceParameters.Sport
{
    public class CardioSetResourceParameters : DateTimeStartEndResourceParameters, ICardioSetForeignKeys
    {
        public int? ExerciseHistoryId { get; set; }
    }
}
