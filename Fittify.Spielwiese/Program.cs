using Fittify.DataModelRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fittify.Spielwiese
{
    class Program
    {
        static void Main(string[] args)
        {
            var _ownerGuid = new Guid("00000000-0000-0000-0000-000000000000");

            var listWorkouts = new List<Workout>()
                {
                    new Workout() {Name = "WorkoutA", OwnerGuid = _ownerGuid},
                    new Workout() {Name = "WorkoutB", OwnerGuid = _ownerGuid},
                    new Workout() {Name = "WorkoutC", OwnerGuid = null},
                    new Workout() {Name = "WorkoutD", OwnerGuid = null}
                };

            var listExercises = new List<Exercise>()
                {
                    new Exercise() {Name = "ExerciseA", OwnerGuid = _ownerGuid},
                    new Exercise() {Name = "ExerciseB", OwnerGuid = _ownerGuid},
                    new Exercise() {Name = "ExerciseC", OwnerGuid = null},
                    new Exercise() {Name = "ExerciseD", OwnerGuid = null},
                };

            var listMapExerciseWorkouts = new List<MapExerciseWorkout>();

            foreach (var workout in listWorkouts)
            {
                foreach (var exercise in listExercises)
                {
                    if (workout.OwnerGuid != null
                        && exercise.OwnerGuid != null
                        && workout.OwnerGuid == exercise.OwnerGuid)
                    {
                        listMapExerciseWorkouts.Add(new MapExerciseWorkout()
                        {
                            OwnerGuid = _ownerGuid,
                            Workout = workout,
                            Exercise = exercise
                        });
                    }
                    else if (workout.OwnerGuid == null
                        && exercise.OwnerGuid == null
                        && workout.OwnerGuid == exercise.OwnerGuid)
                    {
                        listMapExerciseWorkouts.Add(new MapExerciseWorkout()
                        {
                            OwnerGuid = null,
                            Workout = workout,
                            Exercise = exercise
                        });
                    }
                    // we don't want Guid && null or null && Guid
                }
            }


            // CreateAsync the schema in the database
            using (var context = new FittifyContext(
                "Server=.\\SQLEXPRESS2016S1;Database=MyTest;User Id=seifert-1;Password=merlin;"))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.AddRange(listMapExerciseWorkouts);
                context.SaveChanges();
            }
            
            MapExerciseWorkout queryResult;
            using (var context = new FittifyContext(
                "Server=.\\SQLEXPRESS2016S1;Database=MyTest;User Id=seifert-1;Password=merlin;"))
            {
                // Todo: Investigage... all children of children are loaded
                queryResult = context
                    .MapExerciseWorkout.AsNoTracking()
                    .Include(i => i.Exercise).AsNoTracking()
                    .Include(i => i.Workout).AsNoTracking()
                    .FirstOrDefault();
            }
            
            var result = JsonConvert.SerializeObject(queryResult,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });
            
        }
    }
}
