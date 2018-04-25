﻿using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quantus.IDP.Entities;
using Quantus.IDP.Services;

namespace Quantus.IDP.Controllers.UserRegistration
{
    public class UserRegistrationController : Controller
    {
        private readonly IQuantusUserRepository _quantusUserRepository;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _httpContextAccssor;

        public UserRegistrationController(IQuantusUserRepository quantusUserRepository,
             IIdentityServerInteractionService interaction,
             IHttpContextAccessor httpContextAccessor)
        {
            _quantusUserRepository = quantusUserRepository;
            _interaction = interaction;
            _httpContextAccssor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult RegisterUser(RegistrationInputModel registrationInputModel)
        {
            var vm = new RegisterUserViewModel()
            {
                ReturnUrl = registrationInputModel.ReturnUrl,
                Provider = registrationInputModel.Provider,
                ProviderUserId = registrationInputModel.ProviderUserId
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // create user + claims
                var userToCreate = new QuantusUser();
                userToCreate.Password = model.Password;
                userToCreate.Username = model.Username;
                userToCreate.IsActive = true;
                userToCreate.Claims.Add(new UserClaim("country", model.Country));
                userToCreate.Claims.Add(new UserClaim("address", model.Address));
                userToCreate.Claims.Add(new UserClaim("given_name", model.Firstname));
                userToCreate.Claims.Add(new UserClaim("family_name", model.Lastname));
                userToCreate.Claims.Add(new UserClaim("email", model.Email));
                userToCreate.Claims.Add(new UserClaim("subscriptionlevel", "FreeUser"));

                // if we're provisioning a user via external login, we must add the provider &
                // user id at the provider to this user's logins
                if (model.IsProvisioningFromExternal)
                {
                    userToCreate.Logins.Add(new Entities.UserLogin()
                    {
                        LoginProvider = model.Provider,
                        ProviderKey = model.ProviderUserId
                    });
                }

                // add it through the repository
                _quantusUserRepository.AddUser(userToCreate);

                if (!_quantusUserRepository.Save())
                {
                    throw new Exception($"Creating a user failed.");
                }

                if (!model.IsProvisioningFromExternal)
                {
                    // log the user in
                    //await HttpContext.Authentication.SignInAsync(userToCreate.SubjectId, userToCreate.Username);
                    await _httpContextAccssor.HttpContext.SignInAsync(userToCreate.SubjectId, userToCreate.Username);
                }

                // continue with the flow     
                if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            // ModelState invalid, return the view with the passed-in model
            // so changes can be made
            return View(model);
        }

    }
}
