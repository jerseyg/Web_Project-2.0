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
using System.Text;
using System.Threading.Tasks;
using Parse;
using System.Diagnostics;


namespace Web_Project2.Controllers
{

    public class AccountController : Controller
    {
        public const int SALT_BYTE_SIZE = 24;

        [ParseLoginCheck]
        public ActionResult Details()
        {
            return View();
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
        public async Task<ActionResult> Create(User user)
        {
            UserDbContext db = new UserDbContext();

                if (ModelState.IsValid)
                {
                    bool create = await db.CreateUser(user);
                    if (create != false)
                    {
                        await db.login(user);
                        var UserProfile = await db.CreateSessionProfile(user.EmailAddress);
                        Session["UserProfile"] = UserProfile;

                        return RedirectToAction("index", "App");
                    }
                    else
                    {
                        return View();
                    }                   
                }
                return View();
  
        }



        public async Task<ActionResult> Reset()
        {            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reset(User user)
        {
            //MailGunHelper.SendComplexMessage();
            return View();
        }


        public ActionResult Edit(Guid id)
        {
            //User user = db.Users.Find(id);
           // if (user == null)
           // {
           //     return HttpNotFound();
           // }
            return View();
        }

        //
        // POST: /Account/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
               // db.Entry(user).State = EntityState.Modified;
               // db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(Guid id)
        {
            //User user = db.Users.Find(id);
         //   if (user == null)
           // {
           //     return HttpNotFound();
           // }
            return View();
        }

        //
        // POST: /Account/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
           // User user = db.Users.Find(id);
            //db.Users.Remove(user);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}