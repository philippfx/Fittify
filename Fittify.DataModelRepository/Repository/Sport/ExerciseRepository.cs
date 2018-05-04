﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Common.Helpers;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Repository.Sport
{
    public class ExerciseRepository : AsyncCrud<Exercise, ExerciseOfmForGet, int, ExerciseResourceParameters>, IAsyncOwnerIntId
    {
        public ExerciseRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {

        }

        public override Task<Exercise> GetById(int id)
        {
            return FittifyContext.Exercises
                .FirstOrDefaultAsync(wH => wH.Id == id);
        }

        public override PagedList<Exercise> GetCollection(ExerciseResourceParameters resourceParameters)
        {
            var allEntitiesQueryable =
                FittifyContext.Set<Exercise>()
                    .Where(o => o.OwnerGuid == resourceParameters.OwnerGuid || o.OwnerGuid == null) // Exercises may be public
                    .AsNoTracking()
                    .ApplySort(resourceParameters.OrderBy,
                    PropertyMappingService.GetPropertyMapping<ExerciseOfmForGet, Exercise>());
            
            if (!String.IsNullOrWhiteSpace(resourceParameters.Ids))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w =>
                    RangeString.ToCollectionOfId(resourceParameters.Ids).Contains(w.Id));
            }
            if (!String.IsNullOrWhiteSpace(resourceParameters.SearchQuery))
            {
                allEntitiesQueryable = allEntitiesQueryable.Where(w => w.Name.Contains(resourceParameters.SearchQuery));
            }

            return PagedList<Exercise>.Create(allEntitiesQueryable,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}