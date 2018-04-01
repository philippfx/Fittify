using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class WorkoutRepository : AsyncCrud<Workout, int> //: AsyncGetCollectionForEntityDateTimeStartEnd<Workout, WorkoutOfmForGet, int> // Todo implement IAsyncCrudForDateTimeStartEnd
    {
        public WorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Workout> GetById(int id)
        {
            return FittifyContext.Workouts
                .Include(i => i.Category)
                .Include(i => i.ExercisesWorkoutsMap)
                .Include(i => i.WorkoutHistories)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public PagedList<Workout> GetCollection(WorkoutResourceParameters resourceParameters)
        {
            // Todo can be improved by calling base class and this overriding method just adds the INCLUDE statements
            var allEntitiesQueryable = GetAll()
                .Include(i => i.Category)
                .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<WorkoutOfmForGet, Workout>());

            if (!String.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.Name.Contains(resourceParameters.SearchQuery));
            }

            if (resourceParameters.CategoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.CategoryId == resourceParameters.CategoryId);
            }

            return PagedList<Workout>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}


//using System.Threading.Tasks;
//using Fittify.Api.OuterFacingModels.Sport.Get;
//using Fittify.DataModels.Models.Sport;
//using Microsoft.EntityFrameworkCore;

//namespace Fittify.DataModelRepositories.Repository.Sport
//{
//    public class WorkoutRepository : AsyncGetCollectionForEntityName<Workout, WorkoutOfmForGet, int>
//    {
//        public WorkoutRepository()
//        {

//        }

//        public WorkoutRepository(FittifyContext fittifyContext) : base(fittifyContext)
//        {

//        }

//        public override async Task<Workout> GetById(int id)
//        {
//            return await FittifyContext.Workouts
//                .Include(i => i.ExercisesWorkoutsMap)
//                .ThenInclude(i => i.Exercise)
//                .Include(i => i.WorkoutHistories)
//                .FirstOrDefaultAsync(w => w.Id == id);
//        }
//    }
//}
