using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Project2.Database;
namespace Web_Project2.Controllers
{
    public class ErrorHandler
    {

        public static string GetInnerException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return string.Format("{0} > {1} ", ex.InnerException.Message, GetInnerException(ex.InnerException));
            }
            return string.Empty;
        }
    }
}