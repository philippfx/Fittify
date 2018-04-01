using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class ExerciseHistoryViewModelRepository : AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        public ExerciseHistoryViewModelRepository()
        {

        }
        public async Task<ExerciseHistoryViewModel> GetSingleById(int id)
        {
            var exerciseHistoryWorkoutOfmForGet =
                await AsyncGppd.GetCollection<ExerciseHistoryOfmForGet>(
                    "http://localhost:52275/api/exercisehistories/" + id);

            return Mapper.Map<ExerciseHistoryViewModel>(exerciseHistoryWorkoutOfmForGet);
        }

        public async Task<IEnumerable<ExerciseHistoryViewModel>> GetSingleByWorkoutHistoryId(int workoutHistoryId)
        {
            var exerciseHistoryWorkoutOfmForGets =
                await AsyncGppd.GetCollection<IEnumerable<ExerciseHistoryOfmForGet>>(
                    "http://localhost:52275/api/exercisehistories?workoutHistoryId=" + workoutHistoryId);

            return Mapper.Map<IEnumerable<ExerciseHistoryViewModel>>(exerciseHistoryWorkoutOfmForGets);
        }

        public async Task<IEnumerable<ExerciseHistoryViewModel>> GetCollectionByWorkoutHistoryId(int workoutHistoryId)
        {
            // Current ExerciseHistories
            var currentExerciseHistoryOfmForGets =
                await AsyncGppd.GetCollection<ExerciseHistoryOfmForGet>(
                    "http://localhost:52275/api/exercisehistories?workoutHistoryId=" + workoutHistoryId);

            var exerciseHistoryViewModels =
                Mapper.Map<IEnumerable<ExerciseHistoryViewModel>>(currentExerciseHistoryOfmForGets);

            var weightLiftingSetViewModelRepository = new WeightLiftingSetViewModelRepository();
            foreach (var exerciseHistoryOfmForGet in currentExerciseHistoryOfmForGets.Where(w => !String.IsNullOrWhiteSpace(w.RangeOfWeightLiftingSetIds)))
            {
                WeightLiftingSetViewModel[] previousWeightLiftingSetViewModels = null;
                if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
                {
                    var previousExerciseHistory = await AsyncGppd.GetSingle<ExerciseHistoryOfmForGet>(
                        "http://localhost:52275/api/exercisehistories/" +
                        exerciseHistoryOfmForGet.PreviousExerciseHistoryId);

                    var enumerablePpreviousWeightLiftingSetViewModels =
                        await weightLiftingSetViewModelRepository.GetCollectionByExerciseHistoryId(
                            exerciseHistoryOfmForGet.Id);
                    previousWeightLiftingSetViewModels = enumerablePpreviousWeightLiftingSetViewModels.ToArray();
                }

                var enumerableWeightLiftingSetViewModels =
                    await weightLiftingSetViewModelRepository.GetCollectionByExerciseHistoryId(
                        exerciseHistoryOfmForGet.Id);
                var currentWeightLiftingSetViewModels = enumerableWeightLiftingSetViewModels.ToArray();

                int previousWeightliftingSetsLength = previousWeightLiftingSetViewModels?.Length ?? 0;
                int currentWeightliftingSetsLength = currentWeightLiftingSetViewModels?.Length ?? 0;
                int maxValuePairs /* NumberOfColumns*/ =
                    Math.Max(previousWeightliftingSetsLength, currentWeightliftingSetsLength);
                
                var currentExerciseHistoryViewModel =
                    exerciseHistoryViewModels.FirstOrDefault(f => f.Id == exerciseHistoryOfmForGet.Id);

                for (int i = 0; i < maxValuePairs; i++)
                {
                    if (i < previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
                    {
                        currentExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPairs.Add(
                            new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(
                                previousWeightLiftingSetViewModels?[i], currentWeightLiftingSetViewModels?[i]));
                    }

                    if (i < previousWeightliftingSetsLength && i >= currentWeightliftingSetsLength)
                    {
                        currentExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPairs.Add(
                            new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(
                                previousWeightLiftingSetViewModels?[i], null));
                    }

                    if (i >= previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
                    {
                        currentExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPairs.Add(
                            new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(null,
                                currentWeightLiftingSetViewModels?[i]));
                    }
                }
            }
            return exerciseHistoryViewModels;
        }
    }
}

