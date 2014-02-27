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
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
           
            if (ParseUser.CurrentUser != null)
            {
                //ViewResult result = new ViewResult();
                //result.ViewName = "index";
                //filterContext.Result = result;
            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Login";
                filterContext.Result = new RedirectResult("/App/Login");
            }
        }
    }
}
