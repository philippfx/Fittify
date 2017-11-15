using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.Sport
{
    public class CardioSet
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
