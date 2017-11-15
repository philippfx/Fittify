using System.Collections.Generic;

namespace Fittify.Api.OuterFacingModels.Sport
{
    public class CategoryOfm : UniqueIdentifier
    {
        public CategoryOfm()
        {

        }
        
        
        public string Name { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }

    }
}
