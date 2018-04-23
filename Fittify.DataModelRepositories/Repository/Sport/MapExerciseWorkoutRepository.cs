﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModelRepositories.Owned;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class MapExerciseWorkoutRepository : AsyncCrud<MapExerciseWorkout, int>, IAsyncGetCollection<MapExerciseWorkout, MapExerciseWorkoutResourceParameters>, IAsyncOwnerIntId
    {
        public MapExerciseWorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<MapExerciseWorkout> GetById(int id)
        {
            return FittifyContext.MapExerciseWorkout
                .Include(i => i.Exercise)
                .Include(i => i.Workout)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public PagedList<MapExerciseWorkout> GetCollection(MapExerciseWorkoutResourceParameters resourceParameters, Guid ownerGuid)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<MapExerciseWorkout>()
                    .Where(o => o.OwnerGuid == ownerGuid)
                    .AsNoTracking()
                    .Include(i => i.Exercise)
                    .Include(i => i.Workout)
                    .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<MapExerciseWorkoutOfmForGet, MapExerciseWorkout>());

            allEntitiesQueryable = allEntitiesQueryable.Where(o => o.OwnerGuid == ownerGuid);
            
            if (resourceParameters.ExerciseId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseId == resourceParameters.ExerciseId);
            }

            if (resourceParameters.WorkoutId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.WorkoutId == resourceParameters.WorkoutId);
            }

            return PagedList<MapExerciseWorkout>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}