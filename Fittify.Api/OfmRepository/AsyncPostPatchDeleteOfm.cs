//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AutoMapper;
//using Fittify.Api.Extensions;
//using Fittify.Api.Helpers;
//using Fittify.Common;
//using Fittify.Common.Extensions;
//using Fittify.Common.Helpers;
//using Fittify.DataModelRepositories;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Infrastructure;

//namespace Fittify.Api.OfmRepository
//{
//    public class AsyncPostPatchDeleteOfm<TCrudRepository, TEntity, TOfmForGet, TOfmForPost, TOfmForPatch, TId> :
//        Controller
//        //IAsyncPostOfm<TOfmForGet, TOfmForPost>
//        //IAsyncPatchOfm<TOfmForGet, TOfmForPatch>
//        //IAsyncDeleteOfm<TId>

//        where TCrudRepository : IAsyncCrud<TEntity,TId> 
//        where TEntity : class, IEntityUniqueIdentifier<TId>
//        where TOfmForGet : class, IEntityUniqueIdentifier<TId>
//        where TOfmForPost : class
//        where TOfmForPatch : class, new()
//        where TId : struct
//    {
//        private TEntity _cachedEntity;
//        private readonly TCrudRepository _repo;
//        private readonly IActionDescriptorCollectionProvider _adcp;
//        private readonly IUrlHelper _urlHelper;
//        protected readonly HateoasLinkFactory<TOfmForGet, TId> HateoasLinkFactory;

//        public AsyncPostPatchDeleteOfm(TCrudRepository repository,
//            IUrlHelper urlHelper,
//            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
//            Controller controller)
//        {
//            _repo = repository;
//            _adcp = actionDescriptorCollectionProvider;
//            _urlHelper = urlHelper;
//            HateoasLinkFactory = new HateoasLinkFactory<TOfmForGet, TId>(urlHelper, controller.GetType().Name);
//        }

//        public AsyncPostPatchDeleteOfm()
//        {
            
//        }

        

//        //public virtual async Task<bool> Delete(TId id)
//        //{
//        //    var successfullyDeleted = _repo.Delete(id);
//        //    return successfullyDeleted.Result;
//        //}

        

//        //private async Task<StatusCodeResult> SaveChanges()
//        //{
//        //    // Todo decide if SavingToContext should take place here or in data layer
//        //    if (!await _repo.SaveContext())
//        //    {
//        //        return StatusCode(StatusCodes.Status500InternalServerError);
//        //    }
//        //    return Ok();
//        //}

        
//    }
//}
