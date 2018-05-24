using Fittify.Api.OfmRepository.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Api.Controllers
{
    public interface IControllerGuardClauses<TOfmForGet, TOfmForPost, TId> where TOfmForGet : class where TOfmForPost : class where TId : struct
    {
        bool ValidateGetCollection(
            OfmForGetCollectionQueryResult<TOfmForGet> ofmForGetCollectionQueryResult,
            out ObjectResult objectResult);

        bool ValidateGetById(
            OfmForGetQueryResult<TOfmForGet> ofmForGetQueryResult,
            int id,
            out ObjectResult objectResult);

        bool ValidatePost(
            TOfmForPost ofmForPost,
            out ObjectResult objectResult);

        bool ValidateDelete(
            OfmDeletionQueryResult<TId> ofmDeletionQueryResult, 
            TId id,
            out ObjectResult objectResult);
    }
}