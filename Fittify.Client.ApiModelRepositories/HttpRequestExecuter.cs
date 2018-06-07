using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Fittify.Common.CustomExceptions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Fittify.Client.ApiModelRepository
{
    [ExcludeFromCodeCoverage] // Todo: Unit Test this !! 
    public class HttpRequestExecuter : IHttpRequestExecuter
    {
        private readonly IHttpRequestBuilder _httpRequestBuilder;
        public HttpRequestExecuter(IHttpRequestBuilder httpRequestBuilder)
        {
            _httpRequestBuilder = httpRequestBuilder;
        }
        public async Task<HttpResponseMessage> GetSingle(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            string accessToken = await GetAccessToken(appConfiguration, httpContextAccessor);

            _httpRequestBuilder
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri)
                .AddBearerToken(accessToken);

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> GetCollection(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _httpRequestBuilder
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            string accessToken = await GetAccessToken(appConfiguration, httpContextAccessor);

            var builder = _httpRequestBuilder
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri)
                .AddBearerToken(accessToken);

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> Post(
            Uri requestUri, object value, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            string accessToken = await GetAccessToken(appConfiguration, httpContextAccessor);

            _httpRequestBuilder
                .AddMethod(HttpMethod.Post)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value))
                .AddBearerToken(accessToken);
            
            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> Put(
            Uri requestUri, object value)
        {
            _httpRequestBuilder
                .AddMethod(HttpMethod.Put)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> Patch(
            Uri requestUri, JsonPatchDocument jsonPatchDocument /*object jsonPatchDocument*/, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            string accessToken = await GetAccessToken(appConfiguration, httpContextAccessor);

            _httpRequestBuilder
                .AddMethod(new HttpMethod("PATCH"))
                .AddRequestUri(requestUri)
                .AddContent(new PatchContent(jsonPatchDocument))
                .AddBearerToken(accessToken);

            return await _httpRequestBuilder.SendAsync();
        }

        public async Task<HttpResponseMessage> Delete(Uri requestUri, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            string accessToken = await GetAccessToken(appConfiguration, httpContextAccessor);

            _httpRequestBuilder
                .AddMethod(HttpMethod.Delete)
                .AddRequestUri(requestUri)
                .AddBearerToken(accessToken);

            return await _httpRequestBuilder.SendAsync();
        }

        private async Task<string> GetAccessToken(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            string accessToken = string.Empty;
            var currentContext = httpContextAccessor.HttpContext;

            // Should we renew access & refresh tokens?
            var expires_at = await AuthenticationHttpContextExtensions.GetTokenAsync(currentContext,
                "expires_at");

            // compare - make sure to use the exact date formats for comparison 
            // (UTC, in this case)
            if (string.IsNullOrWhiteSpace(expires_at)
                || ((DateTime.Parse(expires_at).AddSeconds(-60)).ToUniversalTime()
                    < DateTime.UtcNow))
            {
                accessToken = await RenewTokens(appConfiguration, httpContextAccessor);
            }
            else
            {
                // get access token
                accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(currentContext,
                    OpenIdConnectParameterNames.AccessToken);
            }

            return accessToken;
        }

        private async Task<string> RenewTokens(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            // get the current HttpContext to access the tokens
            var currentContext = httpContextAccessor.HttpContext;

            // get the metadata
            var discoveryClient = new DiscoveryClient(appConfiguration.GetValue<string>("QuantusIdpBaseUri"));
            var metaDataResponse = await discoveryClient.GetAsync();

            // create a new token client to get new tokens
            var tokenClient = new TokenClient(metaDataResponse.TokenEndpoint,
                "fittifyclient", "secret");

            // get the saved refresh token Asp.Net Core 1.1

            var currentRefreshToken =
                await AuthenticationHttpContextExtensions.GetTokenAsync(currentContext,
                    OpenIdConnectParameterNames.RefreshToken);

            // refresh the tokens
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(currentRefreshToken);

            if (!tokenResult.IsError)
            {
                // Save the tokens.
                var authenticateInfo = await httpContextAccessor.HttpContext
                    .AuthenticateAsync();

                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                authenticateInfo.Properties.UpdateTokenValue("expires_at",
                    expiresAt.ToString("o", CultureInfo.InvariantCulture));

                authenticateInfo.Properties.UpdateTokenValue(
                    OpenIdConnectParameterNames.AccessToken,
                    tokenResult.AccessToken);
                authenticateInfo.Properties.UpdateTokenValue(
                    OpenIdConnectParameterNames.RefreshToken,
                    tokenResult.RefreshToken);

                // we're signing in again with the new values.
                await httpContextAccessor.HttpContext.SignInAsync("Cookies",
                    authenticateInfo.Principal, authenticateInfo.Properties);

                // return the new access token 
                return tokenResult.AccessToken;
            }
            else
            {
                throw new OpenIdConnectException("Problem encountered while refreshing tokens.",
                    tokenResult.Exception);
            }
        }
    }
}

