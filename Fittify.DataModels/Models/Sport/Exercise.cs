using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class Exercise : IEntityName<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExerciseTypeEnum ExerciseType { get; set; }

        public virtual IEnumerable<MapExerciseWorkout> MapExerciseWorkout { get; set; }
        public virtual IEnumerable<ExerciseHistory> ExerciseHistories { get; set; }
        
    }
}
