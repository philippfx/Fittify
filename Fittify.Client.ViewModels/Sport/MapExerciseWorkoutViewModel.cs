using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class MapExerciseWorkoutViewModel : MapExerciseWorkoutOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
    }
}
