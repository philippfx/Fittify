using System;
using System.Collections.Generic;
using System.Text;
using DataModels.Models.Sport;

namespace DataModelRepositories.Repository.Sport
{
    public class ExerciseHistoryRepository : Crud<ExerciseHistory, int>
    {
        public ExerciseHistoryRepository()
        {
            
        }

        public ExerciseHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
