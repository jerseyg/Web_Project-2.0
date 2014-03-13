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
        public void TrackError(ParseException error)
        {
            var errDimensions = new Dictionary<string, string> 
            {
                { "error", Convert.ToString(error.Code) }
            };
            ParseAnalytics.TrackEventAsync("ParseError", errDimensions);
        }
        public void TrackError(Exception error)
        {
            var errDimensions = new Dictionary<string, string> 
            {
                { "error", Convert.ToString(error) }
            };
            ParseAnalytics.TrackEventAsync("ExceptionError", errDimensions);
        }
    }
}