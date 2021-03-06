﻿using System.Diagnostics.CodeAnalysis;
using Fittify.Common;

namespace Fittify.Api.Controllers.Generic
{
    [ApiEntity]
    [ExcludeFromCodeCoverage]
    public class Animals : IEntityName<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
