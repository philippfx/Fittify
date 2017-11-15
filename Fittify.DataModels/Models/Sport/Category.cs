using System.Collections.Generic;

namespace Fittify.DataModels.Models.Sport
{
    public class Category : UniqueIdentifier
    {
        public Category()
        {

        }
        
        public string Name { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }

    }
}
