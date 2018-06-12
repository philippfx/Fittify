using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Fittify.Web.Test.Selenium
{
    [TestFixture]
    class MyTest
    {
        [Test]
        public void TestHomePage()
        {
            string text;
            using (var server = new SeleniumTestServer())
            {
                IWebDriver webDriver = server.WebDriver;

                var title = webDriver.FindElement(By.ClassName("navbar-brand"));
                text = title.Text;
                var url = webDriver.Url;
                var page = webDriver.Title;
                var tag = webDriver.FindElement(By.TagName("body"));

            }
            Assert.AreEqual("Logo", text);
        }
    }
}
