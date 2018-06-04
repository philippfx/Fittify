using Microsoft.EntityFrameworkCore;

namespace Fittify.DataModelRepository.Test.TestHelper.EntityFrameworkCore
{

    public class FileSystemDbContext : DbContext
    {
        public FileSystemDbContext(DbContextOptions<FileSystemDbContext> optionsBuilder) : base(optionsBuilder)
        {
           
        }

        public DbSet<FileTestClass> Files { get; set; }
        ////public DbSet<FileTestClassDoubleKey> FilesWithClassDoubleKeys { get; set; }
    }
}
