using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Entities;

namespace Fittify.ViewModels
{
    public class WorkoutSessionViewModel
    {
        public WorkoutSession CurrentWorkoutSessionInProgress { get; set; }
        public WorkoutSession PreviousWorkoutSession { get; set; }

    }
}
