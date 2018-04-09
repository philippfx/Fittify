using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class WorkoutViewModel : WorkoutOfmBase, IEntityUniqueIdentifier<int>
    {
        public int Id { get; set; }
        //public string Name { get; set; }

        //[ForeignKey("CategoryId")]
        //public int? CategoryId { get; set; }

        public List<ExerciseViewModel> AssociatedExercises { get; set; }
        public List<ExerciseViewModel> AllExercises { get; set; }
        
    }
}
