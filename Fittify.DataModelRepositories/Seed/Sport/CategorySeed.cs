using System.Collections.Generic;
using System.Linq;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DataModelRepositories.Seed.Sport
{
    public static class CategorySeed
    {
        public static void Seed(FittifyContext fittifyContext)
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
            fittifyContext.SaveChanges();
        }
    }
}
