using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using Web_Project2.Models;
using Web_Project2.Controllers;
using System.Text;
using System.Threading.Tasks;
using Parse;
using System.Diagnostics;
using Web_Project2.Database;


namespace Web_Project2.Controllers
{

    public class AccountController : Controller
    {
        ParseDb db = new ParseDb();
        public const int SALT_BYTE_SIZE = 24;

        [IsValidLogin]
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
            

                if (ModelState.IsValid)
                {
                    bool create = await db.CreateUser(user);
                    if (create != false)
                    {
                        await db.Login(user._EmailAddress, user._Password);
                        var UserProfile = await db.CreateSession(user._EmailAddress);
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

    }
}