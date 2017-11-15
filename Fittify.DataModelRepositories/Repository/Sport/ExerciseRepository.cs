namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class ExerciseRepository : Crud<ExerciseHistoryRepository, int>
    {
        public ExerciseRepository()
        {
            
        }

        public ExerciseRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
