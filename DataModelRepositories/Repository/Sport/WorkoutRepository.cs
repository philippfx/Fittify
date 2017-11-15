using System;
using System.Collections.Generic;
using System.Text;
using DataModels.Models.Sport;

namespace DataModelRepositories.Repository.Sport
{
    public class WorkoutRepository : Crud<Workout, int>
    {
        public WorkoutRepository()
        {
            
        }

        public WorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
