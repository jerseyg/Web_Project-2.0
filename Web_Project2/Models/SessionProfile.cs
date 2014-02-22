using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Project2.Models
{
    [Serializable]
    public class SessionProfile
    {
        public string parseID { get; set; }
        public string EmailAddress { get; set; }
        public string fullName { get; set; }
    }
}