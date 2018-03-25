using NUnit.Framework;

namespace Fittify.Api.OuterFacingModels.Test
{
    [TestFixture]
    public class AscendingOrderIntIdRangeAttribute
    {
        [TestFixture]
        public class IsValidMethod
        {
            [Test]
            public void Should_ReturnTrue_ForNullString()
            {
                object str = null;
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsTrue(result);
            }

            [Test]
            public void Should_ReturnFalse_ForStringThatContainsLetters()
            {
                object str = "1,abc,2";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsFalse(result);
            }

            [Test]
            public void Should_ReturnFalse_ForStringThatContainsASingleLetter()
            {
                object str = "a";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsFalse(result);
            }

            [Test]
            public void Should_ReturnFalse_ForDescendingOrder()
            {
                object str = "3,2,1";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsFalse(result);
            }

            [Test]
            public void Should_ReturnFalse_ForWronglySyntaxedStringDoubleHyphen()
            {
                object str = "1--3";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsFalse(result);
            }

            [Test]
            public void Should_ReturnFalse_ForWronglySyntaxedStringDoubleComma()
            {
                object str = "1,,3";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsFalse(result);
            }

            [Test]
            public void Should_ReturnFalse_ForTrailingComma()
            {
                object str = "1,";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsFalse(result);
            }

            [Test]
            public void Should_ReturnFalse_ForTrailingHyphen()
            {
                object str = "1-";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsFalse(result);
            }

            [Test]
            public void Should_ReturnTrue_SingleId()
            {
                object str = "5";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsTrue(result);
            }

            [Test]
            public void Should_ReturnTrue_ForCorrectComplexString()
            {
                object str = "1,2-5,6-10";
                var attributeInstance = new Helpers.ValidAscendingOrderRangeOfIntIdsAttribute();
                var result = attributeInstance.IsValid(str);
                Assert.IsTrue(result);
            }
        }
    }
}
