﻿using System.Collections.Generic;
using Fittify.Api.OuterFacingModels;

namespace Fittify.Api.Helpers
{
    /// <summary>
    /// This class allows a better error message handling when doing erroneous GET queries. (ref and out are not allowed as parameters for async methods)
    /// </summary>
    /// <typeparam name="TOfmForGet">Is the concrete type of any OfmForGet class</typeparam>
    public class OfmForGetCollectionQueryResult<TOfmForGet> where TOfmForGet : LinkedResourceBase
    {
        public OfmForGetCollectionQueryResult()
        {
            ReturnedTOfmForGetCollection = new OfmForGetCollection<TOfmForGet>();
        }
        public OfmForGetCollection<TOfmForGet> ReturnedTOfmForGetCollection { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
