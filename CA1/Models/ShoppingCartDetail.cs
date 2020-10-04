using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA1.Models
{
    public class ShoppingCartDetail
    {
        public string UserID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
