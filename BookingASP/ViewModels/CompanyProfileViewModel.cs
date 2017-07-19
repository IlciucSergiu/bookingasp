using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace BookingASP.ViewModels
{
    public class CompanyProfileViewModel
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    }
}