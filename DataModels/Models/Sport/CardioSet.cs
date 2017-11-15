using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Models.Sport
{
    public class CardioSet : PrimaryKey<CardioSet>
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
