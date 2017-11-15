using System.Collections.Generic;

namespace Fittify.Api.OuterFacingModels.Sport
{
    public class ExerciseOfm : UniqueIdentifier
    {
        public ExerciseOfm()
        {
            
        }

        public string Name { get; set; }

        public MachineAdjustableType? MachineAdjustableType1 { get; set; }
        public int? MachineAdjustableSetting1 { get; set; }

        public MachineAdjustableType? MachineAdjustableType2 { get; set; }
        public int? MachineAdjustableSetting2 { get; set; }

        public virtual ICollection<MapExerciseWorkout> ExercisesWorkoutsMap { get; set; }
        public virtual ICollection<ExerciseHistoryOfm> ExerciseHistories { get; set; }
        
    }
}
