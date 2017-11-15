﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.Sport
{
    public class DateTimeStartEnd
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
