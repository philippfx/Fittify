using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Entities.Workout
{
    public class CardioSet
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }

        [ForeignKey("ExerciseId")]
        public ExerciseHistory Exercise { get; set; }
        public int ExerciseId { get; set; }
    }
}
