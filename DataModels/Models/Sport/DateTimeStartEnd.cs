using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Models.Sport
{
    public class DateTimeStartEnd
    {
        public DateTimeStartEnd()
        {
            
        }

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
