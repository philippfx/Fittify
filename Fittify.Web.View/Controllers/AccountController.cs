using System;
using System.Threading.Tasks;
using Fittify.Common.CustomExceptions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Fittify.Web.View.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private IConfiguration _appConfiguration;
        private IHttpContextAccessor _httpContextAccessor;

        public AccountController(IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            _appConfiguration = appConfiguration;
            _httpContextAccessor = httpContextAccessor;
        }
        [Route("logout")]
        public async Task Logout()
        {
            // get the metadata
            var discoveryClient = new DiscoveryClient(_appConfiguration.GetValue<string>("QuantusIdpBaseUri"));
            var metaDataResponse = await discoveryClient.GetAsync();

            // create a TokenRevocationClient
            var revocationClient = new TokenRevocationClient(
                metaDataResponse.RevocationEndpoint,
                "fittifyclient",
                "secret");

            // get the access token to revoke 
            var accessToken =
                await AuthenticationHttpContextExtensions.GetTokenAsync(
                    _httpContextAccessor.HttpContext,
                    OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var revokeAccessTokenResponse =
                    await revocationClient.RevokeAccessTokenAsync(accessToken);

                if (revokeAccessTokenResponse.IsError)
                {
                    throw new OpenIdConnectException("Problem encountered while revoking the access token."
                        , revokeAccessTokenResponse.Exception);
                }
            }

            // revoke the refresh token as well
            var refreshToken =
                await AuthenticationHttpContextExtensions.GetTokenAsync(
                    _httpContextAccessor.HttpContext,
                    OpenIdConnectParameterNames.RefreshToken);

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var revokeRefreshTokenResponse =
                    await revocationClient.RevokeRefreshTokenAsync(refreshToken);

                if (revokeRefreshTokenResponse.IsError)
                {
                    throw new OpenIdConnectException("Problem encountered while revoking the refresh token."
                        , revokeRefreshTokenResponse.Exception);
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Logging out of client
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme); // Logging out of IDP
        }
    }
}
