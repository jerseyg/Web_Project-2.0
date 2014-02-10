using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using Web_Project2.Models;
using Web_Project2.ExternalHelper;
using Web_Project2.Controllers.DatabaseHandler;
using System.Text;
using Web_Project2.ProjectEntity__Web_Project2;


namespace Web_Project2.Controllers
{

    public class AccountController : Controller
    {
        public const int SALT_BYTE_SIZE = 24;
        private Models_ db = new Models_();

        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //
        // GET: /Account/Details/5

        public ActionResult Details(Guid id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /Account/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Account/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            SQL sql = new SQL();
            

            bool userExists = sql.UserCheck(user, user.EmailAddress);
            if (userExists == true)
            {
                ViewBag.Exists = "Username Already in use";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var salt = PasswordHash.CreateSalt();
                    byte[] byteArraySalt = Encoding.UTF8.GetBytes(salt);
                    var hash = PasswordHash.CreateHash(user.Password, byteArraySalt);

                    Guid UniqueIdentifier = Guid.NewGuid();

                    user.Salt = salt;
                    user.Password = hash;
                    user.UUID = UniqueIdentifier;
                    user.Role_ID = 2;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(user);
            }
        }


        //
        // GET: /Account/Edit/5

        public ActionResult Edit(Guid id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Account/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(Guid id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Account/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}