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


                bool login = await db.login(user);

                if (login != false)
                {
                    //Creates a Session key called "UserProfile"
                    await CreateSession(user.EmailAddress);
                    return RedirectToAction("index");
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

        public async Task CreateSession(string EmailAddress)
        {
            try
            {
                var query = await (from getUser in ParseUser.Query
                                   where getUser.Get<string>("username") == EmailAddress
                                   select getUser).FindAsync();

                var firstUser = query.First();

                var profileData = new SessionProfile()
                {
                    parseID = firstUser.ObjectId,
                    EmailAddress = firstUser.Get<string>("username"),
                    fullName = firstUser.Get<string>("firstName") + " " + firstUser.Get<string>("lastName")
                };

                this.Session["UserProfile"] = profileData;
            }

            catch (ParseException e)
            {
                //TO:DO Find a way to handle user not found in database

                Session.Abandon();
            }


        }

    }
}
