using System;

namespace Fittify.Web.ApiModels.Sport.Post
{
    public class CardioSetForPost
    {
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        
        public int? ExerciseHistoryId { get; set; }
    }
}
