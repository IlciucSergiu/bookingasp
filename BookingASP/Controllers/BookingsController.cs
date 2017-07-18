using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookingASP.Models;

namespace BookingASP.Controllers
{
    public class BookingsController : Controller
    {
        private CompanyDBContext db = new CompanyDBContext();

        // GET: Bookings
        
        public ActionResult Index(int? serviceId)
        {
            string userEmail = Session["User"].ToString();
            int CompanyID = db.Companies.Where(a => a.Email == userEmail).First().ID;
            var bookings = db.Bookings.Include(b => b.Service);
            // bookings = bookings.Include(a => a.Service.Company);
             bookings = bookings.Where(a => a.Service.CompanyID == CompanyID);

            var serviceList = new List<Service>();

            serviceList = db.Services.ToList();
           
            //ViewBag.Services = db.Services.Where(d => d.CompanyID == CompanyID).ToList();
            foreach (var item in bookings)
            {
                ViewBag.ServiceID = new SelectList(serviceList, "ID", "Title", item.ServiceID);
            }

            if (serviceId != null)
                bookings = bookings.Where(a => a.ServiceID == serviceId);

            return View(bookings.ToList());
        }

        
        

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id)
        {
            //ViewBag.ServiceID = new SelectList(db.Services, "ID", "Title");
            //return View();
            return HttpNotFound();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Email,Phone,BookingTime,ServiceID")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceID = new SelectList(db.Services, "ID", "Title", booking.ServiceID);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "Title", booking.ServiceID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Email,Phone,BookingTime,ServiceID")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ID", "Title", booking.ServiceID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
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
