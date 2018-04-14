using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Fittify.Common.Test.Helpers
{
    [TestFixture]
    public class FittifyRegularExpressions
    {
        [TestFixture]
        public class RangeOfIntIdsRegex
        {
            [Test]
            public void Should_ReturnTrue_ForValidString()
            {
                List<string> validStrings = new List<string>()
                {
                    "",
                    "null",
                    "1",
                    "1000",
                    "10001",
                    "10,2",
                    "1132527",
                    "1,1-10",
                    "1-12,5",
                    "1-12,13-5",
                    "1-3,4-6,7,10-12,13,16-17,18,20-25"
                };
                
                foreach (var str in validStrings)
                {
                    Assert.IsTrue(Regex.IsMatch(str, Common.Helpers.FittifyRegularExpressions.RangeOfIntIds));
                }
            }

            [Test]
            public void Should_ReturnFalse_ForInvalidString()
            {
                List<string> validStrings = new List<string>()
                {
                    ",",
                    "-",
                    "0",
                    "01",
                    "01-50",
                    "10-05",
                    "1,",
                    "1-3,4-6,7-9,10-12,13-15,16-17,",
                    "1,,2",
                    ",10",
                    ",10,",
                    "a,b,c"
                };

                foreach (var str in validStrings)
                {
                    Assert.IsFalse(Regex.IsMatch(str, Common.Helpers.FittifyRegularExpressions.RangeOfIntIds));
                }
            }
        }

    }
}
