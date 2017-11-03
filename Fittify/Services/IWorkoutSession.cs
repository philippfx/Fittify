using System.Collections.Generic;
using Fittify.Entities;

namespace Fittify.Services
{
    public interface IWorkoutSession
    {
        ICollection<WorkoutSession> GetAll();
        WorkoutSession Get(int id);
    }
}
