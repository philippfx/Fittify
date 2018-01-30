using System;
using System.Linq;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Test.Core.Seed.Sport
{
    public class ExerciseHistorySeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            DateTime executedOnDateTime;
            int? previousExerciseId;
            string previousExerciseName;

            previousExerciseId = null;
            executedOnDateTime = new DateTime(2017, 05, 01, 13, 04, 12);
            if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
            {
                //var WorkoutHistory = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                //    w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd);
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == "InclinedBenchPressSeed").Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == "DumbBellFlySeed").Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == "NegativeBenchPressSeed").Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == "SitupsSeed").Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == "SpinningBikeSeed").Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });
            }

            executedOnDateTime = new DateTime(2017, 05, 03, 13, 04, 12);
            if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
            {
                previousExerciseName = "DeadLiftSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SeatedPullDownSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "RowSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SitupsSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SpinningBikeSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });
            }

            executedOnDateTime = new DateTime(2017, 05, 05, 13, 04, 12);
            if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
            {
                previousExerciseName = "SquatSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "LegCurlSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "CalfRaiseSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SitupsSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SpinningBikeSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });
            }

            fittifyContext.SaveChanges();

            executedOnDateTime = new DateTime(2017, 05, 08, 13, 04, 12);
            if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
            {
                previousExerciseName = "InclinedBenchPressSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "DumbBellFlySeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "NegativeBenchPressSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SitupsSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SpinningBikeSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });
            }

            executedOnDateTime = new DateTime(2017, 05, 10, 13, 04, 12);
            if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
            {
                previousExerciseName = "DeadLiftSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SeatedPullDownSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "RowSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SitupsSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SpinningBikeSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });
            }

            executedOnDateTime = new DateTime(2017, 05, 12, 13, 04, 12);
            if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
            {
                previousExerciseName = "SquatSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "LegCurlSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "CalfRaiseSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SitupsSeed";
                previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                previousExerciseName = "SpinningBikeSeed";
                fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                {
                    ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                    ExecutedOnDateTime = executedOnDateTime,
                    WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                        w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                    PreviousExerciseHistoryId = previousExerciseId
                });

                fittifyContext.SaveChanges();

                executedOnDateTime = new DateTime(2017, 05, 15, 13, 04, 12);
                if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
                {
                    previousExerciseName = "InclinedBenchPressSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "DumbBellFlySeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "NegativeBenchPressSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "SitupsSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "SpinningBikeSeed";
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });
                }

                executedOnDateTime = new DateTime(2017, 05, 17, 13, 04, 12);
                if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
                {
                    previousExerciseName = "DeadLiftSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "SeatedPullDownSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "RowSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "SitupsSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "SpinningBikeSeed";
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });
                }

                executedOnDateTime = new DateTime(2017, 05, 19, 13, 04, 12);
                if (fittifyContext.ExerciseHistories.FirstOrDefault(f => f.ExecutedOnDateTime == executedOnDateTime) == null)
                {
                    previousExerciseName = "SquatSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "LegCurlSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "CalfRaiseSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "SitupsSeed";
                    previousExerciseId = fittifyContext.ExerciseHistories.ToArray().Reverse().FirstOrDefault(f => f.Exercise.Name == previousExerciseName).Id;
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });

                    previousExerciseName = "SpinningBikeSeed";
                    fittifyContext.ExerciseHistories.Add(new ExerciseHistory()
                    {
                        ExerciseId = fittifyContext.Exercises.FirstOrDefault(e => e.Name == previousExerciseName).Id,
                        ExecutedOnDateTime = executedOnDateTime,
                        WorkoutHistoryId = fittifyContext.WorkoutHistories.FirstOrDefault(w =>
                            w.DateTimeStartEnd.DateTimeStart < executedOnDateTime && executedOnDateTime < w.DateTimeStartEnd.DateTimeEnd).Id,
                        PreviousExerciseHistoryId = previousExerciseId
                    });
                }

                fittifyContext.SaveChanges();
            }
        }
    }
}
