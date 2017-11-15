using System.Collections.Generic;
using DataModels;

namespace Web.Models.Sport
{
    public class Category : Crud<Category, int>
    {
        public Category()
        {
        }

        public Category(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }
        
        public string Name { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }

    }
}
