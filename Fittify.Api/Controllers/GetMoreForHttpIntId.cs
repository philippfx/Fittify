using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{
    public class GetMoreForHttpIntId<TCrudRepository, TEntity, TOfmForGet> : IAsyncGetMoreForHttpForIntId<TOfmForGet>
        where TCrudRepository : AsyncCrud<TEntity, int>
        where TEntity : class, IUniqueIdentifierDataModels<int>
        where TOfmForGet : class
    {
        private readonly TCrudRepository _repo;
        public GetMoreForHttpIntId(TCrudRepository repo)
        {
            _repo = repo;
        }
        public async Task<ICollection<TOfmForGet>> GetByRangeOfIds(string inputStringForRangeOfIds)
        {
            // TODO check input with regex

            ICollection<TEntity> entityCollection;
            List<TOfmForGet> ofmCollection;
            //if (int.TryParse(inputStringForRangeOfIds, out var rangeOfIds))
            //{
            //    entityCollection = await _repo.GetByCollectionOfIds(new List<int> { rangeOfIds });
            //}
            //else
            //{
                entityCollection = await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputStringForRangeOfIds));
            //}
            ofmCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(entityCollection.ToList());
            return ofmCollection;
        }
    }
}
