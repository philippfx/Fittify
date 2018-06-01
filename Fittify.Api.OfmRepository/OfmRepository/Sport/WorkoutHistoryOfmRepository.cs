using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common;
using Fittify.Common.Extensions;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.Repository.Sport;
using Fittify.DataModelRepository.Repository.Sport.ExtendedInterfaces;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;
using IPropertyMappingService = Fittify.Api.OfmRepository.Services.IPropertyMappingService;
using ITypeHelperService = Fittify.Api.OfmRepository.Services.ITypeHelperService;

namespace Fittify.Api.OfmRepository.OfmRepository.Sport
{
    public class WorkoutHistoryOfmRepository : AsyncOfmRepositoryBase<WorkoutHistory, WorkoutHistoryOfmForGet, WorkoutHistoryOfmForPost, WorkoutHistoryOfmForPatch, int, WorkoutHistoryOfmCollectionResourceParameters, WorkoutHistoryResourceParameters>,
        IAsyncOfmRepositoryForWorkoutHistory, IAsyncEntityOwnerIntId

    {
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        private readonly IServiceProvider _serviceProvider;

        public WorkoutHistoryOfmRepository(
            IWorkoutHistoryRepository repo,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            IServiceProvider serviceProvider
        )
            : base(repo, propertyMappingService, typeHelperService)
        {
            _serviceProvider = serviceProvider;
            _workoutHistoryRepository = repo;
        }

        public async Task<OfmForGetQueryResult<WorkoutHistoryOfmForGet>> GetById(int id, WorkoutHistoryOfmResourceParameters resourceParameters, Guid ownerGuid)
        {
            var ofmForGetResult = new OfmForGetQueryResult<WorkoutHistoryOfmForGet>();
            ofmForGetResult = await AsyncGetOfmGuardClause.ValidateGetById(ofmForGetResult, resourceParameters.Fields); // Todo: Validate additional *Include* query parameters

            if (ofmForGetResult.ErrorMessages.Count > 0)
            {
                return ofmForGetResult;
            }

            ////var entity = await Repo.GetById(id);


            //ofmForGetResult.ReturnedTOfmForGet = Mapper.Map<WorkoutHistory, WorkoutHistoryOfmForGet>(entity);

            var workoutHistoryLinqToEntity = Repo.LinqToEntityQueryable();

            if (resourceParameters.IncludeExerciseHistories.ToBool())
            {
                ////var exerciseHistoryRepository =
                ////    _serviceProvider.GetService(typeof(IAsyncCrud<ExerciseHistory, int, ExerciseHistoryResourceParameters>)) as ExerciseHistoryRepository;

                ////if (exerciseHistoryRepository == null) throw new NullReferenceException(nameof(ExerciseHistoryOfmRepository));

                workoutHistoryLinqToEntity =
                    workoutHistoryLinqToEntity
                        .Include(i =>  i.ExerciseHistories)
                        .ThenInclude(i => i.Exercise);

                if (resourceParameters.IncludeWeightLiftingSets.ToBool())
                {
                    workoutHistoryLinqToEntity = 
                        workoutHistoryLinqToEntity
                            .Include(i => i.ExerciseHistories)
                            .ThenInclude(i => i.WeightLiftingSets);

                    if (resourceParameters.IncludePreviousExerciseHistories.ToBool())
                    {
                        workoutHistoryLinqToEntity = workoutHistoryLinqToEntity
                            .Include(i => i.ExerciseHistories)
                            .ThenInclude(i => i.WeightLiftingSets)
                            .Include(i => i.ExerciseHistories)
                            .ThenInclude(i => i.PreviousExerciseHistory)
                            .ThenInclude(i => i.WeightLiftingSets);
                    }
                }

                if (resourceParameters.IncludeCardioSets.ToBool())
                {
                    workoutHistoryLinqToEntity =
                        workoutHistoryLinqToEntity
                        .Include(i => i.ExerciseHistories)
                        .ThenInclude(i => i.CardioSets);

                    if (resourceParameters.IncludePreviousExerciseHistories.ToBool())
                    {
                        workoutHistoryLinqToEntity =
                            workoutHistoryLinqToEntity
                            .Include(i => i.ExerciseHistories)
                            .ThenInclude(i => i.CardioSets)
                            .Include(i => i.ExerciseHistories)
                            .ThenInclude(i => i.PreviousExerciseHistory)
                            .ThenInclude(i => i.CardioSets);
                    }
                }

                ////var exerciseHistories = workoutHistoryLinqToEntity.ToList();

                ////var exerciseHistoryOfmForGets = Mapper.Map<List<ExerciseHistoryOfmForGet>>(exerciseHistories);
                ////ofmForGetResult.ReturnedTOfmForGet.ExerciseHistories = exerciseHistoryOfmForGets;
            }

            var workoutHistory = workoutHistoryLinqToEntity.Include(i => i.Workout).FirstOrDefault(f => f.Id == id && f.OwnerGuid == ownerGuid);
            ofmForGetResult.ReturnedTOfmForGet = Mapper.Map<WorkoutHistoryOfmForGet>(workoutHistory);

            return ofmForGetResult;
        }

        public async Task<WorkoutHistoryOfmForGet> PostIncludingExerciseHistories(WorkoutHistoryOfmForPost ofmForPost, Guid ownerGuid)
        {
            var workoutHistory = Mapper.Map<WorkoutHistoryOfmForPost, WorkoutHistory>(ofmForPost);
            workoutHistory = await _workoutHistoryRepository.CreateIncludingExerciseHistories(workoutHistory, ownerGuid);

            var ofm = Mapper.Map<WorkoutHistory, WorkoutHistoryOfmForGet>(workoutHistory);
            return ofm;
        }
    }
}
