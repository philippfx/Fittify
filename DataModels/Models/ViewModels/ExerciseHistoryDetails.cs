//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Fittify.Models.Sport;
//using Microsoft.EntityFrameworkCore;

//namespace Fittify.Models.ViewModels
//{
//    public class WorkoutHistoryDetails
//    {
//        public WorkoutHistoryDetails(FittifyContext fittifyContext, int workoutHistoryId)
//        {
//            this.WorkoutHistory = fittifyContext.WorkoutHistories
//                .Include(i => i.DateTimeStartEnd)
//                .Include(i => i.ExerciseHistories)
//                .ThenInclude(i => i.Exercise)
//                .Include(i => i.ExerciseHistories)
//                .ThenInclude(i => i.WeightLiftingSets)
//                .Include(i => i.ExerciseHistories)
//                .ThenInclude(i => i.PreviousExerciseHistory)
//                .ThenInclude(i => i.WeightLiftingSets)
//                .Include(i => i.Workout)
//                .FirstOrDefault(wH => wH.Id == workoutHistoryId);

//            this.Exercises = new List<Exercise>();

//            foreach (var eH in WorkoutHistory.ExerciseHistories)
//            {
//                var exercise = new Exercise();
//                //exercise.Name = fittifyContext.ExerciseHistories.FirstOrDefault(e => e.Id == eH.ExerciseId).Exercise.Name;
//                exercise.Name = eH.Exercise.Name;
                
//                var arrayPreviousWeightliftingSets = eH.PreviousExerciseHistory.WeightLiftingSets.ToArray();

//                var arrayCurrentWeightliftingSets = eH.WeightLiftingSets.ToArray();

//                int previousWeightliftingSetsLength = arrayPreviousWeightliftingSets.Length;
//                int currentWeightliftingSetsLength = arrayCurrentWeightliftingSets.Length;
//                int maxDictionaryEntries = NumberOfColumns = Math.Max(previousWeightliftingSetsLength, currentWeightliftingSetsLength);

//                exercise.CurrentAndHistoricWeightLiftingSet = new List<Tuple<WeightLiftingSet, WeightLiftingSet>>();
//                for (int i = 0; i < maxDictionaryEntries; i++)
//                {
//                    if (i < previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
//                    {
//                        exercise.CurrentAndHistoricWeightLiftingSet.Add(new Tuple<WeightLiftingSet, WeightLiftingSet>(arrayPreviousWeightliftingSets[i], arrayCurrentWeightliftingSets[i]));
//                    }

//                    if (i < previousWeightliftingSetsLength && i >= currentWeightliftingSetsLength)
//                    {
//                        exercise.CurrentAndHistoricWeightLiftingSet.Add(new Tuple<WeightLiftingSet, WeightLiftingSet>(arrayPreviousWeightliftingSets[i], null));
//                    }

//                    if (i >= previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
//                    {
//                        exercise.CurrentAndHistoricWeightLiftingSet.Add(new Tuple<WeightLiftingSet, WeightLiftingSet>(null, arrayCurrentWeightliftingSets[i]));
//                    }
//                }

//                Exercises.Add(exercise);
//            }
//        }

//        public WorkoutHistory WorkoutHistory { get; set; }
//        public ICollection<Exercise> Exercises { get; set; }
//        public int NumberOfColumns { get; set; }
//        public class Exercise
//        {
//            public string Name { get; set; }
//            public List<Tuple<WeightLiftingSet, WeightLiftingSet>> CurrentAndHistoricWeightLiftingSet { get; set; }
//        }
        
//    }

//}
