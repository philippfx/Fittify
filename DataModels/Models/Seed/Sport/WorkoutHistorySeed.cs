using System;
using System.Linq;
using Web.Models.Sport;

namespace Web.Models.Seed.Sport
{
    public class WorkoutHistorySeed
    {
        public static void Seed(FittifyContext fittifyContext)
        {
            DateTime SessionStart;
            DateTime SessionEnd;
            string workout;

            SessionStart = new DateTime(2017, 05, 01, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 01, 14, 32, 01);
            workout = "MondayChestSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId = fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id
                });
            }

            SessionStart = new DateTime(2017, 05, 03, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 03, 14, 32, 01);
            workout = "WednesdayBackSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id
                });
            }

            SessionStart = new DateTime(2017, 05, 05, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 05, 14, 32, 01);
            workout = "FridayLegSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId = fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id
                });
            }

            SessionStart = new DateTime(2017, 05, 08, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 08, 14, 32, 01);
            workout = "MondayChestSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == "MondayChestSeed").Id
                });
            }

            SessionStart = new DateTime(2017, 05, 10, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 10, 14, 32, 01);
            workout = "WednesdayBackSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id
                });
            }

            SessionStart = new DateTime(2017, 05, 12, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 12, 14, 32, 01);
            workout = "FridayLegSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId = fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id
                });
            }

            SessionStart = new DateTime(2017, 05, 15, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 15, 14, 32, 01);
            workout = "MondayChestSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id
                });
            }

            SessionStart = new DateTime(2017, 05, 17, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 17, 14, 32, 01);
            workout = "WednesdayBackSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id
                });
            }

            SessionStart = new DateTime(2017, 05, 19, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 19, 14, 32, 01);
            workout = "FridayLegSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStartEnd.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStartEnd = new DateTimeStartEnd()
                    {
                        DateTimeStart = SessionStart,
                        DateTimeEnd = SessionEnd,
                    },
                    WorkoutId = fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id
                });
            }

            fittifyContext.SaveChanges();
        }
    }
}
