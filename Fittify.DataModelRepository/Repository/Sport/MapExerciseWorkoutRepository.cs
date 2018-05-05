using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class MapExerciseWorkoutRepository : AsyncCrudBase<MapExerciseWorkout, MapExerciseWorkoutOfmForGet, int, MapExerciseWorkoutResourceParameters>, IAsyncOwnerIntId
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

        public override PagedList<MapExerciseWorkout> GetCollection(MapExerciseWorkoutResourceParameters resourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<MapExerciseWorkout>()
                    .Where(o => o.OwnerGuid == resourceParameters.OwnerGuid)
                    .AsNoTracking()
                    .Include(i => i.Exercise)
                    .Include(i => i.Workout)
                    .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<MapExerciseWorkoutOfmForGet, MapExerciseWorkout>());
            
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