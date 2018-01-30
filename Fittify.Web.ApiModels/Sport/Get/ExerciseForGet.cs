using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Get
{
    public class ExerciseForGet : UniqueIdentifier<int>
    {
        public string Name { get; set; }
    }
}
