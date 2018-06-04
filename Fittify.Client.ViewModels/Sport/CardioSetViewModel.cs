using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Client.ViewModels.Sport
{
    public class CardioSetViewModel : CardioSetOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }

        //public DateTime DateTimeStart { get; set; }
        //public DateTime DateTimeEnd { get; set; }

        //[ForeignKey("ExerciseHistoryId")]
        //public int? ExerciseHistoryId { get; set; }
    }
}
