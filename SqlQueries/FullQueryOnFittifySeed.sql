SELECT Workouts.Name, 
Exercises.Name, ExerciseHistories.Id, ExerciseHistories.PreviousExerciseHistoryId, WorkoutHistories.DateTimeStart, WorkoutHistories.DateTimeEnd,
WeightLiftingSets.WeightFull, WeightLiftingSets.RepetitionsFull
FROM ExerciseHistories
FULL OUTER JOIN WeightLiftingSets ON WeightLiftingSets.ExerciseHistoryId = ExerciseHistories.Id 
INNER JOIN Exercises ON ExerciseHistories.ExerciseId = Exercises.Id
INNER JOIN WorkoutHistories ON ExerciseHistories.WorkoutHistoryId = WorkoutHistories.Id
INNER JOIN Workouts ON WorkoutHistories.WorkoutId = Workouts.Id
ORDER BY ExerciseHistories.Id