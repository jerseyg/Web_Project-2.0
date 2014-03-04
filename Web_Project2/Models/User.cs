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
        public string UserId { get; set; }
        [Required]
        [UIHint("EmailAddress")]
        public string EmailAddress { get; set; }
        [Required]
        [UIHint("Password")]
        public string Password { get; set; }
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
        public async Task<Boolean> CheckUser(string emailAddress)
        {
      
                var foundUser = await (from user in ParseUser.Query
                                   where user.Get<string>("username") == emailAddress
                                   select user).FindAsync();
                return (foundUser.Count() != 0) ? true : false;
        }

        public async Task<String> GetUserId(string emailAddress)
        {
            try
            {
                var foundUser = await (from user in ParseUser.Query
                                       where user.Get<string>("username") == emailAddress
                                       select user).FindAsync();
                var userId = foundUser.First();
                return userId.ObjectId;

            }
            catch(ParseException e)
            {
                return "";
            }


        }

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
        public async Task<Boolean> CreateUser(User user)
        {

                var salt = PasswordHash.CreateSalt();
                byte[] byteArraySalt = Encoding.UTF8.GetBytes(salt);
                var hash = PasswordHash.CreateHash(user.Password, byteArraySalt);


                user.Salt = salt;
                user.Password = hash;
                user.Role_ID = 2;


                var userBlock = new ParseUser()
                {
                    Username = user.EmailAddress,
                    Password = user.Password,
                    Email = user.EmailAddress
                };

                // other fields can be set just like with ParseObject
                userBlock["firstName"] = user.FirstName;
                userBlock["lastName"] = user.LastName;
                userBlock["salt"] = user.Salt;
                userBlock["role_ID"] = user.Role_ID;


                try
                {
                    await userBlock.SignUpAsync();
                    return true;
                }
                catch (ParseException)
                {
                   
                    return false;
                }




           }

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

