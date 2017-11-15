using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class MapExerciseWorkoutRepository : Crud<MapExerciseWorkout, int>
    {
        public MapExerciseWorkoutRepository()
        {
            
        }

        public MapExerciseWorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
