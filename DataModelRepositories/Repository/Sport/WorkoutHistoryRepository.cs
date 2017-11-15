using System;
using System.Collections.Generic;
using System.Text;
using DataModels.Models.Sport;

namespace DataModelRepositories.Repository.Sport
{
    public class WorkoutHistoryRepository : Crud<WorkoutHistory, int>
    {
        public WorkoutHistoryRepository()
        {
            
        }

        public WorkoutHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
