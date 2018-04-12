using Fittify.Web.ViewModels.Sport;
using Newtonsoft.Json;

namespace Fittify.Web.ViewModels
{
    public class JsonToObjectRepository
    {
        public CategoryViewModel JsonToCategoryViewModel(string json)
        {
            return JsonConvert.DeserializeObject<CategoryViewModel>(json);
        }

        public WorkoutViewModel JsonToWorkoutViewModel(string json)
        {
            return JsonConvert.DeserializeObject<WorkoutViewModel>(json);
        }
    }
}
