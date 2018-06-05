////using System;
////using System.Collections.Generic;
////using System.Text;
////using Fittify.Client.ApiModelRepositories;
////using Microsoft.AspNetCore.Hosting;
////using Microsoft.AspNetCore.TestHost;

////namespace Fittify.Web.View.Test
////{
////    public class FittifyApiTestServer : TestServer, IFittifyApiTestServer
////    {
////        public FittifyApiTestServer(IWebHostBuilder webHostBuilder)
////            : base(webHostBuilder)
////        {
////        }
////        public FittifyHttpClient CreateFittifyHttpClient()
////        {
////            ////FittifyHttpClient fittifyHttpClient = base.CreateClient();
////            return base.CreateClient() as FittifyHttpClient;
////        }
////    }
////}
