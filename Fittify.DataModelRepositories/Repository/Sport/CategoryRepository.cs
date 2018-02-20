using System.Threading.Tasks;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CategoryRepository : AsyncCrudForEntityName<Category,int>
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
