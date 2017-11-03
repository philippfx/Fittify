using System.Collections.Generic;

namespace Fittify.Entities.Workout
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<ExerciseHistory> Exercises { get; set; }
    }
}
