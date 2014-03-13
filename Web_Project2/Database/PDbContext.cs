using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Providers.Entities;
using Web_Project2.Controllers;
using Web_Project2.Models;

namespace Web_Project2.Database
{
    public class PDbContext : ErrorHandler
    {

        public string _Salt { get; set; }

        public async Task<Boolean> UserExists(string emailAddress)
        {

            var foundUser = await (from user in ParseUser.Query
                                   where user.Username == emailAddress
                                   select user).FindAsync();
            return (foundUser.Count() != 0) ? true : false;
        }

        public async Task<ParseObject> ReturnSingleUserObject(string emailAddress)
        {
            var foundUser = await (from user in ParseUser.Query
                                   where user.Username == emailAddress
                                   select user).FindAsync();
            var userObject = foundUser.First();
            return userObject;
        }

        public async Task<IEnumerable<ParseObject>> ReturnAllRowsParseObject(string parseClass)
        {
            var query = from genericClass in ParseObject.GetQuery(parseClass)
                        where true
                        select genericClass;
            IEnumerable<ParseObject> results = await query.FindAsync();

            return results;
        }
        public async Task<Boolean> Login(string emailAddress, string password)
        {
            string email = emailAddress;
            string nonHashedPassword = password;

            try
            {
                var user = await ReturnSingleUserObject(email);
                var salt = user.Get<string>("salt");
                var hashedPassword = HashPassword(email, salt);

                await ParseUser.LogInAsync(email, hashedPassword);

                return true;
            }
            catch (ParseException e)
            {
                TrackError(e);
                return false;
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
            catch (ParseException e)
            {
                TrackError(e);
                return false;
            }
        }

        public async Task<Object> CreateSession(string emailAddress)
        {
            try
            {
                var ParseUser = await ReturnSingleUserObject(emailAddress);
                var profileData = new SessionProfile()
                {
                    _ParseID = ParseUser.ObjectId,
                    _EmailAddress = ParseUser.Get<string>("username"),
                    _FirstName = ParseUser.Get<string>("firstName"),
                    _LastName = ParseUser.Get<string>("lastName")
                };
                return profileData;
            }
            catch (ParseException e)
            {
                TrackError(e);
                return null;
            }
        }

        public async Task<Boolean> IsTokenValid(string userId, string token)
        {
            try
            {
                var tokenQuery = await (from tokenassociate in new ParseQuery<ParseTokenModel>()
                                 where tokenassociate.Token == token
                                 select tokenassociate).FindAsync();
<<<<<<< HEAD
                
=======
>>>>>>> d32a881c01e541a1f3b7855979b42f21c6393328
                var tokenAssociateReference = ParseObject.CreateWithoutData<ParseTokenModel>(tokenQuery.First().ObjectId);

                if (tokenQuery.Count() != 0)
                {
                    var tokenAssociateUser = tokenAssociateReference.Token;

                    var userQuery = await (from user in ParseUser.Query
                                           where user.ObjectId == userId &&
                                                 user.Username == tokenAssociateUser
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
                TrackError(e);
                return false;
            }
            catch (Exception e)
            {
                TrackError(e);
                return false;
            }
        }
        public async Task DeAssociateToken(string token)
        {
            var tokenQuery = await (from tokenassociate in new ParseQuery<ParseTokenModel>()
                                    where tokenassociate.Token == token
                                    select tokenassociate).FindAsync();

            var tokenAssociateReference = ParseObject.CreateWithoutData<ParseTokenModel>(tokenQuery.First().ObjectId);
            await tokenAssociateReference.DeleteAsync();
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