using System;
using Parse;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Project2.Models
{
    [ParseClassName("tokenAssociate")]
    public class ParseTokenModel : ParseObject
    {

        [ParseFieldName("token")]
        public string Token
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
        [ParseFieldName("user")]
        public string User
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
    }
}