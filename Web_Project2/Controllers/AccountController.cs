﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using Web_Project2.Models;
using Web_Project2.Controllers;
using System.Text;
using System.Threading.Tasks;
using Parse;
using System.Diagnostics;
using Web_Project2.Database;


namespace Web_Project2.Controllers
{

    public class AccountController : Controller
    {
        CustomDbContext CustomDb = new CustomDbContext();
        //
        // GET: /Account/Create

        public ActionResult Create()
        {
            return View();
        }

        //
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

    }
}