using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class MapExerciseWorkoutRepository : AsyncGetCollection<MapExerciseWorkout,MapExerciseWorkoutOfmForGet,int>
    {
        public MapExerciseWorkoutRepository()
        {
            
        }

        public MapExerciseWorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public virtual async Task DeleteByWorkoutAndExerciseId(int workoutId, int exerciseId)
        {
            var entity = await FittifyContext.MapExerciseWorkout.FirstOrDefaultAsync(f => f.WorkoutId == workoutId && f.ExerciseId == exerciseId);
            FittifyContext.Remove(entity);
            await FittifyContext.SaveChangesAsync();
        }
    }
}
