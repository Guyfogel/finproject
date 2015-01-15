using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class CarTypes
    {
        [Key]
        public int CarTypeID { get; set; }
        [Required]
        //[Index("IX_Second", 1, IsUnique = true)]
        public string Manufacturer { get; set; }
        [Required]
        //[Index("IX_Second", 2, IsUnique = true)]
        public string CarModel { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal PricePerLateDay { get; set; }
        public string Gear { get; set; }
    }
}
