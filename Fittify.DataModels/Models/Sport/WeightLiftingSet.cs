using System;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    public class WeightLiftingSet : IEntityUniqueIdentifier<int>, IEntityOwner
    {
        public int Id { get; set; }

        public int? WeightFull { get; set; }
        public int? RepetitionsFull { get; set; }

        public int? WeightReduced { get; set; }
        public int? RepetitionsReduced { get; set; }

        public int? WeightBurn { get; set; }
        
        //[ForeignKey("ExerciseHistoryId")]
        public virtual ExerciseHistory ExerciseHistory { get; set; }
        public int? ExerciseHistoryId { get; set; }

        public Guid? OwnerGuid { get; set; }
        
    }
}
