using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Project2.Models
{
    public class User
    {
        [Required]
        [UIHint("EmailAddress")]
        public string EmailAddress { get; set; }
        [Required]
        [UIHint("Password")]
        public string Password { get; set; }
        [Required]
        [UIHint("RetypePassword")]
        public string RetypePassword { get; set; }
        public string Salt { get; set; }
        [UIHint("FirstName")]
        public string FirstName { get; set; }
        [UIHint("LastName")]
        public string LastName { get; set; }
        public int Role_ID { get; set; }
    }



}