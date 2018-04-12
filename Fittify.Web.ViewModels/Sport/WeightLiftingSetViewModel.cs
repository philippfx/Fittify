using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class WeightLiftingSetViewModel : WeightLiftingSetOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
        //public int? WeightFull { get; set; }
        //public int? RepetitionsFull { get; set; }

        //public int? WeightReduced { get; set; }
        //public int? RepetitionsReduced { get; set; }

        //public int? WeightBurn { get; set; }

        //[ForeignKey("ExerciseHistoryId")]
        //public int ExerciseHistoryId { get; set; }
    }
}
