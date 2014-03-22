using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_Project2.Models;

namespace Web_Project2.Database
{
    public class CustomDbContext
    {
        AccountEntities AccountDb = new AccountEntities();
        public async Task<Boolean> IsUserValid(string emailAddress)
        {
            var isValid = new Int32();
                await Task.Run(() =>
                {
                    var query = (from user in AccountDb.Users
                                 where user.EmailAddress == emailAddress
                                 select user).Count();
                    isValid = query;
                    
                });
                return (isValid != 0) ? true : false;
        }
        public async Task<Boolean> IsUserValid(Guid UserId)
        {
            var isValid = new Int32();
            await Task.Run(() =>
            {
                var query = (from user in AccountDb.Users
                             where user.UserId == UserId
                             select user).Count();
                isValid = query;

            });
            return (isValid != 0) ? true : false;
        }


        public async Task<User> GetUserRow(string emailAddress)
        {
            var userRow = new User();
            await Task.Run(() =>
            {

                var query = (from user in AccountDb.Users
                            where user.EmailAddress == emailAddress
                            select user).First();
                userRow = query;
            });
            return userRow;
        }
        public async Task<User> GetUserRow(Guid UserId)
        {
            var userRow = new User();
            await Task.Run(() =>
            {

                var query = (from user in AccountDb.Users
                             where user.UserId == UserId
                             select user).First();
                userRow = query;
            });
            return userRow;
        }


        public async Task CreateNewUser(User userModel)
        {            
            await Task.Run(() =>
            {
                try
                {
                    userModel.UserId = Guid.NewGuid();
                    userModel.Password = HashPassword(userModel.Password);
                    userModel.Salt = Salt;
                    AccountDb.Users.Add(userModel);
                    AccountDb.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    throw;
                }


            });
        }

        public async Task<Boolean> Login(string emailAddress, string password)
        {
            string email = emailAddress;
            string nonHashedPassword = password;

            var isValid = await IsUserValid(email);
            if (isValid)
            {
                var user = await GetUserRow(email);
                var salt = user.Salt;
                var hashedPassword = HashPassword(nonHashedPassword, salt);
                var userDbPassword = user.Password;

                if (hashedPassword == userDbPassword)
                {
                    UserSession.CurrentUser.Username = email;
                    UserSession.CurrentUser.FirstName = user.FirstName;
                    UserSession.CurrentUser.LastName = user.LastName;
                   //Session['UserSession'] = session;
                    return true;
                }
                else
                {
                    //password not match
                    return false;
                }

            }
            else
            {
                //user not found
                return false;
            }
        }
        public string Salt { get; set; }
        public string HashPassword(string password)
        {          
            var salt = PasswordHash.CreateSalt();
            Salt = salt;
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