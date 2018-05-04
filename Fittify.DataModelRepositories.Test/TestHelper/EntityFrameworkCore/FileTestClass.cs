using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.DataModelRepositories.Test.TestHelper.EntityFrameworkCore
{
    /// <summary>
    /// A test class with a few fields to create a test Entity Framework Core DbContext
    /// </summary>
    public class FileTestClass
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public double FileSizeInKb { get; set; }
        public DateTime FileCreatedOnDate { get; set; }
    }
}
