using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class CategoryOfmForPatch : CategoryOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }

    }
}
