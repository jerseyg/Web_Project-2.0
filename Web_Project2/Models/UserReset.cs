using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Project2.Models
{
    public class UserReset
    {
        public string _EmailAddress { get; set; }
        public string _Token { get; set; }

        [UIHint("Password")]
        public string _Password { get; set; }
        [UIHint("RetypePassword")]
        public string _RetypePassword { get; set; }
    }
}

