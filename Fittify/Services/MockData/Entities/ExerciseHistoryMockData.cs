using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Entities;

namespace Fittify.Services.MockData.Entities
{
    public class ExerciseHistoryMockData : IData<ExerciseHistory>
    {
        private List<ExerciseHistory> _exerciseHistories;
        private WeightLiftingSetMockData _weightLiftSets;
        private ExerciseHistory _exerciseHistory;

        public ExerciseHistoryMockData()
        {
            _exerciseHistory = new ExerciseHistory();
            _weightLiftSets = new WeightLiftingSetMockData();

            _exerciseHistories = new List<ExerciseHistory>()
            {
                new ExerciseHistory()
                {
                    Id = 1,
                    ExecutedOnDateTime = DateTime.Now,
                    PreviousExerciseId = null,
                    WeightLiftingSets = _weightLiftSets.GetAll(),
                    CardioSets = null,
                    StandardMachineAdjustable1 = null,
                    StandardMachineAdjustable2 = null,
                    TotalScoreOfExercise = 100
                },
                new ExerciseHistory()
                {
                    Id = 2,
                    ExecutedOnDateTime = DateTime.Now,
                    PreviousExerciseId = 1,
                    PreviousExercise = null /*_exerciseHistoryMockData.Get(1)*/,
                    WeightLiftingSets = new List<WeightLiftingSet>(),
                    CardioSets = null,
                    StandardMachineAdjustable1 = null,
                    StandardMachineAdjustable2 = null,
                    TotalScoreOfExercise = null
                }
            };
        }

        public ICollection<ExerciseHistory> GetAll()
        {
            return _exerciseHistories;
        }

        public ExerciseHistory Get(int id)
        {
            return null;
            //return GetAll().FirstOrDefault(e => e.Id == id);
        }
    }
}
