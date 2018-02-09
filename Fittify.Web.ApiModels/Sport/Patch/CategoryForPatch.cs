using System.Collections.Generic;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Patch
{
    public class CategoryForPatch : UniqueIdentifier<int>
    {
        public string Name { get; set; }
        public virtual IEnumerable<WorkoutForPatch> Workouts { get; set; }

    }
}
