﻿using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CategoryViewModelRepository : GenericViewModelRepository<int, CategoryViewModel, CategoryOfmForGet, CategoryOfmForPost, CategoryOfmCollectionResourceParameters>
    {
        public CategoryViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "Category")
        {
        }
    }
}
