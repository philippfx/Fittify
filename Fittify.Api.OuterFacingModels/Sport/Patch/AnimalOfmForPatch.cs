using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class AnimalOfmForPatch : AnimalOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }

    }
}
