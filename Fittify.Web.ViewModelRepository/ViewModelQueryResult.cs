namespace Fittify.Web.ViewModelRepository
{
    public class ViewModelQueryResult<TViewModel> : ViewModelQueryResultBase where TViewModel : class
    {
        public TViewModel ViewModel { get; set; }
    }
}
