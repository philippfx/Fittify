using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class DateTimeStartEndOfmBase : LinkedResourceBase
    {
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
    }
}
