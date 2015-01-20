using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public int CarID { get; set; }
        [ForeignKey("CarID")]
        public virtual Cars Car { get; set; }
        [Required]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public Users User { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
    }
}
