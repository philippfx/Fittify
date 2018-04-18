using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class ExerciseHistoryViewModelRepository : AsyncGppdOfmRepository<int, WorkoutOfmForPost, WorkoutViewModel>
    {
        private Uri _fittifyApiBaseUri;
        private IHttpContextAccessor _httpContextAccessor;

        public ExerciseHistoryViewModelRepository(Uri fittifyApiBaseUri, IHttpContextAccessor httpContextAccessor)
        {
            _fittifyApiBaseUri = fittifyApiBaseUri;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ExerciseHistoryViewModel> GetSingleById(int id)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<ExerciseHistoryOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/exercisehistories/" + id), _httpContextAccessor);

            return Mapper.Map<ExerciseHistoryViewModel>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }

        public async Task<IEnumerable<ExerciseHistoryViewModel>> GetSingleByWorkoutHistoryId(int workoutHistoryId)
        {
            var exerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<IEnumerable<ExerciseHistoryOfmForGet>>(
                    new Uri(_fittifyApiBaseUri, "api/exercisehistories?workoutHistoryId=" + workoutHistoryId), _httpContextAccessor);

            return Mapper.Map<IEnumerable<ExerciseHistoryViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        }

        public async Task<IEnumerable<ExerciseHistoryViewModel>> GetCollectionByWorkoutHistoryId(int workoutHistoryId)
        {
            // Current ExerciseHistories
            var currentExerciseHistoryOfmCollectionQueryResult =
                await AsyncGppd.GetCollection<ExerciseHistoryOfmForGet>(
                    new Uri(_fittifyApiBaseUri, "api/exercisehistories?workoutHistoryId=" + workoutHistoryId), _httpContextAccessor);

            if (currentExerciseHistoryOfmCollectionQueryResult.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return new List<ExerciseHistoryViewModel>();
            }

            var exerciseHistoryViewModels =
                Mapper.Map<IEnumerable<ExerciseHistoryViewModel>>(currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);

            var weightLiftingSetViewModelRepository = new WeightLiftingSetViewModelRepository(_fittifyApiBaseUri, _httpContextAccessor);
            foreach (var exerciseHistoryOfmForGet in currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection.Where(w => w.Exercise?.ExerciseType == ExerciseTypeEnum.WeightLifting.ToString()))
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

            var cardioSetViewModelRepository = new CardioSetViewModelRepository(_fittifyApiBaseUri, _httpContextAccessor);
            foreach (var exerciseHistoryOfmForGet in currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection.Where(w => w.Exercise?.ExerciseType == ExerciseTypeEnum.Cardio.ToString()))
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

