﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Project2.Models;

namespace Web_Project2.Controllers
{
    public class vApiController : Controller
    {
        //
        // GET: /vApi/
        [HttpGet]
        public ActionResult v1send(UserReset user)
        {
            ViewBag.Test1 = user.token;
            return View();
        }

    }
}
