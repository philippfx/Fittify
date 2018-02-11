using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class Category : IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Workout> Workouts { get; set; }
    }
}
