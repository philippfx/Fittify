using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class WeightLiftingSetOfmForGet : WeightLiftingSetOfmBase, IEntityUniqueIdentifier<int>, IOfmForGet
    {
        public int Id { get; set; }
    }
}
