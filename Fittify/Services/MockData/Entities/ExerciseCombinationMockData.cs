using System;
using System.Collections.Generic;
using Fittify.Entities;

namespace Fittify.Services.MockData.Entities
{
    public class ExerciseCombinationMockData : IData<ExerciseHistory>
    {
        //private List<ExerciseCombination> _exerciseSets;

        //public ExerciseCombinationMockData()
        //{
        //    _exerciseSets = new List<ExerciseCombination>();
        //    _exerciseSets.Add(new ExerciseCombination()
        //    {

        //    });
        //}

        public ICollection<ExerciseHistory> GetAll()
        {
            throw new NotImplementedException();
        }

        public ExerciseHistory Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
