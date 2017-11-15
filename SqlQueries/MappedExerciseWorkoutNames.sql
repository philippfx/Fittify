SELECT Workouts.Name, Exercises.Name
FROM MapExerciseWorkout
INNER JOIN Workouts ON MapExerciseWorkout.WorkoutId = Workouts.Id
INNER JOIN Exercises ON MapExerciseWorkout.ExerciseId = Exercises.Id