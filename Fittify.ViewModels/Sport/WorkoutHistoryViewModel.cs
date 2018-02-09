﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class WorkoutHistoryViewModel : UniqueIdentifier<int>
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }

        [ForeignKey("WorkoutId")]
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }

        public virtual IEnumerable<ExerciseHistoryViewModel> ExerciseHistories { get; set; }

        public virtual IEnumerable<ExerciseViewModel> AllExercises { get; set; }
    }
}
