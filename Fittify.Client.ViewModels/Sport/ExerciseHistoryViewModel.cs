using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class ExerciseHistoryViewModel : WorkoutHistoryOfmBase, IEntityUniqueIdentifier<int>
    {
        public ExerciseHistoryViewModel()
        {
            CurrentAndHistoricWeightLiftingSetPairs = new List<CurrentAndHistoricWeightLiftingSetPair>();
            CurrentAndHistoricCardioSetPairs = new List<CurrentAndHistoricCardioSetPair>();
        }
        public int Id { get; set; }

        public ExerciseViewModel Exercise { get; set; }

        [ForeignKey("WorkoutHistoryId")]
        public int WorkoutHistoryId { get; set; }

        [ForeignKey("PreviousExerciseHistoryId")]
        public int? PreviousExerciseHistoryId { get; set; }
        public ExerciseHistoryViewModel PreviousExerciseHistory { get; set; }

        public List<CurrentAndHistoricWeightLiftingSetPair> CurrentAndHistoricWeightLiftingSetPairs { get; set; }
        public List<CurrentAndHistoricCardioSetPair> CurrentAndHistoricCardioSetPairs { get; set; }

        public class CurrentAndHistoricWeightLiftingSetPair
        {
            public WeightLiftingSetViewModel HistoricWeightLiftingSet { get; set; }
            public WeightLiftingSetViewModel CurrentWeightLiftingSet { get; set; }
        }

        public class CurrentAndHistoricCardioSetPair
        {
            public CardioSetViewModel HistoricCardioSet { get; set; }
            public CardioSetViewModel CurrentCardioSet { get; set; }
        }

    }
}
