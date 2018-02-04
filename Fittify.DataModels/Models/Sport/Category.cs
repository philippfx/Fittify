using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class Category : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Workout> Workouts { get; set; }
    }
}
