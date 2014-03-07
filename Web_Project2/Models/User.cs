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
using Web_Project2.ExternalHelper;

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

    public class UserDbContext
    {

        public async Task<Object> CreateSessionProfile(string EmailAddress)
        {
            try
            {
                var query = await (from getUser in ParseUser.Query
                                   where getUser.Get<string>("username") == EmailAddress
                                   select getUser).FindAsync();

                var firstUser = query.First();

                var profileData = new SessionProfile()
                {
                    parseID = firstUser.ObjectId,
                    EmailAddress = firstUser.Get<string>("username"),
                    firstName = firstUser.Get<string>("firstName"),
                    lastName = firstUser.Get<string>("lastName")
                };

                return profileData;
            }

            catch (ParseException e)
            {
                //TO:DO Find a way to handle user not found in database               
                return e;
            }


        }
    }
}

