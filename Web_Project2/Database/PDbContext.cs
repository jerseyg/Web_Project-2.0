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
    public class PDbContext
    {

        public string _Salt { get; set; }

        public async Task<Boolean> UserExists(string emailAddress)
        {

            var foundUser = await (from user in ParseUser.Query
                                   where user.Username == emailAddress
                                   select user).FindAsync();
            return (foundUser.Count() != 0) ? true : false;
        }

        public async Task<ParseObject> GetSingleUserObject(string emailAddress)
        {
            var foundUser = await (from user in ParseUser.Query
                                   where user.Username == emailAddress
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
                    _ParseID = ParseUser.ObjectId,
                    _EmailAddress = ParseUser.Get<string>("username"),
                    _FirstName = ParseUser.Get<string>("firstName"),
                    _LastName = ParseUser.Get<string>("lastName")
                };
                return profileData;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public async Task<Boolean> IsTokenValid(string userId, string token)
        {
            try
            {
                var tokenQuery = from tokenassociate in new ParseQuery<ParseTokenModel>()
                                 where tokenassociate.Token == token
                                 select tokenassociate;
                IEnumerable<ParseObject> results = await tokenQuery.FindAsync();
                var tokenAssociateReference = ParseObject.CreateWithoutData<ParseTokenModel>(results.First().ObjectId);

                if (results.Count() != 0)
                {
                    var tokenAssociateUser = tokenAssociateReference.Token;

                    var userQuery = await (from user in ParseUser.Query
                                           where user.Get<string>("objectId") == userId &&
                                                 user.Get<string>("username") == tokenAssociateUser
                                           select user).FindAsync();

                    if (userQuery.Count() != 0)
                    {
                        return true;
                    }
                    else
                    {//Userid sent with token does not match database 
                        return false;
                    }

                }
                else
                {
                    //TODO: token does not exist.
                    return false;
                }
            }
            catch (ParseException e)
            {
                //ParseError
                return false;
            }
            catch (Exception e)
            {
                //All other exceptions
                return false;
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