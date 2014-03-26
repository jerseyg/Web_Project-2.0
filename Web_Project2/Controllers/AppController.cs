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

        CustomDbContext CustomDb = new CustomDbContext();
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
                bool login = await CustomDb.Login(userModel.EmailAddress, userModel.Password);

                if (login != false)
                {
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

        public ActionResult Create()
        {
            return View();
        }

        // POST: /Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User userModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await CustomDb.CreateNewUser(userModel);
                    return RedirectToAction("Login", "App");
                }
                catch (Exception ex)
                {
                    var value = ErrorHandler.GetInnerException(ex);
                }
            }
            return View();
        }
       
        public ActionResult LogOff()
        {
            UserSession.KillSession();
            return RedirectToAction("Index", "Home");
        }
    }
}
