using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Api.Controllers.HttpMethodInterfaces;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Fittify.DataModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{
    public class GetMoreForHttpIntId<TCrudRepository, TEntity> : IAsyncGetMoreForHttpForIntId
        where TCrudRepository : AsyncCrud<TEntity, int>
        where TEntity : class, IUniqueIdentifierDataModels<int>
    {
        private readonly TCrudRepository _repo;
        public GetMoreForHttpIntId(TCrudRepository repo)
        {
            _repo = repo;
        }
        public async Task<IActionResult> GetByRangeOfIds(string inputStringForRangeOfIds)
        {
            if (int.TryParse(inputStringForRangeOfIds, out var rangeOfIds))
            {
                return new JsonResult(await _repo.GetByCollectionOfIds(new List<int> { rangeOfIds }));
            }

            // TODO check input with regex
            return new JsonResult(await _repo.GetByCollectionOfIds(RangeString.ToCollectionOfId(inputStringForRangeOfIds)));
        }
    }
}
