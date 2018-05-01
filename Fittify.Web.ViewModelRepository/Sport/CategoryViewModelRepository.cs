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
        private readonly GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryResourceParameters> _asyncGppdOfmCategory;
        public CategoryViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "Category")
        {
            _asyncGppdOfmCategory = new GenericAsyncGppdOfm<int, CategoryOfmForGet, CategoryOfmForPost, CategoryResourceParameters>(appConfiguration, "Category");
        }
    }
}
