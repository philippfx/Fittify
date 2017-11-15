﻿using System.Collections.Generic;

namespace DataModels.Models.Sport
{
    public class Exercise
    {
        public Exercise()
        {
            
        }

        public string Name { get; set; }

        public MachineAdjustableType? MachineAdjustableType1 { get; set; }
        public int? MachineAdjustableSetting1 { get; set; }

        public MachineAdjustableType? MachineAdjustableType2 { get; set; }
        public int? MachineAdjustableSetting2 { get; set; }

        public virtual ICollection<MapExerciseWorkout> ExercisesWorkoutsMap { get; set; }
        public virtual ICollection<ExerciseHistory> ExerciseHistories { get; set; }
        
    }
}
