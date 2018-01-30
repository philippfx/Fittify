using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class WorkoutViewModel : UniqueIdentifier<int>
    {
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }

        public List<ExerciseViewModel> AssociatedExercises { get; set; }
        public List<ExerciseViewModel> AllExercises { get; set; }
        
    }
}
