using System;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Fittify.Web.Test.Selenium
{
    public class SeleniumTestServer : IDisposable
    {
        private Process _process;

        public IWebDriver WebDriver;

        public SeleniumTestServer()
        {
            StartServer();
        }

        private void StartServer()
        {
            string projectName = "Fittify.Web.View";

            var applicationPath = @"C:\Users\PhilippPrivate\Documents\Visual Studio 2017\Projects\Fittify\Fittify.Web.View";

            var test = $@"run --project ""{applicationPath}\{projectName}.csproj""";

            //_process = new Process()
            //{
            //    StartInfo =
            //    {
            //        FileName = @"dotnet.exe",
            //        Arguments = $@"run --project ""{applicationPath}\{projectName}.csproj"""
            //    }
            //};
            //_process.Start();
            WebDriver = new ChromeDriver(@"C:\Selenium\drivers");
            WebDriver.Navigate().GoToUrl("http://localhost:5000/workouts");
        }
        
        public void Dispose()
        {
            WebDriver?.Dispose();
            //if (_process.HasExited == false)
            //{
            //    _process.Kill();
            //}
        }
    }
}
