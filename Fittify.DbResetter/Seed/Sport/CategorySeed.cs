using System.Collections.Generic;
using System.Linq;
using Fittify.DataModelRepository;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DbResetter.Seed.Sport
{
    public static class CategorySeed
    {
        public static bool Seed(FittifyContext fittifyContext)
        {
            var listCategories = new List<Category>()
            {
                new Category() { Name = "ChestSeed" },
                new Category() { Name = "BackSeed" },
                new Category() { Name = "LegsSeed" },
            };

            foreach (var category in listCategories)
            {
                if (fittifyContext.Categories.FirstOrDefault(f => f.Name == category.Name) == null)
                {
                    fittifyContext.Add(category);
                }
            }

            if (fittifyContext.SaveChanges() >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
