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
using System.Drawing;


namespace BookingASP.Controllers
{
    public class CompaniesController : Controller
    {
        private CompanyDBContext db = new CompanyDBContext();

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Email,Password")] CompanyViewModel companyVM)
        {
            if (ModelState.IsValid)
            {
                Company company = new Company();

                company.Name = companyVM.Name;
                company.Email = companyVM.Email;
                company.Password = BCrypt.Net.BCrypt.HashPassword(companyVM.Password);

                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyVM);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            CompanyProfileViewModel companyProfile = new CompanyProfileViewModel();

            if (company == null)
            {
                return HttpNotFound();
            }
            else
            {
                companyProfile.CompanyName = company.CompanyName;
                companyProfile.Description = company.Description;
                companyProfile.Logo = company.byteArrayToImage(company.Logo);
                ViewBag.ProfileImage = companyProfile.Logo;
            }
            return View(companyProfile);
        }

        [Route("FileUpload")]
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            int ID = 1;
            if (ID == null)
            {
                return View("About", "Home");
            }
            if (file != null)
            {
                //string pic = System.IO.Path.GetFileName(file.FileName);

                Image pic = Image.FromStream(file.InputStream,true,true);

                Company company = db.Companies.Find(ID);

                if (company == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    company.Logo = company.imageToByteArray(pic);

                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            // after successfully uploading redirect the user
            return RedirectToAction("Edit", new { id = ID });
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CompanyName,Description,Logo")] CompanyProfileViewModel companyProfile)
        {
            if (ModelState.IsValid)
            {
                Company company = db.Companies.Find(companyProfile.ID);
                //company.ID = companyProfile.ID;
                company.CompanyName = companyProfile.CompanyName;
                company.Description = companyProfile.Description;
                //company.Logo = company.imageToByteArray(companyProfile.Logo);
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyProfile);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
