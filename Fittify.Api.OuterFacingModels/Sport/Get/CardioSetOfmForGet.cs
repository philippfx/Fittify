using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;
using Fittify.Common.IForeignKeys;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class CardioSetOfmForGet : CardioSetOfmBase, IEntityUniqueIdentifier<int>, ICardioSetForeignKeys, IOfmForGet
    {
        public int Id { get; set; }
        public int? ExerciseHistoryId { get; set; }
    }
}
