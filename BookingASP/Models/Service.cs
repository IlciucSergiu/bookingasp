using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace BookingASP.Models
{
    public class Service
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Spaces { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
        public string Availability { get; set; }

        public Company BelongingCompany { get; set; }
    }
}