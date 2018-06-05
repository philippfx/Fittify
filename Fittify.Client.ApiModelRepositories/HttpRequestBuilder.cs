using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fittify.Client.ApiModelRepositories
{
    public class HttpRequestBuilder : IHttpRequestBuilder
    {
        private readonly IHttpClient _httpClient;
        private readonly HttpClient _betterClient;
        public HttpRequestBuilder(IHttpClient httpClient, IServiceProvider serviceProvider)
        {
            _httpClient = httpClient;

            _betterClient = serviceProvider.GetService(typeof(HttpClient)) as HttpClient;
        }
        private HttpMethod _method = null;
        private Uri _requestUri = null;
        private HttpContent _content = null;
        private string _bearerToken = "";
        private string _acceptHeader = "application/json";
        private Dictionary<string, string> _customRequestHeaders = null;
        private TimeSpan _timeout = new TimeSpan(0, 0, 15);
        
        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            this._method = method;
            return this;
        }

        public HttpRequestBuilder AddRequestUri(Uri requestUri)
        {
            this._requestUri = requestUri;
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            this._content = content;
            return this;
        }

        public HttpRequestBuilder AddBearerToken(string bearerToken)
        {
            this._bearerToken = bearerToken;
            return this;
        }

        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            this._acceptHeader = acceptHeader;
            return this;
        }

        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            this._timeout = timeout;
            return this;
        }

        public HttpRequestBuilder AddCustomRequestHeaders(Dictionary<string, string> customRequestHeaders)
        {
            this._customRequestHeaders = customRequestHeaders;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            // Check required arguments
            //EnsureArguments();

            // Setup request
            var request = new HttpRequestMessage
            {
                Method = this._method,
                RequestUri = _requestUri
            };

            if (this._content != null)
            {
                request.Content = this._content;
                //request.Content.Headers.Add("Content-Type", "application/json");
            }

            if (!string.IsNullOrEmpty(this._bearerToken))
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", this._bearerToken);

            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(this._acceptHeader))
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(this._acceptHeader));

            if (this._customRequestHeaders != null)
            {
                foreach (var customHeader in _customRequestHeaders)
                {
                    request.Headers.Add(customHeader.Key, customHeader.Value);
                }
            }

            // Setup client
            ////var client = new HttpClient();
            _httpClient.Timeout = this._timeout;

            _betterClient.DefaultRequestHeaders.Add("X-Integration-Testing", "abcde-12345");
            _betterClient.DefaultRequestHeaders.Add("sub", "d860efca-22d9-47fd-8249-791ba61b07c7");
            //var moreResult = await _betterClient.GetAsync("/root");
            //var moreResponseString = await moreResult.Content.ReadAsStringAsync();


            //var result = await _betterClient.GetAsync("/api/categories"); // THIS WORKS !!!

            var baseAddress = _betterClient.BaseAddress;
            //var result = await _betterClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, @"http://localhost/api/categories")); // WORKS !!!

            //_betterClient.BaseAddress = new Uri(@"https://localhost:44353/");
            //var result = await _betterClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, @"https://localhost:44353/api/categories")); // WORKS !!!
            //var responseString = await result.Content.ReadAsStringAsync();

            //var result3 = await _betterClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, @"https://localhost:44353/api/categories?ids=1,2")); // WORKS !!!
            //var responseString3 = await result3.Content.ReadAsStringAsync();
            //var result2 = await _betterClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, @"http://localhost/api/categories")); // STILL WORKS !!!
            //var responseString2 = await result2.Content.ReadAsStringAsync();

            var result = await _betterClient.SendAsync(request); // STILL WORKS !!!
            var content = await result.Content.ReadAsStringAsync();

            //var result = await _httpClient.SendAsync(request);

            return result;
            //var result = await _betterClient.GetAsync("api/categories");
            //var content = result.Content.ReadAsStringAsync();
            //return result;


            //return await _httpClient.GetAsync("/api/categories");
            //return await _httpClient.SendAsync(request);
        }
    }
}
