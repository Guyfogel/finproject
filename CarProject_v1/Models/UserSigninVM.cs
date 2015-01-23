using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CarRentalMvc.Models
{
    public class UserSigninVM
    {
        public int UserID { get; set; }
        [Required, MaxLength(20, ErrorMessage = "the maximum length of a name is 20 chars")]
        public string Name { get; set; }
        [Required, MaxLength(20, ErrorMessage = "the maximum length of a name is 20 chars")]
        public string UserName { get; set; }
        //לדאוג ליוניק הזה
        [Required, RegularExpression(@"\d{9}", ErrorMessage = "idntity number must be 9 digits only")]
        public string Personnum { get; set; }
        //[Range(typeof(DateTime), "1/1/1900", "1/1/1996",ErrorMessage="user must be at least 18 years old")]
        //public DateTime? BirthDate { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirmPassword { get; set; }
        public byte[] Picture { get; set; }
    }
}