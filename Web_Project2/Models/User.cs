using Parse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using Web_Project2.Controllers;

namespace Web_Project2.Models
{
    public class User
    {
        public string _UserId { get; set; }
        [Required]
        [UIHint("EmailAddress")]
        public string _EmailAddress { get; set; }
        [Required]
        [UIHint("Password")]
        public string _Password { get; set; }
        [UIHint("RetypePassword")]
        public string _RetypePassword { get; set; }
        [UIHint("FirstName")]
        public string _FirstName { get; set; }
        [UIHint("LastName")]
        public string _LastName { get; set; }
    }

 
    
}

