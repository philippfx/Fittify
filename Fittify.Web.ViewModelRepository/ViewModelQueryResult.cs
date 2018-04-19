using Fittify.Web.ApiModelRepositories;

namespace Fittify.Web.ViewModelRepository
{
    public class ViewModelQueryResult<TViewModel> : ApiQueryResultBase where TViewModel : class
    {
        public TViewModel ViewModel { get; set; }
    }
}
