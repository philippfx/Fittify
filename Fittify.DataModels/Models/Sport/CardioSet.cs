using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Common;
using Fittify.Common.IForeignKeys;

namespace Fittify.DataModels.Models.Sport
{
    public class CardioSet : IEntityDateTimeStartEnd<int>, ICardioSetForeignKeys
    {
        public int Id { get; set; }

        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }

        //[ForeignKey("ExerciseHistoryId")]
        public virtual ExerciseHistory ExerciseHistory { get; set; }
        public int? ExerciseHistoryId { get; set; }
    }
}
