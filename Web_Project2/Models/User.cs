using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Project2.Models
{
    public class User
    {
        [UIHint("EmailAddress")]

        public string EmailAddress { get; set; }

        [UIHint("Password")]

        public string Password { get; set; }

        public string Salt { get; set; }
        
        [UIHint("FirstName")]
        public string FirstName { get; set; }

        [UIHint("LastName")]
        public string LastName { get; set; }

        public int Role_ID { get; set; }
    }
}