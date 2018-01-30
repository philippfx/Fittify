using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class WeightLiftingSetViewModel : UniqueIdentifier<int>
    {
        public int? WeightFull { get; set; }
        public int? RepetitionsFull { get; set; }

        public int? WeightReduced { get; set; }
        public int? RepetitionsReduced { get; set; }

        public int? WeightBurn { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public int ExerciseHistoryId { get; set; }
    }
}
