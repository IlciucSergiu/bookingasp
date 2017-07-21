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
using System.Net.Mail;
using System.IO;

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
            Session["User"] = "1";
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

                if (!company.UniqueEmail(companyVM.Email))
                {
                    ViewBag.Verify = "This email already exists!";

                    return View(companyVM);
                }
                company.Name = companyVM.Name;
                company.Email = companyVM.Email;
                company.Password = BCrypt.Net.BCrypt.HashPassword(companyVM.Password);



                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Login");
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

                if (company.Logo == null || company.Logo == "")
                    companyProfile.Logo = "LogoPlaceholder.jpg";
                else
                    companyProfile.Logo = company.Logo;

                ViewBag.ProfileImage = companyProfile.Logo;
            }
            return View(companyProfile);
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
                return View(companyProfile);
            }
            return View(companyProfile);
        }

        #region FileUpload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string FileUpload(HttpPostedFileBase file)
        {
            

            string fileName = "";

            if (file != null)
            {
                //string pic = System.IO.Path.GetFileName(file.FileName);

                Image pic = Image.FromStream(file.InputStream, true, true);
                int companyID = Convert.ToInt32(Session["User"].ToString());
                Company company = db.Companies.Find(companyID);

                

                if (company == null)
                {
                    return "Company not found";
                }
                else
                {
                    fileName = "Logo_" + companyID + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName );
                    file.SaveAs(path);
                    company.Logo = fileName;
                    

                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            // after successfully uploading redirect the user
            return fileName;
        }
        #endregion 
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] CompanyLoginViewModel companyVM)
        {

            if (ModelState.IsValid)
            {
                if (!db.Companies.Any(m => m.Email == companyVM.Email))
                {
                    ViewBag.Verify = "Wrong email or password!";
                    return View(companyVM);
                }

                Company company = db.Companies.First(m => m.Email == companyVM.Email);
                if (BCrypt.Net.BCrypt.Verify(companyVM.Password, company.Password))
                {
                    Session["User"] = company.ID;

                    return RedirectToAction("Index", "Services");

                }
                else
                {
                    ViewBag.Verify = "Wrong email or password!";
                    return View(companyVM);
                }
            }
            return View(companyVM);
        }

        public ActionResult SignOut()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Recover()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Recover(RecoverEmailViewModel recover)
        {
            if (recover.Email == null)
                return View();

            var company = db.Companies.Where(m => m.Email == recover.Email).FirstOrDefault();
            if (company == null)
                return View();

            //string password = RandomString(10);
            string password = "oparola";
            company.Password = BCrypt.Net.BCrypt.HashPassword(password);

            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();

            MailMessage mail = new MailMessage("eu_sergiuu14@yahoo.com", recover.Email);
            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("ilciuc.sergiu@gmail.com", "getodaci1");
            client.Host = "smtp.gmail.com";
            mail.Subject = "BookingApp recover";
            mail.Body = "Your new password is: "+password;
            client.Send(mail);

            ViewBag.Success = "The email was sent, check your email.";

            return View();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
