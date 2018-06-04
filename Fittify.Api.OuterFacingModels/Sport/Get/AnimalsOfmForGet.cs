using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class AnimalOfmForGet : AnimalOfmBase, IEntityUniqueIdentifier<int>, IOfmForGet
    {
        public int Id { get; set; }
    }
}
