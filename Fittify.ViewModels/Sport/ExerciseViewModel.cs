using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Web.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class ExerciseViewModel : ExerciseOfmBase, IUniqueIdentifier<int>
    {
        public int Id { get; set; }
        //public string Name { get; set; }
    }
}
