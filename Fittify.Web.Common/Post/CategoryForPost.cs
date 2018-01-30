using System.Collections.Generic;

namespace Fittify.Web.Common.Post
{
    public class CategoryForPost
    {
        public string Name { get; set; }
        public virtual ICollection<int> WorkoutIds { get; set; }

    }
}
