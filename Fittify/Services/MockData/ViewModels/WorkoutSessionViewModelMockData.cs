using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Services.MockData.Entities;
using Fittify.ViewModels;

namespace Fittify.Services.MockData.ViewModels
{
    public class WorkoutSessionViewModelMockData : IWorkoutSessionViewModelData
    {
        private WorkoutSessionViewModel _trainingSessionViewModelMockData;
        private WorkoutSessionMockData _trainingSessionMockData;

        public WorkoutSessionViewModelMockData()
        {
            _trainingSessionMockData = new WorkoutSessionMockData();
            _trainingSessionViewModelMockData = new WorkoutSessionViewModel()
            {
                CurrentWorkoutSessionInProgress = _trainingSessionMockData.GetAll().FirstOrDefault(s => s.Id == 2),
                PreviousWorkoutSession = _trainingSessionMockData.GetAll().FirstOrDefault(s => s.Id == 1)
            };
            
            
        }

        public WorkoutSessionViewModel GetFirstOrDefault()
        {
            return _trainingSessionViewModelMockData;
        }
        
    }
}
