using System.Collections.Generic;

namespace DataModels.Models.Sport
{
    public class Category
    {
        public Category()
        {

        }
        
        
        public string Name { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }

    }
}
