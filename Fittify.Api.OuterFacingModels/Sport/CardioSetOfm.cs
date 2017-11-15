using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport
{
    public class CardioSetOfm : UniqueIdentifier
    {
        public CardioSetOfm()
        {
            
        }
        
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public virtual ExerciseHistoryOfm ExerciseHistory { get; set; }
        public int? ExerciseHistoryId { get; set; }
    }
}
