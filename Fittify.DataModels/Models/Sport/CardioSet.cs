using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.DataModels.Models.Sport
{
    public class CardioSet : UniqueIdentifier
    {
        public CardioSet()
        {
            
        }
        
        [ForeignKey("DateTimeSetId")]
        public virtual DateTimeStartEnd DateTimeStartEnd { get; set; }
        public int DateTimeSetId { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public virtual ExerciseHistory ExerciseHistory { get; set; }
        public int? ExerciseHistoryId { get; set; }
    }
}
