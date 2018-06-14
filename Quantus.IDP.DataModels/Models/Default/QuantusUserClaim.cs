using System;
using Microsoft.AspNetCore.Identity;

namespace Quantus.IDP.DataModels.Models.Default
{
    public class QuantusUserClaim : IdentityUserClaim<Guid>
    {
        //public new Guid Id { get; set; }
    }
}
