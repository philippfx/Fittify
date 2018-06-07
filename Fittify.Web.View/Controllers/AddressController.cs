using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Fittify.Client.ViewModels;
using Fittify.Common.CustomExceptions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Fittify.Web.View.Controllers
{

    [ExcludeFromCodeCoverage]
    [Authorize]
    [Route("address")]
    public class AddressController : Controller
    {
        [Authorize(Roles = "PayingUser")]
        public async Task<IActionResult> Show()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44364/");
            var metaDataResponse = await discoveryClient.GetAsync();

            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);

            var accessToken =
                await AuthenticationHttpContextExtensions.GetTokenAsync(this.HttpContext, OpenIdConnectParameterNames.AccessToken);

            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new OpenIdConnectException("Problem accessing the UserInfo endpoint.", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(c => c.Type == "address")?.Value;


            return View("Index", new AddressViewModel(address));
        }
    }
}
