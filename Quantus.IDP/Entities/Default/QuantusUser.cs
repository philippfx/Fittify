using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Quantus.IDP.Entities.Default;

namespace Quantus.IDP.Entities
{
    //[Table("Users")]
    public class QuantusUser : IdentityUser<Guid>
    {
        //[Key]
        //[MaxLength(50)]       
        //public string Id { get; set; }
    
        //[MaxLength(100)]
        //[Required]
        //public string Username { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; } = false;

        public bool IsProvisioningFromExternal { get; set; } = true;

        //public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();

        //public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();

        public ICollection<QuantusUserClaim> Claims { get; set; } = new List<QuantusUserClaim>();
        public ICollection<QuantusUserLogin> Logins { get; set; } = new List<QuantusUserLogin>();

    }
}
