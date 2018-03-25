using System;
using System.Collections.Generic;
using System.Text;
using Fittify.Common;

namespace Fittify.DataModelRepositories.Helpers
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
        public bool DidEntityExist { get; set; }
        public bool IsDeleted { get; set; }
        public List<List<IEntityUniqueIdentifier<TId>>> EntitesThatBlockDeletion { get; set; }
    }
}
