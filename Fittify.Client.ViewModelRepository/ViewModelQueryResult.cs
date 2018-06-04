namespace Fittify.Client.ViewModelRepository
{
    public class ViewModelQueryResult<TViewModel> : ViewModelQueryResultBase where TViewModel : class
    {
        public TViewModel ViewModel { get; set; }
    }
}
