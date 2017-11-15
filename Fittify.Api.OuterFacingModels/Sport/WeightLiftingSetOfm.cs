using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Api.OuterFacingModels.Sport
{
    public class WeightLiftingSetOfm : UniqueIdentifier
    {
        public WeightLiftingSetOfm()
        {
            
        }
        
        public int? WeightFull { get; set; }
        public int? RepetitionsFull { get; set; }

        public int? WeightReduced { get; set; }
        public int? RepetitionsReduced { get; set; }

        public int? WeightBurn { get; set; }
        
        public int TotalScore { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public virtual ExerciseHistoryOfm ExerciseHistory { get; set; }
        public int ExerciseHistoryId { get; set; }
        
    }

    public enum MachineAdjustableType
    {
        SeatPosition,
        BenchAngle,
        BackPolsterPosition,
        StartingPositionAngle // For Rotary Torso
    }
}
