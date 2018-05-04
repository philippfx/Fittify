using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.ResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class MapExerciseWorkoutRepository : AsyncCrud<MapExerciseWorkout, MapExerciseWorkoutOfmForGet, int, MapExerciseWorkoutOfmResourceParameters>, IAsyncOwnerIntId
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

        public override PagedList<MapExerciseWorkout> GetCollection(MapExerciseWorkoutOfmResourceParameters ofmResourceParameters, Guid ownerGuid)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<MapExerciseWorkout>()
                    .Where(o => o.OwnerGuid == ownerGuid)
                    .AsNoTracking()
                    .Include(i => i.Exercise)
                    .Include(i => i.Workout)
                    .ApplySort(ofmResourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<MapExerciseWorkoutOfmForGet, MapExerciseWorkout>());
            
            if (ofmResourceParameters.ExerciseId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseId == ofmResourceParameters.ExerciseId);
            }

            if (ofmResourceParameters.WorkoutId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.WorkoutId == ofmResourceParameters.WorkoutId);
            }

            return PagedList<MapExerciseWorkout>.Create(allEntitiesQueryable,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }
    }
}