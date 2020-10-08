using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CA1.Models
{
    public class Session
    {
        public string Id { get; set; }

        [Required]
        public long Timestamp { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
