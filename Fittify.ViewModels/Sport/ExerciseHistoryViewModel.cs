using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Web.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class ExerciseHistoryViewModel : WorkoutHistoryOfmBase, IUniqueIdentifier<int>
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
            public CurrentAndHistoricWeightLiftingSetPair(WeightLiftingSetViewModel historicWeightLiftingSet, WeightLiftingSetViewModel currentWeightLiftingSet)
            {
                HistoricWeightLiftingSet = historicWeightLiftingSet;
                CurrentWeightLiftingSet = currentWeightLiftingSet;
            }
            public WeightLiftingSetViewModel HistoricWeightLiftingSet { get; set; }
            public WeightLiftingSetViewModel CurrentWeightLiftingSet { get; set; }
        }

        public class CurrentAndHistoricCardioSetPair
        {
            public CurrentAndHistoricCardioSetPair(CardioSetViewModel historicCardioSet, CardioSetViewModel currentCardioSet)
            {
                HistoricCardioSet = historicCardioSet;
                CurrentCardioSet = currentCardioSet;
            }
            public CardioSetViewModel HistoricCardioSet { get; set; }
            public CardioSetViewModel CurrentCardioSet { get; set; }
        }

    }
}
