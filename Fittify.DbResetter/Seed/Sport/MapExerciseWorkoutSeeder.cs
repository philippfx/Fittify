﻿using System;
using System.Linq;
using Fittify.DataModelRepository;
using Fittify.DataModels.Models.Sport;

namespace Fittify.DbResetter.Seed.Sport
{
    public static class MapExerciseWorkoutSeeder
    {
        public static bool Seed(FittifyContext fittifyContext)
        {
            var listMapExerciseWorkouts = fittifyContext.MapExerciseWorkout.ToList();
            if (listMapExerciseWorkouts.Count() == 0)
            {
                var listWorkouts = fittifyContext.Workouts.ToList();
                foreach (var workout in listWorkouts)
                {
                    if (workout.Name == "MondayChestSeed")
                    {
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "InclinedBenchPressSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "DumbBellFlySeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "NegativeBenchPressSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SitupsSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SpinningBikeSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                    }

                    if (workout.Name == "WednesdayBackSeed")
                    {
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "DeadLiftSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SeatedPullDownSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "RowSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SitupsSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SpinningBikeSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                    }

                    if (workout.Name == "FridayLegSeed")
                    {
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SquatSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "LegCurlSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "CalfRaiseSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SitupsSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SpinningBikeSeed").Id,
                            OwnerGuid = StaticFields.TestOwnerGuid
                        });
                    }
                }
            }

            if (fittifyContext.SaveChanges() >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
