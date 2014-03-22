using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_Project2.Database;
using Web_Project2.Models;

namespace Web_Project2.Controllers.CustomAttributes
{
    public class ResetController : Controller
    {
        PDbContext db = new PDbContext();
        CustomDbContext CustomDb = new CustomDbContext();
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

            UserTokenModel tokenSession = (UserTokenModel)HttpContext.Session["tokenDataSession"];

            string userId = tokenSession._UserId;
            string token = tokenSession._Token;

            if (userResetModel._Password != "" && userResetModel._RetypePassword != "")
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
            var userExists = await db.UserExists(userModel.EmailAddress);
            if (userExists)
            {
                MailGun mailGun = new MailGun();
                mailGun.EmailAddress = userModel.EmailAddress;
                await mailGun.SendResetMessage();
                ViewBag.Success = "You have been sent an email!";
                return View();
            }
            else
            {
                return View();
            }


        }

        public async Task<Boolean> IsTokenValid(string userId, string token)
        {
            AccountEntities AccountDb = new AccountEntities();
            try
            {
                var tokenQueryCount = new Int32();
                await Task.Run(() =>
                {
                    var tokenQuery = from tokenTable in AccountDb.TokenAssociates
                                     where tokenTable.Token == token &&
                                           tokenTable.UserId == new Guid(userId)
                                     select tokenTable;
                    tokenQueryCount = tokenQuery.Count();
                });
                if (tokenQueryCount != 0)
                {
                    return true;
                }
                else
                {
                    //TODO: token does not exist.
                    return false;
                }
            }
            catch (Exception e)
            {

                return false;
            }
        }

    }
}
