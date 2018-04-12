using System;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class CardioSet : IEntityDateTimeStartEnd<int>
    {
        public int Id { get; set; }

        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        
        public virtual ExerciseHistory ExerciseHistory { get; set; }
        public int? ExerciseHistoryId { get; set; }
    }
}
