using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class WorkoutRepository : AsyncGetCollectionForEntityName<Workout, WorkoutOfmForGet, int>
    {
        public WorkoutRepository()
        {
            
        }

        public WorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public override async Task<Workout> GetById(int id)
        {
            return await FittifyContext.Workouts
                .Include(i => i.ExercisesWorkoutsMap)
                .ThenInclude(i => i.Exercise)
                .Include(i => i.WorkoutHistories)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
