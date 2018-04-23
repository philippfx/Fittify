using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.Helpers;
using Fittify.Api.Helpers.Extensions;
using Fittify.Common;
using Fittify.DataModelRepositories.Owned;
using Fittify.DataModelRepositories.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Fittify.Api.OfmRepository.Owned
{
    public class AsyncGetOfmById<TCrudRepository, TEntity, TOfmForGet, TId> : IAsyncGetOfmById<TOfmForGet, TId>
        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
        where TId : struct
        where TEntity : class, IEntityUniqueIdentifier<TId>
        where TCrudRepository : class, IAsyncCrud<TEntity, TId>
    {
        protected readonly TCrudRepository Repo;
        protected readonly IActionDescriptorCollectionProvider Adcp;
        protected readonly IUrlHelper UrlHelper;
        protected readonly IPropertyMappingService PropertyMappingService;
        protected readonly ITypeHelperService TypeHelperService;
        protected readonly HateoasLinkFactory<TId> HateoasLinkFactory;
        protected readonly string ShortPascalCasedControllerName;
        protected readonly AsyncGetOfmGuardClauses<TOfmForGet, TId> AsyncGetOfmGuardClause;
        protected readonly Controller Controller;

        public AsyncGetOfmById(TCrudRepository repository,
            IUrlHelper urlHelper,
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            Controller controller)
        {
            Repo = repository;
            Adcp = actionDescriptorCollectionProvider;
            UrlHelper = urlHelper;
            PropertyMappingService = propertyMappingService;
            TypeHelperService = typeHelperService;
            HateoasLinkFactory = new HateoasLinkFactory<TId>(urlHelper, controller.GetType().Name);
            ShortPascalCasedControllerName = controller.GetType().Name.ToShortPascalCasedControllerNameOrDefault();
            AsyncGetOfmGuardClause = new AsyncGetOfmGuardClauses<TOfmForGet, TId>(TypeHelperService);
            Controller = controller;
        }
        
        public virtual async Task<OfmForGetQueryResult<TOfmForGet>> GetById(TId id, string fields)
        {
            var ofmForGetResult = new OfmForGetQueryResult<TOfmForGet>();
            ofmForGetResult = await AsyncGetOfmGuardClause.ValidateGetById(ofmForGetResult, fields);

            if (ofmForGetResult.ErrorMessages.Count > 0)
            {
                return ofmForGetResult;
            }

            var entity = await Repo.GetById(id);
            ofmForGetResult.ReturnedTOfmForGet = Mapper.Map<TEntity, TOfmForGet>(entity);
            return ofmForGetResult;
        }
    }
}
