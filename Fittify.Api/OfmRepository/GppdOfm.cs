using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Controllers.Sport;
using Fittify.Api.Extensions;
using Fittify.Api.Helpers;
using Fittify.Common;
using Fittify.Common.Extensions;
using Fittify.Common.Helpers;
using Fittify.Common.Helpers.ResourceParameters;
using Fittify.DataModelRepositories;
using Fittify.DataModelRepositories.Helpers;
using Fittify.DataModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Fittify.Api.OfmRepository
{
    public class GppdOfm<TCrudRepository, TEntity, TOfmForGet, TOfmForPost, TOfmForPatch, TId> :
        Controller,
        IAsyncGppdOfm<TId, TOfmForGet, TOfmForPost, TOfmForPatch>
        
        where TCrudRepository : AsyncCrud<TEntity,TId> 
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TOfmForGet : class
        where TOfmForPost : class
        where TOfmForPatch : class, new()
        where TId : struct
    {
        private TEntity _cachedEntity;
        private readonly TCrudRepository _repo;
        private readonly IActionDescriptorCollectionProvider _adcp;
        private readonly IUrlHelper _urlHelper;

        public GppdOfm(TCrudRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _repo = repository;
            _adcp = actionDescriptorCollectionProvider;
            _urlHelper = urlHelper;
        }

        public GppdOfm()
        {
            
        }

        public virtual async Task<bool> DoesEntityExist(TId id)
        {
            // Todo this async lacks await
            return  _repo.DoesEntityExist(id).Result;
        }

        public virtual async Task<IEnumerable<TOfmForGet>> GetAllPaged(IResourceParameters resourceParameters, ControllerBase controllerBase)
        {
            //// Todo this async lacks await
            var pagedListEntityCollection = _repo.GetAllPaged(resourceParameters);

            var previousPageLink = pagedListEntityCollection.HasPrevious ?
                RsourceUriFactory.CreateAuthorsResourceUri(resourceParameters,
                    _urlHelper,
                    ResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedListEntityCollection.HasNext ?
                RsourceUriFactory.CreateAuthorsResourceUri(resourceParameters,
                    _urlHelper,
                    ResourceUriType.NextPage) : null;

            // Todo Maybe refactor to a type safe class instead of anonymous
            var paginationMetadata = new
            {
                totalCount = pagedListEntityCollection.TotalCount,
                pageSize = pagedListEntityCollection.PageSize,
                currentPage = pagedListEntityCollection.CurrentPage,
                totalPages = pagedListEntityCollection.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            // Todo: Refactor to class taking controller as input instead of only this method
            controllerBase.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var ofmCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(pagedListEntityCollection);
            return ofmCollection;
        }

        public virtual async Task<IEnumerable<TOfmForGet>> GetAll()
        {
            // Todo this async lacks await
            var entityCollection = _repo.GetAll().ToList();
            var ofmCollection = Mapper.Map<List<TEntity>, List<TOfmForGet>>(entityCollection);
            return ofmCollection;
        }

        public virtual async Task<TOfmForGet> GetById(TId id)
        {
            var entity = await _repo.GetById(id);
            var ofm = Mapper.Map<TEntity, TOfmForGet>(entity);
            return ofm;
        }
        
        public virtual async Task<TOfmForGet> Post(TOfmForPost ofmForPost)
        {
            var entity = Mapper.Map<TOfmForPost, TEntity>(ofmForPost);
            try
            {
                entity = await _repo.Create(entity);
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            
            return Mapper.Map<TEntity, TOfmForGet>(entity);
        }
        
        public virtual async Task<bool> Delete(TId id)
        {
            var successfullyDeleted = _repo.Delete(id);
            return successfullyDeleted.Result;
        }
        
        public virtual async Task<List<string>> MyDelete(TId id)
        {
            // Todo Refactor and make return type bool (out errors)
            var entitiesBlockingDeletion = await _repo.MyDelete(id);
            var blockingEntitiesList = new List<string>();
            if (entitiesBlockingDeletion.Count != 0)
            {
                //var namespaceOfTOfmForGet = typeof(TOfmForGet).Namespace;
                //var tOfmForGetAssembly = Assembly.GetAssembly(typeof(TOfmForGet));
                //List<Type> allOfmGetTypes = tOfmForGetAssembly.GetTypes()
                //    .Where(t => String.Equals(t.Namespace, namespaceOfTOfmForGet, StringComparison.Ordinal)).ToList();

                Type entityType = null;
                Type relatedOfmType = null;

                foreach (var list in entitiesBlockingDeletion)
                {
                    string route = null;
                    List<int> intIds = new List<int>();
                    foreach (var entity in list)
                    {
                        entityType = entity.GetType();
                        intIds.Add(entity.Id);

                        if (route == null)
                        {
                            relatedOfmType = RelatedDataOfm.RelatedClasses.FirstOrDefault(f => f.Value == entityType).Key;

                            var routes = _adcp.ActionDescriptors.Items.ToList();
                            var sccOfmForGetName = relatedOfmType?.Name.ToShortCamelCasedOfmForGetNameOrDefault() + "Api";
                            var relatedController = routes.FirstOrDefault(f => f.RouteValues["Controller"].ToLower() == sccOfmForGetName.ToLower() && f.AttributeRouteInfo.Template.Contains("range/"));
                            route = relatedController?.AttributeRouteInfo.Template;
                        }
                        if (route == null) break;
                    }

                    if (route != null)
                    {
                        blockingEntitiesList.Add("There are related " + relatedOfmType?.Name.ToShortCamelCasedOfmForGetNameOrDefault().ToPlural() + " that block the deletion. Please try calling '" + route.Replace("{inputString}", "") + intIds.ToStringOfIds() + "' to delete these blocking entities first.");
                    }
                    else
                    {
                        blockingEntitiesList.Add("There are related database entities of type " + entityType?.Name.ToShortCamelCasedOfmForGetNameOrDefault().ToPlural() + " that block the deletion. Please try to delete these blocking entities with ids='" + intIds.ToStringOfIds() + "' first.");
                    }
                }
            }
            return blockingEntitiesList;
        }

        private async Task<StatusCodeResult> SaveChanges()
        {
            // Todo decide if SavingToContext should take place here or in data layer
            if (!await _repo.SaveContext())
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        public virtual async Task<TOfmForPatch> GetByIdOfmForPatch(TId id)
        {
            _cachedEntity = await _repo.GetById(id);
            var ofmForPatch = Mapper.Map<TEntity, TOfmForPatch>(_cachedEntity);
            return ofmForPatch;
        }

        public async Task<TOfmForGet> UpdatePartially(TOfmForPatch ofmForPatch)
        {
            Mapper.Map(ofmForPatch, _cachedEntity);
            var entity = await _repo.Update(_cachedEntity);
            return Mapper.Map<TEntity, TOfmForGet>(entity);
            
        }
        
    }
}
