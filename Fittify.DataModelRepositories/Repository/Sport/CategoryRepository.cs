using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CategoryRepository : AsyncCrud<Category,int>
    {
        public CategoryRepository()
        {
            
        }

        public CategoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
