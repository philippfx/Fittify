using Fittify.Api.OuterFacingModels.Sport.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    internal abstract class DateTimeStartEndOfmForGet : DateTimeStartEndOfmBase, IEntityUniqueIdentifier<int>, IOfmForGet
    {
        public int Id { get; set; }
    }
}
