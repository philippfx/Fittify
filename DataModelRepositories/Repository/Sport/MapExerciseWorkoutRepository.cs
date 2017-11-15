using System;
using System.Collections.Generic;
using System.Text;
using DataModels.Models.Sport;

namespace DataModelRepositories.Repository.Sport
{
    public class MapExerciseWorkoutRepository : Crud<MapExerciseWorkout, int>
    {
        public MapExerciseWorkoutRepository()
        {
            
        }

        public MapExerciseWorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
