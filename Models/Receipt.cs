using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Receipt
    {
        public int OrderID { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
