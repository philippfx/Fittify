namespace Fittify.Api.OuterFacingModels.ResourceParameters
{
    public interface ISearchQueryResourceParameters : IResourceParameters
    {
        string SearchQuery { get; set; }
    }
}
