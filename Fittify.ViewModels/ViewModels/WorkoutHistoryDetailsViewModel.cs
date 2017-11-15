using System;
using System.Collections.Generic;
using System.Linq;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.ViewModels.ViewModels
{
    public class WorkoutHistoryDetailsViewModel
    {
        public WorkoutHistoryDetailsViewModel(FittifyContext fittifyContext, int workoutHistoryId)
        {
            this.WorkoutHistory = fittifyContext.WorkoutHistories
                .Include(i => i.DateTimeStartEnd)
                .Include(i => i.ExerciseHistories)
                .ThenInclude(i => i.Exercise)
                .Include(i => i.ExerciseHistories)
                .ThenInclude(i => i.WeightLiftingSets)
                .Include(i => i.ExerciseHistories)
                .ThenInclude(i => i.PreviousExerciseHistory)
                .ThenInclude(i => i.WeightLiftingSets)
                .Include(i => i.Workout)
                .FirstOrDefault(wH => wH.Id == workoutHistoryId);

            this.AllExercies = fittifyContext.Exercises.ToList();

            this.ExerciseHistoryVMs = new List<ExerciseHistoryViewModel>();

            foreach (var eH in WorkoutHistory.ExerciseHistories)
            {
                var exerciseHistoryVM = new ExerciseHistoryViewModel();
                //exercise.Name = fittifyContext.ExerciseHistories.FirstOrDefault(e => e.Id == eH.ExerciseId).Exercise.Name;
                exerciseHistoryVM.Name = eH.Exercise.Name;
                exerciseHistoryVM.Id = eH.Id;
                
                var arrayPreviousWeightliftingSets = eH.PreviousExerciseHistory?.WeightLiftingSets.ToArray();

                var arrayCurrentWeightliftingSets = eH.WeightLiftingSets.ToArray();

                // Todo The following logic becomes problematic when user adds a new empty weightliftingSet. Maybe using a mini class instead of tuple?
                int previousWeightliftingSetsLength = arrayPreviousWeightliftingSets?.Length ?? 0;
                int currentWeightliftingSetsLength = arrayCurrentWeightliftingSets.Length;
                int maxDictionaryEntries /*= NumberOfColumns*/ = Math.Max(previousWeightliftingSetsLength, currentWeightliftingSetsLength);

                exerciseHistoryVM.CurrentAndHistoricWeightLiftingSet = new List<ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair>();
                for (int i = 0; i < maxDictionaryEntries; i++)
                {
                    if (i < previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
                    {
                        exerciseHistoryVM.CurrentAndHistoricWeightLiftingSet.Add(new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(arrayPreviousWeightliftingSets[i], arrayCurrentWeightliftingSets[i]));
                    }

                    if (i < previousWeightliftingSetsLength && i >= currentWeightliftingSetsLength)
                    {
                        exerciseHistoryVM.CurrentAndHistoricWeightLiftingSet.Add(new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(arrayPreviousWeightliftingSets[i], null));
                    }

                    if (i >= previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
                    {
                        exerciseHistoryVM.CurrentAndHistoricWeightLiftingSet.Add(new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(null, arrayCurrentWeightliftingSets[i]));
                    }
                }

                ExerciseHistoryVMs.Add(exerciseHistoryVM);
            }
        }

        public WorkoutHistory WorkoutHistory { get; set; }
        public ICollection<ExerciseHistoryViewModel> ExerciseHistoryVMs { get; set; }
        public class ExerciseHistoryViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }

            // Todo this becomes problematic when adding new empty wls. Better add a mini class?
            public virtual List<CurrentAndHistoricWeightLiftingSetPair> CurrentAndHistoricWeightLiftingSet { get; set; }

            public class CurrentAndHistoricWeightLiftingSetPair
            {
                public CurrentAndHistoricWeightLiftingSetPair(WeightLiftingSet historicWeightLiftingSet, WeightLiftingSet currentWeightLiftingSet)
                {
                    HistoricWeightLiftingSet = historicWeightLiftingSet;
                    CurrentWeightLiftingSet = currentWeightLiftingSet;
                    
                }
                public WeightLiftingSet HistoricWeightLiftingSet { get; set; }
                public WeightLiftingSet CurrentWeightLiftingSet { get; set; }
            }
        }
        public virtual List<Exercise> AllExercies { get; set; }

    }

}
