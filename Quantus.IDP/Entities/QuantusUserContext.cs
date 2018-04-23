using Microsoft.EntityFrameworkCore;

namespace Quantus.IDP.Entities
{
    public class QuantusUserContext : DbContext
    {
        public QuantusUserContext(DbContextOptions<QuantusUserContext> options)
           : base(options)
        {
           
        }

        public DbSet<User> Users { get; set; }
    }
}
