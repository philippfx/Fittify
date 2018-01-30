﻿using System;
using Fittify.Api.OuterFacingModels.Sport.Abstract;
using Fittify.Common;

namespace Fittify.Api.OuterFacingModels.Sport.Get
{
    public class ExerciseOfmForGet : ExerciseOfmBase, IUniqueIdentifierDataModels<int>
    {
        public int Id { get; set; }

        //public string Name { get; set; }
        public string RangeOfWorkoutIds { get; set; }
        public string RangeOfExerciseHistoryIds { get; set; }
    }
}
