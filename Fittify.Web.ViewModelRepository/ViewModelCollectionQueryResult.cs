using System.Collections.Generic;
using Fittify.Web.ApiModelRepositories;

namespace Fittify.Web.ViewModelRepository
{
    public class ViewModelCollectionQueryResult<TViewModel> : ViewModelQueryResultBase where TViewModel : class
    {
        public IEnumerable<TViewModel> ViewModelForGetCollection { get; set; }
    }
}
