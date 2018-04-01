using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;
using Fittify.Common.IForeignKeys;

namespace Fittify.Api.OuterFacingModels.Sport.Patch
{
    public class CardioSetOfmForPatch : CardioSetOfmBase, IEntityUniqueIdentifier<int>, ICardioSetForeignKeys
    {
        public int Id { get; set; }

        public int? ExerciseHistoryId { get; set; }
    }
}
