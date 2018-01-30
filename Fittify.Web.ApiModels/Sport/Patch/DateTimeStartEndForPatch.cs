using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Patch
{
    public class DateTimeStartEndForPatch : UniqueIdentifier<int>
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }

        [ForeignKey("CardioSetId")]
        public virtual CardioSetForPatch CardioSet { get; set; }
        public int? CardioSetId { get; set; }

        [ForeignKey("WorkoutHistoryId")]
        public virtual WorkoutHistoryForPatch WorkoutHistory { get; set; }
        public int? WorkoutHistoryId { get; set; }


    }
}
