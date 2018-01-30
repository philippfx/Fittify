using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models;

namespace Fittify.Api.OfmRepository
{
    public class GetMoreForIntId<TCrudRepository, TEntity, TOfmForGet> : IAsyncGetMoreForOfmWithIntId<TOfmForGet>
        where TEntity : class, IUniqueIdentifierDataModels<int>
        where TCrudRepository : AsyncCrud<TEntity, int>
        where TOfmForGet : class
    {
        private readonly TCrudRepository _repo;
        public GetMoreForIntId(TCrudRepository repo)
        {
            _repo = repo;
        }
        public async Task<ICollection<TOfmForGet>> GetByRangeOfIds(string inputStringForRangeOfIds)
        {
            var entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputStringForRangeOfIds));
            return Mapper.Map<ICollection<TOfmForGet>>(entityCollection);
        }
    }
}
