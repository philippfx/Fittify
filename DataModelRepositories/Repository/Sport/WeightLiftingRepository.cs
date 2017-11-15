using System;
using System.Collections.Generic;
using System.Text;
using DataModels.Models.Sport;

namespace DataModelRepositories.Repository.Sport
{
    public class WeightLiftingRepository : Crud<WeightLiftingSet, int>
    {
        public WeightLiftingRepository()
        {
            
        }

        public WeightLiftingRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
