namespace Fittify.Web.ApiModelRepositories
{
    public class OfmQueryResult<TOfmForGet> : ApiQueryResultBase where TOfmForGet : class
    {
        public TOfmForGet OfmForGet { get; set; }
    }
}
