using System.Collections.Generic;
using Fittify.Entities.Workout;

namespace Fittify.Services.MockData.Entities
{
    public class CategoryMockData : IData<Category>
    {
        private List<Category> _categories;
        public CategoryMockData()
        {
            _categories = new List<Category>();
            _categories.Add(new Category() { Id = 1, Name = "Chest" });
            _categories.Add(new Category() { Id = 2, Name = "Back" });
            _categories.Add(new Category() { Id = 3, Name = "Legs" });
        }
        
        public ICollection<Category> GetAll()
        {
            return _categories;
        }

        public Category Get(int id)
        {
            throw new System.NotImplementedException();
        }
    }

    
}
