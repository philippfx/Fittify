using Fittify.Entities.Workout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fittify.Entities.Seed.Workout
{
    public static class CategorySeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            var listCategories = new List<Entities.Workout.Category>()
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
