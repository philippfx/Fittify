using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.Api.OfmRepository.Services.OfmDataRepositoryMapping
{
    public class OfmDataRepositoryMapping<TSource, TDestination> : IOfmDataRepositoryMapping
    {
        public TSource SourceOfmRepository { get; set; }
        public TDestination DestinationRepository { get; set; }
    }
}
