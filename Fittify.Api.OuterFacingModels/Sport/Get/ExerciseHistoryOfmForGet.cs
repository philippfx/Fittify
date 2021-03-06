﻿using System.Collections.Generic;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class ExerciseHistoryOfmForGet : ExerciseHistoryOfmBase, IEntityUniqueIdentifier<int>, IOfmForGet
    {
        public int Id { get; set; }
        public ExerciseHistoryOfmForGet PreviousExerciseHistory { get; set; }

        public class ExerciseOfm : ExerciseOfmBase, IEntityUniqueIdentifier<int>
        {
            public int Id { get; set; }
        }
        public ExerciseOfm Exercise { get; set; }

        public string RangeOfWeightLiftingSetIds { get; set; }
        public IEnumerable<WeightLiftingSetOfmForGet> WeightLiftingSets { get; set; }
        public string RangeOfCardioSetIds { get; set; }
        public IEnumerable<CardioSetOfmForGet> CardioSets { get; set; }
        public int WorkoutHistoryId { get; set; }
    }
}
