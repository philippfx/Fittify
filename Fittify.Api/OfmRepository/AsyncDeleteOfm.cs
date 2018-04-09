using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Common;
using Fittify.DataModelRepositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository
{
    public class AsyncDeleteOfm<TCrudRepository, TEntity, TId> :
        IAsyncDeleteOfm<TId>

        where TCrudRepository : IAsyncCrud<TEntity, TId>
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TId : struct
    {
        private readonly TCrudRepository _repo;
        private readonly IActionDescriptorCollectionProvider _adcp;

        public AsyncDeleteOfm(TCrudRepository repository,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _repo = repository;
            _adcp = actionDescriptorCollectionProvider;
        }

        public virtual async Task<OfmDeletionQueryResult<TId>> Delete(TId id)
        {
            var entityDeletionResult = await _repo.Delete(id);

            var ofmDeletionQueryResult = new OfmDeletionQueryResult<TId>();

            ofmDeletionQueryResult.DidEntityExist = entityDeletionResult.DidEntityExist;
            ofmDeletionQueryResult.IsDeleted = entityDeletionResult.IsDeleted;

            return ofmDeletionQueryResult;

            // OLD CODE BEFORE TAKING CARE OF EF CORE CASCADE DELETION

            //// Todo Refactor and make return type bool (out errors)
            ////var entitiesBlockingDeletion = new List<List<IEntityUniqueIdentifier<int>>>(); // Quick fix for compile error
            //var entityDeletionResult = await _repo.MyDelete(id);

            //var ofmDeletionQueryResult = new OfmDeletionQueryResult<TId>();
            //if (entityDeletionResult.IsDeleted == false && entityDeletionResult.EntitesThatBlockDeletion.Count != 0)
            //{
            //    ofmDeletionQueryResult.IsDeleted = false;

            //    Type entityType = null;
            //    Type relatedOfmType = null;

            //    foreach (var list in entityDeletionResult.EntitesThatBlockDeletion)
            //    {
            //        string route = null;
            //        var listIds = new List<TId>();
            //        foreach (var entity in list)
            //        {
            //            entityType = entity.GetType();
            //            listIds.Add(entity.Id);

            //            if (route == null)
            //            {
            //                relatedOfmType = RelatedDataOfm.RelatedClasses.FirstOrDefault(f => f.Value == entityType).Key;

            //                var routes = _adcp.ActionDescriptors.Items.ToList();
            //                var sccOfmForGetName = relatedOfmType?.Name.ToShortCamelCasedOfmForGetNameOrDefault() + "Api";
            //                var relatedController = routes.FirstOrDefault(f => f.RouteValues["Controller"].ToLower() == sccOfmForGetName.ToLower() && f.AttributeRouteInfo.Name.Contains("ById"));
            //                route = relatedController?.AttributeRouteInfo.Template;
            //            }
            //            if (route != null)
            //            {
            //                ofmDeletionQueryResult.ErrorMessages.Add("There is a related " + relatedOfmType?.Name.ToShortCamelCasedOfmForGetNameOrDefault() + " that blocks the deletion. Please try to (a) break its relation by sending a PATCH request or (b) sending a DELETE request to the route '" + route.Replace("{id}", "") + entity.Id + "' first.");
            //            }
            //            else
            //            {
            //                ofmDeletionQueryResult.ErrorMessages.Add("There is a related " + entityType?.Name.ToShortCamelCasedOfmForGetNameOrDefault() + " that blocks the deletion. Please try to (a) break its relation by removing its association or (b) delete this blocking entity with id='" + entity.Id + "' first.");
            //            }
            //        }

            //    }
            //}
            //return ofmDeletionQueryResult;
        }
    }
}
