﻿using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class Exercise : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MapExerciseWorkout> ExercisesWorkoutsMap { get; set; }
        public virtual ICollection<ExerciseHistory> ExerciseHistories { get; set; }
        
    }
}
