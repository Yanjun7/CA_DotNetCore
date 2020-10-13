using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CA1.Models
{
    public class ShoppingCartDetail
    {
        [MaxLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        public string SessionId { get; set; }

        [MaxLength(50)]
        public string UserId { get; set; }

        [MaxLength(50)]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public virtual Session Session { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
