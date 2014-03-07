using Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Web_Project2.Controllers
{
    class ParseLoginCheckAttribute : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// Attribute Executed before an action call to check if user is logged in.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (ParseUser.CurrentUser != null){}
            else{filterContext.Result = new RedirectResult("/App/Login");}
        }
    }
}
