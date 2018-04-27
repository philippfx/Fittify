using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Quantus.IDP.Entities.Default
{
    public class QuantusUserClaim : IdentityUserClaim<Guid>
    {
        //public new Guid Id { get; set; }
    }
}
