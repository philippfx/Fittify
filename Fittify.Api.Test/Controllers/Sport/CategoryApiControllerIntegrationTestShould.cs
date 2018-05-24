using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fittify.Api.Controllers.Sport;
using Fittify.Api.Helpers;
using Fittify.Api.OfmRepository.Helpers;
using Fittify.Api.OfmRepository.OfmRepository.GenericGppd;
using Fittify.Api.OfmRepository.OfmResourceParameters.Sport;
using Fittify.Api.OuterFacingModels.Sport.Get;
using Fittify.Api.OuterFacingModels.Sport.Patch;
using Fittify.Api.OuterFacingModels.Sport.Post;
using Fittify.Api.Services.ConfigureServices;
using Fittify.Api.Test.TestHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;

namespace Fittify.Api.Test.Controllers.Sport.Sport
{
    [TestFixture]
    class CategoryApiControllerIntegrationTestShould
    {
        private TestServer _server;
        private HttpClient _client;

        [SetUp]
        public void Init()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();

            //AutoMapper.Mapper.Reset();
            //AutoMapperForFittify.Initialize();
        }

        [Test]
        public async Task ReturnOkResult_WhenUsingGetById()
        {
            //var response = await _client.GetAsync("api/categories");
            //var urlhelper = _server.
            //response.EnsureSuccessStatusCode();

            //var responseString = await response.Content.ReadAsStringAsync();

            //var kavaConfig = TestHelpers.TestAppConfiguration.GetApplicationConfiguration(TestContext.CurrentContext.TestDirectory);

            //var builder = new ConfigurationBuilder()
            //    //.SetBasePath(Directory.GetCurrentDirectory())
            //    .SetBasePath(TestContext.CurrentContext.TestDirectory)
            //    .AddJsonFile("appsettings.json"); // includes appsettings.json configuartion file
            //var configuration = builder.Build();

            //// Mock GppdRepo
            //var asyncGppdMock = new Mock<IAsyncGppd<CategoryOfmForGet, CategoryOfmForPost, CategoryOfmForPatch, int, CategoryOfmResourceParameters>>();
            //asyncGppdMock
            //    .Setup(s => s.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(new OfmForGetQueryResult<CategoryOfmForGet>()
            //    {
            //        ReturnedTOfmForGet = new CategoryOfmForGet()
            //        {
            //            Id = 1,
            //            Name = "MockCategory"
            //        }
            //    }));

            //// Mock IUrlHelper
            //var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            //Expression<Func<IUrlHelper, string>> urlSetup
            //    = url => url.Action(It.Is<UrlActionContext>(uac => uac.Action == "GetById" /*&& GetId(uac.Values) != cmd.Id*/));
            //mockUrlHelper.Setup(urlSetup).Returns("a/mock/url/for/testing").Verifiable();

            //// Mock IAppConfiguration to get latest FittifyApiVersion
            ////var appConfiguration = new Mock<IConfiguration>();
            ////appConfiguration.Setup(s => s.GetValue<string>(It.IsAny<string>())).Returns("1");
            ////var appConfiguration = new Mock<ConfigurationWrapper>();
            ////appConfiguration.Setup(s => s.GetValue<string>("LatestApiVersion")).Returns("1");

            //// Mock IHttpContextAccessor
            //var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            //httpContextAccessorMock.Setup(s => s.HttpContext.Items).Returns(new Dictionary<object, object>()
            //{
            //    {
            //        nameof(IncomingRawHeadersMock),
            //        new IncomingRawHeadersMock(configuration)
            //        {
            //            ContentType = "application/json",
            //            ApiVersion = "1", // overrides original appsettings.json value!
            //            IncludeHateoas = "1"
            //        }
            //    }
            //});

            //var categoryController = new CategoryApiController(
            //    asyncGppdMock.Object,
            //    mockUrlHelper.Object,
            //    httpContextAccessorMock.Object);

            //var result = await categoryController.GetById(1, null);

            //Assert.IsFalse(false);
        }
    }

}
