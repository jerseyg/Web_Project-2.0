using Parse;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Web_Project2.Database;
using Web_Project2.Controllers;
using Web_Project2.Models;

namespace Web_Project2.Controllers
{
    public class MailGun
    {

        //Change key if needed
        private readonly string mailGunApiKey = "key-2aelsu6k8tyk3gku66hh9l-4cwb9i0o5";
        //This is the email the receiver will see
        private readonly string mailGunEmail = "Captivate ResetPassword <postmaster@sandbox2255.mailgun.org>";
        //Change domain if needed
        private readonly string mailGunDomain = "sandbox2255.mailgun.org";
        //Change subject if needed
        private readonly string mailGunSubject = "Reset Password";

        public string EmailAddress { get; set; }
        private string UserId { get; set; }
        private string token { get; set; }

        PDbContext db = new PDbContext();


        //TO:DO:  Error Handling

        /// <summary>
        /// Handles the Sending of the reset password email.
        /// </summary>
        /// <param name="user">UserReset Model</param>
        /// <returns>client.Execute(request)</returns>
        public async Task<IRestResponse> SendResetMessage()
        {
            string subjectParam = "Reset Password";
            
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
            request.AddParameter("subject", subjectParam);
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
            var ParseUser = await db.GetSingleUserObject(EmailAddress);
            UserId = ParseUser.ObjectId;
            token = RemoveSpecialCharacters(hash.Remove(0, 5));
            
            //Localhost Testing
            string builtLink = "http://localhost:8674/App/Reset_Password?token=" + UserId + ":" + token;

            return builtLink;
        }
        private async Task SendKey()
        {
                var tokenKey = token;
                var email = EmailAddress;
                
                ParseTokenModel tokenAssociate = new ParseTokenModel();
                tokenAssociate.Token = tokenKey;
                tokenAssociate.User = email;

                await tokenAssociate.SaveAsync();
        }
        private async Task<String> ResetMessage()
        {
            var link = await GenerateResetLink();
            string message = "<html>HTML version of the body" + "<a href='" + link + "'>" + link + "</a>" + "</html>";
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