using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Get
{
    public class WeightLiftingSetForGet : UniqueIdentifier<int>
    {
        public int? WeightFull { get; set; }
        public int? RepetitionsFull { get; set; }

        public int? WeightReduced { get; set; }
        public int? RepetitionsReduced { get; set; }

        public int? WeightBurn { get; set; }
        
        public int TotalScore { get; set; }
        
        public int ExerciseHistoryId { get; set; }
        
    }
}
