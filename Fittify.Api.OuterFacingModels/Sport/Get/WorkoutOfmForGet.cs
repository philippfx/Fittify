﻿using System.Collections.Generic;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class WorkoutOfmForGet : WorkoutOfmBase, IEntityUniqueIdentifier<int>, IOfmForGet
    {
        public int Id { get; set; }
        public string RangeOfExerciseIds { get; set; }
        public IEnumerable<MapExerciseWorkoutOfmForGet> MapsExerciseWorkout { get; set; }
        public string RangeOfWorkoutHistoryIds { get; set; }
    }
}
