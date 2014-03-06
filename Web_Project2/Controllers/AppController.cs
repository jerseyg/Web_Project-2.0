using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web_Project2.ExternalHelper;
using Web_Project2.Models;

namespace Web_Project2.Controllers
{
    
    public class AppController : Controller
    {

        UserDbContext db = new UserDbContext();
        //
        // GET: /App/
        [ParseLoginCheck]
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

        public async Task<ActionResult> Reset_Password(UserReset userModel)
        {
            string userId = userModel.token.Substring(0, 10);
            string token = userModel.token.Remove(0, 11);

            try
            {
                var tokenQuery = from tokenassociate in ParseObject.GetQuery("tokenassociate")
                                 where tokenassociate.Get<string>("token") == token
                                 select tokenassociate;
                IEnumerable<ParseObject> results = await tokenQuery.FindAsync();
                
                if (results.Count() != 0)
                {
                    var tokenAssociateUser = results.First().Get<string>("user");

                    var userQuery = await (from user in ParseUser.Query
                                           where user.Get<string>("objectId") == userId &&
                                                 user.Get<string>("username") == tokenAssociateUser
                                           select user).FindAsync();
                    if (userQuery.Count() != 0)
                    {
                        return View();
                    }
                    else
                    {
                        //Userid sent with token does not match database
                        return View();
                    }

                }
                else
                {
                    //TODO: token does not exist.
                    return View();
                }
            }
            catch (ParseException e)
            {
                //ParseError
                return View();
            }
            catch (Exception e)
            {
                //All other exceptions
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reset_Password()
        {
            return View();
        }

        public ActionResult Reset()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reset(User user)
        {
 
            var checkUser = await db.CheckUser(user.EmailAddress);
            if (checkUser)
            {
                MailGunHelper mailGun = new MailGunHelper();
                mailGun.EmailAddress = user.EmailAddress;
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
