namespace Fittify.Web.ApiModelRepositories
{
    public class OfmQueryResult<TOfmForGet> : OfmQueryResultBase where TOfmForGet : class
    {
        public TOfmForGet OfmForGet { get; set; }
    }
}
