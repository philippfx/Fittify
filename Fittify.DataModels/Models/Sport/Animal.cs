using System;
using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModels.Models.Sport
{
    /// <summary>
    /// This is a class to test Generic AyncCrudBase
    /// </summary>
    public class Animal : IEntityName<int>, IEntityOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? OwnerGuid { get; set; }
    }
}
