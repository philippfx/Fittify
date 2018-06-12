using System;
using System.Threading.Tasks;
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
                    "Server=.\\SQLEXPRESS;Database=FittifyTestCreation;User Id=seifert-1;Password=merlin;"))
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
