using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class WeightLiftingSetOfmForPatch : WeightLiftingSetOfmBase, IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
    }
}
