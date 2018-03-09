namespace Fittify.Common.Helpers.ResourceParameters
{
    public interface IResourceParameters
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string OrderBy { get; set; }
        string Fields { get; set; }
    }
}