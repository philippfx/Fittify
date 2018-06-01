using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fittify.Api.Helpers.Extensions;
using Fittify.Common.Extensions;
using NUnit.Framework;

namespace Fittify.Api.Test.Helpers.Extensions
{
    [TestFixture]
    class StringConvertToDifferentTypeExtensionsShould
    {
        [TestCase("1")]
        [TestCase(" 1")]
        [TestCase("1 ")]
        [TestCase("  1  ")]
        [TestCase("      1   ")]
        [TestCase(" \t 1 \r  \n  ")]
        [TestCase("true")]
        [TestCase(" true")]
        [TestCase("true ")]
        [TestCase("  true  ")]
        [TestCase("      true   ")]
        [TestCase(" \t true \r  \n  ")]
        public async Task ReturnTrue_ForASeriesOfValidInputs(string testCase)
        {
            await Task.Run(() =>
            {
                Assert.AreEqual(true, testCase.ToBool());
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" \t ")]
        [TestCase("  \n  ")]
        [TestCase("      \r  ")]
        [TestCase((string)null)]
        [TestCase(null)]
        [TestCase(" abc ")]
        public async Task ReturnFalse_ForASeriesOfFalseCausingInputs(string testCase)
        {
            await Task.Run(() =>
            {
                Assert.AreEqual(false, testCase.ToBool());
            });
        }
    }
}
