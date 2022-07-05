using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Models
{
    public class Orders
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int userId { get; set; }

        [ForeignKey("userId")]
        public User User { get; set; }

        [Required]
        public string servicesId { get; set; }
        //hfhfgjfgjfjf
        [ForeignKey("servicesId")]
        public Services Services { get; set; }

        [Required]
        public string placeId { get; set; }

        [ForeignKey("placeId")]
        public Place Place { get; set; }

        [Required]
        public DateTime data_time { get; set; }
    }
}
