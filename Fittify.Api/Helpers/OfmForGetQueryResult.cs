using System.Collections.Generic;

namespace Fittify.Api.Helpers
{
    /// <summary>
    /// This class allows a better error message handling when doing erroneous GET queries. (ref and out are not allowed as parameters for async methods)
    /// </summary>
    /// <typeparam name="TOfmForGet">Is the concrete type of any OfmForGet class</typeparam>
    public class OfmForGetQueryResult<TOfmForGet> where TOfmForGet : class
    {
        public OfmForGetQueryResult()
        {
            ErrorMessages = new List<string>();
        }
        public TOfmForGet ReturnedTOfmForGet { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}