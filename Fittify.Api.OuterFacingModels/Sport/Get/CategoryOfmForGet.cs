using System.Collections.Generic;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class CategoryOfmForGet : CategoryOfmBase, IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
    }
}
