using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.Entities
{
    public class WeightLiftingSet
    {
        public int Id { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

        public int? WeightFull { get; set; }
        public int? RepetitionsFull { get; set; }

        public int? WeightReduced { get; set; }
        public int? RepetitionsReduced { get; set; }

        public int? WeightBurn { get; set; }
        
        public MachineAdjustableType MachineAdjustableType1 { get; set; }
        public int MachineAdjustableSetting1 { get; set; }

        public MachineAdjustableType MachineAdjustableType2 { get; set; }
        public int MachineAdjustableSetting2 { get; set; }

        public int Score { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public ExerciseHistory ExerciseHistory { get; set; }
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
