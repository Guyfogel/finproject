using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Locations
    {
        [Key]
        [Column(Order = 1)]
        public int LocationID { get; set; }
        [Required]
        //[Index("IX_First", 4, IsUnique = true)]
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
    }
}
