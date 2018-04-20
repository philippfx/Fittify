﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class ExerciseHistoryViewModelRepository : GenericViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryResourceParameters>
    {
        private GenericAsyncGppdOfm<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryResourceParameters> asyncGppdOfmExerciseHistory;
        private IConfiguration _appConfiguration;

        public ExerciseHistoryViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "ExerciseHistory")
        {
            asyncGppdOfmExerciseHistory = new GenericAsyncGppdOfm<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryResourceParameters>(appConfiguration, httpContextAccessor, "ExerciseHistory");
            _appConfiguration = appConfiguration;
        }
        //public async Task<ExerciseHistoryViewModel> GetSingleById(int id)
        //{
        //    var exerciseHistoryOfmCollectionQueryResult =
        //        await AsyncGppd.GetCollection<ExerciseHistoryOfmForGet>(
        //            new Uri(_fittifyApiBaseUri, "api/exercisehistories/" + id));

        //    return Mapper.Map<ExerciseHistoryViewModel>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        //}

        //public async Task<IEnumerable<ExerciseHistoryViewModel>> GetSingleByWorkoutHistoryId(int workoutHistoryId)
        //{
        //    var exerciseHistoryOfmCollectionQueryResult =
        //        await AsyncGppd.GetCollection<IEnumerable<ExerciseHistoryOfmForGet>>(
        //            new Uri(_fittifyApiBaseUri, "api/exercisehistories?workoutHistoryId=" + workoutHistoryId));

        //    return Mapper.Map<IEnumerable<ExerciseHistoryViewModel>>(exerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);
        //}

        public override async Task<ViewModelCollectionQueryResult<ExerciseHistoryViewModel>> GetCollection(ExerciseHistoryResourceParameters exerciseHistoryResourceParameters)
        {
            // Current ExerciseHistories
            var exerciseHistoryViewModelCollectionQueryResult =
                await base.GetCollection(exerciseHistoryResourceParameters);
            
            var exerciseHistoryViewModels =
                exerciseHistoryViewModelCollectionQueryResult.ViewModelForGetCollection;
            
            var weightLiftingSetViewModelRepository = new WeightLiftingSetViewModelRepository(_appConfiguration, HttpContextAccessor);
            foreach (var exerciseHistoryOfmForGet in exerciseHistoryViewModelCollectionQueryResult.ViewModelForGetCollection.Where(w => w.Exercise?.ExerciseType == ExerciseTypeEnum.WeightLifting.ToString()))
            {
                WeightLiftingSetViewModel[] previousWeightLiftingSetViewModels = null;
                if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
                {
                    var previousWeightLiftingSetViewModelCollectionQueryResult =
                        //await weightLiftingSetViewModelRepository.GetCollectionByExerciseHistoryId(
                        //    exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault());
                        await weightLiftingSetViewModelRepository.GetCollection(new WeightLiftingSetResourceParameters()
                        {
                            ExerciseHistoryId = exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault()
                        });

                    previousWeightLiftingSetViewModels = previousWeightLiftingSetViewModelCollectionQueryResult.ViewModelForGetCollection?.ToArray();
                }

                var currentWeightLiftingSetViewModelCollectionQueryResult =
                    //await weightLiftingSetViewModelRepository.GetCollectionByExerciseHistoryId(
                    //    exerciseHistoryOfmForGet.Id);
                    await weightLiftingSetViewModelRepository.GetCollection(new WeightLiftingSetResourceParameters()
                    {
                        ExerciseHistoryId = exerciseHistoryOfmForGet.Id
                    });
                var currentWeightLiftingSetViewModels = currentWeightLiftingSetViewModelCollectionQueryResult.ViewModelForGetCollection?.ToArray();

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

            var cardioSetViewModelRepository = new CardioSetViewModelRepository(_appConfiguration, HttpContextAccessor);
            foreach (var exerciseHistoryOfmForGet in exerciseHistoryViewModelCollectionQueryResult.ViewModelForGetCollection.Where(w => w.Exercise?.ExerciseType == ExerciseTypeEnum.Cardio.ToString()))
            {
                CardioSetViewModel[] previousCardioSetViewModels = null;
                if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
                {
                    var previousCardioSetViewModelCollecitonResult =
                        //await cardioSetViewModelRepository.GetCollectionByExerciseHistoryId(
                        //    exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault());
                        await cardioSetViewModelRepository.GetCollection(new CardioSetResourceParameters()
                        {
                            ExerciseHistoryId = exerciseHistoryOfmForGet.PreviousExerciseHistoryId
                        });
                    previousCardioSetViewModels = previousCardioSetViewModelCollecitonResult.ViewModelForGetCollection?.ToArray();
                }

                var currentCardioSetViewModelCollectionQueryResult =
                    //await cardioSetViewModelRepository.GetCollectionByExerciseHistoryId(
                    //    exerciseHistoryOfmForGet.Id);
                    await cardioSetViewModelRepository.GetCollection(new CardioSetResourceParameters()
                    {
                        ExerciseHistoryId = exerciseHistoryOfmForGet.Id
                    });
                var currentCardioSetViewModels = currentCardioSetViewModelCollectionQueryResult.ViewModelForGetCollection?.ToArray();

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

            return exerciseHistoryViewModelCollectionQueryResult;
        }

        //public async Task<IEnumerable<ExerciseHistoryViewModel>> GetCollectionByWorkoutHistoryId(int workoutHistoryId)
        //{
        //    // Current ExerciseHistories
        //    var currentExerciseHistoryOfmCollectionQueryResult =
        //        await AsyncGppd.GetCollection<ExerciseHistoryOfmForGet>(
        //            new Uri(_fittifyApiBaseUri, "api/exercisehistories?workoutHistoryId=" + workoutHistoryId));

        //    if ((int)currentExerciseHistoryOfmCollectionQueryResult.HttpStatusCode == 404)
        //    {
        //        return new List<ExerciseHistoryViewModel>();
        //    }

        //    var exerciseHistoryViewModels =
        //        Mapper.Map<IEnumerable<ExerciseHistoryViewModel>>(currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection);

        //    var weightLiftingSetViewModelRepository = new WeightLiftingSetViewModelRepository(_fittifyApiBaseUri);
        //    foreach (var exerciseHistoryOfmForGet in currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection.Where(w => w.Exercise?.ExerciseType == ExerciseTypeEnum.WeightLifting.ToString()))
        //    {
        //        WeightLiftingSetViewModel[] previousWeightLiftingSetViewModels = null;
        //        if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
        //        {
        //            var enumerablePreviousWeightLiftingSetViewModels =
        //                await weightLiftingSetViewModelRepository.GetCollectionByExerciseHistoryId(
        //                    exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault());

        //            previousWeightLiftingSetViewModels = enumerablePreviousWeightLiftingSetViewModels?.ToArray();
        //        }

        //        var enumerableWeightLiftingSetViewModels =
        //            await weightLiftingSetViewModelRepository.GetCollectionByExerciseHistoryId(
        //                exerciseHistoryOfmForGet.Id);
        //        var currentWeightLiftingSetViewModels = enumerableWeightLiftingSetViewModels?.ToArray();

        //        int previousWeightliftingSetsLength = previousWeightLiftingSetViewModels?.Length ?? 0;
        //        int currentWeightliftingSetsLength = currentWeightLiftingSetViewModels?.Length ?? 0;
        //        int maxValuePairs /* NumberOfColumns*/ =
        //            Math.Max(previousWeightliftingSetsLength, currentWeightliftingSetsLength);
                
        //        var currentExerciseHistoryViewModel =
        //            exerciseHistoryViewModels.FirstOrDefault(f => f.Id == exerciseHistoryOfmForGet.Id);

        //        for (int i = 0; i < maxValuePairs; i++)
        //        {
        //            if (i < previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
        //            {
        //                currentExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPairs.Add(
        //                    new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(
        //                        previousWeightLiftingSetViewModels?[i], currentWeightLiftingSetViewModels?[i]));
        //            }

        //            if (i < previousWeightliftingSetsLength && i >= currentWeightliftingSetsLength)
        //            {
        //                currentExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPairs.Add(
        //                    new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(
        //                        previousWeightLiftingSetViewModels?[i], null));
        //            }

        //            if (i >= previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
        //            {
        //                currentExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPairs.Add(
        //                    new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(null,
        //                        currentWeightLiftingSetViewModels?[i]));
        //            }
        //        }
        //    }

        //    var cardioSetViewModelRepository = new CardioSetViewModelRepository(_fittifyApiBaseUri);
        //    foreach (var exerciseHistoryOfmForGet in currentExerciseHistoryOfmCollectionQueryResult.OfmForGetCollection.Where(w => w.Exercise?.ExerciseType == ExerciseTypeEnum.Cardio.ToString()))
        //    {
        //        CardioSetViewModel[] previousCardioSetViewModels = null;
        //        if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
        //        {
        //            var enumerablePreviousCardioSetViewModels =
        //                await cardioSetViewModelRepository.GetCollectionByExerciseHistoryId(
        //                    exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault());
        //            previousCardioSetViewModels = enumerablePreviousCardioSetViewModels?.ToArray();
        //        }

        //        var enumerableCardioSetViewModels =
        //            await cardioSetViewModelRepository.GetCollectionByExerciseHistoryId(
        //                exerciseHistoryOfmForGet.Id);
        //        var currentCardioSetViewModels = enumerableCardioSetViewModels?.ToArray();

        //        int previousCardioSetsLength = previousCardioSetViewModels?.Length ?? 0;
        //        int currentCardioSetsLength = currentCardioSetViewModels?.Length ?? 0;
        //        int maxValuePairs /* NumberOfColumns*/ =
        //            Math.Max(previousCardioSetsLength, currentCardioSetsLength);

        //        var currentExerciseHistoryViewModel =
        //            exerciseHistoryViewModels.FirstOrDefault(f => f.Id == exerciseHistoryOfmForGet.Id);

        //        for (int i = 0; i < maxValuePairs; i++)
        //        {
        //            if (i < previousCardioSetsLength && i < currentCardioSetsLength)
        //            {
        //                currentExerciseHistoryViewModel.CurrentAndHistoricCardioSetPairs.Add(
        //                    new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(
        //                        previousCardioSetViewModels?[i], currentCardioSetViewModels?[i]));
        //            }

        //            if (i < previousCardioSetsLength && i >= currentCardioSetsLength)
        //            {
        //                currentExerciseHistoryViewModel.CurrentAndHistoricCardioSetPairs.Add(
        //                    new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(
        //                        previousCardioSetViewModels?[i], null));
        //            }

        //            if (i >= previousCardioSetsLength && i < currentCardioSetsLength)
        //            {
        //                currentExerciseHistoryViewModel.CurrentAndHistoricCardioSetPairs.Add(
        //                    new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(null,
        //                        currentCardioSetViewModels?[i]));
        //            }
        //        }
        //    }

        //    return exerciseHistoryViewModels;
        //}
    }
}

