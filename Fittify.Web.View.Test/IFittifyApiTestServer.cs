using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;

namespace Fittify.Web.View.Test
{
    public interface IFittifyApiTestServer
    {
        HttpMessageHandler CreateHandler();
        HttpClient CreateClient();
        WebSocketClient CreateWebSocketClient();
        RequestBuilder CreateRequest(string path);
        void Dispose();
        Uri BaseAddress { get; set; }
        IWebHost Host { get; }
        IFeatureCollection Features { get; }
    }
}