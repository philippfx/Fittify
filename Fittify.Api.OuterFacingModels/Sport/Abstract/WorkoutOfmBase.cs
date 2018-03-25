using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class WorkoutOfmBase
    {
        public string Name { get; set; }
        public int? CategoryId { get; set; }
    }
}
