using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class CardioSetOfmForPatch : CardioSetOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
    }
}
