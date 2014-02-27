using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Project2.ExternalHelper
{
    public class MailGunHelper
    {
        public static IRestResponse SendComplexMessage()
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               "key-2aelsu6k8tyk3gku66hh9l-4cwb9i0o5");
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 "sandbox2255.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Captivate ResetPassword <postmaster@sandbox2255.mailgun.org>");
            request.AddParameter("to", "jerseyg@shaw.ca");
            request.AddParameter("subject", "Reset Password");
            request.AddParameter("text", "Testing some Mailgun awesomness!");
            request.AddParameter("html", "<html>HTML version of the body</html>");
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}