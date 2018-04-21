using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CategoryViewModelRepository : GenericViewModelRepository<int, CategoryViewModel, CategoryOfmForGet, CategoryOfmForPost, CategoryResourceParameters>
    {
        private GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryResourceParameters> asyncGppdOfmCategory;
        public CategoryViewModelRepository(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(appConfiguration, httpContextAccessor, "Category")
        {
            asyncGppdOfmCategory = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryResourceParameters>(appConfiguration, httpContextAccessor, "Category");
        }
    }
}
