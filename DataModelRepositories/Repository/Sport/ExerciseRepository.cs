using System;
using System.Collections.Generic;
using System.Text;

namespace DataModelRepositories.Repository.Sport
{
    public class ExerciseRepository : Crud<ExerciseHistoryRepository, int>
    {
        public ExerciseRepository()
        {
            
        }

        public ExerciseRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
