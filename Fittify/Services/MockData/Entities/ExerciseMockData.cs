using System.Collections.Generic;
using Fittify.Entities;
using Fittify.Entities.Workout;

namespace Fittify.Services.MockData.Entities
{
    public class ExerciseMockData : IData<Exercise>
    {
        private List<Exercise> _exercises;
        private ExerciseHistoryMockData _exerciseHistoryMockData;

        public ExerciseMockData()
        {
            _exerciseHistoryMockData = new ExerciseHistoryMockData();
            _exercises = new List<Exercise>();
            _exercises.Add(new Exercise()
            {
                Id = 1,
                Name = "Barbell Bench Press",
                ExerciseHistories = _exerciseHistoryMockData.GetAll()
            });
            _exercises.Add(new Exercise()
            {
                Id = 2,
                Name = "Machine Chest Press",
                ExerciseHistories = _exerciseHistoryMockData.GetAll()
            });
            _exercises.Add(new Exercise()
            {
                Id = 3,
                Name = "Machine Chest Incline",
                ExerciseHistories = _exerciseHistoryMockData.GetAll()
            });
        }
        public ICollection<Exercise> GetAll()
        {
            return _exercises;
        }

        public Exercise Get(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
