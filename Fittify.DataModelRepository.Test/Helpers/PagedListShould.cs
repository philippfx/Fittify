using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fittify.DataModelRepository.Helpers;
using Fittify.DataModelRepository.Test.TestHelper.EntityFrameworkCore;
using NUnit.Framework;

namespace Fittify.DataModelRepository.Test.Helpers
{
    [TestFixture]
    class PagedListShould
    {
        //// Can probably be deleted, because it is cross covered in other tests
        [Test]
        public async Task GetterCorrectlyWorking_ForHasPreviousField()
        {
            await Task.Run(() =>
            {
                List<FileTestClass> list = new List<FileTestClass>();
                for (int i = 1; i <= 10; i++)
                {
                    list.Add(new FileTestClass() { Id = i });
                }
                var pagedList = new PagedList<FileTestClass>(list, 100, 2, 10);

                Assert.AreEqual(true, pagedList.HasPrevious);

            });
        }

        [Test]
        public async Task GetterCorrectlyWorking_ForHasNextField()
        {
            await Task.Run(() =>
            {
                List<FileTestClass> list = new List<FileTestClass>();
                for (int i = 1; i <= 10; i++)
                {
                    list.Add(new FileTestClass() { Id = i });
                }
                var pagedList = new PagedList<FileTestClass>(list, 100, 2, 10);

                Assert.AreEqual(true, pagedList.HasNext);

            });
        }

    }
}
