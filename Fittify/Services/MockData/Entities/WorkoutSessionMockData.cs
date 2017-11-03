using System;
using System.Collections.Generic;
using System.Linq;
using Fittify.Entities;

namespace Fittify.Services.MockData.Entities
{
    public class WorkoutSessionMockData : IWorkoutSession
    {
        private ICollection<WorkoutSession> _workoutSessions;
        private ExerciseMockData _exerciseMockData;

        public WorkoutSessionMockData()
        {
            //_exerciseMockData = new ExerciseMockData();
            
            _workoutSessions = new List<WorkoutSession>();
            //_workoutSessions.Add(new WorkoutSession
            //{
            //    Id = 1,
            //    Name = "Monday Chest",
            //    SessionStart = DateTime.Now.AddHours(-72),
            //    SessionEnd = DateTime.Now.AddHours(-70),
            //    Exercises_WorkoutSessions = null
            //});
            //_workoutSessions.Add(new WorkoutSession
            //{
            //    Id = 2,
            //    Name = "Wednesday Chest",
            //    SessionStart = DateTime.Now.AddHours(-48),
            //    SessionEnd = DateTime.Now.AddHours(-46),
            //    //Exercises_WorkoutSessions = _exerciseMockData.GetAll()
            //});
            //_workoutSessions.Add(new WorkoutSession
            //{
            //    Id = 3,
            //    Name = "Friday Legs",
            //    SessionStart = DateTime.Now.AddHours(-2),
            //    SessionEnd = DateTime.Now,
            //    Exercises_WorkoutSessions = null
            //});
        }
        public ICollection<WorkoutSession> GetAll()
        {
            return _workoutSessions;
        }

        public WorkoutSession Get(int id)
        {
            return GetAll().FirstOrDefault(w => w.Id == id);
        }
    }
}
