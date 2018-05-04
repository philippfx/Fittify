using System;
using System.Collections.Generic;
using System.Text;

namespace Fittify.DataModelRepositories.Test.TestHelper.EntityFrameworkCore
{
    public class FileTestDto
    {
        public int Id { get; set; }
        public string FullFileName { get; set; }
        public double FileSizeInKb { get; set; }
        public int Age { get; set; }
    }
}
