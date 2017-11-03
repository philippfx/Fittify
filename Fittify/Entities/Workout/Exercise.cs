using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Entities.Workout
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ExerciseHistory> ExerciseHistories { get; set; }
    }
}
