////using System;
////using System.Collections.Generic;
////using System.Text;
////using System.Threading.Tasks;
////using Fittify.Api.Test.TestHelpers;
////using Microsoft.AspNetCore.Hosting;
////using Microsoft.AspNetCore.TestHost;
////using NUnit.Framework;

////namespace Fittify.Api.Test
////{
////    [TestFixture]
////    class IntegrationTestShould
////    {
////        public TestServer GetApiTestServerInstance()
////        {
////            return new TestServer(new WebHostBuilder()
////                .UseStartup<ApiTestServerStartup>()
////                .UseEnvironment("TestInMemoryDb"));
////        }

////        public TestServer GetClientTestServerInstance()
////        {
////            return new TestServer(new WebHostBuilder()
////                .UseStartup<ApiTestServerStartup>()
////                .UseEnvironment("Development"));
////        }

////        [Test]
////        public async Task ShowProductsFromApi()
////        {
////            using (var apiServer = GetApiTestServerInstance())
////            using (var clientServer = GetClientTestServerInstance())
////            {
////                var client = clientServer.CreateClient(apiServer);
////                var response = await client.GetAsync("/products");
////                var responseString = await response.Content.ReadAsStringAsync();

////                Assert.AreEqual("{ Shows product data coming from the apu }", responseString);
////            }
////        }
////    }

////}
