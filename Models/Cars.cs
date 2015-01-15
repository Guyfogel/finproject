using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Cars
    {
        [Key]
        public int CarID { get; set; }
        [Required]
        public int CarTypeID { get; set; }
        [ForeignKey("CarTypeID")]
        
        public virtual CarTypes CarType { get; set; }
        [Required]
        public int Kilometrage { get; set; }
        [Required]
        public bool isAvailable { get; set; }
        [Required]
        public int LocationID { get; set; }
        [ForeignKey("LocationID")]
        public virtual Locations Location {get;set;}
        public byte[] Photo { get; set; }

    }
}
