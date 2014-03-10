using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Web_Project2.Controllers;
using Web_Project2.Models;

namespace Web_Project2.Database
{
    public class ParseDb
    {

        public string _Salt { get; set; }

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
            var userObject = foundUser.First();
            return userObject;
        }

        public async Task<Boolean> Login(string emailAddress, string password)
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
                throw;
            }

        }

        public async Task<Boolean> CreateUser(User userModel)
        {
            ParseUser newUser = new ParseUser()
            {
                Username = userModel._EmailAddress,
                Password = HashPassword(userModel._Password),
                Email = userModel._EmailAddress
            };
            newUser["firstName"] = userModel._FirstName;
            newUser["lastName"] = userModel._LastName;
            newUser["salt"] = _Salt;
            try
            {
                await newUser.SignUpAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Object> CreateSession(string emailAddress)
        {
            try
            {
                var ParseUser = await GetSingleUserObject(emailAddress);
                var profileData = new SessionProfile()
                {
                    parseID = ParseUser.ObjectId,
                    EmailAddress = ParseUser.Get<string>("username"),
                    firstName = ParseUser.Get<string>("firstName"),
                    lastName = ParseUser.Get<string>("lastName")
                };
                return profileData;
            }
            catch (Exception e)
            {
                return e;
            }
        }
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
    }
}