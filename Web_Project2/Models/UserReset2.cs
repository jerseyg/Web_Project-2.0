using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Project2.Models
{
    public class UserReset2
    {
        public string emailAddress { get; set; }
        public string token { get; set; }

        [UIHint("Password")]
        public string Password { get; set; }
        [UIHint("RetypePassword")]
        public string RetypePassword { get; set; }
    }
}