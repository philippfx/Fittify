using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Extensions;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using IPropertyMappingService = Fittify.Api.OfmRepository.Services.IPropertyMappingService;
using ITypeHelperService = Fittify.Api.OfmRepository.Services.ITypeHelperService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd.Sport;

namespace Fittify.Api.OfmRepository.OfmRepository.Sport
{
    public class WorkoutOfmRepository : 
        AsyncOfmRepositoryBase<Workout, WorkoutOfmForGet, WorkoutOfmForPost, WorkoutOfmForPatch, int, WorkoutOfmCollectionResourceParameters, WorkoutResourceParameters>,
        IAsyncOfmRepositoryForWorkout, 
        IAsyncOfmOwnerIntId
    {
        public WorkoutOfmRepository(IAsyncCrud<Workout, int, WorkoutResourceParameters> repo,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService
        )
            : base(repo, propertyMappingService, typeHelperService)
        {
            
        }

        public async Task<OfmForGetQueryResult<WorkoutOfmForGet>> GetById(int id, WorkoutOfmResourceParameters resourceParameters, Guid ownerGuid)
        {
            var ofmForGetResult = new OfmForGetQueryResult<WorkoutOfmForGet>();
            ofmForGetResult = await AsyncGetOfmGuardClause.ValidateGetById(ofmForGetResult, resourceParameters.Fields); // Todo: Validate additional *Include* query parameters

            if (ofmForGetResult.ErrorMessages.Count > 0)
            {
                return ofmForGetResult;
            }

            var workoutLinqToEntity = Repo.LinqToEntityQueryable();

            workoutLinqToEntity = workoutLinqToEntity
                .Include(i => i.MapExerciseWorkout)
                .Include(i => i.WorkoutHistories);

            if (resourceParameters.IncludeExercises.ToBool())
            {
                workoutLinqToEntity =
                    workoutLinqToEntity
                        .Include(i => i.MapExerciseWorkout)
                        .ThenInclude(i => i.Exercise);
            }

            var workout = workoutLinqToEntity.FirstOrDefault(f => f.Id == id && f.OwnerGuid == ownerGuid);

            if (workout == null)
            {
                return ofmForGetResult;
            }

            ofmForGetResult.ReturnedTOfmForGet = Mapper.Map<Workout, WorkoutOfmForGet>(workout);

            if (resourceParameters.IncludeExercises.ToBool() && workout.MapExerciseWorkout.Count() > 0)
            {
                ofmForGetResult.ReturnedTOfmForGet.Exercises = Mapper.Map<List<ExerciseOfmForGet>>(workout.MapExerciseWorkout.Select(s => s.Exercise).ToList());
            }

            return ofmForGetResult;
        }

    }
}
