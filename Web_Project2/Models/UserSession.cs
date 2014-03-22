using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Project2.Models
{
    public class UserSession
    {
        public static UserSession CurrentUser
        {
            get
            {
                UserSession session =
                (UserSession)HttpContext.Current.Session["__UserSession__"];
                if (session == null)
                {
                    session = new UserSession();
                    HttpContext.Current.Session["__UserSession__"] = session;
                }
                return session;
            }
        }

        public static void KillSession()
        {
                HttpContext.Current.Session["__UserSession__"] = null;
                HttpContext.Current.Session.Abandon();                         
        }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}