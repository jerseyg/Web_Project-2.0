using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            SQL sql = new SQL();

            string emailAddress = user.EmailAddress;
            string nonHashedPassword = user.Password;

            bool valid = sql.isValidLogin(user, emailAddress, nonHashedPassword);

            if (valid)
            {
                FormsAuthentication.SetAuthCookie(emailAddress, false);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}
