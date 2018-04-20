using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CategoryViewModelRepository : GenericViewModelRepository<int, CategoryViewModel, CategoryOfmForGet, CategoryOfmForPost, CategoryResourceParameters>
    {
        private GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryResourceParameters> asyncGppdOfmCategory;
        public CategoryViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "Category")
        {
            asyncGppdOfmCategory = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryResourceParameters>(appConfiguration, "Category");
        }
    }
}
