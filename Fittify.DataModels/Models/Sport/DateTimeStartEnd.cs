using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class DateTimeStartEnd : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }

        [ForeignKey("CardioSetId")]
        public virtual CardioSet CardioSet { get; set; }
        public int? CardioSetId { get; set; }

        [ForeignKey("WorkoutHistoryId")]
        public virtual WorkoutHistory WorkoutHistory { get; set; }
        public int? WorkoutHistoryId { get; set; }


    }
}
