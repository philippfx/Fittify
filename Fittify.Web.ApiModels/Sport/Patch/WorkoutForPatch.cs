using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Patch
{
    public class WorkoutForPatch : UniqueIdentifier<int>
    {
        public WorkoutForPatch()
        {
            
        }
        
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryForPatch Category { get; set; }
        public int CategoryId { get; set; }
        
        public virtual ICollection<WorkoutHistoryForPatch> WorkoutHistories { get; set; }
    }
}
