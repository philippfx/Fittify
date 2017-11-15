SELECT Workouts.Name, 
DateTimeStartEnd.DateTimeStart, DateTimeStartEnd.DateTimeEnd,
Exercises.Name, ExerciseHistories.ExecutedOnDateTime,
WeightLiftingSets.WeightFull, WeightLiftingSets.RepetitionsFull
FROM ExerciseHistories
FULL OUTER JOIN WeightLiftingSets ON WeightLiftingSets.ExerciseHistoryId = ExerciseHistories.Id 
INNER JOIN Exercises ON ExerciseHistories.ExerciseId = Exercises.Id
INNER JOIN WorkoutHistories ON ExerciseHistories.WorkoutHistoryId = WorkoutHistories.Id
INNER JOIN Workouts ON WorkoutHistories.WorkoutId = Workouts.Id
FULL OUTER JOIN CardioSets ON CardioSets.ExerciseHistoryId = ExerciseHistories.Id
INNER JOIN DateTimeStartEnd ON WorkoutHistories.DateTimeSetId = DateTimeStartEnd.Id
ORDER BY ExerciseHistories.Id