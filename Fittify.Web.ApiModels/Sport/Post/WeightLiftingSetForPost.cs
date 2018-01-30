namespace Fittify.Web.ApiModels.Sport.Post
{
    public class WeightLiftingSetForPost
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
