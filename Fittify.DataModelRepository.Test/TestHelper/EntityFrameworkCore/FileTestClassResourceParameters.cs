using System;
using Fittify.Common;
using Fittify.DataModelRepository.ResourceParameters;

namespace Fittify.DataModelRepository.Test.TestHelper.EntityFrameworkCore
{
    public class FileTestClassResourceParameters : EntityResourceParametersBase, IEntityOwner
    {
        public Guid? OwnerGuid { get; set; }
    }
}
