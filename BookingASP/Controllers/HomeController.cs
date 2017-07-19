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

            return View(db.Services.Where(m=>m.CompanyID == id));

        }
    }
}