﻿namespace Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get
{
    public class WorkoutHistoryOfmResourceParameters
    {
        public string Fields { get; set; }
        public string IncludeExerciseHistories { get; set; }
        public string IncludePreviousExerciseHistories { get; set; }
        public string IncludeWeightLiftingSets { get; set; }
        public string IncludeCardioSets { get; set; }
    }
}
