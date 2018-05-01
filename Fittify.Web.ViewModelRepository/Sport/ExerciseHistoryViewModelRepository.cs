﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class ExerciseHistoryViewModelRepository : GenericViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryResourceParameters>
    {
        private readonly GenericAsyncGppdOfm<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryResourceParameters> _asyncGppdOfmExerciseHistory;
        private readonly IConfiguration _appConfiguration;

        public ExerciseHistoryViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "ExerciseHistory")
        {
            _asyncGppdOfmExerciseHistory = new GenericAsyncGppdOfm<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryResourceParameters>(appConfiguration, "ExerciseHistory");
            _appConfiguration = appConfiguration;
        }
        
        public override async Task<ViewModelCollectionQueryResult<ExerciseHistoryViewModel>> GetCollection(ExerciseHistoryResourceParameters exerciseHistoryResourceParameters)
        {
            // Current ExerciseHistories
            var exerciseHistoryViewModelCollectionQueryResult =
                await base.GetCollection(exerciseHistoryResourceParameters);
            
            var exerciseHistoryViewModels =
                exerciseHistoryViewModelCollectionQueryResult.ViewModelForGetCollection;
            
            var weightLiftingSetViewModelRepository = new WeightLiftingSetViewModelRepository(_appConfiguration);
            foreach (var exerciseHistoryOfmForGet in exerciseHistoryViewModelCollectionQueryResult.ViewModelForGetCollection.Where(w => w.Exercise?.ExerciseType == ExerciseTypeEnum.WeightLifting.ToString()))
            {
                WeightLiftingSetViewModel[] previousWeightLiftingSetViewModels = null;
                if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
                {
                    var previousWeightLiftingSetViewModelCollectionQueryResult =
                        await weightLiftingSetViewModelRepository.GetCollection(new WeightLiftingSetResourceParameters()
                        {
                            ExerciseHistoryId = exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault()
                        });

                    previousWeightLiftingSetViewModels = previousWeightLiftingSetViewModelCollectionQueryResult.ViewModelForGetCollection?.ToArray();
                }

                var currentWeightLiftingSetViewModelCollectionQueryResult =
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

            var cardioSetViewModelRepository = new CardioSetViewModelRepository(_appConfiguration);
            foreach (var exerciseHistoryOfmForGet in exerciseHistoryViewModelCollectionQueryResult.ViewModelForGetCollection.Where(w => w.Exercise?.ExerciseType == ExerciseTypeEnum.Cardio.ToString()))
            {
                CardioSetViewModel[] previousCardioSetViewModels = null;
                if (exerciseHistoryOfmForGet.PreviousExerciseHistoryId != null)
                {
                    var previousCardioSetViewModelCollecitonResult =
                        await cardioSetViewModelRepository.GetCollection(new CardioSetResourceParameters()
                        {
                            ExerciseHistoryId = exerciseHistoryOfmForGet.PreviousExerciseHistoryId
                        });
                    previousCardioSetViewModels = previousCardioSetViewModelCollecitonResult.ViewModelForGetCollection?.ToArray();
                }

                var currentCardioSetViewModelCollectionQueryResult =
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
    }
}

