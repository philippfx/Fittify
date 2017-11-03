using Fittify.ViewModels;

namespace Fittify.Services
{
    public interface IWorkoutSessionViewModelData
    {
        WorkoutSessionViewModel GetFirstOrDefault();
    }
}
