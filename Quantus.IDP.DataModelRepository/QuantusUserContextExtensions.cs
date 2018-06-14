using System;
using System.Collections.Generic;
using System.Linq;
using Quantus.IDP.DataModels.Models.Default;

namespace Quantus.IDP.DataModelRepository
{
    public static class QuantusUserContextExtensions
    {
        public static bool EnsureSeedDataForContext(this QuantusUserContext context)
        {
            // Add 2 demo users if there aren't any users yet
            if (context.Users.Any())
            {
                return false;
            }

            // init users
            var users = new List<QuantusUser>()
            {
                new QuantusUser()
                {
                    Id = new Guid("d860efca-22d9-47fd-8249-791ba61b07c7"),
                    UserName = "Frank",
                    Password = "password",
                    IsActive = true,
                    Claims = {
                        new QuantusUserClaim() { UserId = new Guid("d860efca-22d9-47fd-8249-791ba61b07c7"), ClaimType = "role", ClaimValue = "FreeUser"},
                        new QuantusUserClaim() { UserId = new Guid("d860efca-22d9-47fd-8249-791ba61b07c7"), ClaimType = "given_name", ClaimValue = "Frank" },
                        new QuantusUserClaim() { UserId = new Guid("d860efca-22d9-47fd-8249-791ba61b07c7"), ClaimType = "family_name", ClaimValue = "Underwood" },
                        new QuantusUserClaim() { UserId = new Guid("d860efca-22d9-47fd-8249-791ba61b07c7"), ClaimType = "address", ClaimValue = "Main Road 1" },
                        new QuantusUserClaim() { UserId = new Guid("d860efca-22d9-47fd-8249-791ba61b07c7"), ClaimType = "subscriptionlevel", ClaimValue = "PayingUser" },
                        new QuantusUserClaim() { UserId = new Guid("d860efca-22d9-47fd-8249-791ba61b07c7"), ClaimType = "country", ClaimValue = "nl" }
                    }
                },
                new QuantusUser()
                {
                    Id = new Guid("b7539694-97e7-4dfe-84da-b4256e1ff5c7"),
                    UserName = "Claire",
                    Password = "password",
                    IsActive = true,
                    Claims = {
                        new QuantusUserClaim() { UserId = new Guid("b7539694-97e7-4dfe-84da-b4256e1ff5c7"), ClaimType = "role", ClaimValue = "PayingUser" },
                        new QuantusUserClaim() { UserId = new Guid("b7539694-97e7-4dfe-84da-b4256e1ff5c7"), ClaimType = "given_name", ClaimValue = "Claire" },
                        new QuantusUserClaim() { UserId = new Guid("b7539694-97e7-4dfe-84da-b4256e1ff5c7"), ClaimType = "family_name", ClaimValue = "Underwood" },
                        new QuantusUserClaim() { UserId = new Guid("b7539694-97e7-4dfe-84da-b4256e1ff5c7"), ClaimType = "address", ClaimValue = "Big Street 2" },
                        new QuantusUserClaim() { UserId = new Guid("b7539694-97e7-4dfe-84da-b4256e1ff5c7"), ClaimType = "subscriptionlevel", ClaimValue = "FreeUser" },
                        new QuantusUserClaim() { UserId = new Guid("b7539694-97e7-4dfe-84da-b4256e1ff5c7"), ClaimType = "country",ClaimValue = "nl" }
                            
                    }
                }
            };

            context.Users.AddRange(users);
            if (context.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
