using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class WorkoutRepository : Crud<Workout, int>
    {
        public WorkoutRepository()
        {
            
        }

        public WorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
