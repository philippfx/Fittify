using System;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.Owned;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.DataModelRepositories.Repository.Sport;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Api.OfmRepository.Post
{
    public class AsyncPostOfmForWorkoutHistory : AsyncPostOfm<WorkoutHistoryRepository, WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, int>
    {
        public AsyncPostOfmForWorkoutHistory(WorkoutHistoryRepository workoutHistoryRepository) : base(workoutHistoryRepository)
        {
            
        }
        public async Task<WorkoutHistoryOfmForGet> PostIncludingExerciseHistories(WorkoutHistoryOfmForPost ofmForPost, Guid ownerGuid)
        {
            var workoutHistory = Mapper.Map<WorkoutHistoryOfmForPost, WorkoutHistory>(ofmForPost);
            try
            {
                workoutHistory = await Repo.CreateIncludingExerciseHistories(workoutHistory, ownerGuid);
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }

            var ofm = Mapper.Map<WorkoutHistory, WorkoutHistoryOfmForGet>(workoutHistory);
            return ofm;
        }

        public override async Task<WorkoutHistoryOfmForGet> Post(WorkoutHistoryOfmForPost ofmForPost, Guid ownerGuid)
        {
            var workoutHistory = Mapper.Map<WorkoutHistoryOfmForPost, WorkoutHistory>(ofmForPost);
            try
            {
                workoutHistory = await Repo.Create(workoutHistory, ownerGuid);
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }

            var ofm = Mapper.Map<WorkoutHistory, WorkoutHistoryOfmForGet>(workoutHistory);
            return ofm;
        }
    }
}
