using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingASP.ViewModels
{
    public class CompanyViewModel
    {
        //public int ID { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters long!")]
        //[RegularExpression(@"^ ((?=.*[a - z])(?=.*[A - Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}