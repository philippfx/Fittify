using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fittify.Web.Common;

namespace Fittify.Web.ApiModels.Sport.Get
{
    public class CardioSetForGet : UniqueIdentifier<int>
    {
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        
        public int? ExerciseHistoryId { get; set; }
    }
}
