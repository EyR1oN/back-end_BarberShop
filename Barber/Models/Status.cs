using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Models
{
    public class Status
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(30)]
        public string statusName { get; set; }
    }
}
