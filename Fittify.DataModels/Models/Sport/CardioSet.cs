using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class CardioSet : IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }
        [ForeignKey("DateTimeSetId")]
        public virtual DateTimeStartEnd DateTimeStartEnd { get; set; }
        public int DateTimeSetId { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public virtual ExerciseHistory ExerciseHistory { get; set; }
        public int? ExerciseHistoryId { get; set; }
    }
}
