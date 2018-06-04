using System.Threading.Tasks;
using Fittify.Api.Helpers.Extensions;
using NUnit.Framework;

namespace Fittify.Api.Test.Helpers.Extensions
{
    [TestFixture]
    class StringConvertControllerNameExtensionsShould
    {
        [TestCase(null)]
        [TestCase("Myapicontroller")]
        [TestCase("MyApicontroller")]
        [TestCase("MyapiController")]
        [TestCase("completelyDifferentString")]
        public async Task ReturnUnmodifiedInputString_WhenInputStringDoesNotMatchTargetString_WhenUsing_ToShortCamelCasedControllerName(string testCase)
        {
            await Task.Run(() =>
            {
                Assert.AreEqual(testCase, testCase.ToShortCamelCasedControllerName());
            });
        }

        [TestCase(null)]
        [TestCase("Myapicontroller")]
        [TestCase("MyApicontroller")]
        [TestCase("MyapiController")]
        [TestCase("completelyDifferentString")]
        public async Task ReturnUnmodifiedInputString_WhenInputStringDoesNotMatchTargetString_WhenUsing_ToShortPascalCasedControllerName(string testCase)
        {
            await Task.Run(() =>
            {
                Assert.AreEqual(testCase, testCase.ToShortPascalCasedControllerName());
            });
        }

        [TestCase(null)]
        [TestCase("Myofmforget")]
        [TestCase("MyOfmforget")]
        [TestCase("MyofmForget")]
        [TestCase("MyofmforGet")]
        [TestCase("MyOfmForget")]
        [TestCase("MyofmForGet")]
        [TestCase("completelyDifferentString")]
        public async Task ReturnUnmodifiedInputString_WhenInputStringDoesNotMatchTargetString_WhenUsing_ToShortPascalCasedOfmForGetName(string testCase)
        {
            await Task.Run(() =>
            {
                Assert.AreEqual(testCase, testCase.ToShortPascalCasedOfmForGetName());
            });
        }
    }
}
