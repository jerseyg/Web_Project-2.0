using Parse;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Web_Project2.Models;

namespace Web_Project2.ExternalHelper
{
    public class MailGunHelper
    {

        //Change key if needed
        protected const string mailGunApiKey = "key-2aelsu6k8tyk3gku66hh9l-4cwb9i0o5";
        //This is the email the receiver will see
        protected const string mailGunEmail = "Captivate ResetPassword <postmaster@sandbox2255.mailgun.org>";
        //Change domain if needed
        protected const string mailGunDomain = "sandbox2255.mailgun.org";


        public string EmailAddress { get; set; }
        protected string UserId { get; set; }
        protected string token { get; set; }

        UserDbContext db = new UserDbContext();


        //TO:DO:  Error Handling

        /// <summary>
        /// Handles the Sending of the reset password email.
        /// </summary>
        /// <param name="user">UserReset Model</param>
        /// <returns>client.Execute(request)</returns>
        public async Task<IRestResponse> SendResetMessage()
        {

            
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               mailGunApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain",
                                 mailGunDomain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", mailGunEmail);
            request.AddParameter("to", EmailAddress);
            request.AddParameter("subject", "Reset Password");
            request.AddParameter("html", await ResetMessage());

            //upload token to parse
            await SendKey();


            request.Method = Method.POST;
            return client.Execute(request);
        }
        /// <summary>
        /// Creates a link for reseting a password
        /// </summary>
        /// <returns></returns>
        private async Task<String> GenerateResetLink()
        {
            var salt = PasswordHash.CreateSalt();
            byte[] byteArraySalt = Encoding.UTF8.GetBytes(salt);
            var hash = PasswordHash.CreateHash(EmailAddress, byteArraySalt);
            UserId = await db.GetUserId(EmailAddress);
            token = RemoveSpecialCharacters(hash.Remove(0, 5));
            
            //Localhost Testing
            string builtLink = "http://localhost:8674/App/Reset_Password?token=" + UserId + ":" + token;

            return builtLink;
        }
        private async Task SendKey()
        {
                var tokenKey = token;
                var email = EmailAddress;
                
                ParseObject tokenassociate = new ParseObject("tokenassociate");
                tokenassociate["token"] = tokenKey;
                tokenassociate["user"] = email;

                await tokenassociate.SaveAsync();
        }
        private async Task<String> ResetMessage()
        {
            var link = await GenerateResetLink();
            string message = "<html>HTML version of the body</html>" + "<a href='" + link + "'>" + link + "</a>";
            return message;
        }

        private static string RemoveSpecialCharacters(string str) 
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str) 
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_') 
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}