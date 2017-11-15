using System.ComponentModel.DataAnnotations.Schema;

namespace Fittify.DataModels.Models.Sport
{
    public class WeightLiftingSet : UniqueIdentifier
    {
        public WeightLiftingSet()
        {
            
        }
        
        //public WeightLiftingSet(FittifyContext fittifyContext, int exerciseHistoryId)
        //{
        //    this.ExerciseHistoryId = exerciseHistoryId;
        //    fittifyContext.WeightLiftingSets.Add(this);
        //    fittifyContext.SaveChanges();
        //}

        public int? WeightFull { get; set; }
        public int? RepetitionsFull { get; set; }

        public int? WeightReduced { get; set; }
        public int? RepetitionsReduced { get; set; }

        public int? WeightBurn { get; set; }
        
        public int TotalScore { get; set; }

        [ForeignKey("ExerciseHistoryId")]
        public virtual ExerciseHistory ExerciseHistory { get; set; }
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
