using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quantus.IDP.DataModels.Models.Default;

namespace Quantus.IDP.DataModelRepository
{
    public class QuantusUserContext : IdentityDbContext<QuantusUser, QuantusRole, Guid, QuantusUserClaim, QuantusUserRole, QuantusUserLogin, QuantusRoleClaim, QuantusUserToken>
    {
        public QuantusUserContext(DbContextOptions<QuantusUserContext> options)
           : base(options)
        {
           
        }

        private readonly string _dbConnectionString;
        /// <summary>
        /// Initialize context with dbConnectionString
        /// </summary>
        /// <param name="dbConnectionString"></param>
        public QuantusUserContext(string dbConnectionString)
        {
            if (!String.IsNullOrWhiteSpace(dbConnectionString))
            {
                _dbConnectionString = dbConnectionString;
            }
            else
            {
                throw new ArgumentNullException("dbConnectionString");
            }
            ////OnConfiguring(new DbContextOptionsBuilder()); // Probably obsolete, test Quantus db resetter
        }

        //public DbSet<QuantusUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_dbConnectionString != null)
            {
                //optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Fittify;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionsBuilder.UseSqlServer(_dbConnectionString);
            }
        }
    }
}
