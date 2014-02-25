using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web_Project2.Controllers.DatabaseHandler;
using Web_Project2.ExternalHelper;
using Web_Project2.Models;

namespace Web_Project2.Controllers
{
    public class AppController : Controller
    {
        //
        // GET: /App/

        public ActionResult Index()
        {
            if (ParseUser.CurrentUser != null)
            {
                return View(Session["UserProfile"]);
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(User user)
        {
            UserDbContext db = new UserDbContext();

            if (ModelState.IsValid)
            {
                bool login = await db.login(user);

                if (login != false)
                {
                    //Creates a Session key called "UserProfile"
                    var UserProfile = await db.CreateSessionProfile(user.EmailAddress);
                    Session["UserProfile"] = UserProfile;
                    return RedirectToAction("index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
                
            
            
        }
        public ActionResult LogOff()
        {
            Session.Abandon();
            ParseUser.LogOut();
            return RedirectToAction("Index", "Home");
        }

       

        }

}
