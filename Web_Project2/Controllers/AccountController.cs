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
        Web_Project2.Database.PDbContext db = new Web_Project2.Database.PDbContext();
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
        public async Task<ActionResult> Create(User userModel)
        {
            

                if (ModelState.IsValid)
                {
                    bool create = await db.CreateUser(userModel);
                    if (create != false)
                    {
                        await db.Login(userModel._EmailAddress, userModel._Password);
                        var UserProfile = await db.CreateSession(userModel._EmailAddress);
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