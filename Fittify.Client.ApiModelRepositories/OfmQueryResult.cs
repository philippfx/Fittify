﻿namespace Fittify.Client.ApiModelRepository
{
    public class OfmQueryResult<TOfmForGet> : OfmQueryResultBase where TOfmForGet : class
    {
        public TOfmForGet OfmForGet { get; set; }
    }
}
