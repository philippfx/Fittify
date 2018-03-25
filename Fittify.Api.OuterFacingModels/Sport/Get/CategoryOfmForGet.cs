﻿using System.Collections.Generic;
using Fittify.Api.OuterFacingModels.Helpers;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;
using Fittify.Common.Helpers;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class CategoryOfmForGet : CategoryOfmBase, IEntityUniqueIdentifier<int>, IOfmForGet
    {
        public int Id { get; set; }

        [ValidRegularExpressionRangeOfIntIds(RegularExpressions.RangeOfIntIds)]
        [ValidAscendingOrderRangeOfIntIds]
        public virtual string RangeOfWorkoutIds { get; set; }
    }
}
