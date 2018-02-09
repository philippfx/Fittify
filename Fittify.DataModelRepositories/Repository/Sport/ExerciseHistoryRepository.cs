using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class ExerciseHistoryRepository : AsyncCrud<ExerciseHistory,int>
    {
        public ExerciseHistoryRepository()
        {
            
        }

        public ExerciseHistoryRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        //public void TemporaryCreate(FittifyContext fittifyContext, int workoutHistoryId, int exerciseId)
        //{
        //    var exerciseHistory = new ExerciseHistory();
        //    exerciseHistory.ExerciseId = exerciseId;
        //    exerciseHistory.WorkoutHistoryId = workoutHistoryId;

        //    var eHs = fittifyContext.ExerciseHistories.Include(i => i.WeightLiftingSets).OrderByDescending(o => o.Id).ToList();
        //    var sameE = eHs.Where(eH => eH.ExerciseId == exerciseId).ToList();
        //    var nnWLS = sameE.Where(w => w.WeightLiftingSets != null).ToList();
        //    var countAboveOne = nnWLS.FirstOrDefault(f => f.WeightLiftingSets.Count > 0);

        //    exerciseHistory.PreviousExerciseHistoryId = fittifyContext.ExerciseHistories.Include(i => i.WeightLiftingSets).OrderByDescending(o => o.Id).FirstOrDefault(eH => eH.ExerciseId == exerciseId && eH.WeightLiftingSets != null && eH.WeightLiftingSets.Count > 0).Id;
        //    fittifyContext.Add(exerciseHistory);
        //    fittifyContext.SaveChanges();
        //}

        public async Task<List<ExerciseHistory>> GetCollectionByFkWorkoutHistoryId(int workoutHistoryId)
        {
            return await FittifyContext.Set<ExerciseHistory>()
                .Include(i => i.Exercise)
                .Include(i => i.WeightLiftingSets)
                .Include(i => i.CardioSets)
                .Where(wls => wls.WorkoutHistoryId == workoutHistoryId)
                .ToListAsync();
        }
    }
}
