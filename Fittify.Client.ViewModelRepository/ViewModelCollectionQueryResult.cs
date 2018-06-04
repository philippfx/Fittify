using System.Collections.Generic;

namespace Fittify.Client.ViewModelRepository
{
    public class ViewModelCollectionQueryResult<TViewModel> : ViewModelQueryResultBase where TViewModel : class
    {
        public IEnumerable<TViewModel> ViewModelForGetCollection { get; set; }
    }
}
