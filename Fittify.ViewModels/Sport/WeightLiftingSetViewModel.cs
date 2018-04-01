using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Web.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class WeightLiftingSetViewModel : WeightLiftingSetOfmBase, IUniqueIdentifier<int>
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
