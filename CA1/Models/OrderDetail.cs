using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace CA1.Models
{
    public class OrderDetail
    {
        [MaxLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductId { get; set; }

        [Required]
        [MaxLength(1000)]
        public Guid ActivationCode { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
