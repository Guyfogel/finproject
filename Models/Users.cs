using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        [Required,StringLength(12)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int PersonNum { get; set; }
        [Required, StringLength(20)]
        public string Name { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        
        public Genders Gender { get; set; }
        [Required]
        public Roles Role { get; set; }
        public byte[] Pic { get; set; }
    }
}
