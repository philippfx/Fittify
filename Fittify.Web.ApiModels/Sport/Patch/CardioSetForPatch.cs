using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Patch
{
    public class CardioSetForPatch : UniqueIdentifier<int>
    {
        [ForeignKey("DateTimeSetId")]
        public virtual DateTimeStartEndForPatch DateTimeStartEnd { get; set; }
        public int DateTimeSetId { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public virtual ExerciseHistoryForPatch ExerciseHistory { get; set; }
        public int? ExerciseHistoryId { get; set; }
    }
}