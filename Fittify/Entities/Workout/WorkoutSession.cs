using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Entities.Workout;

namespace Fittify.Entities
{
    public class WorkoutSession
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<WorkoutSessionHistory> WorkoutSessionHistories { get; set; }
    }
}
