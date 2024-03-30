using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollaboraCal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.Sqlite;

namespace CollaboraCal.Testing
{
    [TestClass]
    public class UnitTest1
    {

        private const string DatabaseMigrationError = "Database has not been migrated correctly on this machine. Database migration must be applied on each machine before the program can be tested.";

        [TestMethod]
        public void TestCreateUser()
        {
            try
            {
                Assert.AreEqual<string>("Email field is blank",
                    Application.Accounts.CreateUser("", "Bob", "Hello444", "Hello444"));
                Assert.AreEqual<string>("Password field is blank",
                    Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "", "Hello444"));
                Assert.AreEqual<string>("Confirm Password field is blank",
                    Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", ""));
                Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444");
                Assert.AreEqual<string>("Email already exists",
                    Application.Accounts.CreateUser("mc1174471@gmail.com", "Bob", "Hello444", "Hello444"));
                Assert.AreEqual<string>("Couldn't verify email address",
                    Application.Accounts.CreateUser("hehehehgkhaao", "Bob", "Hello444", "Hello444"));
                Assert.AreEqual<string>("Passwords do not match each other",
                    Application.Accounts.CreateUser("cag200009@utdallas.edu", "Bob", "Hello444", "Hello333"));
                Assert.AreEqual<string>("Password must be at least 8 characters long with at least one uppercase letter, lowercase letter, and number",
                    Application.Accounts.CreateUser("cag200009@utdalas.edu", "Bob", "Hello55", "Hello55"));
                Assert.AreEqual<string>("Email and Password are Valid",
                    Application.Accounts.CreateUser("cag200009@utdallas.edu", "Bob", "Hello555", "Hello555"));

                // Successful test case emails:
                //      mc1174471@gmail.com
                //      cag200009@utdallas.edu

                Application.Database.RemoveUserByEmail("mc1174471@gmail.com");
                Application.Database.RemoveUserByEmail("cag200009@utdallas.edu");

            } catch(SqliteException sqliteException)
            {
                Console.WriteLine($"SQLite Error: {sqliteException.SqliteErrorCode}");
                Assert.Inconclusive(DatabaseMigrationError);
            }
        }

        [TestMethod]
        public void TestLogin()
        {
            string email = "mytestemail@test.com";
            string username = "TestUserName1";
            string password = "MyPassword1234";

            try
            {
                Application.Accounts.CreateUser(email, username, password, password);
                string? result = Application.AuthenticationSystem.Login(email, password);

                Assert.IsNotNull(result);

                Application.Database.RemoveUserByEmail(email);
            }
            catch (SqliteException sqliteException)
            {
                Console.WriteLine($"SQLite Error: {sqliteException.SqliteErrorCode}");
                Assert.Inconclusive(DatabaseMigrationError);
            }

        }
    }
}