//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Quantus.IDP.Entities
//{
//    [Table("Claims")]
//    public class UserClaim
//    {         
//        [Key]
//        [MaxLength(50)]
//        public Guid Id { get; set; }

//        [MaxLength(50)]
//        [Required]
//        public Guid SubjectId { get; set; }

//        [Required]
//        [MaxLength(250)]
//        public string ClaimType { get; set; }

//        [Required]
//        [MaxLength(250)]
//        public string ClaimValue { get; set; }

//        public UserClaim(string claimType, string claimValue)
//        {
//            ClaimType = claimType;
//            ClaimValue = claimValue;
//        }

//        public UserClaim()
//        {

//        }
//    }
//}
