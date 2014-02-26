using Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Web_Project2.Controllers
{
    class ParseLoginAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Debug.WriteLine("1");
            if (ParseUser.CurrentUser != null)
            {
                Debug.WriteLine("Inside OnActionExecuting");
                ViewResult result = new ViewResult();
                result.ViewName = "index";
                filterContext.Result = result;
            }
            else
            {
                Debug.WriteLine("2");
                ViewResult result = new ViewResult();
                result.ViewName = "Login";
                result.ViewBag.ErrorMessage = "You are not authorized to use this page. Please contact administrator!";
                filterContext.Result = result;
            }
        }
    }
}
