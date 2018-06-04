using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepositories;
using Fittify.Client.ApiModelRepositories.OfmRepository.Sport;
using Fittify.Client.ViewModels.Sport;
using Fittify.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public class WorkoutHistoryViewModelRepository : GenericViewModelRepository<int, WorkoutHistoryViewModel, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmCollectionResourceParameters>
    {
        private readonly AsyncWorkoutHistoryOfmRepository asyncGppdOfmWorkoutHistory;
        private readonly IConfiguration _appConfiguration;

        public WorkoutHistoryViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, IHttpRequestHandler httpRequestHandler)
            : base(appConfiguration, httpContextAccessor, "WorkoutHistory", httpRequestHandler)
        {
            asyncGppdOfmWorkoutHistory = new AsyncWorkoutHistoryOfmRepository(appConfiguration, httpContextAccessor, "WorkoutHistoryOfmCollectionResourceParameters", httpRequestHandler);
            _appConfiguration = appConfiguration;
        }

        public override async Task<ViewModelQueryResult<WorkoutHistoryViewModel>> Create(WorkoutHistoryOfmForPost workoutOfmForPost)
        {
            var ofmQueryResult = await asyncGppdOfmWorkoutHistory.Post(workoutOfmForPost);

            var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutHistoryViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 201)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<WorkoutHistoryViewModel>(ofmQueryResult.OfmForGet);
            }
            else
            {
                ofmQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }

            return workoutViewModelQueryResult;
        }

        public async Task<ViewModelQueryResult<WorkoutHistoryViewModel>> GetById(int id, WorkoutHistoryOfmResourceParameters workoutHistoryOfmResourceParameters)
        {
            var ofmQueryResult = await GenericAsyncGppdOfmWorkout.GetSingle(id, workoutHistoryOfmResourceParameters);

            var workoutViewModelQueryResult = new ViewModelQueryResult<WorkoutHistoryViewModel>();
            workoutViewModelQueryResult.HttpStatusCode = ofmQueryResult.HttpStatusCode;

            if ((int)ofmQueryResult.HttpStatusCode == 200)
            {
                workoutViewModelQueryResult.ViewModel =
                    Mapper.Map<WorkoutHistoryViewModel>(ofmQueryResult.OfmForGet);

                foreach (var eH in ofmQueryResult.OfmForGet.ExerciseHistories)
                {
                    var relatedViewModelExerciseHistory = workoutViewModelQueryResult.ViewModel.ExerciseHistories.FirstOrDefault(f => f.Id == eH.Id);
                    if (relatedViewModelExerciseHistory != null)
                    {
                        if(relatedViewModelExerciseHistory.Exercise.ExerciseType == ExerciseTypeEnum.WeightLifting.ToString())
                        {
                            var historicWeightLiftingSets = eH.PreviousExerciseHistory.WeightLiftingSets.OrderBy(o => o.Id).ToArray();
                            var currentWeightLiftingSets = eH.WeightLiftingSets.OrderBy(o => o.Id).ToArray();
                            var historicWeightLiftingSetsCount = eH.PreviousExerciseHistory.WeightLiftingSets.Count();
                            var currentWeightLiftingSetsCount = eH.WeightLiftingSets.Count();
                            var maxWeightLiftingSetsCount = Math.Max(historicWeightLiftingSetsCount, currentWeightLiftingSetsCount);

                            for (int i = 0; i < maxWeightLiftingSetsCount; i++)
                            {
                                var currentAndHistoricWeightLiftingSetPair = new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair();
                                if (historicWeightLiftingSetsCount > i)
                                {
                                    currentAndHistoricWeightLiftingSetPair.HistoricWeightLiftingSet
                                        = Mapper.Map<WeightLiftingSetViewModel>(historicWeightLiftingSets[i]);
                                }

                                if (currentWeightLiftingSetsCount > i)
                                {
                                    currentAndHistoricWeightLiftingSetPair.CurrentWeightLiftingSet
                                        = Mapper.Map<WeightLiftingSetViewModel>(currentWeightLiftingSets[i]);
                                }
                                relatedViewModelExerciseHistory.CurrentAndHistoricWeightLiftingSetPairs.Add(currentAndHistoricWeightLiftingSetPair);
                            }
                        }

                        if (relatedViewModelExerciseHistory.Exercise.ExerciseType == ExerciseTypeEnum.Cardio.ToString())
                        {
                            var historicCardioSets = eH.CardioSets.OrderBy(o => o.Id).ToArray();
                            var currentCardioSets = eH.CardioSets.OrderBy(o => o.Id).ToArray();
                            var historicCardioSetsCount = eH.PreviousExerciseHistory.CardioSets.Count();
                            var currentCardioSetsCount = eH.CardioSets.Count();
                            var maxCardioSetsCount = Math.Max(historicCardioSetsCount, currentCardioSetsCount);

                            for (int i = 0; i < maxCardioSetsCount; i++)
                            {
                                var currentAndHistoricCardioSetPair = new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair();
                                if (historicCardioSetsCount > i)
                                {
                                    currentAndHistoricCardioSetPair.HistoricCardioSet
                                        = Mapper.Map<CardioSetViewModel>(historicCardioSets[i]);
                                }

                                if (currentCardioSetsCount > i)
                                {
                                    currentAndHistoricCardioSetPair.CurrentCardioSet
                                        = Mapper.Map<CardioSetViewModel>(currentCardioSets[i]);
                                }
                                relatedViewModelExerciseHistory.CurrentAndHistoricCardioSetPairs.Add(currentAndHistoricCardioSetPair);
                            }
                        }
                    }                        
                }
            }
            else
            {
                workoutViewModelQueryResult.ErrorMessagesPresented = ofmQueryResult.ErrorMessagesPresented;
            }
            
            // Exercises
            var exerciseViewModelRepository = new ExerciseViewModelRepository(_appConfiguration, HttpContextAccessor, HttpRequestHandler);

            var exerciseViewModelCollectionQueryResult
                = await exerciseViewModelRepository.GetCollection(
                    new ExerciseOfmCollectionResourceParameters());

            workoutViewModelQueryResult.ViewModel.AllExercises
                = exerciseViewModelCollectionQueryResult.ViewModelForGetCollection;

            // Done
            return workoutViewModelQueryResult;
        }
    }
}
