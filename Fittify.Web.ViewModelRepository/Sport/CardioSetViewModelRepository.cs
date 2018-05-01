using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Common.Helpers.ResourceParameters.Sport;
using Fittify.Web.ApiModelRepositories;
using Fittify.Web.ViewModels.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Web.ViewModelRepository.Sport
{
    public class CardioSetViewModelRepository : GenericViewModelRepository<int, CardioSetViewModel, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters>
    {
        private readonly GenericAsyncGppdOfm<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters> _asyncGppdOfmCardioSet;
        public CardioSetViewModelRepository(IConfiguration appConfiguration)
            : base(appConfiguration, "CardioSet")
        {
            _asyncGppdOfmCardioSet = new GenericAsyncGppdOfm<int, CardioSetOfmForGet, CardioSetOfmForPost, CardioSetResourceParameters>(appConfiguration, "CardioSet");
        }
    }
}
