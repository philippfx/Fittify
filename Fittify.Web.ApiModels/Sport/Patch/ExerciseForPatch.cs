using System.Collections.Generic;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Patch
{
    public class ExerciseForPatch : UniqueIdentifier<int>
    {
        public ExerciseForPatch()
        {
            
        }

        public string Name { get; set; }
        
        public virtual ICollection<ExerciseHistoryForPatch> ExerciseHistories { get; set; }
        
    }
}
