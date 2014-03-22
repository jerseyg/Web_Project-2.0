using Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web_Project2.Database;
using Web_Project2.Models;

namespace Web_Project2.Controllers
{
    class IsValidLoginAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Attribute Executed before an action call to check if user is logged in.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (UserSession.CurrentUser.Username != null) { }
            else { filterContext.Result = new RedirectResult("/App/Login"); }
        }
    }
}
