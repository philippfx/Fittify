﻿using System;
using System.Linq;
using Fittify.DataModelRepository;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DbResetter.Seed.Sport
{
    public class WorkoutHistorySeed
    {
        public static bool Seed(FittifyContext fittifyContext)
        {
            DateTime SessionStart;
            DateTime SessionEnd;
            string workout;

            SessionStart = new DateTime(2017, 05, 01, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 01, 14, 32, 01);
            workout = "MondayChestSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId = fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            SessionStart = new DateTime(2017, 05, 03, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 03, 14, 32, 01);
            workout = "WednesdayBackSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            SessionStart = new DateTime(2017, 05, 05, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 05, 14, 32, 01);
            workout = "FridayLegSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId = fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            SessionStart = new DateTime(2017, 05, 08, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 08, 14, 32, 01);
            workout = "MondayChestSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == "MondayChestSeed").Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            SessionStart = new DateTime(2017, 05, 10, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 10, 14, 32, 01);
            workout = "WednesdayBackSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            SessionStart = new DateTime(2017, 05, 12, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 12, 14, 32, 01);
            workout = "FridayLegSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId = fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            SessionStart = new DateTime(2017, 05, 15, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 15, 14, 32, 01);
            workout = "MondayChestSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            SessionStart = new DateTime(2017, 05, 17, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 17, 14, 32, 01);
            workout = "WednesdayBackSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId =
                        fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }

            SessionStart = new DateTime(2017, 05, 19, 12, 01, 05);
            SessionEnd = new DateTime(2017, 05, 19, 14, 32, 01);
            workout = "FridayLegSeed";
            if (fittifyContext.WorkoutHistories.FirstOrDefault(f =>
                    f.DateTimeStart == SessionStart && f.Workout.Name == workout) == null)
            {
                fittifyContext.Add(new WorkoutHistory()
                {
                    DateTimeStart = SessionStart,
                    DateTimeEnd = SessionEnd,
                    WorkoutId = fittifyContext.Workouts.FirstOrDefault(w => w.Name == workout).Id,
                    OwnerGuid = StaticFields.TestOwnerGuid
                });
            }


            if (fittifyContext.SaveChanges() >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
