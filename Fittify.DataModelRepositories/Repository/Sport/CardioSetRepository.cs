using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CardioSetRepository : Crud<CardioSet, int>
    {
        public CardioSetRepository()
        {
            
        }

        public CardioSetRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
