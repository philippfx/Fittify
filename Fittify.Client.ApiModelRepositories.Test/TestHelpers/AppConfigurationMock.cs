using System;
using System.IO;
using System.Reflection;
using Fittify.Client.ApiModelRepository;
using Microsoft.Extensions.Configuration;

namespace Fittify.Client.ApiModelRepositories.Test.TestHelpers
{
    public class AppConfigurationMock : IDisposable
    {
        private string FullFilePath { get; set; }
        public IConfiguration Instance { get; set; }
        public AppConfigurationMock(string appSettingsJsonString)
        {
            var appSettingsFileName = "appsettings_" + Guid.NewGuid() + ".json";
            FullFilePath = Path.GetDirectoryName(typeof(ApiModelRepositoryBase<,,,>).GetTypeInfo().Assembly.Location) + "\\" + appSettingsFileName;
            if (File.Exists(FullFilePath))
            {
                File.Delete(FullFilePath);
            }

            File.Create(Path.Combine(FullFilePath)).Close();
            File.WriteAllText(FullFilePath, appSettingsJsonString);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(ApiModelRepositoryBase<,,,>).GetTypeInfo().Assembly.Location)) // The only way I found to get directory path of unit test project / bin /debug
                .AddJsonFile(appSettingsFileName); // Includes appsettings.json configuartion file
            Instance = builder.Build();
        }

        public void Dispose()
        {
            if (File.Exists(FullFilePath))
            {
                File.Delete(FullFilePath);
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
