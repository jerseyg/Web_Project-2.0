using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web_Project2.Database;
using Web_Project2.Controllers;
using Web_Project2.Models;

namespace Web_Project2.Controllers
{
    
    public class AppController : Controller
    {

        PDbContext db = new PDbContext();
        //
        // GET: /App/
        [IsValidLogin]
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
        public async Task<ActionResult> Login(User userModel)
        {

            if (ModelState.IsValid)
            {
                bool login = await db.Login(userModel._EmailAddress, userModel._Password);

                if (login != false)
                {
                    //Creates a Session key called "UserProfile"
                    var UserProfile = await db.CreateSession(userModel._EmailAddress);
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

        public async Task<ActionResult> Reset_Password(UserTokenModel userTokenModel)
        {
            string userId = userTokenModel._Token.Substring(0, 10);
            string token = userTokenModel._Token.Remove(0, 11);

            var tokenDataSession = new UserTokenModel()
            {
                   _UserId = userId,
                   _Token = token
            };
            Session["tokenDataSession"] = tokenDataSession;

            bool IsTokenValid = await db.IsTokenValid(userId, token);
            if (IsTokenValid) { return View(); }
            else { return View(); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reset_Password(UserReset userResetModel)
        {
            
            UserTokenModel tokenSession = (UserTokenModel) HttpContext.Session["tokenDataSession"];

            string userId = tokenSession._UserId;
            string token = tokenSession._Token;

            if (userResetModel._Password != "" || userResetModel._RetypePassword != "")
            {
                if (userResetModel._Password == userResetModel._RetypePassword)
                {
                    bool IsTokenAuthenticated = await db.IsTokenValid(userId, token);
                    if (IsTokenAuthenticated) 
                    {
                        IDictionary<string, object> cloudDictionary = new Dictionary<string, object>
                        {
                            {"userId", userId},
                            {"password", db.HashPassword(userResetModel._Password)},
                            {"salt", db._Salt}
                        };
                        var result = await ParseCloud.CallFunctionAsync<String>("tokenAuth", cloudDictionary);

                        await db.DeAssociateToken(token);
                        Session.Abandon();

                        return RedirectToAction("Login"); 
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
            else
            {
                return View();
            }
       }

        public ActionResult Reset()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reset(User userModel)
        {
            var userExists = await db.UserExists(userModel._EmailAddress);
            if (userExists)
            {
                MailGun mailGun = new MailGun();
                mailGun.EmailAddress = userModel._EmailAddress;
                await mailGun.SendResetMessage();
                ViewBag.Success = "You have been sent an email!";
                return View();
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
