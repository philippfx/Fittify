using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepositories.Test.TestHelper.EntityFrameworkCore
{
    public class FileSystemDbContext : DbContext
    {
        public FileSystemDbContext(DbContextOptions<FileSystemDbContext> options) : base(options)
        {

        }

        public DbSet<FileTestClass> File { get; set; }
    }
}
