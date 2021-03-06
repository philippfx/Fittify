﻿using System.Collections.Generic;
using Fittify.Common;

namespace Fittify.DataModelRepository.Helpers
{
    /// <summary>
    /// This class allows a better error message handling when doing erroneous DELETE queries. (ref and out are not allowed as parameters for async methods)
    /// </summary>
    public class EntityDeletionResult<TId> where TId : struct
    {
        public EntityDeletionResult()
        {
            EntitesThatBlockDeletion = new List<List<IEntityUniqueIdentifier<TId>>>();
        }

        public bool DidEntityExist { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public List<List<IEntityUniqueIdentifier<TId>>> EntitesThatBlockDeletion { get; set; }
    }
}
