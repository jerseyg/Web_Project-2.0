using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Project2.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web_Project2.Models;
namespace Web_Project2.Database.Tests
{
    [TestClass()]
    public class CustomDbContextTests
    {
        CustomDbContext db = new CustomDbContext();
        [TestMethod()]
        public async Task IsUserValidUserFound()
        {
            
            string email = "test@email.com";  
            var isValid = await db.IsUserValid(email);
            Assert.AreEqual(true, isValid, "Value not correct");
        }

        [TestMethod()]
        public async Task GetUserRowTest()
        {
            
            string email = "test@email.com";
            var userId = new Guid("3c76a335-90d4-4f62-8aff-00ca11b4ddc1");
            var UserRow = await db.GetUserRow(email);
            Assert.AreEqual(userId, UserRow.UserId, "Value not correct");
        }

        /// <summary>
        /// Only need to run once
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task CreateNewUserTest()
        {
            User userModel = new User();
            userModel.EmailAddress = "test@email.com";
            userModel.Password = "test";
            userModel.FirstName = "test";
            userModel.LastName = "tube";
            await db.CreateNewUser(userModel);
            var isValid = await db.IsUserValid(userModel.EmailAddress);
            Assert.AreEqual(true, isValid, "Value not correct");
        }
    }
}
