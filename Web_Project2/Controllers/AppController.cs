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
                ViewData["User"] = ParseUser.CurrentUser.Username;
                return View();
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
        

            string emailAddress = user.EmailAddress;
            string nonHashedPassword = user.Password;

            var query = await (from getUser in ParseUser.Query
                              where getUser.Get<string>("username") == emailAddress
                              select getUser).FindAsync();

            //TODO: add to check if user exists before adding info to variables

            var firstUser = query.First();
            var salt = firstUser.Get<string>("Salt");

            byte[] byteArraySalt = Encoding.UTF8.GetBytes(salt);
            var hash = PasswordHash.CreateHash(nonHashedPassword, byteArraySalt);

            try
            {
                await ParseUser.LogInAsync(emailAddress, hash);
                return RedirectToAction("index");
            }
            catch(ParseException)
            {
                return View();
            }
            
        }
        public ActionResult LogOff()
        {
            ParseUser.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
