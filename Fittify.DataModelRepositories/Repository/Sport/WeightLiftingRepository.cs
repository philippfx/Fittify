using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class WeightLiftingRepository : Crud<WeightLiftingSet, int>
    {
        public WeightLiftingRepository()
        {
            
        }

        public WeightLiftingRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public void TemporaryCreate(FittifyContext fittifyContext, int exerciseHistoryId)
        {
            var wls = new WeightLiftingSet();
            wls.ExerciseHistoryId = exerciseHistoryId;
            fittifyContext.WeightLiftingSets.Add(wls);
            fittifyContext.SaveChanges();
        }
    }
}
