﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Quantus.IDP.Controllers.UserRegistration
{
    public class RegisterUserViewModel
    {
        // credentials       
        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        // claims 
        [Required]
        [MaxLength(100)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(100)]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }
        
        [MaxLength(200)]
        public string Address { get; set; }
        
        [MaxLength(2)]
        public string Country { get; set; }

        public SelectList CountryCodes { get; set; } =
            new SelectList(
                new[] 
                {
                    new { Id = "BE", Value = "Belgium" },
                    new { Id = "US", Value = "United States of America" },
                    new { Id = "IN", Value = "India" },
                    new { Id = "NL", Value = "Netherlands" }
                }, "Id", "Value");

        public string ReturnUrl { get; set; }

        public string Provider { get; set; }

        public string ProviderUserId { get; set; }
        
        public bool IsProvisioningFromExternal
        {
            get
            {
                return (Provider != null);
            }
        }

    }
}
