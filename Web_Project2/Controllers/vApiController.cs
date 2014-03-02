using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web_Project2.Models;

namespace Web_Project2.Controllers
{
    public class vApiController : Controller
    {
        //
        // GET: /vApi/
        [HttpGet]
        public ActionResult v1(UserReset user)
        {
            ViewBag.Test1 = user.token;
            return View();
        }
        public async Task<ActionResult> SomeActionMethod()
        {
                IDictionary<string, object> aa = new Dictionary<string, object>
                {
                 { "email", "jerseyg@shaw.ca" }
                };
               ///var result = await ParseCloud.CallFunctionAsync<IDictionary<string, object>>("averageStars", aa);
                var result = await ParseCloud.CallFunctionAsync<List<object>>("reset", aa);
                return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
