using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using System.Drawing;
using System.ComponentModel.DataAnnotations;



namespace BookingASP.Models
{
    public class Company
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage ="The password must be at least 6 characters long!")]
        //[RegularExpression(@"^ ((?=.*[a - z])(?=.*[A - Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string CompanyName { get; set; }
        public string Description { get; set; }
        public byte[] Logo { get; set; }

       

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            if(imageIn == null)
                return new byte[0];

            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null)
                return new Bitmap(20,20);
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }

    public class CompanyDBContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
    }


}