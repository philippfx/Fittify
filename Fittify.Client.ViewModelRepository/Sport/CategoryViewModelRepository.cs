﻿using Fittify.Api.OfmRepository.OfmResourceParameters.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Client.ApiModelRepository;
using Fittify.Client.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ViewModelRepository.Sport
{
    public class CategoryViewModelRepository : ViewModelRepositoryBase<int, CategoryViewModel, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmResourceParameters, CategoryOfmCollectionResourceParameters>
    {
        public CategoryViewModelRepository(
            //IConfiguration appConfiguration,
            //IHttpContextAccessor httpContextAccessor,
            //IHttpRequestExecuter httpRequestExecuter,
            IApiModelRepository<int, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters> categoryApiModelRepository)
            : base(
                ////appConfiguration,
                ////httpContextAccessor,
                ////"Category",
                ////httpRequestExecuter,
                categoryApiModelRepository)
        {
        }
    }
}
