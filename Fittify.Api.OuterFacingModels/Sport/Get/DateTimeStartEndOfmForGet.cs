using Fittify.Api.OuterFacingModels.Sport.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    internal abstract class DateTimeStartEndOfmForGet : DateTimeStartEndOfmBase, IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
    }
}
