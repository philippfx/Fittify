using System.Linq;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CategoryRepository : Crud<Category, int>
    {
        public CategoryRepository()
        {
            
        }

        public CategoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
