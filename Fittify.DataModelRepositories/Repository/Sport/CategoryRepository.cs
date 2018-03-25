using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CategoryRepository : AsyncGetCollectionForEntityName<Category, CategoryOfmForGet, int>
    {
        public CategoryRepository()
        {
            
        }

        public CategoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public override async Task<Category> GetById(int id)
        {
            return await FittifyContext.Categories
                .Include(i => i.Workouts)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

    }
}
