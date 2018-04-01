using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Fittify.Web.View.ViewModelRepository.Sport
{
    public class WorkoutHistoryViewModelRepository : AsyncGppdRepository<int, WorkoutHistoryOfmForPost, WorkoutHistoryViewModel>
    {
        public WorkoutHistoryViewModelRepository()
        {

        }

        public virtual async Task<IEnumerable<WorkoutHistoryViewModel>> GetCollectionByWorkoutId(int workoutId)
        {
            var workoutHistoriesOfmForGets = await AsyncGppd.GetCollection<WorkoutHistoryOfmForGet>("http://localhost:52275/api/workouthistories?workoutId=" + workoutId);
            var result = Mapper.Map<IEnumerable<WorkoutHistoryViewModel>>(workoutHistoriesOfmForGets);
            return result;
        }

        public virtual async Task<WorkoutHistoryViewModel> GetDetailsById(int workoutHistoryId)
        {
            var workoutHistoryOfmForGet = await AsyncGppd.GetSingle<WorkoutHistoryOfmForGet>("http://localhost:52275/api/workouthistories/" + workoutHistoryId);
            var workoutHistoryViewModel = Mapper.Map<WorkoutHistoryViewModel>(workoutHistoryOfmForGet);

            try
            {
                var gppdRepoExerciseHistory = new ExerciseHistoryViewModelRepository();

                workoutHistoryViewModel.ExerciseHistories = await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(workoutHistoryViewModel.Id);

                var exerciseViewModelRepository = new ExerciseViewModelRepository();
                var allExercises = await exerciseViewModelRepository.GetAll();
                workoutHistoryViewModel.AllExercises = allExercises;


                //var previousWeightLiftingSetIds = new List<int>();
                //foreach (var eH in workoutHistoryViewModel.ExerciseHistories)
                //{
                //    //var gppdRepoWeightLiftingSet = new WeightLiftingSetViewModelRepository();
                //    //var currentWeightliftingSets = gppdRepoWeightLiftingSet.GetCollectionByExerciseHistoryId(eH.Id);



                //}


                //foreach (var eH in workoutHistoryViewModel.ExerciseHistories)
                //{
                //    var gppdRepoWeightLiftingSet = new WeightLiftingSetViewModelRepository();
                //    var currentWeightliftingSets = gppdRepoWeightLiftingSet.GetCollectionByExerciseHistoryId(eH.Id);

                //    WeightLiftingSetViewModel[] previousWeightliftingSets = null;
                //    if (eH.PreviousExerciseHistoryId != null)
                //    {
                //        gppdRepoExerciseHistory = new AsyncGppdRepository<,,>(
                //            "http://localhost:52275/api/exercisehistories?workoutHistoryId=" +
                //            eH.PreviousExerciseHistoryId);
                //        eH.PreviousExerciseHistory =
                //            await gppdRepoExerciseHistory.GetCollectionByWorkoutHistoryId(eH.PreviousExerciseHistoryId.GetValueOrDefault());
                //        gppdRepoExerciseHistory.GetCollection().Result.FirstOrDefault();

                //        gppdRepoWeightLiftingSet = new AsyncGppdRepository<,,>(
                //            "http://localhost:52275/api/weightliftingsets?exerciseHistoryId=" +
                //            eH.PreviousExerciseHistoryId);
                //        previousWeightliftingSets = gppdRepoWeightLiftingSet.GetCollection().Result.ToArray();
                //    }

                //    int previousWeightliftingSetsLength = previousWeightliftingSets?.Length ?? 0;
                //    int currentWeightliftingSetsLength = currentWeightliftingSets?.Length ?? 0;
                //    int maxValuePairs /* NumberOfColumns*/ =
                //        Math.Max(previousWeightliftingSetsLength, currentWeightliftingSetsLength);

                //    eH.CurrentAndHistoricWeightLiftingSetPairs =
                //        new List<ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair>();
                //    for (int i = 0; i < maxValuePairs; i++)
                //    {
                //        if (i < previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
                //        {
                //            eH.CurrentAndHistoricWeightLiftingSetPairs.Add(
                //                new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(
                //                    previousWeightliftingSets[i], currentWeightliftingSets[i]));
                //        }

                //        if (i < previousWeightliftingSetsLength && i >= currentWeightliftingSetsLength)
                //        {
                //            eH.CurrentAndHistoricWeightLiftingSetPairs.Add(
                //                new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(
                //                    previousWeightliftingSets[i], null));
                //        }

                //        if (i >= previousWeightliftingSetsLength && i < currentWeightliftingSetsLength)
                //        {
                //            eH.CurrentAndHistoricWeightLiftingSetPairs.Add(
                //                new ExerciseHistoryViewModel.CurrentAndHistoricWeightLiftingSetPair(null,
                //                    currentWeightliftingSets[i]));
                //        }
                //    }

                //    CardioSetViewModel[] previousCardioSets = null;
                //    CardioSetViewModel[] currentCardioSets = null;
                //    if (eH.PreviousExerciseHistoryId != null)
                //    {
                //        gppdRepoExerciseHistory = new AsyncGppdRepository<,,>(
                //            "http://localhost:52275/api/exercisehistories?workoutHistoryId=" +
                //            eH.PreviousExerciseHistoryId);
                //        eH.PreviousExerciseHistory = gppdRepoExerciseHistory.GetCollection().Result.FirstOrDefault();

                //        var gppdRepoCardioSet = new AsyncGppdRepository<,,>(
                //            "http://localhost:52275/api/cardiosets?exerciseHistoryId=" + eH.PreviousExerciseHistoryId);
                //        previousCardioSets = gppdRepoCardioSet.GetCollection().Result.ToArray();

                //        gppdRepoCardioSet =
                //            new AsyncGppdRepository<,,>(
                //                "http://localhost:52275/api/cardiosets?exerciseHistoryId=" + eH.Id);
                //        currentCardioSets = gppdRepoCardioSet.GetCollection().Result.ToArray();
                //    }

                //    int previousCardioSetsLength = previousCardioSets?.Length ?? 0;
                //    int currentCardioSetsLength = currentCardioSets?.Length ?? 0;
                //    maxValuePairs /* NumberOfColumns*/ = Math.Max(previousCardioSetsLength, currentCardioSetsLength);

                //    eH.CurrentAndHistoricCardioSetPairs =
                //        new List<ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair>();
                //    for (int i = 0; i < maxValuePairs; i++)
                //    {
                //        if (i < previousCardioSetsLength && i < currentCardioSetsLength)
                //        {
                //            eH.CurrentAndHistoricCardioSetPairs.ToList().Add(
                //                new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(previousCardioSets[i],
                //                    currentCardioSets[i]));
                //        }

                //        if (i < previousCardioSetsLength && i >= currentCardioSetsLength)
                //        {
                //            eH.CurrentAndHistoricCardioSetPairs.ToList().Add(
                //                new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(previousCardioSets[i],
                //                    null));
                //        }

                //        if (i >= previousCardioSetsLength && i < currentCardioSetsLength)
                //        {
                //            eH.CurrentAndHistoricCardioSetPairs.ToList().Add(
                //                new ExerciseHistoryViewModel.CurrentAndHistoricCardioSetPair(null,
                //                    currentCardioSets[i]));
                //        }
                //    }

                //    var gppdRepoExercises =
                //        new AsyncGppdRepository<,,>("http://localhost:52275/api/exercises");
                //    workoutHistory.AllExercises = gppdRepoExercises.GetCollection().Result.ToList();
                //}
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }

            return workoutHistoryViewModel;
        }
    }
}
