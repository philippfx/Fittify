using System.Collections.Generic;
using System.Threading.Tasks;
using Fittify.Common.Helpers;
using NUnit.Framework;

namespace Fittify.Common.Test.Helpers
{
    [TestFixture]
    class RangeStringShould
    {
        [TestCase("")]
        [TestCase((string)null)]
        public async Task ReturnEmptyListOfInts_ForNullOrWhiteSpaceInputString(string inputString)
        {
            await Task.Run(() =>
            {
                Assert.AreEqual(new List<int>(), RangeString.ToCollectionOfId(inputString));
            });
        }
        
        [TestCase("1,2,3,4,5")]
        public async Task ReturnCorrectListOfInts_ForValidInputStringThatContainsCommas(string inputString)
        {
            await Task.Run(() =>
            {
                Assert.AreEqual(new List<int>() { 1, 2, 3, 4, 5}, RangeString.ToCollectionOfId(inputString));
            });
        }

        [Test]
        public async Task ReturnNull_ForNullListOfInts()
        {
            await Task.Run(() =>
            {
                Assert.IsNull(RangeString.ToStringOfIds(null));
            });
        }

        [Test]
        public async Task ReturnWhiteSpaceString_ForEmptyListOfInts()
        {
            await Task.Run(() =>
            {
                Assert.AreEqual("", new List<int>().ToStringOfIds());
            });
        }

        [Test]
        public async Task ReturnValidString_ForListOfIntsThatContainsOnlyOneItem()
        {
            await Task.Run(() =>
            {
                Assert.AreEqual("42", new List<int>() { 42 }.ToStringOfIds());
            });
        }

        [Test]
        public async Task ReturnValidString_ForUnorderedListOfInts()
        {
            await Task.Run(() =>
            {
                Assert.AreEqual("2-5,10-11,13", new List<int>() { 5, 10, 3, 2, 4, 13, 11 }.ToStringOfIds());
            });
        }
    }
}
