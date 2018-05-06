using System;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Common.Helpers;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.Repository;
using Fittify.DataModelRepository.ResourceParameters.Sport;
using Fittify.DataModels.Models.Sport;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Test.TestHelper.EntityFrameworkCore
{
    public class FileTestClassRepository : AsyncCrudBase<FileTestClass, int, FileTestClassResourceParameters>, IAsyncOwnerIntId
    {
        public FileTestClassRepository(FileSystemDbContext fileSystemDbyContext) : base(fileSystemDbyContext)
        {

        }
    }
}