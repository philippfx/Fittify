using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Fittify.Api.Test.Controllers.Sport;
using Microsoft.Extensions.Configuration;

namespace Fittify.Api.Test.TestHelpers
{
    public class AppConfigurationMock : IDisposable
    {
        private string FilePath { get; set; }
        public IConfiguration Instance { get; set; }
        public AppConfigurationMock(string appSettingsJsonString)
        {
            FilePath = Path.GetDirectoryName(typeof(CategoryApiControllerShould).GetTypeInfo().Assembly.Location) + "\\appsettings_" + Guid.NewGuid() + ".json";
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            File.Create(Path.Combine(FilePath)).Close();
            File.WriteAllText(FilePath, appSettingsJsonString);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(CategoryApiControllerShould).GetTypeInfo().Assembly.Location)) // The only way I found to get directory path of unit test project / bin /debug
                .AddJsonFile("appsettings.json"); // Includes appsettings.json configuartion file
            Instance = builder.Build();
        }

        public void Dispose()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        ////public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        ////{
        ////    return new ConfigurationBuilder()
        ////        .SetBasePath(outputPath)
        ////        .AddJsonFile("appsettings.json", optional: true)
        ////        //.AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
        ////        .AddEnvironmentVariables()
        ////        .Build();
        ////}

        ////public static IConfiguration GetDefaultAppConfiguration()
        ////{
        ////    var builder = new ConfigurationBuilder()
        ////        .SetBasePath(Path.GetDirectoryName(typeof(CategoryApiControllerShould).GetTypeInfo().Assembly.Location)) // The only way I found to get directory path of unit test project / bin /debug
        ////        .AddJsonFile("appsettings.json"); // Includes appsettings.json configuartion file
        ////    var configuration = builder.Build();

        ////    return configuration;
        ////}

        ////public static IConfiguration GetDefaultAppConfiguration(string appSettingsJsonString)
        ////{
        ////    var path = Path.GetDirectoryName(typeof(CategoryApiControllerShould).GetTypeInfo().Assembly.Location);
        ////    string fileName = path + "\\appsettings.json";

        ////    FileInfo fileInfo = new FileInfo(fileName);

        ////    if (File.Exists(fileName))
        ////    {
        ////        File.Delete(fileName);
        ////    }

        ////    File.Create(Path.Combine(fileName)).Close();
        ////    File.WriteAllText(fileName, appSettingsJsonString);

        ////    var builder = new ConfigurationBuilder()
        ////        .SetBasePath(Path.GetDirectoryName(typeof(CategoryApiControllerShould).GetTypeInfo().Assembly.Location)) // The only way I found to get directory path of unit test project / bin /debug
        ////        .AddJsonFile("appsettings.json"); // Includes appsettings.json configuartion file
        ////    var configuration = builder.Build();

        ////    return configuration;
        ////}

    }
}
