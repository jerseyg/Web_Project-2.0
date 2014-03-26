using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Project2.Models;

namespace Web_Project2.Controllers
{
    [IsValidLogin]
    public class RSVPController : Controller
    {
        private DevEntities db = new DevEntities();

        //
        // GET: /RSVP/

        public ActionResult Index()
        {
            var events = db.Events.Include(e => e.User);
            return View(events.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event eventModel)
        {
            eventModel.EventId = Guid.NewGuid();
            eventModel.UserId = UserSession.CurrentUser.UserId;
            eventModel.CreatedAt = DateTime.Today;
            eventModel.UpdatedAt = DateTime.Today;
            db.Events.Add(eventModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CreateInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInfo(EventInfo eventInfoModel)
        {

            db.EventInfoes.Add(eventInfoModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }   
    }
}