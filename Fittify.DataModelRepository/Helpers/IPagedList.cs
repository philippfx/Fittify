namespace Fittify.DataModelRepository.Helpers
{
    public interface IPagedList
    {
        int CurrentPage { get; set; }
        int TotalPages { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }

        bool HasPrevious { get; }
        bool HasNext { get; }
    }
}
