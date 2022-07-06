using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(30)]
        public string username { get; set; }
        [Required]
        [StringLength(45)]
        public string password { get; set; }

        [Required]
        [StringLength(45)]
        public string email { get; set; }
        [StringLength(45)]
        public string name { get; set; }
        [StringLength(45)]
        public string surname { get; set; }

        public Status Status { get; set; }
        [ForeignKey("Status")]


        [Required]
        public int statusId { get; set; }
    }
}
