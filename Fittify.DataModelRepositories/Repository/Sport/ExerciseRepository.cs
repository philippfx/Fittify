using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class ExerciseRepository : AsyncCrudForEntityName<Exercise,int>
    {
        public ExerciseRepository()
        {
            
        }

        public ExerciseRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        //public override IQueryable<Exercise> GetAll()
        //{
        //    return FittifyContext.Set<Exercise>().Include(i => i.ExercisesWorkoutsMap).ThenInclude(i => i.Workout).Include(i => i.ExerciseHistories).AsNoTracking();
        //}

        //public override async Task<Exercise> GetById(int id)
        //{
        //    return await GetAll().FirstOrDefaultAsync(f => f.Id == id);
        //}

        public async Task<List<Exercise>> GetCollectionByFkWorkoutId(int workoutId)
        {
            return await FittifyContext.Set<MapExerciseWorkout>().Where(eW => eW.WorkoutId == workoutId).Select(e => e.Exercise).ToListAsync();
        }

    }
}
