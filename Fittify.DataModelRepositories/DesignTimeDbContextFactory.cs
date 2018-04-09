﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fittify.DataModelRepositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FittifyContext>
    {
        public FittifyContext CreateDbContext(string[] args)
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            var builder = new DbContextOptionsBuilder<FittifyContext>();

            //var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Fittify;User Id=seifert-1;Password=merlin;");

            return new FittifyContext(builder.Options);
        }
    }
}