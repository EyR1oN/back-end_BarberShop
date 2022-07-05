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

        public User User { get; set; }
        [ForeignKey("User")]

        [Required]
        public int userId { get; set; }

        public Services Services { get; set; }
        [ForeignKey("Services")]     

        [Required]
        public int servicesId { get; set; }

        public Place Place { get; set; }
        [ForeignKey("Place")]       

        [Required]
        public int placeId { get; set; }

        [Required]
        public DateTime data_time { get; set; }
    }
}
