using System.Collections.Generic;
using Fittify.Api.OuterFacingModels;
using Fittify.DataModelRepositories.Helpers;

namespace Fittify.Api.Helpers
{
    /// <summary>
    /// This class allows a better error message handling when doing erroneous GET queries. (ref and out are not allowed as parameters for async methods)
    /// </summary>
    /// <typeparam name="TOfmForGet">Is the concrete type of any OfmForGet class</typeparam>
    public class OfmForGetCollectionQueryResult<TOfmForGet> : IPagedList where TOfmForGet : class
    {
        public OfmForGetCollectionQueryResult()
        {
            ReturnedTOfmForGetCollection = new OfmForGetCollection<TOfmForGet>();
            ErrorMessages = new List<string>();
        }
        public OfmForGetCollection<TOfmForGet> ReturnedTOfmForGetCollection { get; set; }
        public List<string> ErrorMessages { get; set; }


        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
    }
}
