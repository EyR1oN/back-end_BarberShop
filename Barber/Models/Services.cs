﻿using Microsoft.AspNetCore.Http;
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
        public byte[] picture { get; set; }

        [Required]
        public int price { get; set; }

        [Required]
        public DateTime timeToMake { get; set; }
        
        public Category Category { get; set; }
        [ForeignKey("Category")]

        [Required]
        public int categoryId { get; set; }
    }
}
