using System.Collections.Generic;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Get
{
    public class CategoryForGet : UniqueIdentifier<int>
    {
        public string Name { get; set; }
        public virtual string RangeOfWorkoutIds { get; set; }

    }
}
