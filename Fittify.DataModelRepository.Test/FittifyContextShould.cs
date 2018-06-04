using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fittify.DataModels.Models.Sport;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fittify.DataModelRepository.Test
{
    [TestFixture]
    class FittifyContextShould
    {
        [Test]
        public async Task CreateDatabase_ForSimpleConnectenString()
        {
            await Task.Run(() =>
            {
                using (var context = new FittifyContext(
                    "Server=.\\SQLEXPRESS2016S1;Database=FittifyTestCreation;User Id=seifert-1;Password=merlin;"))
                {
                    try
                    {
                        Assert.That(() => context.Database.EnsureDeletedAsync(), Throws.Nothing);
                        Assert.That(() => context.Database.EnsureCreatedAsync(), Throws.Nothing);
                    }
                    finally
                    {
                        Assert.That(() => context.Database.EnsureDeletedAsync(), Throws.Nothing);
                    }
                }
            });
        }

        [Test]
        public async Task ThrowArgumentNullException_ForNullConnectenString()
        {
            await Task.Run(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws<ArgumentNullException>(() => new FittifyContext(dbConnectionString: null));
            });
        }
    }
}
