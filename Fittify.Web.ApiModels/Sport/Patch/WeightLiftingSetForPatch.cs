using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Patch
{
    public class WeightLiftingSetForPatch : UniqueIdentifier<int>
    {
        public WeightLiftingSetForPatch()
        {
            
        }

        public int? WeightFull { get; set; }
        public int? RepetitionsFull { get; set; }

        public int? WeightReduced { get; set; }
        public int? RepetitionsReduced { get; set; }

        public int? WeightBurn { get; set; }
        
        public int TotalScore { get; set; }

        //[ForeignKey("ExerciseHistoryId")]
        //public virtual ExerciseHistoryOfmForPpp ExerciseHistory { get; set; }
        //public int ExerciseHistoryId { get; set; }
        
    }
}
