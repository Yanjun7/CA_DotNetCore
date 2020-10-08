using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CA1.Models
{
    public class Product
    {
        [MaxLength(50)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [MaxLength(500)]
        public string PhotoLink { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string PhotoTag { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
