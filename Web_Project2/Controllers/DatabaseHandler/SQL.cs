using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Project2.Models;
using System.Text;
using Web_Project2.ExternalHelper;


namespace Web_Project2.Controllers.DatabaseHandler
{
    public class SQL
    {
        private ProjectEntities db = new ProjectEntities();

        public bool UserCheck(User user, string EmailAddress)
        {

            var query = from row in db.Users
                        where row.EmailAddress.Contains(user.EmailAddress)
                        select row;
            var value = query.FirstOrDefault();

            if (value != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public string retrievePassword(User user, string EmailAddress){

            var query = from row in db.Users
                        where row.EmailAddress.Contains(user.EmailAddress)
                        select row;
            var hashedValue = query.First().Password;

            return hashedValue;
        }

        public string retrieveSalt(User user, string EmailAddress){
            var query = from row in db.Users
                        where row.EmailAddress.Contains(user.EmailAddress)
                        select row;
            var saltValue = query.First().Salt;

            return saltValue;
        }


        public bool isValidLogin(User user, string EmailAddress, string password)
        {
            bool validUser = UserCheck(user, EmailAddress);
            
            if (validUser)
            {
                //Query for password
                var query = from row in db.Users
                            where row.EmailAddress.Contains(EmailAddress)
                            select row;
                var passwordValue = query.First().Password;

                //Query for salt
                var saltValue = retrieveSalt(user, EmailAddress);

                //Creates a byte array of the salt
                byte[] byteArraySalt = Encoding.UTF8.GetBytes(saltValue);
                //validates the password given with the password in the database
                return PasswordHash.ValidatePassword(password, passwordValue, byteArraySalt);

            }
            else
            {
               return false;
            }

        }
    }
}