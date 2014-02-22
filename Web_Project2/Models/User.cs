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

    public class UserDbContext
    {
        public async Task<Boolean> login(User user)
        {
            string emailAddress = user.EmailAddress;
            string nonHashedPassword = user.Password;
            
            
            try
            {
            var query = await (from getUser in ParseUser.Query
                               where getUser.Get<string>("username") == emailAddress
                               select getUser).FindAsync();

            var firstUser = query.First();
            var salt = firstUser.Get<string>("salt");

            byte[] byteArraySalt = Encoding.UTF8.GetBytes(salt);
            var hash = PasswordHash.CreateHash(nonHashedPassword, byteArraySalt);


                await ParseUser.LogInAsync(emailAddress, hash);
                return true;
            }
            catch (ParseException)
            {
                return false;
            }
            
        }
    }

}