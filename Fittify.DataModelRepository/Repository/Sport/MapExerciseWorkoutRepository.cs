using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class MapExerciseWorkoutRepository : AsyncCrudBase<MapExerciseWorkout, int, MapExerciseWorkoutResourceParameters>, IAsyncOwnerIntId
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

        public override async Task<PagedList<MapExerciseWorkout>> GetPagedCollection(MapExerciseWorkoutResourceParameters ofmResourceParameters)
        {
            var linqToEntityQuery = await base.CreateCollectionQueryable(ofmResourceParameters);

            linqToEntityQuery = linqToEntityQuery
                    .Include(i => i.Exercise)
                    .Include(i => i.Workout)
                    .Where(o => o.OwnerGuid == ofmResourceParameters.OwnerGuid);
            
            if (ofmResourceParameters.ExerciseId != null)
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.ExerciseId == ofmResourceParameters.ExerciseId);
            }

            if (ofmResourceParameters.WorkoutId != null)
            {
                linqToEntityQuery = linqToEntityQuery.Where(w => w.WorkoutId == ofmResourceParameters.WorkoutId);
            }

            return await PagedList<MapExerciseWorkout>.CreateAsync(linqToEntityQuery,
                ofmResourceParameters.PageNumber,
                ofmResourceParameters.PageSize);
        }
    }
}