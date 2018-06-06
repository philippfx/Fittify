using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class CategoryRepository : AsyncCrudBase<Category, int, CategoryResourceParameters>, IAsyncEntityOwnerIntId
    {
        public CategoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }
    }
}