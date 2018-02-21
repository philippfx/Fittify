//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AutoMapper;
//using Fittify.Common;
//using Fittify.DataModelRepositories;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Infrastructure;

//namespace Fittify.Api.OfmRepository
//{
//    public class PostOfm<TId, TEntity, TCrudRepository, TOfmForGet, TOfmForPost> : IAsyncPostOfm<TOfmForGet, TOfmForPost>
//                where TEntity : class, IEntityUniqueIdentifier<TId>
//                where TId : struct
//                where TCrudRepository : IAsyncCrud<TEntity, TId>
//                where TOfmForGet : class
//                where TOfmForPost : class
//    {
//        private readonly TCrudRepository _repo;
//        private readonly IActionDescriptorCollectionProvider _adcp;
//        private readonly IUrlHelper _urlHelper;

//        public PostOfm(TCrudRepository repository,
//            IUrlHelper urlHelper,
//            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
//        {
//            _repo = repository;
//            _adcp = actionDescriptorCollectionProvider;
//            _urlHelper = urlHelper;
//        }

//        public virtual async Task<TOfmForGet> Post(TOfmForPost ofmForPost)
//        {
//            var entity = Mapper.Map<TOfmForPost, TEntity>(ofmForPost);
//            try
//            {
//                entity = await _repo.Create(entity);
//            }
//            catch (Exception e)
//            {
//                var msg = e.Message;
//            }

//            return Mapper.Map<TEntity, TOfmForGet>(entity);
//        }

//    }
//}
