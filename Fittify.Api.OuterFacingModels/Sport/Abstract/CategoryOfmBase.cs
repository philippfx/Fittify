using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class CategoryOfmBase
    {
        public virtual string Name { get; set; }
        public virtual string RangeOfWorkoutIds { get; set; }
    }
}
