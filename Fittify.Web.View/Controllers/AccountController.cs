using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Fittify.Web.View.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        [Route("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
