using BookingASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookingASP.Controllers
{
    public class HomeController : Controller
    {
        private CompanyDBContext db = new CompanyDBContext();
        // GET: Home
        public ActionResult Index()
        {

            return View(db.Companies.ToList());

        }

        public ActionResult Services(int id)
        {
            Company company = db.Companies.Find(id);

            if (company.Logo == "" || company.Logo == null)
                ViewBag.Logo = "LogoPlaceholder.jpg";
            else
            ViewBag.Logo = company.Logo;

            ViewBag.CompanyName = company.Name;
            ViewBag.Description = company.Description;

            return View(db.Services.Where(m=>m.CompanyID == id));

        }
    }
}