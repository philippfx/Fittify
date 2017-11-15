using System;
using System.Collections.Generic;
using System.Text;
using DataModels.Models.Sport;

namespace DataModelRepositories.Repository.Sport
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
