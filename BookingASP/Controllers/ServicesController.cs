using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookingASP.Models;
using BookingASP.ViewModels;
using AutoMapper;
using System.Net.Mail;

namespace BookingASP.Controllers
{
    public class ServicesController : Controller
    {
        private CompanyDBContext db = new CompanyDBContext();

        // GET: Services
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return View("Error");

            string userID = Session["User"].ToString();
            // Company company = db.Companies.Where(x => x.Email == userEmail).FirstOrDefault();
            int companyId = Convert.ToInt32(userID);
            var services = db.Services.Where(m => m.CompanyID == companyId);
            return View(services.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            //  ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Duration,Spaces,Price,Rating,Availability")] ServiceViewModel serviceVM)
        {
            if (ModelState.IsValid)
            {


                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ServiceViewModel, Service>();
                });

                IMapper mapper = config.CreateMapper();
                Service service = mapper.Map<ServiceViewModel, Service>(serviceVM);

                string userId = Session["User"].ToString();
                service.CompanyID = Convert.ToInt32(userId);

                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", service.CompanyID);
            return View(serviceVM);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", service.CompanyID);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Duration,Spaces,Price,Rating,Availability,CompanyID")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "Name", service.CompanyID);
            return View(service);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string AddBooking(CreateBookingViewModel bookingVM)
        {

            //try {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CreateBookingViewModel, Booking>();
                });

                IMapper mapper = config.CreateMapper();
                Booking booking = mapper.Map<CreateBookingViewModel, Booking>(bookingVM);

                //booking.ServiceID = ViewBag.ServiceID;
                db.Bookings.Add(booking);
                db.SaveChanges();


                MailMessage mail = new MailMessage("", booking.Email);
                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("", "");
                client.Host = "smtp.gmail.com";
                mail.Subject = "Service booking";
                mail.Body = "Your booking has been saved. Time"+booking.BookingTime;
                client.Send(mail);


                return "Success";
            }
            else return "Invalid data";


            //}
            //catch(Exception e) { return e.Message; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}
