﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class ExerciseHistoryViewModelRepository : AsyncGppdRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        private string _fittifyApiBaseUrl;

        public ExerciseHistoryViewModelRepository(string fittifyApiBaseUrl)
        {
            _fittifyApiBaseUrl = fittifyApiBaseUrl;
        }
        public async Task<ExerciseHistoryViewModel> GetSingleById(int id)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<ExerciseHistoryOfmForGet>(
                    _fittifyApiBaseUrl + "api/exercisehistories/" + id);

            return Mapper.Map<ExerciseHistoryViewModel>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }

        public async Task<IEnumerable<ExerciseHistoryViewModel>> GetSingleByWorkoutHistoryId(int workoutHistoryId)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<IEnumerable<ExerciseHistoryOfmForGet>>(
                    _fittifyApiBaseUrl + "api/exercisehistories?workoutHistoryId=" + workoutHistoryId);

            return Mapper.Map<IEnumerable<ExerciseHistoryViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }

        public async Task<IEnumerable<ExerciseHistoryViewModel>> GetCollectionByWorkoutHistoryId(int workoutHistoryId)
        {
            // Current ExerciseHistories
            var currentExerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<ExerciseHistoryOfmForGet>(
                    _fittifyApiBaseUrl + "api/exercisehistories?workoutHistoryId=" + workoutHistoryId);

            if (currentExerciseHistoryOfmCollectionQueryResult.HttpStatusCode == 404)
            {
                return new List<ExerciseHistoryViewModel>();
            }

            var exerciseHistoryViewModels =
                Mapper.Map<IEnumerable<ExerciseHistoryViewModel>>(currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);

            var weightLiftingSetViewModelRepository = new WeightLiftingSetViewModelRepository(_fittifyApiBaseUrl);
            foreach (var exerciseHistoryOfmForGet in currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection.Where(w => w.Exercise.ExerciseType == ExerciseTypeEnum.WeightLifting.ToString()))
            {
                WeightLiftingSetViewModel[] previousWeightLiftingSetViewModels = null;
                if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
                {
                    var enumerablePreviousWeightLiftingSetViewModels =
                        await weightLiftingSetViewModelRepository.GetCollectionByExerciseHistoryId(
                            exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault());
                    previousWeightLiftingSetViewModels = enumerablePreviousWeightLiftingSetViewModels?.ToArray();
                }

                var enumerableWeightLiftingSetViewModels =
                    await weightLiftingSetViewModelRepository.GetCollectionByExerciseHistoryId(
                        exerciseHistoryOfmForGet.Id);
                var currentWeightLiftingSetViewModels = enumerableWeightLiftingSetViewModels?.ToArray();

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

            var cardioSetViewModelRepository = new CardioSetViewModelRepository(_fittifyApiBaseUrl);
            foreach (var exerciseHistoryOfmForGet in currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection.Where(w => w.Exercise.ExerciseType == ExerciseTypeEnum.Cardio.ToString()))
            {
                CardioSetViewModel[] previousCardioSetViewModels = null;
                if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
                {
                    var enumerablePreviousCardioSetViewModels =
                        await cardioSetViewModelRepository.GetCollectionByExerciseHistoryId(
                            exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault());
                    previousCardioSetViewModels = enumerablePreviousCardioSetViewModels?.ToArray();
                }

                var enumerableCardioSetViewModels =
                    await cardioSetViewModelRepository.GetCollectionByExerciseHistoryId(
                        exerciseHistoryOfmForGet.Id);
                var currentCardioSetViewModels = enumerableCardioSetViewModels?.ToArray();

                int previousCardioSetsLength = previousCardioSetViewModels?.Length ?? 0;
                int currentCardioSetsLength = currentCardioSetViewModels?.Length ?? 0;
                int maxValuePairs /* NumberOfColumns*/ =
                    Math.Max(previousCardioSetsLength, currentCardioSetsLength);

                var currentExerciseHistoryViewModel =
                    exerciseHistoryViewModels.FirstOrDefault(f => f.Id == exerciseHistoryOfmForGet.Id);

                for (int i = 0; i < maxValuePairs; i++)
                {
                    if (i < previousCardioSetsLength && i < currentCardioSetsLength)
                    {
                        currentExerciseHistoryViewModel.CurrentAndHistoricCardioSetPairs.Add(
                            new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(
                                previousCardioSetViewModels?[i], currentCardioSetViewModels?[i]));
                    }

                    if (i < previousCardioSetsLength && i >= currentCardioSetsLength)
                    {
                        currentExerciseHistoryViewModel.CurrentAndHistoricCardioSetPairs.Add(
                            new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(
                                previousCardioSetViewModels?[i], null));
                    }

                    if (i >= previousCardioSetsLength && i < currentCardioSetsLength)
                    {
                        currentExerciseHistoryViewModel.CurrentAndHistoricCardioSetPairs.Add(
                            new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(null,
                                currentCardioSetViewModels?[i]));
                    }
                }
            }

            return exerciseHistoryViewModels;
        }
    }
}

