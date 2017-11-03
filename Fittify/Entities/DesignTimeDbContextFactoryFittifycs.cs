using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fittify.Entities
{
    public class DesignTimeDbContextFactoryFittifycs : IDesignTimeDbContextFactory<FittifyContext>
    {
        public FittifyContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<FittifyContext>();

            var connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new FittifyContext(builder.Options);
        }
    }
}
