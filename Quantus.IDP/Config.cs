using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;

namespace Marvin.IDP
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Frank",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Frank"),
                        new Claim("family_name", "Underwood"),
                        new Claim("address", "1, Main Road"),
                        new Claim("role", "PayingUser"),
                        new Claim("subscriptionlevel", "PayingUser"),
                        new Claim("country", "nl")
                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Claire",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Claire"),
                        new Claim("family_name", "Underwood"),
                        new Claim("address", "2, Big Street"),
                        new Claim("role", "FreeUser"),
                        new Claim("subscriptionlevel", "PayingUser"),
                        new Claim("country", "nl")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), // ensures that subjectId is returned
                new IdentityResources.Profile(), // ensures that claims are returned
                new IdentityResources.Address(),
                new IdentityResource("roles", "Your role(s)", new List<string>() {"role"}),
                new IdentityResource("country", "The country you're living in", new List<string> { "country"}),
                new IdentityResource("subscriptionlevel", "Your subscriptionlevel", new List<string> { "subscriptionlevel"})
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("fittifyapi", "Fittify API",
                new List<string>() { "role" })
                {
                    ApiSecrets = { new Secret("apisecret".Sha256()) }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientName = "Fittify.Web",
                    ClientId = "fittifyclient",
                    AllowedGrantTypes = new List<string>() { GrantType.Hybrid }, // Todo: Must be implicit for Fittify!

                    //AccessTokenType = AccessTokenType.Jwt, // Self contained token, fittifyapi can validate this token on its own and does not need to talk to IDP
                    AccessTokenType = AccessTokenType.Reference, // Everytime the fittifyapi receives a request, fittifyapi forwards reference token to IDP and gets real accesstoken returned

                    // IdentityTokenLifetime = 300,
                    // AuthorizationCodeLifetime = 300, 
                    AccessTokenLifetime = 600, // remember that api adds additional 5 minutes to deal with time offsets

                    //AbsoluteRefreshTokenLifetime = (default 30 days)

                    //RefreshTokenExpiration = TokenExpiration.Sliding, // only refreshes the lifetime of the same token until AbsoluteRefreshTokenLifetime limit is reached
                    //SlidingRefreshTokenLifetime = ...

                    UpdateAccessTokenClaimsOnRefresh = true, // if an attribute on the IDP Level changes, for example, you update your address, accesstoken will be updated
                    AllowOfflineAccess = true, // allows the user to continue using the app, although user is logged out of IDP

                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44328/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "fittifyapi",
                        "country",
                        "subscriptionlevel"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44328/signout-callback-oidc"
                    }
                    //AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}
