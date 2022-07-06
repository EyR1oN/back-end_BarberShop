using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Models
{
    public class Category
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
        [StringLength(1000)]
        public string picture { get; set; }


    }
}
