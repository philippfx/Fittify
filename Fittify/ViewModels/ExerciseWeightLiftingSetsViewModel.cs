using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Entities;

namespace Fittify.ViewModels
{
    public class ExerciseWeightLiftingSetsViewModel
    {
        // Exercise
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }

        // Settings
        public MachineAdjustableType MachineAdjustableType1 { get; set; }
        public int MachineAdjustableSetting1 { get; set; }

        public MachineAdjustableType MachineAdjustableType2 { get; set; }
        public int MachineAdjustableSetting2 { get; set; }

        // Set1
        public int WeightLiftingSet1Id { get; set; }
        public int WeightFull1 { get; set; }
        public int RepetitionsFull1 { get; set; }

        public int WeightReduced1 { get; set; }
        public int RepetitionsReduced1 { get; set; }

        public int WeightBurn1 { get; set; }

        // Set2
        public int WeightLiftingSet2Id { get; set; }
        public int WeightFull2 { get; set; }
        public int RepetitionsFull2 { get; set; }

        public int WeightReduced2 { get; set; }
        public int RepetitionsReduced2 { get; set; }

        public int WeightBurn2 { get; set; }

        // Set3
        public int WeightLiftingSet3Id { get; set; }
        public int WeightFull3 { get; set; }
        public int RepetitionsFull3 { get; set; }

        public int WeightReduced3 { get; set; }
        public int RepetitionsReduced3 { get; set; }

        public int WeightBurn3 { get; set; }

        // Set4
        public int WeightLiftingSet4Id { get; set; }
        public int WeightFull4 { get; set; }
        public int RepetitionsFull4 { get; set; }

        public int WeightReduced4 { get; set; }
        public int RepetitionsReduced4 { get; set; }

        public int WeightBurn4 { get; set; }

        // After having finished
        public int? TotalScoreOfTodaysSet { get; set; }
    }
}
