﻿using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Repository.Sport
{
    public class CardioSetRepository : AsyncCrud<CardioSet, int> //: AsyncGetCollectionForEntityDateTimeStartEnd<CardioSet, CardioSetOfmForGet, int> // Todo implement IAsyncCrudForDateTimeStartEnd
    {
        public CardioSetRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }

        public override Task<CardioSet> GetById(int id)
        {
            return FittifyContext.CardioSets
                .Include(i => i.ExerciseHistory)
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public PagedList<CardioSet> GetCollection(CardioSetResourceParameters resourceParameters)
        {
            var allEntitiesQueryable = GetAll()
                .Include(i => i.ExerciseHistory)
                .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<CardioSetOfmForGet, CardioSet>());

            if (resourceParameters.FromDateTimeStart != null && resourceParameters.UntilDateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.FromDateTimeStart && a.DateTimeEnd <= resourceParameters.UntilDateTimeEnd);
            }
            else if (resourceParameters.FromDateTimeStart != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeStart >= resourceParameters.FromDateTimeStart);
            }
            else if (resourceParameters.UntilDateTimeEnd != null)
            {
                allEntitiesQueryable = allEntitiesQueryable
                    .Where(a => a.DateTimeEnd <= resourceParameters.UntilDateTimeEnd);
            }

            if (resourceParameters.ExerciseHistoryId != null)
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.ExerciseHistoryId == resourceParameters.ExerciseHistoryId);
            }

            return PagedList<CardioSet>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
