using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Barber.Models
{
    public class Order
    {
        [Key]
        public int id { get; set; }

        public User User { get; set; }
        [ForeignKey("User")]

        [Required]
        public int userId { get; set; }

        public Service Service { get; set; }
        [ForeignKey("Service")]     

        [Required]
        public int serviceId { get; set; }

        public Place Place { get; set; }
        [ForeignKey("Place")]       

        [Required]
        public int placeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime date { get; set; }

        [Required]
        public TimeSpan time { get; set; }
    }
}
