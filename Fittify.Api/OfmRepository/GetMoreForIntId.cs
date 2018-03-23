﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Common;
using Fittify.Common.Helpers;
using Fittify.DataModelRepositories;

namespace Fittify.Api.OfmRepository
{
    public class GetMoreForIntId<TCrudRepository, TEntity, TOfmForGet> : IAsyncGetMoreForOfmWithIntId<TOfmForGet>
        where TEntity : class, IEntityUniqueIdentifier<int>
        where TCrudRepository : AsyncCrud<TEntity, int>
        where TOfmForGet : class
    {
        private readonly TCrudRepository _repo;
        public GetMoreForIntId(TCrudRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<TOfmForGet>> GetByRangeOfIds(string inputStringForRangeOfIds)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputStringForRangeOfIds));
            return Mapper.Map<IEnumerable<TOfmForGet>>(entityCollection);
        }
    }
}
