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
        public string _ParseID { get; set; }
        public string _EmailAddress { get; set; }
        public string _FirstName { get; set; }
        public string _LastName { get; set; }
    }
}