using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingASP.ViewModels
{
    public class ServiceViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public int Duration { get; set; }

        public int Spaces { get; set; }

        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
        public string Availability { get; set; }
    }
}