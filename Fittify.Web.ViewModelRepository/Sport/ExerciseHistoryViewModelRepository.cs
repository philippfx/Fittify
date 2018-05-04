using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class ExerciseHistoryViewModelRepository : GenericViewModelRepository<int, ExerciseHistoryViewModel, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters>
    {
        private readonly GenericAsyncGppdOfm<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters> _asyncGppdOfmExerciseHistory;
        private readonly IConfiguration _appConfiguration;

        public ExerciseHistoryViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "ExerciseHistory")
        {
            _asyncGppdOfmExerciseHistory = new GenericAsyncGppdOfm<int, ExerciseHistoryOfmForGet, ExerciseHistoryOfmForPost, ExerciseHistoryOfmResourceParameters>(appConfiguration, httpContextAccessor, "ExerciseHistory");
            _appConfiguration = appConfiguration;
        }
        
        public override async Task<ViewModelCollectionQueryResult<ExerciseHistoryViewModel>> GetCollection(ExerciseHistoryOfmResourceParameters exerciseHistoryResourceParameters)
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
                        await weightLiftingSetViewModelRepository.GetCollection(new WeightLiftingSetOfmResourceParameters()
                        {
                            ExerciseHistoryId = exerciseHistoryOfmForGet.PreviousExerciseHistoryId.GetValueOrDefault()
                        });

                    previousWeightLiftingSetViewModels = previousWeightLiftingSetViewModelCollectionQueryResult.ViewModelForGetCollection?.ToArray();
                }

                var currentWeightLiftingSetViewModelCollectionQueryResult =
                    await weightLiftingSetViewModelRepository.GetCollection(new WeightLiftingSetOfmResourceParameters()
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
                        await cardioSetViewModelRepository.GetCollection(new CardioSetOfmResourceParameters()
                        {
                            ExerciseHistoryId = exerciseHistoryOfmForGet.PreviousExerciseHistoryId
                        });
                    previousCardioSetViewModels = previousCardioSetViewModelCollecitonResult.ViewModelForGetCollection?.ToArray();
                }

                var currentCardioSetViewModelCollectionQueryResult =
                    await cardioSetViewModelRepository.GetCollection(new CardioSetOfmResourceParameters()
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

