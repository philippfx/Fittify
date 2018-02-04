using System.Collections.Generic;
using Fittify.Api.OuterFacingModels.Helpers;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;
using Fittify.Common.Helpers;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class CategoryOfmForPatch : CategoryOfmBase, IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
    }
}
