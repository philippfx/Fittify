﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Fittify.Api.OuterFacingModels.Helpers;
using Fittify.Common.Helpers;

namespace Fittify.Api.OuterFacingModels.Sport.Abstract
{
    public abstract class CategoryOfmBase : LinkedResourceBase
    {
        [Required]
        [MaxStringLength(128)]
        public virtual string Name { get; set; }

        [RegularExpressionRangeOfIntId(RegularExpressions.RangeOfIntIds)]
        [AscendingOrderIntIdRange]
        public virtual string RangeOfWorkoutIds { get; set; }
    }
}
