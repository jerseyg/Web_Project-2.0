using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Web_Project2.ExternalHelper;

namespace Web_Project2.Controllers.DatabaseHelper
{
    public class ParseDb
    {
        public async Task<Boolean> UserExists(string emailAddress)
        {

            var foundUser = await (from user in ParseUser.Query
                                   where user.Get<string>("username") == emailAddress
                                   select user).FindAsync();
            return (foundUser.Count() != 0) ? true : false;
        }

        public async Task<ParseObject> GetSingleUserObject(string emailAddress)
        {
            var foundUser = await (from user in ParseUser.Query
                                   where user.Get<string>("username") == emailAddress
                                   select user).FindAsync();
            var userId = foundUser.First();
            return userId;
        }
        public string _Salt { get; set; }
        public string HashPassword(string password)
        {
            var salt = PasswordHash.CreateSalt();
            _Salt = salt;
            byte[] byteArraySalt = Encoding.UTF8.GetBytes(salt);
            var hash = PasswordHash.CreateHash(password, byteArraySalt);
            return hash;
        }
        public string HashPassword(string password, string salt)
        {
            byte[] byteArraySalt = Encoding.UTF8.GetBytes(salt);
            var hash = PasswordHash.CreateHash(password, byteArraySalt);
            return hash;
        }

        public async Task<Boolean> login(string emailAddress, string password)
        {
            string email = emailAddress;
            string nonHashedPassword = password;

            try
            {
                var user = await GetSingleUserObject(email);
                var salt = user.Get<string>("salt");
                var hashedPassword = HashPassword(email, salt);

                await ParseUser.LogInAsync(email, hashedPassword);

                return true;
            }
            catch (ParseException)
            {
                return false;
            }

        }
    }
}