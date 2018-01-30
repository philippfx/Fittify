using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ViewModels.Sport
{
    public class CardioSetViewModel : UniqueIdentifier<int>
    {
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public int? ExerciseHistoryId { get; set; }
    }
}
