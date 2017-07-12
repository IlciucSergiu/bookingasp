using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookingASP.Models;
using BookingASP.ViewModel;

namespace BookingASP.Controllers
{
    public class CompanyViewModelsController : Controller
    {
        private CompanyDBContext db = new CompanyDBContext();

        // GET: CompanyViewModels
        public ActionResult Index()
        {
            return View(db.CompanyViewModels.ToList());
        }

        // GET: CompanyViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyViewModel companyViewModel = db.CompanyViewModels.Find(id);
            if (companyViewModel == null)
            {
                return HttpNotFound();
            }
            return View(companyViewModel);
        }

        // GET: CompanyViewModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Email,Password")] CompanyViewModel companyViewModel)
        {
            if (ModelState.IsValid)
            {
                db.CompanyViewModels.Add(companyViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyViewModel);
        }

        // GET: CompanyViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyViewModel companyViewModel = db.CompanyViewModels.Find(id);
            if (companyViewModel == null)
            {
                return HttpNotFound();
            }
            return View(companyViewModel);
        }

        // POST: CompanyViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Email,Password")] CompanyViewModel companyViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyViewModel);
        }

        // GET: CompanyViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyViewModel companyViewModel = db.CompanyViewModels.Find(id);
            if (companyViewModel == null)
            {
                return HttpNotFound();
            }
            return View(companyViewModel);
        }

        // POST: CompanyViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyViewModel companyViewModel = db.CompanyViewModels.Find(id);
            db.CompanyViewModels.Remove(companyViewModel);
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
