using System.Linq;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models.Sport;

namespace Fittify.Test.Core.Seed.Sport
{
    public static class MapExerciseWorkoutSeeder
    {
        public static void Seed(FittifyContext fittifyContext)
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
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "InclinedBenchPressSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "DumbBellFlySeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "NegativeBenchPressSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SitupsSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SpinningBikeSeed").Id
                        });
                    }

                    if (workout.Name == "WednesdayBackSeed")
                    {
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "DeadLiftSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SeatedPullDownSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "RowSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SitupsSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SpinningBikeSeed").Id
                        });
                    }

                    if (workout.Name == "FridayLegSeed")
                    {
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SquatSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "LegCurlSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "CalfRaiseSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SitupsSeed").Id
                        });
                        fittifyContext.Add(new MapExerciseWorkout()
                        {
                            WorkoutId = workout.Id,
                            ExerciseId = fittifyContext.Exercises.FirstOrDefault(f => f.Name == "SpinningBikeSeed").Id
                        });
                    }
                }

                fittifyContext.SaveChanges();
            }
        }
    }
}
