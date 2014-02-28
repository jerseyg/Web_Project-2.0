using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Project2.Models
{
    public class UserReset
    {
        public string emailAddress { get; set; }
        public string token { get; set; }
    }
}