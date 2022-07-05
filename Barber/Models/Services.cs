using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Models
{
    public class Services
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }
        [Required]
        [StringLength(1000)]
        public string description { get; set; }

        [Required]
        public IFormFile picture { get; set; }

        [Required]
        public int price { get; set; }

        [Required]
        public DateTime timeToMake { get; set; }

        [Required]
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public Category Category { get; set; }
    }
}
