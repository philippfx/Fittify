using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Fittify.Api.Test.TestHelpers
{
    public static class TestAppConfiguration
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                //.AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
                .AddEnvironmentVariables()
                .Build();
        }

        public static KavaDocsConfiguration GetApplicationConfiguration(string outputPath)
        {
            var configuration = new KavaDocsConfiguration();

            var iConfig = GetIConfigurationRoot(outputPath);

            iConfig
                .GetSection("LatestApiVersion")
                .GetSection("KavaDocs")
                .Bind(configuration);

            return configuration;
        }

        public class KavaDocsConfiguration
        {
            public class EmailClass
            {
                public string MailServer { get; set; }
                public string MailServerUsername { get; set; }
                public string MailServerPassword { get; set; }
                public bool UseSsl { get; set; }
            }

            public string ConnectionString { get; set; }
            public EmailClass Email { get; set; }

            public string LatestApiVersion { get; set; }



            //public KavaDocs TheKavaDocs { get; set; }

            //public class KavaDocs
            //{
            //    public string ConnectionString { get; set; }
            //    public EmailClass EmailClass { get; set; }
            //}
        }
    }
}
