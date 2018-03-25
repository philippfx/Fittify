using System.Collections.Generic;

namespace Fittify.Api.Helpers
{
    /// <summary>
    /// This class allows a better error message handling when doing erroneous DELETE queries. (ref and out are not allowed as parameters for async methods)
    /// </summary>
    public class OfmDeletionQueryResult<TId> where TId : struct
    {
        public OfmDeletionQueryResult()
        {
            ErrorMessages = new List<string>();
        }

        public bool IsDeleted { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
